using System;
using System.Collections.Generic;
using System.Linq;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class TourService
    {
        private readonly TravelVNEntities _db;

        public TourService(TravelVNEntities db)
        {
            _db = db;
        }

        // Phương thức tìm kiếm tour
        public List<Tour> SearchTours(string budget, string departure, string destination, DateTime? departureDate, string[] tourType, string[] transport)
        {
            // Sử dụng đúng tên DbSet là Tours
            var tours = _db.Tours.Include("LoaiTour").Include("TinhThanh").AsQueryable();

            // Lọc theo ngân sách
            if (!string.IsNullOrEmpty(budget))
            {
                switch (budget)
                {
                    case "Duoi5Trieu":
                        tours = tours.Where(t => t.Gia < 5000000);
                        break;
                    case "5-10Trieu":
                        tours = tours.Where(t => t.Gia >= 5000000 && t.Gia <= 10000000);
                        break;
                    case "10-20Trieu":
                        tours = tours.Where(t => t.Gia >= 10000000 && t.Gia <= 20000000);
                        break;
                    case "Tren20Trieu":
                        tours = tours.Where(t => t.Gia > 20000000);
                        break;
                }
            }

            // Lọc theo điểm khởi hành (departure)
            if (!string.IsNullOrEmpty(departure))
            {
                tours = tours.Where(t => t.TinhThanh.TenTinh == departure);
            }

            // Lọc theo điểm đến (destination)
            if (!string.IsNullOrEmpty(destination))
            {
                tours = tours.Where(t => t.TinhThanh.TenTinh == destination);
            }

            // Lọc theo ngày khởi hành (departureDate)
            if (departureDate.HasValue)
            {
                tours = tours.Where(t => t.NgayKhoiHanh >= departureDate.Value);
            }

            // Lọc theo loại tour (tourType)
            if (tourType != null && tourType.Length > 0)
            {
                tours = tours.Where(t => tourType.Contains(t.LoaiTour.MaLoaiTour));
            }

            // Lọc theo phương tiện (transport) - nếu có cột PhuongTien trong bảng Tour


            return tours.ToList(); // Trả về danh sách các tour sau khi lọc
        }

    }
}