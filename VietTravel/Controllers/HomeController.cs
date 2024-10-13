using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class HomeController : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        public ActionResult TrangChu()
        {
            // Lấy danh sách tỉnh thành từ database
            var tinhList = db.TinhThanhs.ToList();

            // Kiểm tra danh sách có dữ liệu hay không
            if (tinhList == null || !tinhList.Any())
            {
                return View("Error", new { message = "Không có dữ liệu tỉnh thành." });
            }

            ViewBag.TinhList = tinhList;  

            return View();
        }


        public ActionResult Search(string MaTinh, DateTime? checkIn, DateTime? checkOut)
        {
            
            var tinhList = db.TinhThanhs.ToList();
            ViewBag.TinhList = tinhList;

            
            var selectedProvince = db.TinhThanhs.FirstOrDefault(t => t.MaTinh == MaTinh);
            ViewBag.SelectedMaTinh = MaTinh; 
            ViewBag.ProvinceName = selectedProvince != null ? selectedProvince.TenTinh : "Chưa chọn tỉnh";


            ViewBag.CheckIn = checkIn?.ToString("yyyy-MM-dd") ?? string.Empty; 
            ViewBag.CheckOut = checkOut?.ToString("yyyy-MM-dd") ?? string.Empty;


            var hotels = db.Hotels
                           .Where(h => h.MaTinh == MaTinh)
                           .ToList();

            return View("DanhSachKhachSan", hotels);
        }





        public IEnumerable<Hotel> GetHotelsByMaTinh(string MaTinh)
        {
            // Truy vấn khách sạn theo mã tỉnh
            return db.Hotels.Where(h => h.MaTinh == MaTinh).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose(); 
            }
            base.Dispose(disposing);
        }
        public ActionResult ChiTietKhachSan(string id, DateTime? checkIn, DateTime? checkOut)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Hotel hotel = db.Hotels
                 .Include("Phongs.LoaiPhong")
                 .Include("Phongs.TrangThaiPhong")
                 .FirstOrDefault(h => h.MaKhachSan == id);

            if (hotel == null)
            {
                return HttpNotFound();
            }
            ViewBag.CheckIn = checkIn?.ToString("yyyy-MM-dd") ?? string.Empty;
            ViewBag.CheckOut = checkOut?.ToString("yyyy-MM-dd") ?? string.Empty;
            return View(hotel);
        }
    }
}
