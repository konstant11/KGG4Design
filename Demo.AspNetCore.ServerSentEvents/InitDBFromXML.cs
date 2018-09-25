using System;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using Demo.AspNetCore.ServerSentEvents.Services;

namespace Demo.AspNetCore.ServerSentEvents
{
    public static class InitDBFromXML
    {
    
        public static void  FillDb(this MySqliteDBContext db, DbSet<dbContact> dbSet)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load("Contacts.xml");

            XmlElement root = doc.DocumentElement;

            // This is the node we are looking for in the XML string
            XmlNodeList nodes = root.SelectNodes("//record");
            foreach (XmlNode node in nodes)
            {
                XmlNode fieldid = node.SelectSingleNode("Id");
                XmlNode fieldfname = node.SelectSingleNode("Full_Name");
                XmlNode fieldcountry = node.SelectSingleNode("Country");
                XmlNode fieldphonenumber = node.SelectSingleNode("Phone_Number");
                XmlNode fieldcreatedat = node.SelectSingleNode("Created_At");
                XmlNode fieldzipcode = node.SelectSingleNode("Zip_Code");
                XmlNode fieldwebsite = node.SelectSingleNode("Web_Site");
                XmlNode fieldemail = node.SelectSingleNode("Email");
                dbContact cnt = new dbContact()
                {
                    //Id = int.Parse(fieldid.InnerText),
                    Full_Name = fieldfname.InnerText,
                    Country = fieldcountry.InnerText,
                    Phone_Number = fieldphonenumber.InnerText,
                    Created_At = fieldcreatedat.InnerText,
                    Zip_Code = fieldzipcode.InnerText,
                    Email = fieldemail.InnerText
                };
                dbSet.Add(cnt);
                db.SaveChanges();
            }

        }
    }
}
