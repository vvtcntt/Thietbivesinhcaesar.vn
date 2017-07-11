using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Caesar.Models;
namespace Caesar.Controllers.Admin
{
    public class AdminController : Controller
    {
        CaesarContext db = new CaesarContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult partialBanner()
        {
            ViewBag.donhang = db.tblOrders.Where(p => p.Status == false && p.Active==true).ToList().Count;
            return PartialView();
        }
    }
}