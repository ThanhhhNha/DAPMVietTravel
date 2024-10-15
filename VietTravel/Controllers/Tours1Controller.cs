using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VietTravel.Models;

namespace VietTravel.Controllers
{
    public class Tours1Controller : Controller
    {
        private TravelVNEntities db = new TravelVNEntities();

        // GET: Tours1
        public ActionResult Index()
        {
            var tours = db.Tours.Include(t => t.LoaiTour).Include(t => t.TinhThanh);
            return View(tours.ToList());
        }

        // GET: Tours1/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // GET: Tours1/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiTour = new SelectList(db.LoaiTours, "MaLoaiTour", "TenLoaiTour");
            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh");
            return View();
        }

        // POST: Tours1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTour,TenTour,MoTa,NgayKhoiHanh,ThoiGian,Gia,MaLoaiTour,MaTinh")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Tours.Add(tour);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiTour = new SelectList(db.LoaiTours, "MaLoaiTour", "TenLoaiTour", tour.MaLoaiTour);
            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh", tour.MaTinh);
            return View(tour);
        }

        // GET: Tours1/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiTour = new SelectList(db.LoaiTours, "MaLoaiTour", "TenLoaiTour", tour.MaLoaiTour);
            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh", tour.MaTinh);
            return View(tour);
        }

        // POST: Tours1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTour,TenTour,MoTa,NgayKhoiHanh,ThoiGian,Gia,MaLoaiTour,MaTinh")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiTour = new SelectList(db.LoaiTours, "MaLoaiTour", "TenLoaiTour", tour.MaLoaiTour);
            ViewBag.MaTinh = new SelectList(db.TinhThanhs, "MaTinh", "TenTinh", tour.MaTinh);
            return View(tour);
        }

        // GET: Tours1/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // POST: Tours1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Tour tour = db.Tours.Find(id);
            db.Tours.Remove(tour);
            db.SaveChanges();
            return RedirectToAction("Index");
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
