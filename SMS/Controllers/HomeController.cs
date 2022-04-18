using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using SMS.Contracts;
using SMS.Models;
using System.Collections.Generic;

namespace SMS.Controllers
{
    public class HomeController : Controller
    {

        private IUserService userService;
        private IProductService productService;

        public HomeController(
            Request request,
            IUserService _userService,
            IProductService _productService) 
            : base(request)
        {
            userService = _userService;
            productService = _productService;
        }

        public Response Index()
        {
            if (User.IsAuthenticated)
            {
                string username =  userService.GetUserName(User.Id);

                var model = new
                {
                    Username = username,
                    IsAuthenticated = true,
                    Products =  productService.GetProducts()
                    
                };

                return View(model, "/Home/IndexLoggedIn");
            }
            
            return this.View(new { IsAuthenticated = false});
        }
    }
}