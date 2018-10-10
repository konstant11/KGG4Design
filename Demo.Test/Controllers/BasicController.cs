using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Demo.AspNetCore.ServerSentEvents.Controllers;
using Demo.AspNetCore.ServerSentEvents.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Text;
using Newtonsoft.Json;

namespace Demo.Controllers
{
    public class TestBasicController
    {
        WebClient client { get; set; }
       
        BasicController controller { get; set; }
        ILogger log { get; set; }
        private readonly ITestOutputHelper _testOutputHelper;

        public TestBasicController(ITestOutputHelper _testOutputHelper)
        {
            this._testOutputHelper = _testOutputHelper;
            var optionsBuilder = new DbContextOptionsBuilder<MySqliteDBContext>();
            optionsBuilder.UseSqlite(@"Data Source=/Users/Konstantin/Desktop/Projects/Demo.Asp.Net/Demo.Test/bin/Debug/netcoreapp2.1/blogging.db");
            MySqliteDBContext db = new MySqliteDBContext(optionsBuilder.Options);
            controller = new BasicController(db);

        }
        [Fact]
        public void Test01_DBAccessReadContactsTable()
        {
            if (controller != null && controller.SQLite() is ViewResult vr)
            {
                _testOutputHelper.WriteLine("Check total records...");
                int numbers = ((MySqliteDBContext)vr.Model).ContactPresentation.Count;
                _testOutputHelper.WriteLine("Test returns Database with {0} contacts",numbers);
                Assert.True(numbers == 1001,"check todal number of db records in table 'Contacts'");
            }

        }
        [Fact]
        public void Test02_DBAccessSearchInContactsTable()
        {
            var request = new Mock<HttpRequest>();
            string rq = "{\"Full_Name\":\"ab\",\"Country\":\"\",\"Zip_Code\":\"\"}";
            int len = rq.Length + 1;
            byte[] bt = Encoding.ASCII.GetBytes(rq);
            request.Setup(s => s.Body).Returns(new MemoryStream(bt));
            //request.Object.Body.Read(Encoding.ASCII.GetBytes("first: \"sss\""),0,len);

            var context = new Mock<HttpContext>();
            context.SetupGet(x => x.Request).Returns(request.Object);

           
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = context.Object;


             if (controller != null && controller.SearchDB() is ViewResult vr)
                {
                    _testOutputHelper.WriteLine("Check total records...");
                    int numbers = ((MySqliteDBContext)vr.Model).ContactPresentation.Count;
                    _testOutputHelper.WriteLine("Test returns Database with {0} contacts", numbers);
                    Assert.True(numbers == 10, "check todal number of db records in table 'Contacts' after search for 'ab'");
                }

        }
        [Fact]
        public void Test03_ContactViewValidation()
        {
            if (controller != null && controller.Contact() is ViewResult vr)
            {
                _testOutputHelper.WriteLine("Check Contact View...");
                string domName = vr.ViewName;
                Assert.Equal("Contact", domName);
                _testOutputHelper.WriteLine("Test returns View named: {0} For MyContact page", domName);
                _testOutputHelper.WriteLine("Current Dir: {0} ", execBash("pwd", null));

            }

        }
        [Fact]
        public void Test04_AboutViewValidation()
        {
            if (controller != null && controller.About() is ViewResult vr)
            {
                _testOutputHelper.WriteLine("Check Contact View...");
                string domName = vr.ViewName;
                Assert.Equal("About", domName);
                _testOutputHelper.WriteLine("Test returns View named: {0} For About page", domName);
                _testOutputHelper.WriteLine("Current Dir: {0} ", execBash("pwd", null));

            }

        }
        [Fact]
        public void Test05_SQLiteDBAccessViewWebSitesTableAccessValidation()
        {
            if (controller != null && controller.SQLiteContacts() is ViewResult vr)
            {
                _testOutputHelper.WriteLine("Check SQLiteContacts ('Contacts' table) View...");
                string domName = vr.ViewName;
                Assert.Equal("SQLiteContacts", domName);
                _testOutputHelper.WriteLine("Test returns View named: {0} For DB Contacts table ", domName);
                _testOutputHelper.WriteLine("Current Dir: {0} ", execBash("pwd", null));

            }

        }

        string execBash(string cmd,string args)
        {
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = cmd;
            p.StartInfo.Arguments = args;
            p.Start();

            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }
        
    }
}
