using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Thesis.Controllers
{
    public class MapController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

    }
}