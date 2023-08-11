using Inżynierka_Common.ServiceRegistrationAttributes;
using Inżynierka.Shared.Entities;
using Microsoft.Extensions.Logging;
using Inżynierka_Common.Helpers;
using Inżynierka.Shared.IRepositories;
using AutoMapper;
using Inżynierka.Shared.DTOs.UserPreferenceForm;
using System.Text.RegularExpressions;
using Inżynierka_Services.Listing;
using Microsoft.AspNetCore.Mvc;
using Inżynierka.Shared.DTOs.Email;
using MimeKit;
using Inżynierka.Shared.ViewModels.UserPreferenceForm;

namespace Inżynierka_Services.Services
{
    [ScopedRegistration]
    public class UserPreferenceFormService
    {
        private readonly IUserPreferenceFormRepository _userPreferenceFormRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly ISingleFormProposalRepository _singleFormProposalRepository;
        private readonly ILogger<UserPreferenceFormService> _logger;

        private readonly IMapper _mapper;

        public UserPreferenceFormService(IUserPreferenceFormRepository userPreferenceFormRepository, ILogger<UserPreferenceFormService> logger, 
            IMapper mapper, IUserRepository userRepository, IOfferRepository offerRepository, ISingleFormProposalRepository singleFormProposalRepository)
        {
            _userPreferenceFormRepository = userPreferenceFormRepository;
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
            _offerRepository = offerRepository;
            _singleFormProposalRepository = singleFormProposalRepository;
        }

