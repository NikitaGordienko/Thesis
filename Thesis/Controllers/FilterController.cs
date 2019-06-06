using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Thesis.Data;
using Thesis.Models;
using Newtonsoft.Json.Linq;

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


        public async Task<User> GetCurrentUser()
        {
            return await userManager.GetUserAsync(User);
        }


        public ActionResult Index(int page = 1)
        {
            var objList = context.Objects.ToList();

            foreach (var obj in objList)
            {
                obj.District = context.Districts.First(t => t.Id == obj.DistrictId);
                obj.Type = context.ObjectTypes.First(t => t.Id == obj.TypeId);
                obj.Terrain = context.Terrains.First(t => t.Id == obj.TerrainId);
                obj.Photo = context.Files.First(t => t.Id == obj.PhotoId);
            }

            var objListPaged = objList.AsQueryable().GetPaged(page, 3);

            ViewData["filtered"] = false;
            return View(objListPaged);

        }

        public ActionResult Testt (PagingResult<Models.Object> pagg)
        {
            return View(pagg);
        }

        [HttpPost]
        public ActionResult ApplyFilter(string data)
        {
            JObject parsedData = JObject.Parse(data);
            // полученные параметры свойств
            JToken filterFormDistricts = parsedData.GetValue("districtsChosen");
            JToken filterFormTypes = parsedData.GetValue("typesChosen");
            JToken filterFormTerrains = parsedData.GetValue("terrainsChosen");
            JToken filterFormLight = parsedData.GetValue("lightChosen");

            // свойства фильтра
            List<string> districtFilter = new List<string>();
            List<string> typeFilter = new List<string>();
            List<string> terrainFilter = new List<string>();
            List<bool> lightFilter = new List<bool>(); // нужен ли список?

            // если у свойства фильтра не выбран ни один параметр, должны выбираться все записи по этому свойству
            // районы
            if (filterFormDistricts.Count() > 0)
            {
                foreach (var filterFormDistrict in filterFormDistricts)
                {
                    districtFilter.Add(filterFormDistrict.Value<string>("id"));
                }
            }
            else
            {
                foreach (District district in context.Districts.ToList())
                {
                    districtFilter.Add(district.Id);
                }
            }

            // типы
            if (filterFormTypes.Count() > 0)
            {
                foreach (var filterFormType in filterFormTypes)
                {
                    typeFilter.Add(filterFormType.Value<string>("id"));
                }
            }
            else
            {
                foreach (ObjectType type in context.ObjectTypes.ToList())
                {
                    typeFilter.Add(type.Id);
                }
            }

            // покрытия
            if (filterFormTerrains.Count() > 0)
            {
                foreach (var filterFormTerrain in filterFormTerrains)
                {
                    terrainFilter.Add(filterFormTerrain.Value<string>("id"));
                }
            }
            else
            {
                foreach (Terrain terrain in context.Terrains.ToList())
                {
                    terrainFilter.Add(terrain.Id);
                }
            }

            // освещение
            if (filterFormLight.ToString() == "light-true")
            {
                lightFilter.Add(true);
            }
            else if (filterFormLight.ToString() == "light-false")
            {
                lightFilter.Add(false);
            }
            else 
            {
                lightFilter.Add(true);
                lightFilter.Add(false);
            }


            var objListFiltered = from obj
                           in context.Objects.ToList()
                           where districtFilter.Contains(obj.DistrictId) &&
                                 typeFilter.Contains(obj.TypeId) &&
                                 terrainFilter.Contains(obj.TerrainId) &&
                                 lightFilter.Contains(obj.Light)
                           select obj;

            foreach (var objFiltered in objListFiltered)
            {
                objFiltered.District = context.Districts.First(t => t.Id == objFiltered.DistrictId);
                objFiltered.Type = context.ObjectTypes.First(t => t.Id == objFiltered.TypeId);
                objFiltered.Terrain = context.Terrains.First(t => t.Id == objFiltered.TerrainId);
                objFiltered.Photo = context.Files.First(t => t.Id == objFiltered.PhotoId);
            }

            var objListFilteredPaged = objListFiltered.AsQueryable().GetPaged(1, objListFiltered.Count());

            ViewData["filtered"] = true;
            return View("Index", objListFilteredPaged);

        }


        [HttpGet]
        public ActionResult Subscribe(string id)
        {
            var currentUser = GetCurrentUser().Result;
            UserObject userObject = new UserObject
            {
                Object = context.Objects.First(t => t.Id == id),
                User = currentUser,
                Id = Guid.NewGuid().ToString()
            };

            context.UserObjects.Add(userObject);
            context.SaveChanges();

            return Json(new
            {
                res = "success",
                resUser = userObject.UserId,
                resObject = userObject.ObjectId,
            });
        }


        public ActionResult Unsubscribe(string id)
        {
            var currentUser = GetCurrentUser().Result;
            UserObject userObjectToRemove = context.UserObjects.First(x => x.UserId == currentUser.Id && x.ObjectId == id);
            context.UserObjects.Remove(userObjectToRemove);
            context.SaveChanges();

            return Json(new
            {
                res = "success",
                resUser = currentUser.Id,
                resObject = id
            });
        }

    }
}