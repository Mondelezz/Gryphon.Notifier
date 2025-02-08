using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GroupEventToTopic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_GroupEvents_GroupEventId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "GroupEvents");

            migrationBuilder.RenameColumn(
                name: "GroupEventId",
                table: "Events",
                newName: "TopicId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_GroupEventId",
                table: "Events",
                newName: "IX_Events_TopicId");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TimeEventStart",
                table: "Events",
                type: "time without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TimeEventEnded",
                table: "Events",
                type: "time without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TopicType = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', current_timestamp)"),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', current_timestamp)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Topics_TopicId",
                table: "Events",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Topics_TopicId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "Events",
                newName: "GroupEventId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_TopicId",
                table: "Events",
                newName: "IX_Events_GroupEventId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeEventStart",
                table: "Events",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeEventEnded",
                table: "Events",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GroupEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', current_timestamp)"),
                    GroupEventType = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', current_timestamp)"),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupEvents", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Events_GroupEvents_GroupEventId",
                table: "Events",
                column: "GroupEventId",
                principalTable: "GroupEvents",
                principalColumn: "Id");
        }
    }
}