        public bool Create(UserPreferenceFormCreateViewModel viewModel, out string _errorMessage)
        {
            UserPreferenceFormCreateDTO dto = _mapper.Map<UserPreferenceFormCreateDTO>(viewModel);
            dto.EstateType = EnumHelper.GetDescriptionFromEnum(viewModel.EstateType);
            dto.OfferType = EnumHelper.GetDescriptionFromEnum(viewModel.OfferType);
            UserPreferenceForm? result = null, result2 = null, result3=null;

            var phoneRegex = new Regex(@"^([0-9]{9})$");
            var emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            if (dto.City == null && dto.EstateType == null && dto.OfferType==null && dto.MinArea == null && 
                dto.MaxArea == null && dto.MaxPrice == null && dto.RoomCount==null)
            {
                _errorMessage = ErrorMessageHelper.CantCreateEmptyForm; return false;
            }

            if(dto.MinArea <= 0 && dto.MaxArea <= 0 && dto.MaxPrice <= 0 && dto.RoomCount <= 0)
            {
                _errorMessage = ErrorMessageHelper.InvalidData; return false;
            }

            if (dto.ClientEmail == null || dto.ClientPhone== null)
            {
                _errorMessage = ErrorMessageHelper.EnterContactData;  return false;
            }

            if (!phoneRegex.IsMatch(dto.ClientPhone))
            {
                _errorMessage = ErrorMessageHelper.InvalidPhoneNumber; return false;
            }
            if (!emailRegex.IsMatch(dto.ClientEmail))
            {
                _errorMessage = ErrorMessageHelper.InvalidEmail; return false;
            }
                        
            List<int>? agents = _userPreferenceFormRepository.GetThreeAgentsForUserPreferenceForm(dto, out string extraMessage);

            if (agents == null || !agents.Any())
            {
                _errorMessage = extraMessage; return false;
            }

            List<UserPreferenceForm> forms = new();
            if(agents.Count > 0) 
            { 
                foreach(var agent in agents)
                {
                    UserPreferenceForm form = _mapper.Map<UserPreferenceForm>(dto);
                    form.AgentId = agent;
                    form.EmailVerificationGuid = Guid.NewGuid();
                    forms.Add(form);
                }
            }

            try
            {
                if (forms != null && forms.Count>0)
                {
                    _userPreferenceFormRepository.AddRangeAndSaveChanges(forms);
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ErrorMessageHelper.ErrorCreatingUserPreferenceForm;
                return false;
            }

            _errorMessage = extraMessage; return true;
        }

        public List<UserPreferenceFormThumbnailDTO>? GetMyForms(int agentId, out string _errorMessage)
        {
            User? user = _userRepository.GetById(agentId);
            if(user == null)
            {
                _errorMessage = ErrorMessageHelper.NoUser;
                return null;
            }
            if(user.RoleName != "AGENCY")
            {
                _errorMessage = ErrorMessageHelper.UserIsntAnAgent;
                return null;
            }

            var myForms = _userPreferenceFormRepository.GetMyForms(agentId);

            if (myForms == null)
            {
                _errorMessage = ErrorMessageHelper.NoForms;
                return null;
            }

            List<UserPreferenceFormThumbnailDTO>? forms = new();
            foreach(var form in myForms)
            {
                UserPreferenceFormThumbnailDTO dto = _mapper.Map<UserPreferenceFormThumbnailDTO>(form);
                forms.Add(dto);
            }

            if(forms == null)
            {
                _errorMessage = ErrorMessageHelper.NoFormsMapperError;
                return null;
            }
            else 
            {
                _errorMessage = "";
                return forms;
            }
        }

        public FormPageListing? GetForm(bool forReply, int formId, int userId, string imgPath, out string _errorMessage)
        {
            

            UserPreferenceForm? form = _userPreferenceFormRepository.GetById(formId);
            if (form == null)
            {
                _errorMessage = ErrorMessageHelper.NoForms;
                return null;
            }
            if(!forReply && form.AgentId != userId)
            {
                _errorMessage = ErrorMessageHelper.NotYourForm;
                return null;
            }

            FormPageListing result = _mapper.Map<FormPageListing>(form);
            if (result==null)
            {
                _errorMessage = ErrorMessageHelper.NoFormsMapperError;
                return null;
            }

            IEnumerable<SingleFormProposal>? singleFormProposals = _userPreferenceFormRepository.GetFormsProposals(formId);
            List<SingleFormProposalReadDTO>? dtoList = new();

            if (singleFormProposals != null)
            {
                foreach (var prop in singleFormProposals)
                {
                    string? offerType = _offerRepository.GetTypeOfOffer(prop.OfferId);
                    if (offerType != null)
                    {
                        Offer? offer = _offerRepository.GetOffer(prop.OfferId, offerType);
                        if (offer != null)
                        {
                            string offerImgPath = Path.Combine(imgPath, "Offers", offer.Id.ToString());
                            SingleFormProposalReadDTO dto = new()
                            {
                                OfferId = offer.Id,
                                OfferCity = offer.City,
                                OfferPrice = offer.Price,
                                OfferTitle = offer.OfferTitle,
                                OfferType = offer.OfferType,
                                OfferThumbnail = _offerRepository.GetPhoto(Path.Combine(offerImgPath)).Result
                            };
                            dtoList.Add(dto);
                        }
                        else 
                        {
                            _errorMessage = ErrorMessageHelper.NoOffer;
                            return null;
                        }
                    }
                    else
                    {
                        _errorMessage = ErrorMessageHelper.NoOffer;
                        return null;
                    }
                }
            }
            result.guid = form.EmailVerificationGuid;
            result.FormResponses = dtoList;
            _errorMessage = "";
            return result;
        }

        public bool AddAsSuggestion(int userId, int formId, int offerId, out string _errorMessage)
        {
            //get the type of offer - if it gets a type it means that an offer with given id exists!
            string? offerType = _offerRepository.GetTypeOfOffer(offerId);
            if(offerType == null)
            {
                _errorMessage = ErrorMessageHelper.NoOffer;
                return false;
            }

            //check if it's my offer
            Offer? offer = _offerRepository.GetOffer(offerId, offerType);
            if(offer!=null && offer.SellerId!=userId)
            {
                _errorMessage = ErrorMessageHelper.NotYourOffer; 
                return false;
            }


            //get the form - check if one with given id exists
            UserPreferenceForm? form = _userPreferenceFormRepository.GetById(formId);
            if (form == null)
            {
                _errorMessage = ErrorMessageHelper.NoForms;
                return false;
            }

            //create a new proposal - a new entry on the list
            SingleFormProposal proposal = new()
            {
                OfferId = offerId,
                FormId = formId
            };

            SingleFormProposal? checkIfExists = _singleFormProposalRepository.GetProposal(formId, offerId);
            if(checkIfExists != null) 
            {
                _errorMessage = ErrorMessageHelper.ProposalExists;
                return false;
            }

            try
            {
                if (proposal != null)
                {
                    _singleFormProposalRepository.AddAndSaveChanges(proposal);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _errorMessage = ErrorMessageHelper.ErrorAddingToProposal;
                return false;
            }

            _errorMessage = "";
            return true;
        }

        public bool RemoveSuggestion(int formId, int offerId, int userId, out string _errorMessage)
        {
            
            //get the type of offer - if it gets a type it means that an offer with given id exists!
            string? offerType = _offerRepository.GetTypeOfOffer(offerId);
            if (offerType == null)
            {
                _errorMessage = ErrorMessageHelper.NoOffer;
                return false;
            }

            //get the form - check if one with given id exists
            UserPreferenceForm? form = _userPreferenceFormRepository.GetById(formId);
            if (form == null)
            {
                _errorMessage = ErrorMessageHelper.NoForms;
                return false;
            }

            //check if user is the owner of given form
            if(form.AgentId != userId)
            {
                _errorMessage = ErrorMessageHelper.NotYourForm;
                return false;
            }

            //create a new proposal - a new entry on the list
            SingleFormProposal? proposal = _singleFormProposalRepository.GetProposal(formId, offerId);
            try
            {
                if (proposal != null)
                {
                    _singleFormProposalRepository.DeleteAndSaveChanges(proposal);
                }
                else
                {
                    _errorMessage = ErrorMessageHelper.NoProposal;
                    return false;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _errorMessage = ErrorMessageHelper.ErrorRemovingProposal;
                return false;
            }

            _errorMessage = "";
            return true;
        }

        public bool SendEmail(string partURL, int formId, int userId, IUrlHelper url, out string _errorMessage) 
        {
            //get the form - check if one with given id exists
            UserPreferenceForm? form = _userPreferenceFormRepository.GetForm(formId);
            if (form == null)
            {
                _errorMessage = ErrorMessageHelper.NoForms;
                return false;
            }

            //check if user is the owner of given form
            if (form.AgentId != userId)
            {
                _errorMessage = ErrorMessageHelper.NotYourForm;
                return false;
            }

            //check if the form has any suggestions to send
            if (form.SingleFormProposals == null || form.SingleFormProposals.Count == 0)
            {
                _errorMessage = "Nie można wysłać pustej listy propozycji!";
                return false;
            }

            //logic here
            //var responseUrl = url.Action("ReplyToSuggestions", "UserPreferenceForm", new { formId = formId, guid = form.EmailVerificationGuid }, protocol: "https");
            var responseUrl = $"{partURL}/UserPreferenceForm/Reply/{formId}/{form.EmailVerificationGuid}";
            string subject = MessageHelper.ResponseFromAgent;
            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = $"<h2>Otrzymano odpowiedź na przekazane preferencje od agenta: {form.Agent.FirstName + " " + form.Agent.LastName}</h2>";
            bodyBuilder.TextBody = $"Otrzymano odpowiedź na przekazane preferencje od agenta: {form.Agent.FirstName + " " + form.Agent.LastName} \n";

            bodyBuilder.HtmlBody += $"<h4>Prosimy kliknąć <a href=\"{responseUrl}\">TUTAJ</a>, aby przejść do listy propozycji oraz do formularza ich oceny.</h4>";
            bodyBuilder.TextBody += $"Prosimy kliknąć <a href=\"{responseUrl}\">TUTAJ</a>, aby przejść do listy propozycji oraz do formularza ich oceny.";

            //create message
            MimeMessage mail = new();
            mail.To.Add(MailboxAddress.Parse(form.ClientEmail));
            mail.Subject = subject;
            mail.Body = bodyBuilder.ToMessageBody();

            //get account details
            SmtpAccountDTO? dto = new()
            {
                Host = "smtp.gmail.com",
                Login = "yourmail@here",
                Password = "16-character long remote access password",
                Port = 465,
                Sender = "The name of the sender",
            };

            if (dto == null)
            {
                _errorMessage = ErrorMessageHelper.ErrorSendingEmail;
                return false;
            }
            
            //send created message
            EmailHelper.SendPredefinedEmail(mail, dto.Login, dto.Password, dto.Sender, dto.Host, dto.Port);

            _errorMessage = MessageHelper.EmailSent;
            return true;
        }

        public bool ReplyToSuggestions(string? comment, int formId, Guid guid, out string _errorMessage)
        {
            //get the form - check if one with given id exists
            UserPreferenceForm? form = _userPreferenceFormRepository.GetForm(formId);
            if (form == null || form.EmailVerificationGuid != guid)
            {
                _errorMessage = ErrorMessageHelper.NoForms;
                return false;
            }

            form.ClientComment = comment;
            
            try
            {
                _userPreferenceFormRepository.UpdateAndSaveChanges(form);
            }
            catch (Exception ex)
            {
                _errorMessage = ErrorMessageHelper.ErrorUpdatingForm;
                _logger.LogError(ex.Message);
                return false;
            }

            _errorMessage = "";
            return true;

        }
    }
}