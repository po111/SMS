using SMS.Contracts;
using SMS.Data.Common;
using SMS.Data.Models;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository repo;

        private readonly IValidationService validationService;

        public UserService(
            IRepository _repo,
            IValidationService _validationService)
        {
            repo = _repo;
            validationService = _validationService; 
        }
        public (bool registered, string error) Regiser(RegisterViewModel model)
        {
            bool registered = false;
            string error = null;

            var (isValid, validationError) =validationService.ValidateModel(model);

            if (!isValid)
            {
                return (isValid, validationError);
            }

            Cart cart = new Cart();

            User user = new User()
            {
                Email = model.Email,
                Password = CalculateHash(model.Password),
                Username = model.Username,
                Cart = cart,
                CartId=cart.Id
            };

            try
            {
                repo.Add(user);
                repo.SaveChanges();
                registered = true;
            }
            catch (Exception)
            {
                error = "Could not save user in DB";
            }

            return (registered, error);
        }

        private string CalculateHash(string password)
        {
            byte[] passwordArray = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(passwordArray));
            }
        }

        public string Login(LoginViewModel model)
        {
            var user = repo.All<User>()
                .Where(u=>u.Username==model.Username)
                .Where(u => u.Password == CalculateHash(model.Password))
                .SingleOrDefault();

            return user?.Id;
        }

        public string GetUserName(string userId)
        {
            return repo.All<User>()
                .FirstOrDefault(x=>x.Id==userId)?
                .Username;
        }
    }
}

//•	Has a Username – a string with min length 5 and max length 20 (required)
//•	Has an Email – a string, which holds only valid email (required)
//•	Has a Password – a string with min length 6 and max length 20 - hashed in the database (required)
//•	Has a Cart – a Cart object (required)

