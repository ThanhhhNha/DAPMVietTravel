using System;
using System.Collections.Generic;
using VietTravel.Models; // Thay thế bằng namespace phù hợp của bạn nếu cần

namespace VietTravel.Models // Đảm bảo tên namespace khớp với thư mục dự án
{
    public class SearchViewModel
    {
        // Danh sách tỉnh thành để hiển thị trong form
        public List<TinhThanh> TinhThanhList { get; set; }
        public List<Tour> Tours { get; set; }
        public List<LoaiTour> LoaiTourList { get; set; }
        // Ngân sách (budget)
        public string Budget { get; set; }

        // Điểm khởi hành (departure)
        public string Departure { get; set; }

        // Điểm đến (destination)
        public string Destination { get; set; }

        // Ngày khởi hành (departureDate)
        public DateTime? DepartureDate { get; set; }

        // Loại tour (tourType)
        public string[] TourType { get; set; }

        // Phương tiện (transport)
        public string[] Transport { get; set; }
    }
}
