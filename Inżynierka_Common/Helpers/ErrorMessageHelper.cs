using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Common.Helpers
{
    public static class ErrorMessageHelper
    {
        //Auth
        public const string UserExists = "Taki uzytkownik już istnieje";
        public const string ConfirmationFailed = "Potwierdzenie zakończone niepowodzeniem";
        public const string NoUserWithGivenRecoveryGuid = "Nie znaleziono użytkownika o podanych danych odzyskiwania konta!";
        public const string WrongCredentials = "Nieprawidłowe dane logowania!";
        public const string AccountDoesntExist = "Dane konto użytkownika nie istnieje!";
        public const string WrongPhoneNumber = "Podany numer telefonu nie jest poprawny. Należy użyć numeru składającego się z 9 cyfr.";

        //Users
        public const string NotFound = "Nie zaleziono";
        public const string NoUser = "Nie znaleziono użytkownika";
        public const string ErrorDeletingUser = "Podczas usuwania użytkownika wystąpił błąd";
        public const string ErrorUpdatingUser = "Podczas aktualizacji użytkownika wystąpił błąd";
        public const string ForbiddenSymbol = "Imię i nazwisko mogą zawierać wyłącznie litery, spację oraz myślniki";
        public const string CannotEditSomeoneElsesAccountDetails = "Nie można edytować danych nieswojego konta";

        //Language
        public const string NoLanguage = "Nie określono języka";

        //Offer
        public const string ErrorCreatingOffer = "Podczas tworzenia oferty wystąpił błąd";
        public const string ErrorDeletingOffer = "Error deleting offer";
        public const string ErrorSavingPhotos = "Error while saving photos. Make sure the files are in .PNG or .JPEG format!";
        public const string NoOffer = "Nie znaleziono oferty o podanym numerze, lub została ona usunięta";
        public const string NotTheOwnerOfOffer = "Aktualny użytkownik nie jest właścicielem tej oferty";
        public const string ErrorUpdatingOffer = "Error while updating given offer";
        public const string OfferUpdateError_UnknownType = "Offer Update Error - Unknown Type of Offer";
        public const string ErrorAddingToFavourites = "Error while adding given offer to users favourite list";
        public const string ErrorGeneratingContract = "Error while generating a contract";
        public const string ErrorRemovingFromFavourites = "Given offer couldn't be removed from your favourite list - perhaps it doesn't exist anymore or wasn't liked in the first place";
        public const string UnrecognizedOfferType = "Nie można określić typu wskazanej oferty";


        //UserEvent
        public const string NotYourEvent = "Nie można przeglądać czyichś zdarzeń";
        public const string NoEvent = "Nie znaleziono zdarzenia o podanym identyfikatorze";
        public const string ErrorUpdatingEvent = "Aktualizacja zdarzenia zakończona niepowodzeniem";
        public const string CantCreateOfferNotRelatedToYou = "Nie można utworzyć zdarzenia związanego z ofertą, której nie jest się właścicielem";
        public const string ErrorGettingUserEvents = "Podczas pobierania zdarzeń wystąpił problem";
        public const string ErrorCreatingUserEvent = "Podczas tworzenia zdarzenia wystąpił problem";


        //Agency
        public const string ErrorDeletingAgency = "Podczas usuwania agencji wystąpił błąd.";
        public const string NoAgencyWithGivenCode = "Nieprawidłowy kod zaproszeniowy";
        public const string NoAgency = "Nie znaleziono agencji";
        public const string ErrorUpdatingAgency = "Error while updating given agency";
        public const string NotTheOwnerOfAgency = "You're not the owner of given agency";
        public const string ErrorCreatingAgency = "Error while creating a new agency";
        public const string AlreadyInAgency = "Nie można utworzyć nowej agencji będąc już w istniejącej";
        public const string AgencyOwnerNotFound = "Couldn't find the owner of agency";
        public const string CantViewAgentListOfAgency = "Nie można wyświetlić listy agentów agencji, w której się nie pracuje";
        public const string UserIsntAnAgent = "You're not a member of an agency";
        public const string UserIsAlreadyInAnAgency = "Given user is already working for an agency";
        public const string UserAlreadyHasOwnOffers = "Can't add given user to agency - user already has private offers";

        public const string ForbiddenSymbolInPhoneNumber = "Forbidden symbol in phone number detected - only numbers allowed";
        public const string WrongNIP = "Given text wasn't recognised as a valid NIP number";
        public const string WrongREGON = "Given text wasn't recognised as a valid REGON number";
        public const string WrongEmail = "Given text wasn't recognised as a valid email address";
        public const string WrongPostalCode = "Given text wasn't recognised as a valid polish postal code (format: xx-xxx)";

        //Contract
        public const string UnsupportedContractType = "Non-existing contract type";

        //User Preference Form
        public const string ErrorCreatingUserPreferenceForm = "Error while creating the user preference form";
        public const string NoOffersMatchingPreferences = "Nie znaleziono ofert spełniających zadane kryteria";
        public const string SimilarOffersFound= "Nie znaleziono ofert spełniających wszystkie kryteria, lecz znaleziono zbliżone oferty";
        public const string CantCreateEmptyForm = "Unable to create an empty preference form. Enter some values to the form first!";
        public const string EnterContactData = "Please enter your contact data.";
        public const string InvalidPhoneNumber = "Please enter a phone number valid in Poland";
        public const string InvalidEmail = "Please enter a valid email address";
        public const string InvalidData = "Please enter valid data";
        public const string NoForms = "Nie znaleziono formularza";
        public const string NoFormsMapperError = "No forms found due to a potential mapper error";
        public const string NotYourForm = "Nie jesteś właścicielem tego formularza";
        public const string ErrorAddingToProposal = "Podczas dodawania propozycji wystąpił błąd";
        public const string ProposalExists = "Wybrana propozycja już istnieje na liście";
        public const string ErrorRemovingProposal = "Podczas usuwania propozycji wystąpił błąd";
        public const string NoProposal = "Nie znaleziono takiej propozycji";
        public const string ErrorSendingEmail = "An error occured when sending email";
        public const string ErrorUpdatingForm = "An error occured when updating the form";
        public const string NotYourOffer = "Nie można zaproponować oferty, której nie jest się właścicielem";
    }
}
