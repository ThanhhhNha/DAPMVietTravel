using System;
using System.Collections.Generic;
using System.Linq;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class TourService
    {
        private readonly TravelVNEntities _db;

        // Constructor để khởi tạo với database context
        public TourService(TravelVNEntities db)
        {
            _db = db;
        }

        // Phương thức tìm kiếm tour dựa trên các tiêu chí
        public IEnumerable<Tour> SearchTours(string budget, string departure, string destination,
            DateTime? departureDate, string[] tourType, string[] transport)
        {
            var query = _db.Tours.AsQueryable();

            // Lọc theo ngân sách
            if (!string.IsNullOrEmpty(budget))
            {
                switch (budget)
                {
                    case "Duoi5Trieu":
                        query = query.Where(t => t.Gia < 5000000);
                        break;
                    case "5-10Trieu":
                        query = query.Where(t => t.Gia >= 5000000 && t.Gia <= 10000000);
                        break;
                    case "10-20Trieu":
                        query = query.Where(t => t.Gia >= 10000000 && t.Gia <= 20000000);
                        break;
                    case "Tren20Trieu":
                        query = query.Where(t => t.Gia > 20000000);
                        break;
                }
            }

            // Lọc theo điểm khởi hành
            if (!string.IsNullOrEmpty(departure))
            {
                query = query.Where(t => t.TinhThanh.TenTinh == departure);
            }

           

            // Lọc theo ngày khởi hành
            if (departureDate.HasValue)
            {
                query = query.Where(t => t.NgayKhoiHanh.Date == departureDate.Value.Date);
            }

            // Lọc theo loại tour
            if (tourType != null && tourType.Length > 0)
            {
                query = query.Where(t => tourType.Contains(t.LoaiTour.TenLoaiTour));
            }

            
           

            return query.ToList();
        }
    }
}