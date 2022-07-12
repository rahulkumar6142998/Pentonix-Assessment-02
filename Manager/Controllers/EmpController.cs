using Manager.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Manager.Controllers
{
    public class EmpController : Controller
    {
        
        private pentonixEntities1 _db = new pentonixEntities1();
        static string Id;
       
        
        public ActionResult EmpDashboard()
        {
            
            return View();
        }

        
        public ActionResult TodayAssigned()
        {
            var date = System.DateTime.Today;

            var email = Session["Email"].ToString();
            var name = Session["Name"].ToString();

            if (Session["Email"] == null)
            {
                Session.Clear();
                return RedirectToRoute(new { controller = "Login", action = "Login" });

            }
           

            
            
                return View(_db.Tasks.Where(x => x.Name == name && x.Date == date));
            
            
        }

       
        public ActionResult Assigned()
        {
            if (Session["Email"] == null)
            {
                Session.Clear();
                return RedirectToRoute(new { controller = "Login", action = "Login" });

            }
            var name = Session["Name"].ToString();
            var task = _db.Tasks.ToList();
            return View(_db.Tasks.Where(x => x.Name == name).ToList());
        }





        public ActionResult EditComment(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task assignedTask = _db.Tasks.Find(id);
            Id = id;

            ViewBag.Id = id;
            if (assignedTask == null)
            {
                return HttpNotFound();
            }
            return View(assignedTask);
            
        }
        [HttpPost]
        
        public ActionResult EditComment(string comment , string status)
        {
            var id = Id;
            var update = _db.Tasks.Where(x => x.TicketNo == id).FirstOrDefault();
            update.Comment = comment;
            update.TaskStatus = status;

            if (ModelState.IsValid)
            {
                _db.Entry(update).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("TodayAssigned");
            }
            return View();
        }


    }

}