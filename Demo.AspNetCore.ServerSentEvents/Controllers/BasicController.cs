using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Demo.AspNetCore.ServerSentEvents.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo.AspNetCore.ServerSentEvents.Controllers
{
    public class BasicController : Controller
    {
        MySqliteDBContext SqliteDBContext;
        public BasicController(MySqliteDBContext sqliteDBContext)
        {
            SqliteDBContext = sqliteDBContext;
            SqliteDBContext.InitializeDB();
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

        [ActionName("ContactsDB")]
        [AcceptVerbs("GET")]
        public IActionResult SQLiteContacts()
        {
             SqliteDBContext.searchCriteria.Full_Name = "";
            return View("SQLiteContacts", SqliteDBContext);
        }

        [ActionName("searchDB")]
        [AcceptVerbs("POST")]
        public IActionResult SearchDB()
        {
            Stream req = Request.Body;
            //req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            dbContact criteria = JsonConvert.DeserializeObject<dbContact>(json);
            SqliteDBContext.DoSearchQuery(criteria);
            SqliteDBContext.IsFilteredBySearch = true;
            SqliteDBContext.searchCriteria = criteria;
            return View("SQLiteContacts", SqliteDBContext);
        }

        [ActionName("AddContact")]
        [AcceptVerbs("POST")]
        public IActionResult AddContact(MySqliteDBContext context)
        {
            Stream req = Request.Body;
            //req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            dbContact criteria = JsonConvert.DeserializeObject<dbContact>(json);
            SqliteDBContext.searchCriteria = criteria;
            //return View("SQLiteNewContacts", SqliteDBContext);
            dbContact cnt = SqliteDBContext.searchCriteria;
            return View("SQLiteNewContacts", cnt);
        }

        [ActionName("register-contact")]
        [AcceptVerbs("POST")]
        public IActionResult RegisterContact(dbContact cnt)
        {
            MySqliteDBContext context = new MySqliteDBContext(); 
            SqliteDBContext.AddContactRecord(cnt);
            context.searchCriteria = cnt;
            return View("SQLiteContacts", SqliteDBContext);
        }

        //[ActionName("AddContact")]
        //[AcceptVerbs("Get")]
        //public IActionResult AddContact()
        //{
        //    Stream req = Request.Body;
        //    //req.Seek(0, System.IO.SeekOrigin.Begin);
        //    string json = new StreamReader(req).ReadToEnd();
        //    dbContact criteria = JsonConvert.DeserializeObject<dbContact>(json);
        //    SqliteDBContext.searchCriteria = criteria;
        //    return View("SQLiteNewContacts", SqliteDBContext);
        //}

        [ActionName("Geolocation")]
        [AcceptVerbs("GET")]
        public IActionResult GeoLocation()
        {
            return View("GeoLocation");
        }

    }
}
