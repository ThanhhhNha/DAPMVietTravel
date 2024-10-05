using Antlr.Runtime;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class AccountController : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("DangNhap");
            }
            else
            {
                return RedirectToAction("TrangChu", "Home");
            }
        }

        public ActionResult DangNhap()
        {
            return View();
        }

        // POST: Account/DangNhap
        [HttpPost]
        public ActionResult DangNhap(string username, string password)
        {
            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ThongBao = "Tên đăng nhập và mật khẩu không được để trống";
                return View();
            }

            // Tìm user trong cơ sở dữ liệu
            var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Đăng nhập thành công - Lưu thông tin user vào Session
                Session["Account"] = user;

                // Điều hướng về trang chủ hoặc trang phù hợp
                return RedirectToAction("TrangChu", "Home");
            }
            else
            {
                // Đăng nhập thất bại
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
        }

        // GET: Account/DangXuat
        public ActionResult DangXuat()
        {
            // Xóa session khi người dùng đăng xuất
            Session["Account"] = null;
            return RedirectToAction("DangNhap", "Account");
        }
    
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(string tenUser, string username, string password, string email, string dienThoai)
        {
            // Kiểm tra thông tin nhập
            if (string.IsNullOrWhiteSpace(tenUser) || string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
            {
                TempData["error"] = "Vui lòng điền đầy đủ thông tin!";
                return View();
            }

            // Kiểm tra tên đăng nhập đã tồn tại
            var existingUser = db.Users.SingleOrDefault(x => x.Username.ToLower() == username.ToLower());
            if (existingUser != null)
            {
                TempData["error"] = "Tên đăng nhập đã tồn tại!";
                return View();
            }

            // Tạo mã người dùng
            string maUser = "U" + (db.Users.Count() + 1).ToString("D9"); // Tạo mã người dùng với độ dài tối đa là 10 ký tự


            // Tạo một đối tượng người dùng mới
            User newUser = new User
            {
                MaUser = maUser, // Gán mã người dùng
                TenUser = tenUser,
                Username = username,
                Password = password,
                Email = email,
                DienThoai = dienThoai
            };

            try
            {
                // Lưu đối tượng vào cơ sở dữ liệu
                db.Users.Add(newUser);
                db.SaveChanges();

                TempData["success"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay.";
                return RedirectToAction("DangNhap");
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        TempData["error"] += validationError.ErrorMessage + "<br/>";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Có lỗi xảy ra trong quá trình đăng ký: " + ex.Message;
                return View();
            }
        }

        public ActionResult ChinhSua(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua([Bind(Include = "MaUser,TenUser,Username,Password,Email,DienThoai")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ChinhSua", "Account", new { id = user.MaUser });
            }
            return View(user);
        }


        public ActionResult CaNhan(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            User user = db.Users.SingleOrDefault(u => u.MaUser == id);

            if (user == null)
            {
                return HttpNotFound();
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

