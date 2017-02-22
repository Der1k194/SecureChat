using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalRMvc.App_Data;
using SignalRMvc.Hubs;

namespace SignalRMvc.Controllers
{
    public class HomeController : Controller
    {
        private OnlineChatEntities _db = new OnlineChatEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string username = "sdasd", string password = "asdasd")
        {
            Users user = new Users
            {
                Login = username,
                Password = password    
            };
            
           var a =  _db.Users.Where(x => x.Login == username && x.Password == password ).ToList();
            if (a.Count != 0)
            {
                return View("Index");
            }
            return View();
        }

        public string Registration(string username, string password)
        {
            Users user = new Users
            {
                Login = username,
                Password = password
            };
            return "sad";
        }
    }

}