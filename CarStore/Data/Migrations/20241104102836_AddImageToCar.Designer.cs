﻿// <auto-generated />
using System;
using CarStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarStore.Data.Migrations
{
    [DbContext(typeof(CarStoreContext))]
    [Migration("20241104102836_AddImageToCar")]
    partial class AddImageToCar
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("CarStore.Entities.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ColorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<int>("ModelId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("ReleasedDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ColorId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ModelId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CarStore.Entities.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Colors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Red"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Black"
                        },
                        new
                        {
                            Id = 3,
                            Name = "White"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Green Midnight"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Golden"
                        });
                });

            modelBuilder.Entity("CarStore.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Toyota"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Mercedes"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Porches"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Mazda"
                        },
                        new
                        {
                            Id = 5,
                            Name = "BMW"
                        });
                });

            modelBuilder.Entity("CarStore.Entities.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Models");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Camry v3"
                        },
                        new
                        {
                            Id = 2,
                            Name = "GMJ"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Cayan"
                        },
                        new
                        {
                            Id = 4,
                            Name = "CX6"
                        },
                        new
                        {
                            Id = 5,
                            Name = "i8 v3"
                        });
                });

            modelBuilder.Entity("CarStore.Entities.Car", b =>
                {
                    b.HasOne("CarStore.Entities.Color", "Color")
                        .WithMany()
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarStore.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarStore.Entities.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Color");

                    b.Navigation("Company");

                    b.Navigation("Model");
                });
#pragma warning restore 612, 618
        }
    }
}
