using Inżynierka_Common.Helpers;
using Inżynierka.Shared.DTOs.Offers;
using Inżynierka.Shared.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Inżynierka_Services.Services;
using System.Collections.Generic;
using System.Diagnostics;

namespace Inżynierka.Server.Controllers
{
    [ApiController]
    [Route("homeController")]
    public class HomeController : ControllerBase
    {
        private HomeService _homeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(HomeService homeService, IWebHostEnvironment webHostEnvironment)
        {
            _homeService = homeService;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Homepage of the web application.
        /// </summary>
        /// <returns>JSON strings representing lists of selected objects of the Offer class</returns>
        /// <response code="200">lists of offers</response>
        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(typeof(IEnumerable<HomepageOffersDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IEnumerable<HomepageOffersDTO> Index()
        {
            //LogUserAction("HomeController", "Index", "", _userActionService);
            IEnumerable<HomepageOffersDTO>? homepageOffers = _homeService.GetHomepageOffersList(Path.Combine(_webHostEnvironment.ContentRootPath, "img", "Offers"));

            return homepageOffers;
        }

    }
}