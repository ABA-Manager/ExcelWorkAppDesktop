﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClinicDOM
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Clinic_AppContext : DbContext
    {
        public Clinic_AppContext()
            : base("name=Clinic_AppContext")
        {
        }
        public Clinic_AppContext(string config)
    : base(config)
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180; // seconds
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__EFMigrationsHistory> C__EFMigrationsHistory { get; set; }
        public virtual DbSet<Agreement> Agreement { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Contractor> Contractor { get; set; }
        public virtual DbSet<ContractorType> ContractorType { get; set; }
        public virtual DbSet<Diagnosis> Diagnosis { get; set; }
        public virtual DbSet<PatientAccount> PatientAccount { get; set; }
        public virtual DbSet<Payroll> Payroll { get; set; }
        public virtual DbSet<Period> Period { get; set; }
        public virtual DbSet<PlaceOfService> PlaceOfService { get; set; }
        public virtual DbSet<Procedure> Procedure { get; set; }
        public virtual DbSet<ReleaseInformation> ReleaseInformation { get; set; }
        public virtual DbSet<ServiceLog> ServiceLog { get; set; }
        public virtual DbSet<SubProcedure> SubProcedure { get; set; }
        public virtual DbSet<UnitDetail> UnitDetail { get; set; }
    }
}
