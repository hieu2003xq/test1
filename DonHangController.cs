using appweb.help;
using appweb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace appweb.Controllers
{
    public class DonHangController : Controller
    {
        cart  cart =new cart();
        
        // GET: DonHang
        [HttpGet]
        public ActionResult Index(int id)
        {
          
            if (Session["gmail"] != null)
            {
                var key = Session["gmail"].ToString();
                qlBhEntities db = new qlBhEntities();
                List<gioHang>a=memory_cache.getValue(key) as List<gioHang>;

                if (a != null)
                {
                    gioHang sp = a.Find(m => m.maSP == id);
                    donHang b=db.donHang.Where(m=>m.maSP==sp.maSP).FirstOrDefault();
                    if (b != null)
                    {
                        b.soLuong++;
                        b.thanhtien = sp.giaBan * b.soLuong;
                        b.tongTien = b.thanhtien;
                        b.Gmail = key;
                        sp.soLuong--;
                        if(sp.soLuong == 0)
                        {
                            a.Remove(sp);
                        }
                    }
                    else
                    {
                        b=new donHang();
                        b.Gmail = key;
                        b.soLuong = 1;
                        b.maSP=sp.maSP;
                        b.thanhtien = sp.giaBan * b.soLuong;
                        b.tongTien = b.thanhtien;
                        sp.soLuong--;
                        if (sp.soLuong == 0)
                        {
                            a.Remove(sp);
                        }

                    }
                    db.donHang.AddOrUpdate(b);
                    db.SaveChanges();

                }
                
                return RedirectToAction("GioHang","gioHang");
            }
            return RedirectToAction("GioHang", "gioHang");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int id,string tenkh,string sdt,string diachi)
        {
            qlBhEntities db=new qlBhEntities();
            List<gioHang> a = cart.laygioHang();
            if (a != null)
            {
                DonHangRieng b = new DonHangRieng();
                gioHang sp = a.Find(m => m.maSP == id);
                if (b.maSP != sp.maSP)
                {
                    b.maSP = sp.maSP;
                    b.tenKH = tenkh;
                    b.SDT = sdt;
                    b.diaChi = diachi;
                    b.soLuong = 1;
                    b.thanhTien = sp.giaBan * b.soLuong;
                    b.tongTien = b.thanhTien;
                    sp.soLuong--;
                    if (sp.soLuong == 0)
                    {
                        a.Remove(sp);
                    }
                    db.DonHangRieng.Add(b);


                }
                else
                {
                    b.soLuong++;
                }
                db.SaveChanges();
            }

            return RedirectToAction("GioHang", "gioHang");
        }
        [HttpGet]
        public ActionResult thongtinDon(int id) 
        {
            if (Session["gmail"] == null)
            {
                donHang c=new donHang();
                c.soLuong = 1;
                List<gioHang> a = cart.laygioHang();
                gioHang b = a.Find(m => m.maSP == id);
                ViewBag.thanhtien = b.giaBan * c.soLuong;
                return View(b);
            }
            else
            {
                donHang c = new donHang();
                c.soLuong = 1;
                List<gioHang> a = cart.laygioHang(Session["gmail"].ToString()) ;
                gioHang b = a.Find(m => m.maSP == id);
                ViewBag.thanhtien = b.giaBan * c.soLuong;
                return View(b);
            }
       
        }
        public ActionResult DanhSachDon()
        {
            qlBhEntities db=new qlBhEntities();
            if (Session["gmail"] != null)
            {
                var key = Session["gmail"].ToString();
                List<donHang> a = db.donHang.Where(m => m.Gmail == key).ToList();
                if(a==null)
                {
                    a=new List<donHang>();
                }

               return View(a);
            }
            else
            {
                List<DonHangRieng> a=new List<DonHangRieng>();
            }
            return View();
        }
        
        

    }
}