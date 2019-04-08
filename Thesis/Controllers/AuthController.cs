using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Thesis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;


namespace Thesis.Controllers
{
    public class AuthController : Controller
    {

        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signinManager;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signinManager) // , SignInManager<User> signInManager
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.signinManager = signinManager ?? throw new ArgumentNullException(nameof(signinManager));
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
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


        [HttpGet]
        public ActionResult Profile()
        {
            return View();
        }
    }
}