using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GymsHouse.Data.Migrations
{
    public partial class initialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LockoutReason",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture_1",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture_2",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnLockReason",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Center",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: false),
                    Picture_1 = table.Column<string>(nullable: true),
                    Picture_2 = table.Column<string>(nullable: true),
                    Picture_3 = table.Column<string>(nullable: true),
                    Picture_4 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Center", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Coupon",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CouponFor = table.Column<string>(nullable: false),
                    CouponType = table.Column<string>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    MinimumAmount = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Picture = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instructor",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false),
                    Awards = table.Column<string>(nullable: true),
                    Experiences = table.Column<string>(nullable: false),
                    History = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Picture_1 = table.Column<string>(nullable: true),
                    Picture_2 = table.Column<string>(nullable: true),
                    Picture_3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructor", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Instructor_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Major",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Major", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TrainingClass",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Duration = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Picture_1 = table.Column<string>(nullable: true),
                    Picture_2 = table.Column<string>(nullable: true),
                    Picture_3 = table.Column<string>(nullable: true),
                    Picture_4 = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingClass", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BusinessHours",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    CenterId = table.Column<string>(nullable: false),
                    DaysOfWeek = table.Column<string>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    From = table.Column<string>(maxLength: 8, nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    To = table.Column<string>(maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessHours", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BusinessHours_Center_CenterId",
                        column: x => x.CenterId,
                        principalTable: "Center",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    CenterId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Picture_1 = table.Column<string>(nullable: true),
                    Picture_2 = table.Column<string>(nullable: true),
                    Picture_3 = table.Column<string>(nullable: true),
                    Picture_4 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Location_Center_CenterId",
                        column: x => x.CenterId,
                        principalTable: "Center",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MajorOfInstructor",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    InstructorId = table.Column<string>(nullable: false),
                    MajorId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MajorOfInstructor", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MajorOfInstructor_Instructor_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MajorOfInstructor_Major_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Major",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleHeader",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    ClassId = table.Column<string>(nullable: false),
                    ClassSize = table.Column<int>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    InstructorId = table.Column<string>(nullable: false),
                    LocationId = table.Column<string>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleHeader", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ScheduleHeader_TrainingClass_ClassId",
                        column: x => x.ClassId,
                        principalTable: "TrainingClass",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduleHeader_Instructor_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduleHeader_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MembersInClass",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    ScheduleHeaderId = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembersInClass", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MembersInClass_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MembersInClass_ScheduleHeader_ScheduleHeaderId",
                        column: x => x.ScheduleHeaderId,
                        principalTable: "ScheduleHeader",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleDetails",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    DayOfWeek = table.Column<string>(maxLength: 10, nullable: false),
                    From = table.Column<string>(maxLength: 8, nullable: false),
                    ScheduleHeaderId = table.Column<string>(nullable: false),
                    To = table.Column<string>(maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ScheduleDetails_ScheduleHeader_ScheduleHeaderId",
                        column: x => x.ScheduleHeaderId,
                        principalTable: "ScheduleHeader",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessHours_CenterId",
                table: "BusinessHours",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructor_ApplicationUserId",
                table: "Instructor",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_CenterId",
                table: "Location",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_MajorOfInstructor_InstructorId",
                table: "MajorOfInstructor",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_MajorOfInstructor_MajorId",
                table: "MajorOfInstructor",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_MembersInClass_ApplicationUserId",
                table: "MembersInClass",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MembersInClass_ScheduleHeaderId",
                table: "MembersInClass",
                column: "ScheduleHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDetails_ScheduleHeaderId",
                table: "ScheduleDetails",
                column: "ScheduleHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleHeader_ClassId",
                table: "ScheduleHeader",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleHeader_InstructorId",
                table: "ScheduleHeader",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleHeader_LocationId",
                table: "ScheduleHeader",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BusinessHours");

            migrationBuilder.DropTable(
                name: "Coupon");

            migrationBuilder.DropTable(
                name: "MajorOfInstructor");

            migrationBuilder.DropTable(
                name: "MembersInClass");

            migrationBuilder.DropTable(
                name: "ScheduleDetails");

            migrationBuilder.DropTable(
                name: "Major");

            migrationBuilder.DropTable(
                name: "ScheduleHeader");

            migrationBuilder.DropTable(
                name: "TrainingClass");

            migrationBuilder.DropTable(
                name: "Instructor");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Center");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutReason",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Picture_1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Picture_2",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UnLockReason",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
