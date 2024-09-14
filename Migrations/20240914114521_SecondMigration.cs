using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BackendApp.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Posts_PostId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_RegularUsers_PostedById",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_RegularUsers_JobPosts_JobPostId",
                table: "RegularUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RegularUsers_Posts_PostId",
                table: "RegularUsers");

            migrationBuilder.DropTable(
                name: "JobPosts");

            migrationBuilder.DropIndex(
                name: "IX_RegularUsers_JobPostId",
                table: "RegularUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "JobPostId",
                table: "RegularUsers");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "PostBase");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "RegularUsers",
                newName: "PostBaseId");

            migrationBuilder.RenameIndex(
                name: "IX_RegularUsers_PostId",
                table: "RegularUsers",
                newName: "IX_RegularUsers_PostBaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_PostedById",
                table: "PostBase",
                newName: "IX_PostBase_PostedById");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_PostId",
                table: "PostBase",
                newName: "IX_PostBase_PostId");

            migrationBuilder.AddColumn<long>(
                name: "AssociatedPostId",
                table: "Notifications",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsReply",
                table: "PostBase",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "PostBase",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PostBase",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PostBase",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "PostBase",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "Requirements",
                table: "PostBase",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostBase",
                table: "PostBase",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AssociatedPostId",
                table: "Notifications",
                column: "AssociatedPostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostFile_PostBaseId",
                table: "PostFile",
                column: "PostBaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_PostBase_AssociatedPostId",
                table: "Notifications",
                column: "AssociatedPostId",
                principalTable: "PostBase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostBase_PostBase_PostId",
                table: "PostBase",
                column: "PostId",
                principalTable: "PostBase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostBase_RegularUsers_PostedById",
                table: "PostBase",
                column: "PostedById",
                principalTable: "RegularUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegularUsers_PostBase_PostBaseId",
                table: "RegularUsers",
                column: "PostBaseId",
                principalTable: "PostBase",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_PostBase_AssociatedPostId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PostBase_PostBase_PostId",
                table: "PostBase");

            migrationBuilder.DropForeignKey(
                name: "FK_PostBase_RegularUsers_PostedById",
                table: "PostBase");

            migrationBuilder.DropForeignKey(
                name: "FK_RegularUsers_PostBase_PostBaseId",
                table: "RegularUsers");

            migrationBuilder.DropTable(
                name: "PostFile");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_AssociatedPostId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostBase",
                table: "PostBase");

            migrationBuilder.DropColumn(
                name: "AssociatedPostId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PostBase");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PostBase");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "PostBase");

            migrationBuilder.DropColumn(
                name: "Requirements",
                table: "PostBase");

            migrationBuilder.RenameTable(
                name: "PostBase",
                newName: "Posts");

            migrationBuilder.RenameColumn(
                name: "PostBaseId",
                table: "RegularUsers",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_RegularUsers_PostBaseId",
                table: "RegularUsers",
                newName: "IX_RegularUsers_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostBase_PostedById",
                table: "Posts",
                newName: "IX_Posts_PostedById");

            migrationBuilder.RenameIndex(
                name: "IX_PostBase_PostId",
                table: "Posts",
                newName: "IX_Posts_PostId");

            migrationBuilder.AddColumn<long>(
                name: "JobPostId",
                table: "RegularUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsReply",
                table: "Posts",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "JobPosts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    PostedById = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    JobTitle = table.Column<string>(type: "text", nullable: false),
                    PostedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Requirements = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobPosts_RegularUsers_PostedById",
                        column: x => x.PostedById,
                        principalTable: "RegularUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegularUsers_JobPostId",
                table: "RegularUsers",
                column: "JobPostId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPosts_PostedById",
                table: "JobPosts",
                column: "PostedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Posts_PostId",
                table: "Posts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_RegularUsers_PostedById",
                table: "Posts",
                column: "PostedById",
                principalTable: "RegularUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegularUsers_JobPosts_JobPostId",
                table: "RegularUsers",
                column: "JobPostId",
                principalTable: "JobPosts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegularUsers_Posts_PostId",
                table: "RegularUsers",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
