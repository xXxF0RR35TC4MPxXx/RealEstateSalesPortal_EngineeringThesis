using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Common.Helpers
{
    public class MessageHelper
    {
        //Auth
        public const string UserCreated = "User created successfully";
        public const string AccountConfirmed = "Account confirmed. Your password has been changed";
        public const string RecoveryEmailSent = "Wiadomość do odzyskania konta została wysłana.";
        public const string PasswordChanged = "Hasło zostało zmienione!";
        public const string PasswordDuplicate = "Podane hasło jest identyczne do poprzedniego!";

        //Email
        public const string RegistrationSubject = "Registration";
        public static string RegistrationBody(string url)
        {
            return $"Registration successful. Click confirmation link to complete the process. \n {url}";
        }

        public const string PasswordRecoverySubject = "Password Recovery";
        public static string PasswordRecoveryBody(string url)
        {
            return $"Password recovery link: {url}";
        }
        public const string EmailSent = "Email sent successfully";


        //Offer
        public const string OfferCreatedSuccessfully = "Offer created successfully";
        public const string OfferDeletedSuccessfully = "Offer deleted successfully";
        public const string OfferEndedSuccessfully = "Offer ended successfully";
        public const string OfferEditedSuccessfully = "Offer edited successfully";
        public const string OfferAddedToFavouritesSuccessfully = "Offer added to favourites.";
        public const string OfferRemovedFromFavouritesSuccessfully = "Offer removed from favourites.";
        public const string EmptyOfferList = "The list of offers fullfilling given parameters is empty";

        //UserFavourite
        public const string UserFavListEmpty = "User favourites list is empty";


        //User
        public static string UserEditSuccess = "Editing was successful";
        public static string UserDeleteSuccess = "User deleted successfully";

        //Language
        public const string LanguageChangeSuccess = "Language changed successfully";

        //UserEvent
        public const string UserEventDeletedSuccessfully = "Zdarzenie usunięte pomyślnie";
        public const string UserEventCreatedSuccessfully = "Zdarzenie utworzone pomyślnie";
        public const string UserEventUpdatedSuccessfully = "Zdarzenie zaktualizowane pomyślnie";

        //Agency
        public const string AgencyDeletedSuccessfully = "Agencja usunięta pomyślnie";
        public const string AgencyCreatedSuccessfully = "Agencja utworzona pomyślnie";
        public const string AgencyLeftSuccessfully = "Agencja opuszczona pomyślnie";
        public const string NoAgentsInThisAgency = "Nie znaleziono agentów";
        public const string AgentAddedSuccessfully = "Agent dodany pomyślnie";

        //User Preference Form
        public const string UserPreferenceFormCreatedSuccessfully = "User preference form created successfully";
        public const string ResponseFromAgent = "Otrzymano odpowiedź od agenta!";
    }
}
