using Manager.Models;
using System.Linq;
using System.Web.Mvc;

namespace Manager.Controllers
{
    public class EmpController : Controller
    {
        
        private pentonixEntities1 _db = new pentonixEntities1();
       
        [Authorize]
        public ActionResult EmpDashboard()
        {
           
            return View();
        }

        
        public ActionResult TodayAssigned()
        {
            var date = System.DateTime.Today;

            var name = Session["Name"].ToString();

            return View(_db.Tasks.Where(x => x.Name == name && x.Date == date));
        }

       
        public ActionResult Assigned()
        {
            var name = Session["Name"].ToString();
            var task = _db.Tasks.ToList();
            return View(_db.Tasks.Where(x => x.Name == name).ToList());
        }



      
    }

}