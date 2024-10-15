using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class TravelController : Controller
    {
        private readonly TravelVNEntities db = new TravelVNEntities();

        // Phương thức trả về danh sách các tour du lịch
        public ActionResult DuLichTravel()
        {
            try
            {
                // Lấy danh sách tour, tỉnh thành, và loại tour từ cơ sở dữ liệu
                var tours = db.Tours
               .Include("TinhThanh")  // Sử dụng tên chuỗi
               .Include("LoaiTour")   // Sử dụng tên chuỗi
               .ToList();

                var tinhThanhList = db.TinhThanhs.ToList();
                var loaiTourList = db.LoaiTours.ToList(); // Lấy danh sách loại tour

                // Kiểm tra nếu không có dữ liệu
                if (!tours.Any() || !tinhThanhList.Any() || !loaiTourList.Any())
                {
                    return View("Error"); // Trả về trang lỗi nếu không có dữ liệu
                }

                // Tạo ViewModel và truyền dữ liệu vào model
                var model = new SearchViewModel
                {
                    Tours = tours,
                    TinhThanhList = tinhThanhList,
                    LoaiTourList = loaiTourList // Truyền danh sách loại tour vào ViewModel
                };

                return View(model); // Trả về view với model chứa dữ liệu
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }



        // Phương thức trả về chi tiết tour theo MaTour
        public ActionResult TourDetail(string maTour)
        {
            try
            {
                using (var db = new TravelVNEntities())
                {
                    var tour = db.Tours.FirstOrDefault(t => t.MaTour == maTour); // Tìm tour theo MaTour
                    if (tour == null)
                    {
                        return HttpNotFound(); // Trả về lỗi 404 nếu không tìm thấy tour
                    }
                    return View(tour); // Trả về view chi tiết tour
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về trang lỗi
                return View("Error");
            }
        }

        // Phương thức trả về kết quả tìm kiếm dựa trên các tiêu chí
        public ActionResult SearchResults(string budget, string departure, string destination, DateTime? departureDate, string[] tourType, string[] transport)
        {
            try
            {
                using (var db = new TravelVNEntities())
                {
                    var tourService = new TourService(db);
                    var tours = tourService.SearchTours(budget, departure, destination, departureDate, tourType ?? new string[] { }, transport ?? new string[] { });
                    return View(tours); // Trả về kết quả tìm kiếm tại view SearchResults
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi tại đây và trả về trang lỗi nếu có lỗi xảy ra
                return View("Error");
            }
        }

        // Phương thức xử lý tìm kiếm từ form và trả về kết quả
        [HttpPost]
        public ActionResult Search()
        {
            try
            {
                using (var db = new TravelVNEntities())
                {
                    // Lấy danh sách tỉnh thành từ cơ sở dữ liệu
                    var tinhThanhList = db.TinhThanhs.ToList();

                    // Kiểm tra nếu danh sách không có dữ liệu
                    if (tinhThanhList == null || !tinhThanhList.Any())
                    {
                        // Nếu không có tỉnh thành nào, trả về lỗi hoặc thông báo
                        return View("Error");
                    }

                    // Tạo model và truyền danh sách tỉnh thành vào
                    var model = new SearchViewModel
                    {
                        TinhThanhList = tinhThanhList
                    };

                    return View(model); // Trả về view với model chứa dữ liệu
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return View("Error");
            }
        }
        public ActionResult LichKhoiHanh(string maTour, int month, int year)
        {
            using (var db = new TravelVNEntities())
            {
                // Lấy danh sách ngày khởi hành dựa trên tháng và năm
                var ngayKhoiHanhList = db.Tours
                    .Where(t => t.MaTour == maTour && t.NgayKhoiHanh.Month == month && t.NgayKhoiHanh.Year == year)
                    .Select(t => new {
                        NgayKhoiHanh = t.NgayKhoiHanh,
                        Gia = t.Gia
                    }).ToList();

                return Json(ngayKhoiHanhList, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetTourDetails(string maTour, int day, int month, int year)
        {
            using (var db = new TravelVNEntities())
            {
                var tourDetail = db.TourDetails
                    .FirstOrDefault(td => td.MaTour == maTour && td.NgayBatDau.Day == day && td.NgayBatDau.Month == month && td.NgayBatDau.Year == year);

                if (tourDetail != null)
                {
                    return Json(new
                    {
                        phuongTien = "Phương tiện ví dụ",
                        ngayDi = tourDetail.NgayBatDau.ToString("dd/MM/yyyy"),
                        ngayKetThuc = tourDetail.NgayKetThuc.ToString("dd/MM/yyyy"),
                        soLuongNguoi = tourDetail.SoLuongNguoi
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(null, JsonRequestBehavior.AllowGet); // Không tìm thấy
            }
        }


    }
}
