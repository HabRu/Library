using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services.AccountControlServices
{
    public interface IAccountControlService
    {
        Task<IActionResult> Login(LoginViewModel model, Controller controller);
        Task<IActionResult> Register(UserRegisterViewModel model, Controller controller);
        Task<IActionResult> ConfirmEmail(string userId, string code, Controller controller);


    }
}
