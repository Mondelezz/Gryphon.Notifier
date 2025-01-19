using Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGroupEventAddedGroupEventType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:group_event_type", "default,smart")
                .Annotation("Npgsql:Enum:importance", "low,medium,high")
                .OldAnnotation("Npgsql:Enum:importance", "low,medium,high");

            migrationBuilder.AddColumn<GroupEventType>(
                name: "GroupEventType",
                table: "GroupEvents",
                type: "group_event_type",
                nullable: false,
                defaultValue: GroupEventType.Default);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupEventType",
                table: "GroupEvents");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:importance", "low,medium,high")
                .OldAnnotation("Npgsql:Enum:group_event_type", "default,smart")
                .OldAnnotation("Npgsql:Enum:importance", "low,medium,high");
        }
    }
}
