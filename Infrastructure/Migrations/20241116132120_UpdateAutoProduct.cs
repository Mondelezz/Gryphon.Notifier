using System;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAutoProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsuranceProducts");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:available_bonuses_and_discounts", "loyalty_bonus,seasonal_sale,promo_code,new_customer_discount,special_promotion")
                .Annotation("Npgsql:Enum:brand_auto", "ford,toyota,volkswagen,honda,bmw,mercedes,audi")
                .Annotation("Npgsql:Enum:covered_risk", "theft,car_accident,natural_disasters,fire,vandalism,third_party_liability,animal_damage,collision_with_object,other")
                .Annotation("Npgsql:Enum:insurance_type", "osago,casco")
                .Annotation("Npgsql:Enum:state", "actual,archived")
                .Annotation("Npgsql:Enum:vehicle_insurance_class", "sedan,commercial,suv,minivan,sports,electric")
                .Annotation("Npgsql:Enum:vehicle_usage_type", "private,commercial")
                .OldAnnotation("Npgsql:Enum:state", "actual,archived");

            migrationBuilder.CreateTable(
                name: "AutoInsuranceProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    State = table.Column<State>(type: "state", nullable: false),
                    AgeAuto = table.Column<int>(type: "integer", nullable: false),
                    BrandAuto = table.Column<BrandAuto>(type: "brand_auto", nullable: false),
                    AgeDrivingExpirience = table.Column<int>(type: "integer", nullable: false),
                    AgeDriver = table.Column<int>(type: "integer", nullable: false),
                    BonusesAndDiscounts = table.Column<AvailableBonusesAndDiscounts[]>(type: "available_bonuses_and_discounts[]", nullable: false),
                    UsageRules = table.Column<string>(type: "text", nullable: false),
                    GeographicalLimitations = table.Column<string>(type: "text", nullable: false),
                    VehicleUsageType = table.Column<VehicleUsageType>(type: "vehicle_usage_type", nullable: false),
                    CoveredRisk = table.Column<CoveredRisk[]>(type: "covered_risk[]", nullable: false),
                    InsuranceType = table.Column<InsuranceType>(type: "insurance_type", nullable: false),
                    VehicleInsuranceClass = table.Column<VehicleInsuranceClass>(type: "vehicle_insurance_class", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', current_timestamp)"),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', current_timestamp)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoInsuranceProducts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoInsuranceProducts");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:state", "actual,archived")
                .OldAnnotation("Npgsql:Enum:available_bonuses_and_discounts", "loyalty_bonus,seasonal_sale,promo_code,new_customer_discount,special_promotion")
                .OldAnnotation("Npgsql:Enum:brand_auto", "ford,toyota,volkswagen,honda,bmw,mercedes,audi")
                .OldAnnotation("Npgsql:Enum:covered_risk", "theft,car_accident,natural_disasters,fire,vandalism,third_party_liability,animal_damage,collision_with_object,other")
                .OldAnnotation("Npgsql:Enum:insurance_type", "osago,casco")
                .OldAnnotation("Npgsql:Enum:state", "actual,archived")
                .OldAnnotation("Npgsql:Enum:vehicle_insurance_class", "sedan,commercial,suv,minivan,sports,electric")
                .OldAnnotation("Npgsql:Enum:vehicle_usage_type", "private,commercial");

            migrationBuilder.CreateTable(
                name: "InsuranceProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', current_timestamp)"),
                    State = table.Column<State>(type: "state", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', current_timestamp)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceProducts", x => x.Id);
                });
        }
    }
}
