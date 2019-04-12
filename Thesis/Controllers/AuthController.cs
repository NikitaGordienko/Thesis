using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Thesis.Data;
using Thesis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Thesis.Controllers
{
    public class AuthController : Controller
    {
        private ApplicationDbContext context;
        // для получения полного пути wwwroot
        private IHostingEnvironment appEnvironment;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signinManager;

        public AuthController(ApplicationDbContext context, IHostingEnvironment appEnvironment, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signinManager) 
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.signinManager = signinManager ?? throw new ArgumentNullException(nameof(signinManager));
        }


        private async Task<User> GetCurrentUserAsync()
        {
            return await userManager.GetUserAsync(User);
        }


        [HttpGet]
        public ActionResult Signup()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Profile", "Auth");
            }
            else
            {
                return View();
            }
           
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signup(SignupModel signupModel)
        {
            if (ModelState.IsValid)
            {

                User user = new User
                {
                    UserName = signupModel.Login,
                    Name = signupModel.Name,
                    Surname = signupModel.Surname,
                    Email = signupModel.Email, // TODO: проверка на уникальность email
                    Avatar = context.Files.First(t => t.Name == "user_default.png"),
                    PreferredAddress = "123", // null по умолчанию?
                };

                var signupResult = await userManager.CreateAsync(user, signupModel.Password);

                if (signupResult.Succeeded)
                {
                    IdentityResult assignRoleResult;
                    var userRoleCheck = await roleManager.RoleExistsAsync("User");
                    if (!userRoleCheck)
                    {
                        await roleManager.CreateAsync(new IdentityRole("User"));
                    }

                    assignRoleResult = await userManager.AddToRoleAsync(user, "User");
                    if (!assignRoleResult.Succeeded)
                    {
                        foreach (var err in assignRoleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, err.Description);
                        }
                    }

                    return RedirectToAction("Signin", "Auth"); // или редирект сразу в профиль?
                }
                else
                {
                    foreach (var error in signupResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }


            }

            return View(signupModel);
           
        }

        [HttpGet]
        public ActionResult Signin()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signin(SigninModel signinModel)
        {
            
            if (ModelState.IsValid)
            {
                var authResult = await signinManager.PasswordSignInAsync(signinModel.Login, signinModel.Password, signinModel.RememberMe, false);
                if (authResult.Succeeded)
                {
                    return RedirectToAction("Profile", "Auth");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неверный логин и/или пароль");
                }
            }

            return View(signinModel);

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Profile()
        {
            User currentUser = await GetCurrentUserAsync();
            // получаем аватар пользователя (из другой таблицы)
            currentUser.Avatar = context.Files.FirstOrDefault(t => t.Id == currentUser.AvatarId);
            return View(currentUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(EditProfileModel editProfileModel, User userModel, IFormFile uploadedAvatar)
        {
            if (ModelState.IsValid)
            {

                var currentUser = await GetCurrentUserAsync();
                currentUser.Name = editProfileModel.Name;
                currentUser.Surname = editProfileModel.Surname;
                currentUser.Email = editProfileModel.Email;
                currentUser.PreferredAddress = editProfileModel.PreferredAddress;

                // сохранение загруженного аватара
                if (uploadedAvatar != null)
                {
                    string path = "/images/avatars/" + uploadedAvatar.FileName;
                    using (var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedAvatar.CopyToAsync(fileStream);
                    }
                    FileModel file = new FileModel { Name = uploadedAvatar.FileName, Path = path };
                    context.Files.Add(file);
                    // присвоение Id только что добавленной картинки
                    currentUser.AvatarId = file.Id;
                }

                IdentityResult editResult;
                editResult = await userManager.UpdateAsync(currentUser);
                if (editResult.Succeeded)
                {
                    return RedirectToAction("Profile", "Auth");
                }
                else
                {
                    foreach (var error in editResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // TODO: показывать ошибки модели, а не просто перезагружать страницу при неверном вводе
            //return View(editProfileModel);
            return RedirectToAction("Profile", "Auth");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Logout()
        {
            await signinManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}