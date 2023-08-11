using Inżynierka.Shared.Entities;
using Inżynierka_Common.Enums;
using MatBlazor;
using Microsoft.AspNetCore.Components;

namespace Inżynierka.Client.Handlers
{
    public static class OfferSearchHelper
    {
        public static MatTheme theme = new()
        {
            Primary = "green",
            Secondary = "green",
            Surface = "#ffffff"
        };

        public static string AddStandardQueryParams(string uri, string estateT, string offerT, string? City, 
            int? MaxPrice, HowRecent? HowRecentEnum, int? MinPrice, int? OfferId, 
            string? DescriptionFragment, bool? RemoteControl)
        {
            uri += $"Offer/Search/{estateT}/";
            if (estateT != "Plot")
            {
                uri += $"{offerT}";
            }
            if (City != null && City != "")
            {
                uri = AddQueryParm(uri, "City", City);
            }
            if (MaxPrice != null && MaxPrice.Value > 0)
            {
                uri = AddQueryParm(uri, "maxPrice", MaxPrice.Value.ToString());
            }
            if (HowRecentEnum != null)
            {
                uri = AddQueryParm(uri, "HowRecent", HowRecentEnum.ToString());
            }
            if (MinPrice != null && MinPrice.Value >= 0)
            {
                uri = AddQueryParm(uri, "minPrice", MinPrice.Value.ToString());
            }
            if (OfferId != null && OfferId.Value >= 0)
            {
                uri = AddQueryParm(uri, "OfferId", OfferId.Value.ToString());
            }
            if (DescriptionFragment != null && DescriptionFragment != "")
            {
                uri = AddQueryParm(uri, "DescriptionFragment", DescriptionFragment);
            }
            if (RemoteControl != null)
            {
                uri = AddQueryParm(uri, "RemoteControl", RemoteControl.Value.ToString());
            }
            return uri;
        }

        public static string AddQueryParm(string uri, string parmName, string parmValue)
        {
            var uriBuilder = new UriBuilder(uri);
            var q = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            q[parmName] = parmValue;
            uriBuilder.Query = q.ToString();
            var newUrl = uriBuilder.ToString();
            return newUrl;
        }
    }
}
