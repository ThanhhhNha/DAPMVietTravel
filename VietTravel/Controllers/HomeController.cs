using System;
using System.Collections.Generic;
using System.Linq;
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


        public ActionResult Search(string MaTinh)
        {
            if (MaTinh == null)
            {
                return View("Error", new { message = "Vui lòng chọn tỉnh thành." });
            }

            // Lấy danh sách khách sạn theo mã tỉnh thành
            var hotels = GetHotelsByMaTinh(MaTinh);

            return View("ListHotels", hotels);  // Hiển thị danh sách khách sạn
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
    }
}
