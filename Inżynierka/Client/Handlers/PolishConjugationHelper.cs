using Inżynierka.Shared.Map;

namespace Inżynierka.Client.Handlers
{
    public static class PolishConjugationHelper
    {
        public static string GetRoomWord(int? count)
        {
            if (count == 1)
            {
                return "pokój";
            }
            else if ((count % 10 == 2 || count % 10 == 3 || count % 10 == 4) && !(count >= 10 && count <= 20))
            {
                return "pokoje";
            }
            else
            {
                return "pokoi";
            }
        }

        public static string Requests(int? count)
        {
            if (count == 1)
            {
                return "prośbę";
            }
            else if ((count % 10 == 2 || count % 10 == 3 || count % 10 == 4) && !(count >= 10 && count <= 20))
            {
                return "prośby";
            }
            else
            {
                return "próśb";
            }
        }
        
        public static string Agents(int? count)
        {
            if (count == 1)
            {
                return "agenta";
            }
            else
            {
                return "agentów";
            }
        }

        public static string ActiveOffers(int? count)
        {
            if (count == 1)
            {
                return "aktywną ofertę!";
            }
            else if ((count % 10 == 2 || count % 10 == 3 || count % 10 == 4) && !(count >= 10 && count <= 20))
            {
                return "aktywne oferty!";
            }
            else
            {
                return "aktywnych ofert!";
            }
        }

        public static string FavOffers(int? count)
        {
            if(count == 0)
            {
                return "Nie spodobała Ci się jeszcze żadna oferta :(";
            }
            else if (count == 1)
            {
                return $"Spodobała Ci się łącznie {count} oferta!";
            }
            else if ((count % 10 == 2 || count % 10 == 3 || count % 10 == 4) && !(count >= 10 && count <= 20))
            {
                return $"Spodobały Ci się łącznie {count} oferty!";
            }
            else
            {
                return $"Spodobało Ci się łącznie {count} ofert!";
            }
        }
    }
}
