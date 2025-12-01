using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Animal_Clinic_Project_MVC_Group_3.Models;

public partial class AnimalCareClinicDbContext : DbContext
{
    public AnimalCareClinicDbContext()
    {
    }

    public AnimalCareClinicDbContext(DbContextOptions<AnimalCareClinicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<ClinicUser> ClinicUsers { get; set; }

    public virtual DbSet<ClinicalHistory> ClinicalHistories { get; set; }

    public virtual DbSet<DailyAppointment> DailyAppointments { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<Receptionist> Receptionists { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<TimeSlot> TimeSlots { get; set; }

    public virtual DbSet<VPetHistory> VPetHistories { get; set; }

    public virtual DbSet<Veterinarian> Veterinarians { get; set; }

    public virtual DbSet<VetsAvailability> VetsAvailabilities { get; set; }

    public virtual DbSet<VetsWorkload> VetsWorkloads { get; set; }

    // For obtaining monthly reports with the Store Procedure
    public virtual DbSet<ReportMonthlyResult> ReportMonthlyResults { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=.; Encrypt=false; Database=AnimalCareClinicDb; Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Administ__1788CCACD5AD4248");

            entity.ToTable("Administrator");

            entity.HasIndex(e => e.Email, "UQ__Administ__A9D10534442C50F7").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA28E20D61F");

            entity.ToTable("Appointment", t => t
                .HasTrigger("tr_prevent_past_appointments")); // Especificar el trigger cuando una tabla tiene trigger

            entity.Property(e => e.AppointmentId)
                .ValueGeneratedNever()
                .HasColumnName("AppointmentID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.PetId).HasColumnName("PetID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("('Scheduled')");
            entity.Property(e => e.VeterinarianId).HasColumnName("VeterinarianID");

            entity.HasOne(d => d.Pet).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Appointme__PetID__46E78A0C");

            entity.HasOne(d => d.Veterinarian).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.VeterinarianId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Appointme__Veter__47DBAE45");
        });

        modelBuilder.Entity<ClinicUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ClinicUser");

            entity.Property(e => e.Availability).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Speciality).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserType)
                .HasMaxLength(13)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ClinicalHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__Clinical__4D7B4ADDFF4AEC5B");

            entity.ToTable("ClinicalHistory");

            entity.Property(e => e.HistoryId)
                .ValueGeneratedNever()
                .HasColumnName("HistoryID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.PetId).HasColumnName("PetID");
            entity.Property(e => e.VeterinarianId).HasColumnName("VeterinarianID");

            entity.HasOne(d => d.Pet).WithMany(p => p.ClinicalHistories)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__ClinicalH__PetID__4AB81AF0");

            entity.HasOne(d => d.Veterinarian).WithMany(p => p.ClinicalHistories)
                .HasForeignKey(d => d.VeterinarianId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__ClinicalH__Veter__4BAC3F29");
        });

        modelBuilder.Entity<DailyAppointment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("daily_appointments");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.OwnersName).HasMaxLength(101);
            entity.Property(e => e.PetName).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.VeterinarianId).HasColumnName("VeterinarianID");
            entity.Property(e => e.VetsName).HasMaxLength(101);
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.OwnerId).HasName("PK__Owner__81938598F5F1009C");

            entity.ToTable("Owner", tb =>
                {
                    tb.HasTrigger("cancel_pet_future_appointments");
                    tb.HasTrigger("delete_pets_from_owner");
                });

            entity.Property(e => e.OwnerId)
                .ValueGeneratedNever()
                .HasColumnName("OwnerID");
            entity.Property(e => e.Email).HasMaxLength(25);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.PetId).HasName("PK__Pet__48E538028EED512B");

            entity.ToTable("Pet", tb => tb.HasTrigger("delete_clinicalHistory_from_pet"));

            entity.Property(e => e.PetId)
                .ValueGeneratedNever()
                .HasColumnName("PetID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
            entity.Property(e => e.Species).HasMaxLength(50);

            entity.HasOne(d => d.Owner).WithMany(p => p.Pets)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Pet__OwnerID__4222D4EF");
        });

        modelBuilder.Entity<Receptionist>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Receptio__1788CCACF03760CA");

            entity.ToTable("Receptionist");

            entity.HasIndex(e => e.Email, "UQ__Receptio__A9D10534773190FD").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__D5BD48E5A2407361");

            entity.ToTable("Report");

            entity.Property(e => e.ReportId)
                .ValueGeneratedNever()
                .HasColumnName("ReportID");
            entity.Property(e => e.AppointmentsCancelled).HasDefaultValueSql("((0))");
            entity.Property(e => e.AppointmentsMade).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<TimeSlot>(entity =>
        {
            entity.HasKey(e => e.Slot).HasName("PK__TimeSlot__BC7BA946553136A7");

            entity.ToTable("TimeSlot");
        });

        modelBuilder.Entity<VPetHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_pet_history");

            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.OwnersName).HasMaxLength(101);
            entity.Property(e => e.PetId).HasColumnName("PetID");
            entity.Property(e => e.PetName).HasMaxLength(50);
            entity.Property(e => e.Species).HasMaxLength(50);
            entity.Property(e => e.VetName).HasMaxLength(101);
        });

        modelBuilder.Entity<Veterinarian>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Veterina__1788CCAC265C8B04");

            entity.ToTable("Veterinarian", tb => tb.HasTrigger("cancel_appointments_afterDelete"));

            entity.HasIndex(e => e.Email, "UQ__Veterina__A9D10534D93778CA").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.Availability).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.Speciality).HasMaxLength(100);
        });

        modelBuilder.Entity<VetsAvailability>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vets_availability");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VetName).HasMaxLength(101);
        });

        modelBuilder.Entity<VetsWorkload>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vets_workload");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VetName).HasMaxLength(101);
        });

        OnModelCreatingPartial(modelBuilder);

        // Because ReportMonthlyResult doesn't have a primary key 
        modelBuilder.Entity<ReportMonthlyResult>().HasNoKey();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
