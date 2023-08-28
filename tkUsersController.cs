using appweb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using appweb.help;
using System.Security.Cryptography;

namespace appweb.Controllers
{
    public class tkUsersController : Controller
    {
        checkGmail check1=new checkGmail();
       
        public ActionResult DangKy() { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(DangKy model) { 
            qlBhEntities db=new qlBhEntities();
            
            var check = db.tkUsers.Where(x => x.tenDN == model.tenDN || x.Gmail == model.Gmail);
            ViewBag.tk = model.tenDN;
           
            ViewBag.Pass=model.Pass;
            if (check.Count() == 1)
            {
                TempData["error"] = "tk da ton tai";
                return View();
            }
            else 
            {
                if (check1.ktra(model.Gmail) == true)
                {


                    tkUsers a = new tkUsers();
                    if (model.Pass == model.RefestPass)
                    {
                        maHoaMD5 md5 = new maHoaMD5();
                        string passmaHoa = maHoaMD5.ChuyenVao(model.Pass+"@-xy");
                        a.tenDN = model.tenDN;
                        a.Gmail = model.Gmail;
                        a.Pass = passmaHoa;
                        db.tkUsers.Add(a);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["checkemail"] = "Gmail nhập không đúng kiểu";
                    if (model.Pass != model.RefestPass)
                    {
                        TempData["pass"] = "Mật khẩu nhập lại không trùng với mật khẩu đã nhập";
                        return View();
                    }
                        return View();
                }
                
                
            }
            
            
        }
        public ActionResult DangNhap() { return View(); }
        [HttpPost]
        [ValidateAntiForgeryToken]
 public ActionResult DangNhap(string user, string password)
        {

            qlBhEntities db = new qlBhEntities();
            var a = HttpUtility.HtmlEncode(user);
            var b=HttpUtility.HtmlEncode(password);
            var check=db.tkUsers.Where(m=>m.tenDN==a && m.Pass==b);
            if (check.Count() > 0)
            {
                TempData["t"] = a;
                return View();
            }
            else
            {
                TempData["x"] = b;
                return View();
            }
        }
        public ActionResult DangXuat() 
        {
            Session.Remove("user");
            Session.Remove("gmail");
            FormsAuthentication.SignOut();
            return RedirectToAction("DangNhap"); 
        }
      
        

    }
}