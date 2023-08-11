using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.DTOs.Offers;
using Inżynierka.Shared.DTOs.User;
using Inżynierka.Shared.Entities;
using Inżynierka.Shared.IRepositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inżynierka.Shared.DTOs.Offers.Delete;
using Inżynierka_Common.Helpers;
using Inżynierka.Shared.Repositories;
using Inżynierka.Shared.DTOs.Agency;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Inżynierka_Common.Listing;
using Inżynierka_Services.Listing;
using Org.BouncyCastle.Bcpg;
using Inżynierka_Common.Enums;
using Inżynierka.Shared.ViewModels.Agency;
using Inżynierka.Shared.ViewModels.User;
using iText.Kernel.Events;
using Microsoft.AspNetCore.Mvc;

namespace Inżynierka_Services.Services
{
    [ScopedRegistration]
    public class AgencyService
    {
        private readonly ILogger<AgencyService> _logger;
        private readonly IOfferRepository _offerRepository;
        private readonly IAgencyRepository _agencyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserEventRepository _userEventRepository;
        public AgencyService(ILogger<AgencyService> logger, IOfferRepository offerRepository, IAgencyRepository agencyRepository, IUserRepository userRepository, IMapper mapper, IUserEventRepository userEventRepository)
        {
            _logger = logger;
            _offerRepository = offerRepository;
            _agencyRepository = agencyRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userEventRepository = userEventRepository;
        }

        public AgencyUpdateViewModel? GetUpdateViewModel(int userId)
        {
            User? user = _userRepository.GetById(userId);
            if (user == null || user.OwnerOfAgencyId==null) { return null; }

            Agency? agency = _agencyRepository.GetById(user.OwnerOfAgencyId.Value);
            if(agency==null) { return null; }

            AgencyUpdateViewModel result = new()
            {
                AgencyName = agency.AgencyName,
                Email = agency.Email,
                PhoneNumber = agency.PhoneNumber,
                City = agency.City,
                Address = agency.Address,
                PostalCode = agency.PostalCode,
                Description = agency.Description,
                NIP = agency.NIP,
                REGON = agency.REGON,
                LicenceNumber = agency.LicenceNumber
            };
            return result;
        }

