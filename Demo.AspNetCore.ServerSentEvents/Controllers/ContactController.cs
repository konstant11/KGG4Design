using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Demo.AspNetCore.ServerSentEvents.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo.AspNetCore.ServerSentEvents.Controllers
{
    public class ContactController : Controller
    {
        MySqliteDBContext SqliteDBContext;
        public ContactController(MySqliteDBContext sqliteDBContext)
        {
            SqliteDBContext = sqliteDBContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [ActionName("show-page-edit-contact")]
        [AcceptVerbs("POST")]
        public IActionResult EditContact()
        {
            Stream req = Request.Body;
            //req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            dbContact criteria = JsonConvert.DeserializeObject<dbContact>(json);
            dbContact res = SqliteDBContext.Contacts.Find(criteria.Id);
            return View("EditContact", res);
        }

        [ActionName("update-contact")]
        [AcceptVerbs("POST")]
        public IActionResult UpdateContact(dbContact cnt)
        {
            SqliteDBContext.UpdateContactRecord(cnt);
            SqliteDBContext.searchCriteria = cnt;
            SqliteDBContext.ContactPresentation = SqliteDBContext.DoSearchQuery(cnt);            //return View("/Basic/SQLiteContacts", SqliteDBContext);
            return View("~/Views/Basic/SQLiteContacts.cshtml",SqliteDBContext);
        }

        [ActionName("delete-contact")]
        [AcceptVerbs("POST")]
        public IActionResult DeleteContact()
        {
            Stream req = Request.Body;
            //req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            dbContact criteria = JsonConvert.DeserializeObject<dbContact>(json);
            dbContact record = SqliteDBContext.Contacts.Find(criteria.Id);
           // Contract.Ensures(Contract.Result<IActionResult>() != null);
            MySqliteDBContext context = new MySqliteDBContext();
            SqliteDBContext.DeleteContactRecord(record.Id);
            context.searchCriteria = record;
            return View("~/Views/Basic/SQLiteContacts.cshtml",SqliteDBContext);
        }

    }
}
