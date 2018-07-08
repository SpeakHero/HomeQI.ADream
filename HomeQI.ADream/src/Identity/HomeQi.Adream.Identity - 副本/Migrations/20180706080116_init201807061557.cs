using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeQI.ADream.Identity.Migrations
{
    public partial class init201807061557 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    EditedTime = table.Column<DateTimeOffset>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CretaedUser = table.Column<string>(maxLength: 50, nullable: true),
                    EditeUser = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    OrgCode = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: false),
                    ZipCode = table.Column<string>(maxLength: 15, nullable: false),
                    WebSite = table.Column<string>(maxLength: 100, nullable: false),
                    Contacts = table.Column<string>(maxLength: 100, nullable: false),
                    Phone = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    EditedTime = table.Column<DateTimeOffset>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CretaedUser = table.Column<string>(maxLength: 50, nullable: true),
                    EditeUser = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    PermissionId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: false),
                    ClaimValue = table.Column<string>(nullable: false),
                    Regular = table.Column<string>(nullable: true),
                    IsEnable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    EditedTime = table.Column<DateTimeOffset>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CretaedUser = table.Column<string>(maxLength: 50, nullable: true),
                    EditeUser = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    NormalizedUserName = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Sex = table.Column<int>(nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    SecurityAudit = table.Column<string>(nullable: true),
                    UserClaimId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserClaims_UserClaimId",
                        column: x => x.UserClaimId,
                        principalTable: "UserClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    EditedTime = table.Column<DateTimeOffset>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CretaedUser = table.Column<string>(maxLength: 50, nullable: true),
                    EditeUser = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Controller = table.Column<string>(nullable: false),
                    Action = table.Column<string>(nullable: false),
                    Params = table.Column<string>(nullable: true),
                    IsAllowAnonymous = table.Column<bool>(nullable: false),
                    IsEnable = table.Column<bool>(nullable: false),
                    AreaName = table.Column<string>(nullable: true),
                    ShowSort = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    EditedTime = table.Column<DateTimeOffset>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CretaedUser = table.Column<string>(maxLength: 50, nullable: true),
                    EditeUser = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    ProviderKey = table.Column<string>(nullable: true),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserOrgs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    EditedTime = table.Column<DateTimeOffset>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CretaedUser = table.Column<string>(maxLength: 50, nullable: true),
                    EditeUser = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    OrganizationId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOrgs_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOrgs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    EditedTime = table.Column<DateTimeOffset>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CretaedUser = table.Column<string>(maxLength: 50, nullable: true),
                    EditeUser = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    EditedTime = table.Column<DateTimeOffset>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CretaedUser = table.Column<string>(maxLength: 50, nullable: true),
                    EditeUser = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    EditedTime = table.Column<DateTimeOffset>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CretaedUser = table.Column<string>(maxLength: 50, nullable: true),
                    EditeUser = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NormalizedName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    UserRoleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false),
                    EditedTime = table.Column<DateTimeOffset>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CretaedUser = table.Column<string>(maxLength: 50, nullable: true),
                    EditeUser = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    PermissionId = table.Column<string>(nullable: false),
                    ClaimValue = table.Column<string>(nullable: true),
                    Regular = table.Column<string>(nullable: true),
                    IsEnable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_UserId",
                table: "Permission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserId",
                table: "Roles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserRoleId",
                table: "Roles",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrgs_OrganizationId",
                table: "UserOrgs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrgs_UserId",
                table: "UserOrgs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserClaimId",
                table: "Users",
                column: "UserClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserOrgs");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserClaims");
        }
    }
}