        public bool JoinAgencyByCode(Guid invitationGuid, int userId)
        {
            Agency? agency = _agencyRepository.GetAgencyByInvitationGuid(invitationGuid);
            if (agency == null) { return false; }

            User? user = _userRepository.GetById(userId);
            if (user == null) { return false; }

            try
            {
                user.AgentInAgencyId = agency.Id;
                user.RoleName = UserRoles.AGENCY.ToString();
                _userRepository.UpdateAndSaveChanges(user);

                //przypisanie ich do właściciela agencji
                var offers = _offerRepository.GetAll().Where(o => o.SellerId == user.Id);
                foreach (var offer in offers)
                {
                    offer.SellerType = UserRoles.AGENCY.ToString();
                    offer.LastEditedDate = DateTime.Now;
                }
                _offerRepository.UpdateRangeAndSaveChanges(offers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            return true;
        }
        public bool Update(int agencyId, int currentUserId, AgencyUpdateDTO updateDTO, out string _errorMessage)
        {

            Agency? agency = _agencyRepository.GetById(agencyId);

            if (agency == null)
            {
                _errorMessage = ErrorMessageHelper.NoAgency;
                return false;
            }
            if (agency.OwnerId != currentUserId)
            {
                _errorMessage = ErrorMessageHelper.NotTheOwnerOfAgency;
                return false;
            }

            if (updateDTO.PhoneNumber != null && !Regex.IsMatch(updateDTO.PhoneNumber, @"^([0-9]{9})$"))
            {
                _errorMessage = ErrorMessageHelper.ForbiddenSymbolInPhoneNumber;
                return false;
            }

            if (updateDTO.Email != null && !Regex.IsMatch(updateDTO.Email, @"[a-z0-9]+@[a-z]+\.[a-z]{2,3}"))
            {
                _errorMessage = ErrorMessageHelper.WrongEmail;
                return false;
            }

            if (updateDTO.NIP != null && !Regex.IsMatch(updateDTO.NIP, @"^\d{10}$"))
            {
                _errorMessage = ErrorMessageHelper.WrongNIP;
                return false;
            }

            if (updateDTO.REGON != null && (!Regex.IsMatch(updateDTO.REGON, @"^\d+$") || (updateDTO.REGON.Length != 9 && updateDTO.REGON.Length!=14)))
            {
                _errorMessage = ErrorMessageHelper.WrongREGON;
                return false;
            }

            if (updateDTO.PostalCode != null && !Regex.IsMatch(updateDTO.PostalCode, @"^[0-9]{2}-[0-9]{3}$"))
            {
                _errorMessage = ErrorMessageHelper.WrongPostalCode;
                return false;
            }

            if (updateDTO.LicenceNumber != null)
                agency.LicenceNumber = updateDTO.LicenceNumber;

            if (updateDTO.AgencyName != null)
                agency.AgencyName = updateDTO.AgencyName;

            if (updateDTO.City != null)
                agency.City = updateDTO.City;

            if (updateDTO.Address != null)
                agency.Address = updateDTO.Address;

            if (updateDTO.Description != null)
                agency.Description = updateDTO.Description;

            if (updateDTO.PhoneNumber != null)
                agency.PhoneNumber = updateDTO.PhoneNumber;

            agency.LastUpdatedDate = DateTime.UtcNow;
            
            if(updateDTO.PostalCode!=null) 
                agency.PostalCode = updateDTO.PostalCode;

            try
            {
                _agencyRepository.UpdateAndSaveChanges(agency);
            }
            catch (Exception ex)
            {
                _errorMessage = ErrorMessageHelper.ErrorUpdatingAgency;
                _logger.LogError(ex.Message);
                return false;
            }

            _errorMessage = "";
            return true;
        }

        public bool Delete(int userId, AgencyDeleteDTO dto, out string _errorMessage)
        {
            try
            {
                Agency? agency = _agencyRepository.GetById(dto.Id);

                if (agency == null || agency.DeletedDate != null)
                {
                    _errorMessage = ErrorMessageHelper.NoAgency;
                    return false;
                }
                if (agency.OwnerId != userId)
                {
                    _errorMessage = ErrorMessageHelper.NotTheOwnerOfOffer;
                    return false;
                }

                agency.LastUpdatedDate = dto.LastUpdatedDate;
                agency.DeletedDate = dto.DeletedDate;
                agency.OwnerId = null;
                agency.Owner = null;
                //dla każdego usera, który jest agentem lub właścicielem agencji - usuń wszystko związane z agencją
                IQueryable<User> agents = _userRepository.GetAllAgents(agency.Id);
                IQueryable<Offer>? offers = null;
                foreach (var agent in agents)
                {
                    agent.AgentInAgencyId = null;
                    agent.OwnerOfAgencyId = null;
                    agent.RoleName = UserRoles.PRIVATE.ToString();
                    
                    offers = _offerRepository.GetAll().Where(o => o.SellerId == agent.Id);
                    //przypisanie ich do właściciela agencji
                    foreach (var offer in offers)
                    {
                        offer.SellerType = UserRoles.PRIVATE.ToString();
                        offer.OfferStatus = EnumHelper.GetDescriptionFromEnum(OfferStatus.ENDED);
                        offer.DeletedDate= DateTime.Now;
                        offer.LastEditedDate= DateTime.Now;
                    }
                }
                if(offers != null && offers.Any())
                {
                    _offerRepository.UpdateRangeAndSaveChanges(offers);
                }
                _userRepository.UpdateRangeAndSaveChanges(agents);
                _agencyRepository.UpdateAndSaveChanges(agency);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _errorMessage = ErrorMessageHelper.ErrorDeletingAgency;
                return false;
            }

            _errorMessage = "";
            return true;
        }

        public int? CreateAgency(AgencyCreateDTO agencyCreateDTO, int ownerId, out string _errorMessage)
        {
            Agency? result = _mapper.Map<Agency>(agencyCreateDTO);
            if(result == null)
            {
                _errorMessage = "Error while mapping DTO to Agency object";
                return null;
            }

            result.OwnerId = ownerId;
            result.CreatedDate = DateTime.Now;

            User? owner = _userRepository.GetById(ownerId);
            if(owner == null)
            {
                _errorMessage = ErrorMessageHelper.NoUser;
                return null;
            }

            bool alreadyInAgency = owner.AgentInAgencyId != null || owner.OwnerOfAgencyId != null;
            if (alreadyInAgency)
            {
                _errorMessage = ErrorMessageHelper.AlreadyInAgency;
                return null;
            }

            try
            {
                if (result != null)
                {
                    result = _agencyRepository.AddAndSaveChanges(result);

                    owner.OwnerOfAgencyId = result.Id;
                    owner.RoleName = UserRoles.AGENCY.ToString();
                    _userRepository.UpdateAndSaveChanges(owner);

                    //przypisanie ich do właściciela agencji
                    var offers = _offerRepository.GetAll().Where(o => o.SellerId == owner.Id);
                    foreach (var offer in offers)
                    {
                        offer.SellerType = UserRoles.AGENCY.ToString();
                        offer.LastEditedDate = DateTime.Now;
                    }
                    _offerRepository.UpdateRangeAndSaveChanges(offers);
                }
                else
                {
                    _errorMessage = "Błąd przy zapisie Agencji w bazie danych";
                    return null;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _errorMessage = ErrorMessageHelper.ErrorCreatingAgency;
                return null;
            }

            

            _errorMessage = MessageHelper.AgencyCreatedSuccessfully;
            return result.Id;
        }

        public async Task<(bool UploadSuccessful, string errorMessage)> UploadAgencyLogo(string? photo, int agencyId, string imgPath)
        {
            //imgPath= Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Avatars")
            imgPath = Path.Combine(imgPath, agencyId.ToString());

            //if there are no photos to be added
            if (photo == null)
            {
                return (true, "Agency information updated!");
            }

            //if the file is valid
            if (photo.Length > 0)
            {
                //create directory if there is none
                if (!Directory.Exists(imgPath))
                {
                    Directory.CreateDirectory(imgPath);
                }

                //set the next name
                string filenameWithExtension = "logo.jpg";
                var path = Path.Combine(imgPath, filenameWithExtension);

                //get list of files in offers directory
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                await File.WriteAllBytesAsync($"{path}", Convert.FromBase64String(photo));
            }
            else return (false, "Agency information updated successfully but the file was invalid or corrupted");

            return (true, @"Agency information updated successfully / Files uploaded successfully");
        }



        public AgentsListing? GetAgents(int? agencyId, string imgPath)
        {
            IQueryable<User> agents = _userRepository.GetAllUsers();
            agents = agents.Where(u => !u.DeletedDate.HasValue);

            if (agencyId.HasValue)
            {
                agents = agents.Where(a => a.AgentInAgencyId == agencyId || a.OwnerOfAgencyId == agencyId).OrderByDescending(a=>a.OwnerOfAgencyId);
            }
            else
            {
                return null;
            }

            AgentsListing agentsListing = new()
            {
                TotalCount = agents.Count(),
                AgentsDTOs = agents
                .Select(x => new AgentsDTO(x.Id, $"{x.FirstName} {x.LastName}", x.Email,
                _offerRepository.GetPhoto(Path.Combine(imgPath, "Avatars", x.Id.ToString())).Result,
                x.PhoneNumber, _offerRepository.GetUserOfferCount(x.Id)))
            };

            return agentsListing;
        } 

        public Agency? GetAgency(int agencyId)
        {
            Agency? result = _agencyRepository.GetById(agencyId);
            return result;
        }

        public bool LeaveAgency(int agentId, int? ownerId, out string _errorMessage)
        {
            User? user = _userRepository.GetById(agentId);

            if (user != null)
            {
                if (user.AgentInAgencyId == null)
                {
                    _errorMessage = ErrorMessageHelper.UserIsntAnAgent;
                    return false;
                }

                int? previousAgencyId = user.AgentInAgencyId;

                //pobranie właściciela agencji 
                Agency? agency = previousAgencyId == null ? null : _agencyRepository.GetById(previousAgencyId.Value);
                int? agencyOwnerId = agency?.OwnerId;

                //przypisanie ofert i wpisów w kalendarzu byłego agenta do właściciela agencji
                if (agencyOwnerId == null)
                {
                    _errorMessage = ErrorMessageHelper.AgencyOwnerNotFound;
                    return false;
                }
                else if (ownerId != null && agencyOwnerId != ownerId)
                {
                    _errorMessage = ErrorMessageHelper.NotTheOwnerOfAgency;
                    return false;
                }
                else
                {
                    //pobranie wszystkich ofert byłego agenta
                    var offers = _offerRepository.GetAll().Where(o => o.SellerId == agentId);

                    //przypisanie ich do właściciela agencji
                    foreach (var offer in offers)
                    {
                        offer.SellerId = agencyOwnerId.Value;
                    }
                    _offerRepository.UpdateRangeAndSaveChanges(offers);

                    //przepisanie wpisów z kalendarza usuwanego agenta do właściciela agencji
                    IEnumerable<UserEvent>? userEvents = _userEventRepository.GetAllUserEvents(agentId);
                    if (userEvents != null && userEvents.Any())
                    {
                        foreach (var userEvent in userEvents)
                        {
                            userEvent.SellerId = agencyOwnerId.Value;
                        }
                        _userEventRepository.UpdateRangeAndSaveChanges(userEvents);
                    }

                    //usunięcie usera z agencji
                    user.AgentInAgencyId = null;
                    user.AgentInAgency = null;
                    user.RoleName = UserRoles.PRIVATE.ToString();
                    _userRepository.UpdateAndSaveChanges(user);
                }
                _errorMessage = MessageHelper.AgencyLeftSuccessfully;
                return true;
            }
            else
            {
                _errorMessage = ErrorMessageHelper.NoUser;
                return false;
            }
        }

        public bool AddAgent(int agentId, int ownerId, out string _errorMessage)
        {
            //sprawdzam, czy użytkownik, którego chcę dodać istnieje
            User? user = _userRepository.GetById(agentId);
            if (user != null)
            {
                //i czy nie jest już agentem w innej agencji
                if (user.AgentInAgencyId != null || user.OwnerOfAgencyId != null)
                {
                    _errorMessage = ErrorMessageHelper.UserIsAlreadyInAnAgency;
                    return false;
                }

                //sprawdzam, czy użytkownik posiada już jakieś oferty
                int? userOfferCount = user.UserOffers?.Count;
                if (userOfferCount != 0 && userOfferCount is not null)
                {
                    _errorMessage = ErrorMessageHelper.UserAlreadyHasOwnOffers;
                    return false;
                }

                //sprawdzam, czy użytkownik który próbuje go dodać jest właścicielem agencji
                User? owner = _userRepository.GetById(ownerId);
                int? agencyId = owner?.OwnerOfAgencyId; 
                Agency? agency = agencyId == null ? null : _agencyRepository.GetById(agencyId.Value);
                
                if (agencyId == null)
                {
                    _errorMessage = ErrorMessageHelper.NoAgency;
                    return false;
                }
                else if (agency?.OwnerId != ownerId)
                {
                    _errorMessage = ErrorMessageHelper.NotTheOwnerOfAgency;
                    return false;
                }
                else
                {
                    user.AgentInAgencyId = agencyId;
                    user.AgentInAgency = agency;
                    user.RoleName = EnumHelper.GetDescriptionFromEnum(UserRoles.AGENCY);
                    _userRepository.UpdateAndSaveChanges(user);

                    _errorMessage = MessageHelper.AgentAddedSuccessfully;
                    return true;
                }
            }
            else
            {
                _errorMessage = ErrorMessageHelper.NoUser;
                return false;
            }
        }
    }
}
