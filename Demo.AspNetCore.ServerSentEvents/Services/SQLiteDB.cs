using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Demo.AspNetCore.ServerSentEvents.Services
{
    public class MySqliteDBContext : DbContext
    {
        public DbSet<WebSite> WebSites { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<dbContact> Contacts { get; set; }
        public dbContact searchCriteria = new dbContact();
        public List<dbContact> ContactPresentation { get; set; }
        public bool IsFilteredBySearch { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=blogging.db");
        }
        public MySqliteDBContext()
        {
            try
            {
                DoSearchQuery(null);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex);
            }
        }
        public MySqliteDBContext(DbContextOptions options)
            :base(options)
        {
            try
            {
                DoSearchQuery(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
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
        public List<dbContact> DoSearchQuery(dbContact criteria)
        {
            //string displayed_fields = null;
            //string value_fields = null;
            //bool NotEmptysearch = false;
            //StringBuilder sql = new StringBuilder("Select ");

            //if (!string.IsNullOrWhiteSpace(criteria.Full_Name))
            //{
            //    displayed_fields += displayed_fields != null ? ", Full_Name" : "Full_Name";
            //    value_fields += value_fields != null ? " and Full_Name like '%" + criteria.Full_Name+"%'" : " Full_Name like '%" + criteria.Full_Name + "%'";
            //    NotEmptysearch = true;
            //}
            //if (!string.IsNullOrWhiteSpace(criteria.Zip_Code))
            //{
            //    displayed_fields += displayed_fields != null ? ", Zip_Code" : "ZipCode";
            //    value_fields += value_fields != null ? " and Zip_Code like '%" + criteria.Zip_Code + "%'" : "Zip_Code like '%" + criteria.Zip_Code + "%'";
            //    NotEmptysearch = true;
            //}
            //if (!string.IsNullOrWhiteSpace(criteria.Country))
            //{
            //    displayed_fields += displayed_fields != null ? ", Country" : "Country";
            //    value_fields += value_fields != null ?" and Country like '%" +  criteria.Country + "%'" : " Country like '%" + criteria.Country + "%'";
            //    NotEmptysearch = true;
            //}
            //if(NotEmptysearch)
            //{
            //    sql.Append( "(" + displayed_fields + ") where " + value_fields);

            //}
           List<dbContact> outlist = new List<dbContact>();
            foreach (dbContact cnt in Contacts){
                if (criteria == null || cnt.Full_Name.Contains(criteria.Full_Name))
                    if (criteria == null ||(cnt.Zip_Code != null && cnt.Zip_Code.Contains(criteria.Zip_Code)))
                        if (criteria == null || (cnt.Country != null && cnt.Country.Contains(criteria.Country)))
                            outlist.Add(cnt);
            }
            ContactPresentation = outlist;
            return outlist;
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
