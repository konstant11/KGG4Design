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
        public DbSet<dbContact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=blogging.db");
        }

        public void InitializeDB()
        {
         
            try
            {
                //this.FillDb(Contacts);
               

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

    [Table("Contacts")]
    public class dbContact
    {
        [Key]
        public int Id { get; set; }
        public string Full_Name { get; set; }
        public string Email { get; set; }
        public string Phone_Number { get; set; }
        public string Country { get; set; }
        public string Zip_Code { get; set; }
        public string Created_At { get; set; }
        public string Web_Site { get; set; }
    }

}
