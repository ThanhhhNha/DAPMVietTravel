﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VietTravel.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TravelVNEntities : DbContext
    {
        public TravelVNEntities()
            : base("name=TravelVNEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Combo> Comboes { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<FlightDetail> FlightDetails { get; set; }
        public virtual DbSet<HotelDetail> HotelDetails { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<LoaiPhong> LoaiPhongs { get; set; }
        public virtual DbSet<LoaiTour> LoaiTours { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Phong> Phongs { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<TinhThanh> TinhThanhs { get; set; }
        public virtual DbSet<Tour> Tours { get; set; }
        public virtual DbSet<TourDetail> TourDetails { get; set; }
        public virtual DbSet<TrangThaiPhong> TrangThaiPhongs { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
