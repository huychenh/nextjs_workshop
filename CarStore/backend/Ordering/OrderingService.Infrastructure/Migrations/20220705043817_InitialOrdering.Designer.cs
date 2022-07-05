﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OrderingService.Infrastructure.Data;

#nullable disable

namespace OrderingService.Infrastructure.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20220705043817_InitialOrdering")]
    partial class InitialOrdering
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OrderingService.AppCore.Core.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<int>("BuyerId")
                        .HasColumnType("integer")
                        .HasColumnName("buyer_id");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer")
                        .HasColumnName("owner_id");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("text")
                        .HasColumnName("picture_url");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer")
                        .HasColumnName("product_id");

                    b.Property<string>("ProductName")
                        .HasColumnType("text")
                        .HasColumnName("product_name");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_orders_id");

                    b.ToTable("orders", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
