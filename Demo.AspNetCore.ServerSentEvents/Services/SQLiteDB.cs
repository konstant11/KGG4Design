using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public MySqliteDBContext(DbContextOptions options)
            : base(options)
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

        public void AddContactRecord(dbContact record)
        {
            Contacts.Add(record);
            SaveChanges();
        }

        public void UpdateContactRecord(dbContact record)
        {
            var entity = Contacts.Find(record.Id);
            entity.Full_Name = record.Full_Name;
            entity.Avatar = record.Avatar;
            entity.Country = record.Country;
            entity.Created_At = record.Created_At;
            entity.DeviceFingerPrint = record.DeviceFingerPrint;
            entity.Email = record.Email;
            entity.Phone_Number = record.Phone_Number;
            entity.Web_Site = record.Web_Site;
            entity.Zip_Code = record.Zip_Code;
            Contacts.Update(entity);
            SaveChanges();
        }

        public void DeleteContactRecord(int id)
        {
            Contacts.Remove(Contacts.Find(id));
            SaveChanges();

        }

    public List<dbContact> DoSearchQuery(dbContact criteria)
        {
           List<dbContact> outlist = new List<dbContact>();
            foreach (dbContact cnt in Contacts){
                if (criteria == null ||(cnt.Full_Name != null && cnt.Full_Name.Contains(criteria.Full_Name, StringComparison.InvariantCultureIgnoreCase)))
                if (criteria == null ||(cnt.Zip_Code != null && cnt.Zip_Code.Contains(criteria.Zip_Code, StringComparison.InvariantCultureIgnoreCase)))
                if (criteria == null || (cnt.Country != null && cnt.Country.Contains(criteria.Country,StringComparison.InvariantCultureIgnoreCase)))
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
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string Full_Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone_Number { get; set; }
        public string Country { get; set; }
        [DataType(DataType.PostalCode)]
        public string Zip_Code { get; set; }
        [DataType(DataType.DateTime)]
        public string Created_At { get; set; }
        [DataType(DataType.Url)]
        public string Web_Site { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string Avatar { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string DeviceFingerPrint { get; set; }
    }

}
