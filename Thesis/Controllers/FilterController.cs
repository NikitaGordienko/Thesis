using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Thesis.Data;
using Thesis.Models;

namespace Thesis.Controllers
{
    public class FilterController : Controller
    {
        private ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        //private readonly RoleManager<IdentityRole> roleManager;
        //private readonly SignInManager<User> signinManager;

        public FilterController(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signinManager)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            //this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            //this.signinManager = signinManager ?? throw new ArgumentNullException(nameof(signinManager));
        }

        public ActionResult Index()
        {
            var objList = context.Objects.ToList();
            foreach (var obj in objList)
            {
                obj.District = context.Districts.First(t => t.Id == obj.DistrictId);
                obj.Type = context.ObjectTypes.First(t => t.Id == obj.TypeId);
                obj.Terrain = context.Terrains.First(t => t.Id == obj.TerrainId);
                obj.Photo = context.Files.First(t => t.Id == obj.PhotoId);
            }

            return View(objList);
            //return View();
        }
    }
}