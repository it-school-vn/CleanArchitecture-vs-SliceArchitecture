using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    AccountType = table.Column<byte>(type: "smallint", nullable: false),
                    Role = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TimeZone = table.Column<byte>(type: "smallint", nullable: false),
                    Skills = table.Column<string[]>(type: "text[]", maxLength: 450, nullable: true),
                    DesiredJob = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    InterviewDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AvatarUrl = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    JobStatus = table.Column<int>(type: "integer", nullable: false),
                    About = table.Column<string>(type: "character varying(2500)", maxLength: 2500, nullable: true),
                    Approval_Reason = table.Column<string>(type: "text", nullable: false),
                    Approval_Yes = table.Column<bool>(type: "boolean", nullable: false),
                    Point_Total = table.Column<int>(type: "integer", nullable: false),
                    Profession_Company = table.Column<string>(type: "text", nullable: true),
                    Profession_YearOfExperience = table.Column<int>(type: "integer", nullable: true),
                    Student_School = table.Column<string>(type: "text", nullable: true),
                    Student_YearOfGraduation = table.Column<int>(type: "integer", nullable: true),
                    Added = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EducationInfos = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureFlags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(25)", unicode: false, maxLength: 25, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    Added = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureFlags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", nullable: false),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    BannerUrl = table.Column<string>(type: "character varying(2000)", unicode: false, maxLength: 2000, nullable: true),
                    Fee = table.Column<decimal>(type: "numeric", nullable: false),
                    FeeRate = table.Column<decimal>(type: "numeric", nullable: false),
                    EventType = table.Column<byte>(type: "smallint", nullable: false),
                    MaxAttendee = table.Column<int>(type: "integer", nullable: false),
                    OpenDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OpenTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    ClosedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ClosedTime = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    DateAt = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeAt = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentMethod = table.Column<byte>(type: "smallint", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    TimeZone = table.Column<byte>(type: "smallint", nullable: false),
                    Approval_Reason = table.Column<string>(type: "text", nullable: false),
                    Approval_Yes = table.Column<bool>(type: "boolean", nullable: false),
                    Conference_MeetingId = table.Column<string>(type: "text", nullable: true),
                    Conference_PassCode = table.Column<string>(type: "text", nullable: true),
                    Conference_Tool = table.Column<byte>(type: "smallint", nullable: false),
                    Conference_Type = table.Column<byte>(type: "smallint", nullable: false),
                    Conference_Url = table.Column<string>(type: "text", nullable: false),
                    Location_City = table.Column<string>(type: "text", nullable: false),
                    Location_Country = table.Column<string>(type: "text", nullable: false),
                    Location_District = table.Column<string>(type: "text", nullable: false),
                    Location_Line = table.Column<string>(type: "text", nullable: false),
                    Location_Line2 = table.Column<string>(type: "text", nullable: true),
                    Location_State = table.Column<string>(type: "text", nullable: true),
                    Location_Ward = table.Column<string>(type: "text", nullable: false),
                    Location_ZipCode = table.Column<string>(type: "text", nullable: true),
                    Added = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Accounts_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventAttendees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(26)", nullable: false),
                    EventId = table.Column<string>(type: "character varying(26)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Fee = table.Column<decimal>(type: "numeric", nullable: false),
                    Paid = table.Column<bool>(type: "boolean", nullable: false),
                    Confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TransactionId = table.Column<string>(type: "character varying(26)", nullable: true),
                    Added = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAttendees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventAttendees_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventAttendees_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTags",
                columns: table => new
                {
                    OwnerId = table.Column<string>(type: "character varying(26)", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTags", x => new { x.OwnerId, x.Id });
                    table.ForeignKey(
                        name: "FK_EventTags_Events_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventAttendees_AccountId",
                table: "EventAttendees",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_EventAttendees_EventId",
                table: "EventAttendees",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OwnerId",
                table: "Events",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventAttendees");

            migrationBuilder.DropTable(
                name: "EventTags");

            migrationBuilder.DropTable(
                name: "FeatureFlags");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
