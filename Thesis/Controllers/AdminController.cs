using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Thesis.Data;
using Thesis.Models;

namespace Thesis.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext context;
        private IHostingEnvironment appEnvironment;

        public AdminController (ApplicationDbContext context, IHostingEnvironment appEnvironment)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var suggestedList = context.SuggestedInfo.ToList();
            foreach (SuggestedInfo suggestedItem in suggestedList)
            {
                suggestedItem.Photo = context.Files.First(x => x.Id == suggestedItem.PhotoId);
                suggestedItem.District = context.Districts.First(x => x.Id == suggestedItem.DistrictId);
                suggestedItem.Type = context.ObjectTypes.First(x => x.Id == suggestedItem.TypeId);
                suggestedItem.Terrain = context.Terrains.First(x => x.Id == suggestedItem.TerrainId);
            }

            return View(suggestedList);
        }

        public ActionResult Accept(string id)
        {
            SuggestedInfo itemToAccept = context.SuggestedInfo.First(x => x.Id == id);
            itemToAccept.Photo = context.Files.First(x => x.Id == itemToAccept.PhotoId);
            itemToAccept.District = context.Districts.First(x => x.Id == itemToAccept.DistrictId);
            itemToAccept.Type = context.ObjectTypes.First(x => x.Id == itemToAccept.TypeId);
            itemToAccept.Terrain = context.Terrains.First(x => x.Id == itemToAccept.TerrainId);

            Models.Object itemToAdd = new Models.Object()
            {
                Address = itemToAccept.Address,
                Photo = itemToAccept.Photo,
                District = itemToAccept.District,
                Type = itemToAccept.Type,
                Terrain = itemToAccept.Terrain,
                Light = itemToAccept.Light
            };

            // сначала добавляем в таблицу Objects, потом удаляем из таблицы SuggestedInfo
            context.Objects.Add(itemToAdd);
            context.SuggestedInfo.Remove(itemToAccept);
            context.SaveChanges();

            return Json(new
            {
                result = "success",
            });
        }

        public ActionResult Decline(string id)
        {
            SuggestedInfo itemToRemove = context.SuggestedInfo.First(x => x.Id == id);
            FileModel itemImageToRemove = context.Files.First(x => x.Id == itemToRemove.PhotoId);

            // кроме удаления записей из БД, необходимо удалить загруженную пользователем картинку
            string imagePath = appEnvironment.WebRootPath + "/images/suggested/" + itemImageToRemove.Name;
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            context.SuggestedInfo.Remove(itemToRemove);
            context.Files.Remove(itemImageToRemove);
            context.SaveChanges();

            return Json(new {
                result = "success",
            });
        }
    }
}