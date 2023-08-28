using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using appweb.help;
using appweb.Models;


namespace appweb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
      cart b=new cart();
      
        qlBhEntities db = new qlBhEntities();
        home md = new home();
       
        public ActionResult Index()
        {
            if (Session["gmail"] == null)
            {
                
             
                ViewBag.tongSL = b.tongSL();

            }
            else
            {
                ViewBag.tongSL = b.tongSL(Session["gmail"].ToString());
            }
            md.sanPham = db.sanPham.ToList();
            md.hsx=db.HSX.ToList();
            return View(md);
        }
     


    }
}