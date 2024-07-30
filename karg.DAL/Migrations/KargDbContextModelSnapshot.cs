﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using karg.DAL.Context;

#nullable disable

namespace karg.DAL.Migrations
{
    [DbContext(typeof(KargDbContext))]
    partial class KargDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<int>("TitleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DescriptionId");

                    b.HasIndex("TitleId");

                    b.ToTable("Advices", (string)null);
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

                    b.Property<int>("StoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DescriptionId");

                    b.HasIndex("NameId");

                    b.HasIndex("StoryId");

                    b.ToTable("Animals", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Contacts", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Culture", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.HasKey("Code");

                    b.ToTable("Cultures", (string)null);
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

                    b.ToTable("FAQs", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AdviceId")
                        .HasColumnType("int");

                    b.Property<int?>("AnimalId")
                        .HasColumnType("int");

                    b.Property<int?>("PartnerId")
                        .HasColumnType("int");

                    b.Property<int?>("RescuerId")
                        .HasColumnType("int");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("YearResultId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdviceId");

                    b.HasIndex("AnimalId");

                    b.HasIndex("PartnerId");

                    b.HasIndex("RescuerId");

                    b.HasIndex("YearResultId");

                    b.ToTable("Images", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.JwtToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Tokens", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Localization", b =>
                {
                    b.Property<int>("LocalizationSetId")
                        .HasColumnType("int");

                    b.Property<string>("CultureCode")
                        .HasColumnType("varchar(2)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("varchar(5000)");

                    b.HasKey("LocalizationSetId", "CultureCode");

                    b.HasIndex("CultureCode");

                    b.ToTable("Localizations", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.LocalizationSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LocalizationsSets", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Partner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Uri")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Partners", (string)null);
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

                    b.HasIndex("TokenId")
                        .IsUnique();

                    b.ToTable("Rescuers", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.YearResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DescriptionId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("Year")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("DescriptionId");

                    b.ToTable("YearsResults", (string)null);
                });

            modelBuilder.Entity("karg.DAL.Models.Advice", b =>
                {
                    b.HasOne("karg.DAL.Models.LocalizationSet", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.LocalizationSet", "Title")
                        .WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Description");

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

                    b.HasOne("karg.DAL.Models.LocalizationSet", "Story")
                        .WithMany()
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Description");

                    b.Navigation("Name");

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
                    b.HasOne("karg.DAL.Models.Advice", "Advice")
                        .WithMany("Images")
                        .HasForeignKey("AdviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.Animal", "Animal")
                        .WithMany("Images")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.Partner", "Partner")
                        .WithMany("Images")
                        .HasForeignKey("PartnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.Rescuer", "Rescuer")
                        .WithMany("Images")
                        .HasForeignKey("RescuerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("karg.DAL.Models.YearResult", "YearResult")
                        .WithMany("Images")
                        .HasForeignKey("YearResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Advice");

                    b.Navigation("Animal");

                    b.Navigation("Partner");

                    b.Navigation("Rescuer");

                    b.Navigation("YearResult");
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

            modelBuilder.Entity("karg.DAL.Models.Rescuer", b =>
                {
                    b.HasOne("karg.DAL.Models.JwtToken", "Token")
                        .WithOne("Rescuer")
                        .HasForeignKey("karg.DAL.Models.Rescuer", "TokenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Token");
                });

            modelBuilder.Entity("karg.DAL.Models.YearResult", b =>
                {
                    b.HasOne("karg.DAL.Models.LocalizationSet", "Description")
                        .WithMany()
                        .HasForeignKey("DescriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Description");
                });

            modelBuilder.Entity("karg.DAL.Models.Advice", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("karg.DAL.Models.Animal", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("karg.DAL.Models.JwtToken", b =>
                {
                    b.Navigation("Rescuer");
                });

            modelBuilder.Entity("karg.DAL.Models.LocalizationSet", b =>
                {
                    b.Navigation("Localizations");
                });

            modelBuilder.Entity("karg.DAL.Models.Partner", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("karg.DAL.Models.Rescuer", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("karg.DAL.Models.YearResult", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
