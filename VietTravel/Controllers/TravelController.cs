using System;
using System.Linq;
using System.Web.Mvc;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class TravelController : Controller
    {
        // Ph??ng th?c tr? v? danh sách các tour du l?ch
        public ActionResult DuLichTravel()
        {
            try
            {
                using (var db = new TravelVNEntities())
                {
                    var tours = db.Tours.ToList();
                    return View(tours); // Tr? v? danh sách tour t?i view DuLichTravel
                }
            }
            catch (Exception ex)
            {
                // X? lý l?i t?i ?ây
                return View("Error"); // Tr? v? trang l?i n?u có l?i x?y ra
            }
        }

        // Ph??ng th?c tr? v? chi ti?t tour theo MaTour
        public ActionResult TourDetail(string maTour)
        {
            try
            {
                using (var db = new TravelVNEntities())
                {
                    var tour = db.Tours.FirstOrDefault(t => t.MaTour == maTour);
                    if (tour == null)
                    {
                        return HttpNotFound();
                    }
                    return View(tour);
                }
            }
            catch (Exception ex)
            {
                // X? lý l?i t?i ?ây
                return View("Error");
            }
        }

        // Ph??ng th?c tr? v? k?t qu? tìm ki?m d?a trên các tiêu chí
        public ActionResult SearchResults(string budget, string departure, string destination, DateTime? departureDate, string[] tourType, string[] transport)
        {
            try
            {
                using (var db = new TravelVNEntities())
                {
                    var tourService = new TourService(db);
                    var tours = tourService.SearchTours(budget, departure, destination, departureDate, tourType ?? new string[] { }, transport ?? new string[] { });
                    return View(tours); // Tr? v? k?t qu? tìm ki?m t?i view SearchResults
                }
            }
            catch (Exception ex)
            {
                // X? lý l?i t?i ?ây
                return View("Error");
            }
        }

        // Ph??ng th?c x? lý tìm ki?m t? form và tr? v? k?t qu?
        [HttpPost]
        public ActionResult Search(string budget, string departure, string destination, DateTime? departureDate, string[] tourType, string[] transport)
        {
            try
            {
                var tourService = new TourService(new TravelVNEntities());
                var tours = tourService.SearchTours(budget, departure, destination, departureDate, tourType, transport);
                return View("SearchResults", tours);
            }
            catch (Exception ex)
            {
                // X? lý l?i t?i ?ây
                return View("Error"); // Tr? v? view l?i n?u có l?i x?y ra
            }
        }
    }
}
