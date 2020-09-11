using Library.Models;
using Library.Services.EmailServices;
using Library.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Library.Models.Results;

namespace Library.Services.AccountControlServices
{
    public class AccountControlService : IAccountControlService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService emailService;
        private readonly IMapper mapper;

        public AccountControlService(UserManager<User> userManager,
                                     SignInManager<User> signInManager,
                                     EmailService email,
                                     IMapper mapper)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this.emailService = email;
            this.mapper = mapper;
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string code, Controller controller)
        {
            if (userId == null || code == null)
            {
                return controller.View("Error");
            }
            //Находим пользователя
            var user = await _userManager.FindByIdAsync(userId);
            //Если user=null
            if (user == null)
            {
                //Возвращаем страницу  ошибок
                return controller.View("Error");
            }
            //Подтверждаем email  и сохраняем в бд
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                return controller.RedirectToAction("Index", "Home");
            else
                return controller.View("Error");
        }

        public async Task<IActionResult> Login(LoginViewModel model, Controller controller)
        {
            //Проверка правильности ввода логина и пароля
            if (controller.ModelState.IsValid)
            {
                //Находим пользователя
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    // проверяем, подтвержден ли email
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        controller.ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                        return controller.View(model);
                    }
                }


                // Авторизация с помощью  Identity-метода 
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // Проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && controller.Url.IsLocalUrl(model.ReturnUrl))
                    {
                        //Если да, то переходим в предыдущую страницу 
                        return controller.Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        //Если нет, то переходим в начальную сраницу
                        return controller.RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    controller.ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return controller.View(model);
        }

        public async Task<IActionResult> Register(UserRegisterViewModel model, Controller controller)
        {
            //#IF1:Проверяем валидность переменных форм
            if (controller.ModelState.IsValid)
            {
                //Если валидно то
                //Создаем переменную для пользователя
                User user = mapper.Map<User>(model);
                //Добавляем в бд и хешируем пароль, и получаем result,который хранить состояние операции
                var result = await _userManager.CreateAsync(user, model.Password);
                //#IF2:Если состояние операции Succeeded
                if (result.Succeeded)
                {
                    //Генерируем токен для подтверждения
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //Создаем ссылку для подтверждения
                    var callbackUrl = controller.Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: controller.HttpContext.Request.Scheme);
                    //То пользователю добавляем роль user по умолчанию,результат операции возвращается 
                    result = await _userManager.AddToRoleAsync(user, "user");
                    //Отправляем письмо для подтверждения
                    await emailService.SendEmailAsync(model.Email, "Подтвердите свой аккаунт",
                       $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                    return new HtmlResult($"<b>Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме</b> <b><i><a href={controller.Url.Action("Index", "Home")}>Начальная страница</a></i></b>");
                }
                //#IF2:Если состояние операции не Succeeded,то
                else
                {
                    //Добавляем ошибки
                    foreach (var error in result.Errors)
                    {
                        controller.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return controller.View(model);
        }
    }
}
