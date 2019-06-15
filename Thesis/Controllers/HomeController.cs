using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Thesis.Models;
using Thesis.Data;

namespace Thesis.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context;

        public HomeController (ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public void InitData()
        {
            var filePath = @"C:\Users\Nekit\Desktop\ВКР\Data init\uploaddata.xlsx";
            FileInfo file = new FileInfo(filePath);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;
                int columnPhoto = 1;
                int columnAddress = 2;
                int columnDistrict = 3;
                int columnType = 4;
                int columnTerrain = 5;
                int columnLight = 6;

                List<Models.Object> typesInit = new List<Models.Object>();
                for (int row = 2; row <= rowCount; row++) // со второй строки
                {
                    bool light = true;
                    if (worksheet.Cells[row, columnLight].Value.ToString() == "false")
                    {
                        light = false;
                    }
                    string photoId = worksheet.Cells[row, columnPhoto].Value.ToString();
                    FileModel photo = new FileModel()
                    {
                        Name = photoId,
                        Path = "http://op.mos.ru/MEDIA/showFile?id=" + photoId
                    };
                    context.Files.Add(photo);
                    context.SaveChanges();

                    Models.Object typeToInit = new Models.Object()
                    {
                        Photo = photo,
                        Address = worksheet.Cells[row, columnAddress].Value.ToString(),
                        District = context.Districts.First(x => x.Id == worksheet.Cells[row, columnDistrict].Value.ToString()),
                        Type = context.ObjectTypes.First(x => x.Id == worksheet.Cells[row, columnType].Value.ToString()),
                        Terrain = context.Terrains.First(x => x.Id == worksheet.Cells[row, columnTerrain].Value.ToString()),
                        Light = light

                    };
                    typesInit.Add(typeToInit);

                }

                context.Objects.AddRange(typesInit);
                context.SaveChanges();
            }
        }
    }
}
