using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Library.ViewModels;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Library.Services.EmailServices;

namespace Library.Controllers
{
    public class AccountController:Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        public EmailService emailService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager,EmailService email) 
        {
            emailService = email;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        //Контроллер возврата страницы  для входа
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        //Контроллер обрабатывающий post-запрс при входе в личный кабинет
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //Проверка правильности ввода логина и пароля
            if (ModelState.IsValid)
            {
                // Авторизация с помощью  Identity-метода 
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // Проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        //Если да, то переходим в предыдущую страницу 
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        //Если нет, то переходим в начальную сраницу
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
        //
        //Контроллер для деавторизации
        [Authorize]
        [HttpGet]
        public IActionResult LogOfff()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        //Get-контролер возврата страницы для регистрации 
       [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        //Post-контроллер,обработки формы регистрации 
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            //#IF1:Проверяем валидность переменных форм
            if (ModelState.IsValid)
            {
                //Если валидно то
                //Создаем переменную для пользователя
                User user = new User { Email = model.Email, UserName = model.Email,NameUser=model.Name };
                //Добавляем в бд и хешируем пароль, и получаем result,который хранить состояние операции
                var result = await _userManager.CreateAsync(user, model.Password); 
                //#IF2:Если состояние операции Succeeded
                if (result.Succeeded)
                {
                    //То пользователю добавляем роль user по умолчанию,результат операции возвращается 
                    result = await _userManager.AddToRoleAsync(user, "user");
                    //#IF3:Елси рузельтат-Succeded 
                    if (result.Succeeded)
                    {
                        //Авторизация
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                //#IF2:Если состояние операции не Succeeded,то
                else
                {
                    //Добавляем ошибки
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
          
            return View(model);
        }
    }
}
