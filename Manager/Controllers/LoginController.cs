using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        pentonixEntities1 _db = new pentonixEntities1();

        [HttpGet]
        public ActionResult Login()
        {
            LoginModel model = new LoginModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {


                var f_password = login.Password;
                var data = _db.Users.Where(s => s.Email.Equals(login.Email) && s.Password.Equals(f_password)).ToList();



                if (data.Count() > 0)
                {
                    ViewData["Message"] = "Success";
                    if (data[0].Type == "Admin")
                    {
                        
                        Session["Email"] = data.FirstOrDefault().Email;
                        ViewBag.Name = data[0].Email;
                        return RedirectToRoute(new { controller = "Admin", action = "AdminDashboard" });

                    }
                    else
                    {
                        Session["Email"] = data.FirstOrDefault().Email;
                        ViewBag.Name = data[0].Email;
                        return RedirectToRoute(new { controller = "Emp", action = "EmpDashboard" });
                    }
                }
                else
                {
                    ViewData["Message"] ="Fail";
                    return RedirectToAction("Login", login);
                }
            }
            return View(login);
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

       

    }
}