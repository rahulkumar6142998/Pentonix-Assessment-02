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


                var password = login.Password;
                var check = _db.Users.Where(s => s.Email.Equals(login.Email) && s.Password.Equals(password)).ToList();



                if (check.Count() > 0)
                {
                    ViewData["Message"] = "Success";
                    if (check[0].Type == "Admin")
                    {
                        
                        Session["Email"] = check.FirstOrDefault().Email;
                        Session["Name"] = check.FirstOrDefault().FirstName;
                        ViewBag.Name = check[0].FirstName;
                       
                        //  return RedirectToRoute(new { controller = "Admin", action = "AdminDashboard" });
                        return View("~/Views/Admin/AdminDashboard.cshtml");

                    }
                    else
                    {
                        Session["Email"] = check.FirstOrDefault().Email;
                        Session["Name"] = check.FirstOrDefault().FirstName;
                        ViewBag.Name = check[0].FirstName;
                        

                        //return RedirectToRoute(new { controller = "Emp", action = "EmpDashboard" });
                        return View("~/Views/Emp/EmpDashboard.cshtml");

                    }
                }
                else
                {
                    ViewData["Message"] ="Fail";
                    return View("Login", login);
                }
            }
            return View(login);
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToRoute(new { controller = "Login", action = "Login" });
        }

       

    }
}