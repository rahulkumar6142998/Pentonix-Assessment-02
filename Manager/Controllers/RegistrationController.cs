using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    public class RegistrationController : Controller
    {
        pentonixEntities1 _db = new pentonixEntities1();
        // GET: Registration
        public ActionResult Registration()
        {
            var model = new User();
            return View(model);
        }
        [HttpPost]
        public ActionResult Registration(User registration)
        {

            if (ModelState.IsValid)
            {
                var check = _db.Users.FirstOrDefault(s => s.Email == registration.Email);
                if (check == null)
                {
                    registration.Password = registration.Password;
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.Users.Add(registration);
                    _db.SaveChanges();

                }
                else
                {
                    ViewBag.error = "Email already exists";
                    var model = new User();
                    return View(model);
                }


            }
            return RedirectToRoute(new { controller = "Login", action = "Login" });
        }
    }
}