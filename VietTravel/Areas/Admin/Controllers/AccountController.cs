using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VietTravel.Models;

namespace VietTravel.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        // GET: Admin/Account
        public ActionResult AdminDangNhap()
        {
            return View();
        }


        public ActionResult AdminTrangChu()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("AdminDangNhap");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        public ActionResult DangXuat()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("AdminDangNhap", "Account", new { area = "Admin" });
        }

        [HttpPost]
        public ActionResult AdminDangNhap(string username, string password)
        {
            var admin = db.Admins.SingleOrDefault(x => x.Username.ToLower() == username.ToLower() && x.Passwords == password);

            if (admin != null)
            {
                Session["user"] = admin; 
                Session["userName"] = admin.Username; 
                return RedirectToAction("AdminTrangChu", "Account", new { area = "Admin" });
            }
            else
            {
                TempData["error"] = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return RedirectToAction("AdminDangNhap", "Account", new { area = "Admin" });
            }
        }
        public ActionResult AdminCaNhan(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VietTravel.Models.Admin admin = db.Admins.SingleOrDefault(u => u.MaAdmin == id);

            if (admin == null)
            {
                return HttpNotFound();
            }

            return View(admin);
        }
        public ActionResult ChinhSua(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VietTravel.Models.Admin admin  = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua([Bind(Include = "MaUser,TenUser,Username,Password,Email,DienThoai")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CaNhan", "Account", new { id = user.MaUser });
            }
            return View(user);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

