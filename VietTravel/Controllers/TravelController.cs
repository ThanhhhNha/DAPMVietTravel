using System;
using System.Linq;
using System.Web.Mvc;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class TravelController : Controller
    {
        // Ph??ng th?c tr? v? danh s�ch c�c tour du l?ch
        public ActionResult DuLichTravel()
        {
            try
            {
                using (var db = new TravelVNEntities())
                {
                    var tours = db.Tours.ToList();
                    return View(tours); // Tr? v? danh s�ch tour t?i view DuLichTravel
                }
            }
            catch (Exception ex)
            {
                // X? l� l?i t?i ?�y
                return View("Error"); // Tr? v? trang l?i n?u c� l?i x?y ra
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
                // X? l� l?i t?i ?�y
                return View("Error");
            }
        }

        // Ph??ng th?c tr? v? k?t qu? t�m ki?m d?a tr�n c�c ti�u ch�
        public ActionResult SearchResults(string budget, string departure, string destination, DateTime? departureDate, string[] tourType, string[] transport)
        {
            try
            {
                using (var db = new TravelVNEntities())
                {
                    var tourService = new TourService(db);
                    var tours = tourService.SearchTours(budget, departure, destination, departureDate, tourType ?? new string[] { }, transport ?? new string[] { });
                    return View(tours); // Tr? v? k?t qu? t�m ki?m t?i view SearchResults
                }
            }
            catch (Exception ex)
            {
                // X? l� l?i t?i ?�y
                return View("Error");
            }
        }

        // Ph??ng th?c x? l� t�m ki?m t? form v� tr? v? k?t qu?
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
                // X? l� l?i t?i ?�y
                return View("Error"); // Tr? v? view l?i n?u c� l?i x?y ra
            }
        }
    }
}
