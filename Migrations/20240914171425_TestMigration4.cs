using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackendApp.Migrations
{
    /// <inheritdoc />
    public partial class TestMigration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    UserRole = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegularUserHideableInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    PhoneNumberIsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    LocationIsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CurrentPosition = table.Column<string>(type: "text", nullable: true),
                    CurrentPositionIsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    Experience = table.Column<List<string>>(type: "text[]", nullable: false),
                    ExperienceIsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    Capabilities = table.Column<List<string>>(type: "text[]", nullable: false),
                    CapabilitiesArePublic = table.Column<bool>(type: "boolean", nullable: false),
                    Education = table.Column<List<string>>(type: "text[]", nullable: false),
                    EducationIsPublic = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegularUserHideableInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    SentById = table.Column<long>(type: "bigint", nullable: false),
                    SentToId = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Accepted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    SentById = table.Column<long>(type: "bigint", nullable: false),
                    SentToId = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Read = table.Column<bool>(type: "boolean", nullable: false),
                    ToUserId = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AssociatedPostId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostBase",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PostedById = table.Column<long>(type: "bigint", nullable: false),
                    PostedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    JobTitle = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Requirements = table.Column<string[]>(type: "text[]", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    IsReply = table.Column<bool>(type: "boolean", nullable: true),
                    PostId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostBase_PostBase_PostId",
                        column: x => x.PostId,
                        principalTable: "PostBase",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostFile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Path = table.Column<string>(type: "text", nullable: false),
                    FileType = table.Column<string>(type: "text", nullable: false),
                    PostBaseId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostFile_PostBase_PostBaseId",
                        column: x => x.PostBaseId,
                        principalTable: "PostBase",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RegularUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    HideableInfoId = table.Column<long>(type: "bigint", nullable: false),
                    PostBaseId = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    UserRole = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegularUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegularUsers_PostBase_PostBaseId",
                        column: x => x.PostBaseId,
                        principalTable: "PostBase",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegularUsers_RegularUserHideableInfo_HideableInfoId",
                        column: x => x.HideableInfoId,
                        principalTable: "RegularUserHideableInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_SentById",
                table: "Connections",
                column: "SentById");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_SentToId",
                table: "Connections",
                column: "SentToId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SentById",
                table: "Messages",
                column: "SentById");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SentToId",
                table: "Messages",
                column: "SentToId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AssociatedPostId",
                table: "Notifications",
                column: "AssociatedPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ToUserId",
                table: "Notifications",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostBase_PostId",
                table: "PostBase",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostBase_PostedById",
                table: "PostBase",
                column: "PostedById");

            migrationBuilder.CreateIndex(
                name: "IX_PostFile_PostBaseId",
                table: "PostFile",
                column: "PostBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_RegularUsers_HideableInfoId",
                table: "RegularUsers",
                column: "HideableInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegularUsers_PostBaseId",
                table: "RegularUsers",
                column: "PostBaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_RegularUsers_SentById",
                table: "Connections",
                column: "SentById",
                principalTable: "RegularUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_RegularUsers_SentToId",
                table: "Connections",
                column: "SentToId",
                principalTable: "RegularUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_RegularUsers_SentById",
                table: "Messages",
                column: "SentById",
                principalTable: "RegularUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_RegularUsers_SentToId",
                table: "Messages",
                column: "SentToId",
                principalTable: "RegularUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_PostBase_AssociatedPostId",
                table: "Notifications",
                column: "AssociatedPostId",
                principalTable: "PostBase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_RegularUsers_ToUserId",
                table: "Notifications",
                column: "ToUserId",
                principalTable: "RegularUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostBase_RegularUsers_PostedById",
                table: "PostBase",
                column: "PostedById",
                principalTable: "RegularUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostBase_RegularUsers_PostedById",
                table: "PostBase");

            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PostFile");

            migrationBuilder.DropTable(
                name: "RegularUsers");

            migrationBuilder.DropTable(
                name: "PostBase");

            migrationBuilder.DropTable(
                name: "RegularUserHideableInfo");
        }
    }
}
