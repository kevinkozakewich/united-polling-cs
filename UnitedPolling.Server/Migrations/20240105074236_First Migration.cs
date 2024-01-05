using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnitedPolling.Server.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AspId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastLoggedIn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Poll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UrlShareable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poll", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Poll_User_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Poll_User_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PollAdministrators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PollId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollAdministrators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollAdministrators_Poll_PollId",
                        column: x => x.PollId,
                        principalTable: "Poll",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PollAdministrators_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollParticipants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PollId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollParticipants_Poll_PollId",
                        column: x => x.PollId,
                        principalTable: "Poll",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PollParticipants_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PollId = table.Column<int>(type: "int", nullable: false),
                    QuestionType = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Question = table.Column<string>(type: "VARCHAR(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollQuestion_Poll_PollId",
                        column: x => x.PollId,
                        principalTable: "Poll",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollQuestionOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PollQuestionId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollQuestionOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollQuestionOption_PollQuestion_PollQuestionId",
                        column: x => x.PollQuestionId,
                        principalTable: "PollQuestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollQuestionResponse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PollQuestionOptionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WrittenResponse = table.Column<string>(type: "VARCHAR(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollQuestionResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollQuestionResponse_PollQuestionOption_PollQuestionOptionId",
                        column: x => x.PollQuestionOptionId,
                        principalTable: "PollQuestionOption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PollQuestionResponse_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Poll_CreatedUserId",
                table: "Poll",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Poll_UpdatedUserId",
                table: "Poll",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PollAdministrators_PollId",
                table: "PollAdministrators",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_PollAdministrators_UserId",
                table: "PollAdministrators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PollParticipants_PollId",
                table: "PollParticipants",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_PollParticipants_UserId",
                table: "PollParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PollQuestion_PollId",
                table: "PollQuestion",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_PollQuestionOption_PollQuestionId",
                table: "PollQuestionOption",
                column: "PollQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_PollQuestionResponse_PollQuestionOptionId",
                table: "PollQuestionResponse",
                column: "PollQuestionOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PollQuestionResponse_UserId",
                table: "PollQuestionResponse",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PollAdministrators");

            migrationBuilder.DropTable(
                name: "PollParticipants");

            migrationBuilder.DropTable(
                name: "PollQuestionResponse");

            migrationBuilder.DropTable(
                name: "PollQuestionOption");

            migrationBuilder.DropTable(
                name: "PollQuestion");

            migrationBuilder.DropTable(
                name: "Poll");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
