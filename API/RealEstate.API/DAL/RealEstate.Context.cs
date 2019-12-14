﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealEstate.API.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RealEstateEntities : DbContext
    {
        public RealEstateEntities()
            : base("name=RealEstateEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountRole> AccountRoles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Estate_Types> Estate_Types { get; set; }
        public virtual DbSet<LoginLog> LoginLogs { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<RentUnit> RentUnits { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SaleUnit> SaleUnits { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<EstateMedia> EstateMedias { get; set; }
        public virtual DbSet<Estate> Estates { get; set; }
    }
}