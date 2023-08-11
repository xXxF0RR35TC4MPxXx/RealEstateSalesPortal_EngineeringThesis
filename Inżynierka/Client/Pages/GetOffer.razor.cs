using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Inżynierka.Client.Pages
{
    public class GetOfferBase : ComponentBase
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; }
        
    }
}
