using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Thesis.Data;
using Thesis.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Thesis.Controllers
{
    public class MapController : Controller
    {

        ApplicationDbContext context;
        UserManager<User> userManager;

        public MapController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }


        public async Task<User> GetCurrentUser()
        {
            return await userManager.GetUserAsync(User);
        }

        public ActionResult Index()
        {

            //var objects = context.Objects.ToList();
            //foreach (var item in objects)
            //{
            //    item.Photo = context.Files.First(t => t.Id == item.PhotoId);
            //    item.District = context.Districts.First(i => i.Id == item.DistrictId);
            //    item.Type = context.ObjectTypes.First(j => j.Id == item.TypeId);
            //    item.Terrain = context.Terrains.First(e => e.Id == item.TerrainId);
            //}

            //return View(objects);
            return View();
        }


        [HttpGet]
        public ActionResult GetMarkers()
        {
            //Пара - id и координаты объекта
            List<(string id, string coordinates)> info = new List<(string, string)>();
            var objects = context.Objects.ToList();
            foreach (var item in objects)
            {
                (string id, string coordinates) tupleItem = (item.Id, item.Address);
                info.Add(tupleItem);
            }

            return Json(new { info });
        }

        // AJAX-получение информации при клике на маркер площадки
        [HttpGet]
        public ActionResult GetObjectInfo(string id) // должен быть именно id, иначе не сработает
        {

            var obj = context.Objects.First(t => t.Id == id);
            var objPhoto = context.Files.First(t => t.Id == obj.PhotoId);
            var objDistrict = context.Districts.First(t => t.Id == obj.DistrictId);
            var objType = context.ObjectTypes.First(t => t.Id == obj.TypeId);
            var objTerrain = context.Terrains.First(t => t.Id == obj.TerrainId);

            return Json(new {
                photo = objPhoto.Path,
                district = objDistrict.Name,
                type = objType.Name,
                terrain = objTerrain.Name,
                light = obj.Light
            });
        }

        [HttpGet]
        public ActionResult GetObjectEvents(string id)
        {
            return Json(new
            {

            });
        }

        [HttpPost]
        public ActionResult CreateEvent([FromBody]JObject createEventModel)
        {

            var currentUser = GetCurrentUser().Result; // без .Result не работает
            currentUser.Avatar = context.Files.First(t => t.Id == currentUser.AvatarId);

            Event newEvent = new Event()
            {
                Object = context.Objects.First(t => t.Id == (string)createEventModel.GetValue("ObjectId")),
                Date = (DateTime)createEventModel.GetValue("Date"),
                TimeStart = (DateTime)createEventModel.GetValue("TimeFrom"),
                TimeEnd = (DateTime)createEventModel.GetValue("TimeTo"),
                Description = (string)createEventModel.GetValue("Description"),
                User = currentUser, 
            };

            context.Events.Add(newEvent);
            context.SaveChanges();

            // TODO: рассылка подписанным

            return Json(new
            {
                objectId = newEvent.Object.Id, // для проверки перед append
                avatar = currentUser.Avatar.Path,
                date = newEvent.Date.ToString("dd.MM.yyyy"),
                timeFrom = newEvent.TimeStart.ToString("H:mm"),
                timeTo = newEvent.TimeEnd.ToString("H:mm"),
                description = newEvent.Description,
            });
        }

    }
}