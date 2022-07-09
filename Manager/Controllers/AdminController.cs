using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    public class AdminController : Controller
    {
        private pentonixEntities1 _db = new pentonixEntities1();

        // GET: Admin
        public ActionResult AdminDashboard()
        {
            return View();
        }



        public ActionResult AllUser()
        {

            return View(_db.Users.ToList());
        }

        public ActionResult TaskAssigned()
        {
            return RedirectToRoute(new { controller = "TasksAssign", action = "Index" });
        }
    }
}