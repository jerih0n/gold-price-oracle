﻿// <auto-generated />
using System;
using GoldPriceOracle.Connection.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GoldPriceOracle.Connection.Database.Migrations
{
    [DbContext(typeof(OracleDbContext))]
    partial class OracleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("GoldPriceOracle.Connection.Database.Asset", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .UseIdentityColumn();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.HasKey("Id");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("GoldPriceOracle.Connection.Database.AssetHistoricData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<short>("AssetId")
                        .HasColumnType("smallint");

                    b.Property<short>("FiatCurrencyId")
                        .HasColumnType("smallint");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,4)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AssetId");

                    b.HasIndex("FiatCurrencyId");

                    b.ToTable("AssetHistoricDatas");
                });

            modelBuilder.Entity("GoldPriceOracle.Connection.Database.FiatCurrency", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .UseIdentityColumn();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.HasKey("Id");

                    b.ToTable("FiatCurrencies");
                });

            modelBuilder.Entity("GoldPriceOracle.Connection.Database.NodeData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityColumn();

                    b.Property<string>("ActiveAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MnemonicPhraseEncrypted")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrivateKeyEncrypted")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NodeData");
                });

            modelBuilder.Entity("GoldPriceOracle.Connection.Database.AssetHistoricData", b =>
                {
                    b.HasOne("GoldPriceOracle.Connection.Database.Asset", "Asset")
                        .WithMany()
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GoldPriceOracle.Connection.Database.FiatCurrency", "FiatCurrency")
                        .WithMany()
                        .HasForeignKey("FiatCurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset");

                    b.Navigation("FiatCurrency");
                });
#pragma warning restore 612, 618
        }
    }
}
