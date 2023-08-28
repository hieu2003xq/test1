using appweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using appweb.Controllers;
using appweb.help;
using System.Web.Services.Protocols;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using System.Web.Razor.Generator;

namespace appweb.Controllers
{
    public class gioHangController : Controller
    {
      qlBhEntities db=new qlBhEntities();   

        // GET: gioHang
        cart b=new cart();

        [HttpGet]
        public ActionResult themGioHang(int id)
        {
            qlBhEntities db = new qlBhEntities();
            if (Session["gmail"] != null)
            {
                var key = Session["gmail"].ToString();
                if (key != null)
                {
                    List<gioHang> a = b.laygioHang(key);
                    gioHang sp = a.Find(m => m.maSP == id);
                        if(sp != null)
                        {
                         sp.soLuong++;
                      }
                      else
                     {
                        sp = new gioHang(id);
                        a.Add(sp);

                    }memory_cache.add(key,a,DateTimeOffset.Now.AddDays(1));
                    return RedirectToAction("GioHang");
                }
                return RedirectToAction("GioHang");

            }
            else
            {
                List<gioHang> a = b.laygioHang();
                gioHang sp = a.Find(m => m.maSP == id);
                if (sp != null)
                {
                    sp.soLuong++;
                }
                else
                {
                    sp = new gioHang(id);
                    a.Add(sp);
                }
                return RedirectToAction("GioHang");
            }


        }
        [HttpGet]
        public ActionResult GioHang()
        {
            
            if (Session["gmail"] != null)
            {
                var key = Session["gmail"].ToString();

                if (key != null)
                {

                    List<gioHang> a = b.laygioHang(key);
                    ViewBag.tongtien =  b.tongTien(key);
                    ViewBag.tongSL = b.tongSL(key);
                    if (a == null)
                    {
                        a = new List<gioHang>();
                        memory_cache.add(key, a, DateTimeOffset.Now.AddDays(1));
                    }
                    else
                    {
                        return View(a);
                    }

                    return View(a);
                }

            }
            else
            {
                List<gioHang> a = b.laygioHang();
                ViewBag.gioHang = b.laygioHang();
                ViewBag.tongtien = b.tongTien();
                ViewBag.tongSL = b.tongSL();
                if (a.Count() == 0)
                {
                    RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(a);
                }
            }
            
            return View(); 
            


        }
        [HttpGet]
        public ActionResult xoaGioHang(int id)
        {
            
            qlBhEntities db = new qlBhEntities();
            if (Session["gmail"] != null)
            {
                var key = Session["gmail"].ToString();
                List<gioHang>a=memory_cache.getValue(key)as List<gioHang>;
                if(a != null)
                {
                    gioHang sp = a.Find(m => m.maSP == id);
                    if(sp != null)
                    {
                        sp.soLuong--;
                        if(sp.soLuong == 0)
                        {
                            a.Remove(sp);
                        }
                    }
                    memory_cache.add(key, a, DateTimeOffset.Now.AddDays(1));
                    return RedirectToAction("GioHang");

                }
                else
                {
                    memory_cache.delete(key);
                    return RedirectToAction("GioHang");
                }

            }
            else
            {
                List<gioHang> a = b.laygioHang();
                if (a != null)
                {
                    gioHang sp = a.Find(m => m.maSP == id);
                    if (sp != null)
                    {
                        sp.soLuong--;
                        if (sp.soLuong == 0)
                        {
                            a.Remove(sp);
                        }
                    }
                    return RedirectToAction("GioHang");
                }
            }
            return RedirectToAction("GioHang");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TimKiem(string ten)
        {
          
            qlBhEntities db = new qlBhEntities();
            Session["ten"] = ten;
            List<sanPham> sp = db.sanPham.Where(m => m.tenSP.ToLower().Contains(ten.ToLower()) == true).ToList();
            if (sp.Count > 0)
            {
                return View(sp);
            }
            else
            {
                TempData["kTimThay"] = "khong tim thay san pham";
               
                return RedirectToAction("GioHang");
            }

        }
    }             
    }
