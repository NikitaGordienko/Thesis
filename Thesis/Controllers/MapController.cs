using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Thesis.Data;
using Thesis.Models;

namespace Thesis.Controllers
{
    public class MapController : Controller
    {

        ApplicationDbContext context;

        public MapController(ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ActionResult Index()
        {

            Models.Object obj1 = new Models.Object
            {
                Address = "55.524980, 37.559807",
                District = context.Districts.First(t => t.Name == "Проспект Вернадского"),
                Photo = context.Files.FirstOrDefault(t => t.Id == "21d645dc-e902-4a72-8017-4c5d05a1e1d4"),
                Type = context.ObjectTypes.First(j => j.Name == "Для игровых видов спорта"),
                Terrain = context.Terrains.First(ee => ee.Name == "Газон"),
                Light = true
            };
            Models.Object obj2 = new Models.Object
            {
                Address = "55.689824, 37.498785",
                District = context.Districts.First(t => t.Name == "Раменки"),
                Photo = context.Files.FirstOrDefault(t => t.Id == "21d645dc-e902-4a72-8017-4c5d05a1e1d4"),
                Type = context.ObjectTypes.First(j => j.Name == "Для занятий воркаутом"),
                Terrain = context.Terrains.First(ee => ee.Name == "Асфальт"),
                Light = false
            };
            Models.Object obj3 = new Models.Object
            {
                Address = "55.647956, 37.497046",
                District = context.Districts.First(t => t.Name == "Теплый Стан"),
                Photo = context.Files.FirstOrDefault(t => t.Id == "21d645dc-e902-4a72-8017-4c5d05a1e1d4"),
                Type = context.ObjectTypes.First(j => j.Name == "Скейт-парк"),
                Terrain = context.Terrains.First(ee => ee.Name == "Асфальт"),
                Light = false
            };
            Models.Object obj4 = new Models.Object
            {
                Address = "55.655559, 37.489408",
                District = context.Districts.First(t => t.Name == "Теплый Стан"),
                Photo = context.Files.FirstOrDefault(t => t.Id == "21d645dc-e902-4a72-8017-4c5d05a1e1d4"),
                Type = context.ObjectTypes.First(j => j.Name == "Для занятий воркаутом"),
                Terrain = context.Terrains.First(ee => ee.Name == "Резиновая крошка"),
                Light = true
            };
            Models.Object obj5 = new Models.Object
            {
                Address = "55.674052, 37.483163",
                District = context.Districts.First(t => t.Name == "Проспект Вернадского"),
                Photo = context.Files.FirstOrDefault(t => t.Id == "21d645dc-e902-4a72-8017-4c5d05a1e1d4"),
                Type = context.ObjectTypes.First(j => j.Name == "Для игровых видов спорта"),
                Terrain = context.Terrains.First(ee => ee.Name == "Газон"),
                Light = false
            };


            List<Models.Object> objs = new List<Models.Object>
            {
                obj1,
                obj2,
                obj3,
                obj4,
                obj5
            };
            //context.Objects.AddRange(objs);
            //context.SaveChanges();

            //var objects = context.Objects.ToList();
            //foreach (var item in objects)
            //{
            //    item.Photo = context.Files.First(t => t.Id == item.PhotoId);
            //}

            //return View(objects);
            return View();
        }

    }
}