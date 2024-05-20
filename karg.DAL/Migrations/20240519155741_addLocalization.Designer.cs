﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using karg.DAL.Context;

#nullable disable

namespace karg.DAL.Migrations
{
    [DbContext(typeof(KargDbContext))]
    [Migration("20240519155741_addLocalization")]
    partial class addLocalization
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("karg.DAL.Models.Advice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateOnly>("Created_At")
                        .HasColumnType("date");

                    b.Property<int>("DescriptionId")
                        .HasColumnType("int");

                    b.Property<int?>("ImageId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("TitleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DescriptionId");

                    b.HasIndex("ImageId")
                        .IsUnique();

                    b.HasIndex("TitleId");

                    b.ToTable("Advice", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("DescriptionId")
                        .HasColumnType("int");

                    b.Property<decimal>("Donats")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("NameId")
                        .HasColumnType("int");

                    b.Property<int>("Short_DescriptionId")
                        .HasColumnType("int");

                    b.Property<int>("StoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DescriptionId");

                    b.HasIndex("NameId");

                    b.HasIndex("Short_DescriptionId");

                    b.HasIndex("StoryId");

                    b.ToTable("Animal", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Contact", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Culture", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.HasKey("Code");

                    b.ToTable("Culture", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.FAQ", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AnswerId")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("QuestionId");

                    b.ToTable("FAQ", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AnimalId")
                        .HasColumnType("int");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.ToTable("Image", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.JwtToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("RescuerId")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Token", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Localization", b =>
                {
                    b.Property<int>("LocalizationSetId")
                        .HasColumnType("int");

                    b.Property<string>("CultureCode")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("varchar(5000)");

                    b.HasKey("LocalizationSetId", "CultureCode");

                    b.HasIndex("CultureCode");

                    b.ToTable("Localization", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.LocalizationSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LocalizationSet", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Partner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ImageId")
                        .IsUnique();

                    b.ToTable("Partner", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Rescuer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Current_Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Previous_Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TokenId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ImageId")
                        .IsUnique();

                    b.HasIndex("TokenId")
                        .IsUnique();

                    b.ToTable("Rescuer", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.YearResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DescriptionId")
                        .HasColumnType("int");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("Year")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("DescriptionId");

                    b.HasIndex("ImageId")
                        .IsUnique();

                    b.ToTable("YearResult", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Advice", b =>
                {
                    b.HasOne("karg.DAL.Models.LocalizationSet", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.Image", "Image")
                        .WithOne("Advice")
                        .HasForeignKey("karg.DAL.Models.Advice", "ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.LocalizationSet", "Title")
                        .WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Description");

                    b.Navigation("Image");

                    b.Navigation("Title");
                });

            modelBuilder.Entity("karg.DAL.Models.Animal", b =>
                {
                    b.HasOne("karg.DAL.Models.LocalizationSet", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.LocalizationSet", "Name")
                        .WithMany()
                        .HasForeignKey("NameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.LocalizationSet", "Short_Description")
                        .WithMany()
                        .HasForeignKey("Short_DescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.LocalizationSet", "Story")
                        .WithMany()
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Description");

                    b.Navigation("Name");

                    b.Navigation("Short_Description");

                    b.Navigation("Story");
                });

            modelBuilder.Entity("karg.DAL.Models.FAQ", b =>
                {
                    b.HasOne("karg.DAL.Models.LocalizationSet", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.LocalizationSet", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("karg.DAL.Models.Image", b =>
                {
                    b.HasOne("karg.DAL.Models.Animal", "Animal")
                        .WithMany("Images")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Animal");
                });

            modelBuilder.Entity("karg.DAL.Models.Localization", b =>
                {
                    b.HasOne("karg.DAL.Models.Culture", "Culture")
                        .WithMany()
                        .HasForeignKey("CultureCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.LocalizationSet", "LocalizationSet")
                        .WithMany("Localizations")
                        .HasForeignKey("LocalizationSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Culture");

                    b.Navigation("LocalizationSet");
                });

            modelBuilder.Entity("karg.DAL.Models.Partner", b =>
                {
                    b.HasOne("karg.DAL.Models.Image", "Image")
                        .WithOne("Partner")
                        .HasForeignKey("karg.DAL.Models.Partner", "ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");
                });

            modelBuilder.Entity("karg.DAL.Models.Rescuer", b =>
                {
                    b.HasOne("karg.DAL.Models.Image", "Image")
                        .WithOne("Rescuer")
                        .HasForeignKey("karg.DAL.Models.Rescuer", "ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.JwtToken", "Token")
                        .WithOne("Rescuer")
                        .HasForeignKey("karg.DAL.Models.Rescuer", "TokenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Token");
                });

            modelBuilder.Entity("karg.DAL.Models.YearResult", b =>
                {
                    b.HasOne("karg.DAL.Models.LocalizationSet", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.Image", "Image")
                        .WithOne("YearResult")
                        .HasForeignKey("karg.DAL.Models.YearResult", "ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Description");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("karg.DAL.Models.Animal", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("karg.DAL.Models.Image", b =>
                {
                    b.Navigation("Advice");

                    b.Navigation("Partner");

                    b.Navigation("Rescuer");

                    b.Navigation("YearResult");
                });

            modelBuilder.Entity("karg.DAL.Models.JwtToken", b =>
                {
                    b.Navigation("Rescuer");
                });

            modelBuilder.Entity("karg.DAL.Models.LocalizationSet", b =>
                {
                    b.Navigation("Localizations");
                });
#pragma warning restore 612, 618
        }
    }
}
