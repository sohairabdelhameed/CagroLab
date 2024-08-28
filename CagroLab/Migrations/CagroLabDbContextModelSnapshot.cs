﻿// <auto-generated />
using System;
using CagroLab.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CagroLab.Migrations
{
    [DbContext(typeof(CagroLabDbContext))]
    partial class CagroLabDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CagroLab.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Account_Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Full_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Is_Active")
                        .HasColumnType("bit");

                    b.Property<int>("Lab_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Last_Activity")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Last_Login")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Main_Account")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Lab_Id");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("CagroLab.Models.Lab", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lab_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lab_Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lab_Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lab_Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Lab", (string)null);
                });

            modelBuilder.Entity("CagroLab.Models.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Account_Id")
                        .HasColumnType("int");

                    b.Property<int>("Lab_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Package_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Package_Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Account_Id");

                    b.HasIndex("Lab_Id");

                    b.ToTable("Package", (string)null);
                });

            modelBuilder.Entity("CagroLab.Models.Sample", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Account_Id")
                        .HasColumnType("int");

                    b.Property<int>("Package_Id")
                        .HasColumnType("int");

                    b.Property<int>("Patient_Id")
                        .HasColumnType("int");

                    b.Property<string>("Patient_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patient_Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sample_Type_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Account_Id");

                    b.HasIndex("Package_Id");

                    b.ToTable("Sample", (string)null);
                });

            modelBuilder.Entity("CagroLab.Models.Account", b =>
                {
                    b.HasOne("CagroLab.Models.Lab", "Lab")
                        .WithMany("Accounts")
                        .HasForeignKey("Lab_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");
                });

            modelBuilder.Entity("CagroLab.Models.Package", b =>
                {
                    b.HasOne("CagroLab.Models.Account", "Account")
                        .WithMany("Packages")
                        .HasForeignKey("Account_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CagroLab.Models.Lab", "Lab")
                        .WithMany("Packages")
                        .HasForeignKey("Lab_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Lab");
                });

            modelBuilder.Entity("CagroLab.Models.Sample", b =>
                {
                    b.HasOne("CagroLab.Models.Account", "Account")
                        .WithMany("Samples")
                        .HasForeignKey("Account_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CagroLab.Models.Package", "Package")
                        .WithMany("Samples")
                        .HasForeignKey("Package_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("CagroLab.Models.Account", b =>
                {
                    b.Navigation("Packages");

                    b.Navigation("Samples");
                });

            modelBuilder.Entity("CagroLab.Models.Lab", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Packages");
                });

            modelBuilder.Entity("CagroLab.Models.Package", b =>
                {
                    b.Navigation("Samples");
                });
#pragma warning restore 612, 618
        }
    }
}
