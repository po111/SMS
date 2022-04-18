using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Contracts
{
    public interface IUserService
    {

        (bool registered, string error) Regiser(RegisterViewModel model);
        string Login(LoginViewModel model);

        string GetUserName(string userId);
    }
}
