using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Thesis.Models;

namespace Thesis.Controllers
{
    public class AuthController : Controller
    {

        public IActionResult Signup()
        {
            return View();
        }

        // Task?
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(SignupModel signupModel)
        {
            try
            {
                // логика

                return RedirectToAction("Index", "Signin"); // или редирект сразу в профиль?
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Signin()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signin(SigninModel signinModel)
        {
            try
            {


                return RedirectToAction("Index", "Profile");
            }
            catch
            {
                return View();
            }
        }

    }
}