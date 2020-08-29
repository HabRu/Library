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
using Microsoft.AspNetCore.Http;
using Library.Models.Results;

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
                //Находим пользователя
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    // проверяем, подтвержден ли email
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                        return View(model);
                    }
                }


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
                    //Генерируем токен для подтверждения
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //Создаем ссылку для подтверждения
                    var callbackUrl=Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    //То пользователю добавляем роль user по умолчанию,результат операции возвращается 
                    result = await _userManager.AddToRoleAsync(user, "user");
                    //Отправляем письмо для подтверждения
                    await emailService.SendEmailAsync(model.Email, "Подтвердите свой аккаунт",
                       $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                    return new HtmlResult($"<b>Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме</b> <b><i><a href={Url.Action("Index", "Home")}>Начальная страница</a></i></b>");
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
        //Get-контроллер. Для подтверждения "email"а
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            //Находим пользователя
            var user = await _userManager.FindByIdAsync(userId);
            //Если user=null
            if (user == null)
            {
                //Возвращаем страницу  ошибок
                return View("Error");
            }
            //Подтверждаем email  и сохраняем в бд
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }
    }
}
