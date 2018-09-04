using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.AspNetCore.ServerSentEvents.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo.AspNetCore.ServerSentEvents.Controllers
{
    public class BasicController : Controller
    {
        MySqliteDBContext SqliteDBContext;
        public BasicController(MySqliteDBContext sqliteDBContext)
        {
            SqliteDBContext = sqliteDBContext;
           // SqliteDBContext.InitializeDB();
        }
        // GET: /<controller>/
        [ActionName("index")]
        [AcceptVerbs("GET")]
        public IActionResult Index()
        {
            return View();
        }
        [ActionName("Contact")]
        [AcceptVerbs("GET")]
        public IActionResult Contact()
        {
            return View("Contact");
        }
        [ActionName("About")]
        [AcceptVerbs("GET")]
        public IActionResult About()
        {
            return View("About");
        }
        [ActionName("SQLiteDBAccess")]
        [AcceptVerbs("GET")]
        public IActionResult SQLite()
        {
            return View("SQLite", SqliteDBContext);
        }
        [ActionName("Geolocation")]
        [AcceptVerbs("GET")]
        public IActionResult GeoLocation()
        {
            return View("GeoLocation");
        }

    }
}
