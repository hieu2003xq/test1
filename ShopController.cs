using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using appweb.Models;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using appweb.Controllers;
using PagedList;
using appweb.help;
using System.Web.WebPages;
using System.Linq.Dynamic;

namespace appweb.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        cart b = new cart();

        public ActionResult Index(int? page, int? pageSize)
        {
            qlBhEntities db = new qlBhEntities();
            List<sanPham> dsSP = db.sanPham.ToList();
            if (page == null) page = 1;
            if (pageSize == null) pageSize = 3;
            ViewBag.pagesize = pageSize;
            if (Session["gmail"] == null)
            {


                ViewBag.tongSL = b.tongSL();

            }
            else
            {
                ViewBag.tongSL = b.tongSL(Session["gmail"].ToString());
            }

            return View(dsSP.ToPagedList((int)page, (int)pageSize));
        }
        
    }
    }
