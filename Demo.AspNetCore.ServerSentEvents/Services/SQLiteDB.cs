using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Demo.AspNetCore.ServerSentEvents.Services
{
    public class MySqliteDBContext : DbContext
    {
        public DbSet<WebSite> WebSites { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=blogging.db");
        }

        public void InitializeDB()
        {
            WebSite myWebSite;
            try
            {
                myWebSite = new WebSite() { SiteId = 5, Url = "Https://gazeta.ru" };
                WebSites.Add(myWebSite);
               // SaveChangesAsync();
                myWebSite = new WebSite() { SiteId = 6, Url = "https://www.utro.ru" };
                WebSites.Add(myWebSite);
               // SaveChangesAsync();
                myWebSite = new WebSite() { SiteId = 7, Url = "https://www.w3schools.com" };
                WebSites.Add(myWebSite);
                //SaveChangesAsync();
                myWebSite = new WebSite() { SiteId = 8, Url = "Https://apple.com" };
                WebSites.Add(myWebSite);
                SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string error = ex.ToString();

            }

        }
    }
    [Table("WebSite")]
    public class WebSite
    {
        [Key]
        public int SiteId { get; set; }
        public string Url { get; set; }

        public List<Comment> Comments { get; set; }
    }
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int SiteId { get; set; }
    }

}
