using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyLibrary.Migrations
{
    public partial class Collections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookCollections",
                columns: table => new
                {
                    BookCollectionId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UkrainianId = table.Column<int>(nullable: false),
                    AlgebraId = table.Column<int>(nullable: false),
                    GeometryId = table.Column<int>(nullable: false),
                    EnglishId = table.Column<int>(nullable: false),
                    RussianId = table.Column<int>(nullable: false),
                    UkraineHistoryId = table.Column<int>(nullable: false),
                    WorldHistoryId = table.Column<int>(nullable: false),
                    ChemistryId = table.Column<int>(nullable: false),
                    BiologyId = table.Column<int>(nullable: false),
                    InformaticsId = table.Column<int>(nullable: false),
                    MusicId = table.Column<int>(nullable: false),
                    LiteratureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCollections", x => x.BookCollectionId);
                    table.ForeignKey(
                        name: "FK_BookCollections_BookObjects_AlgebraId",
                        column: x => x.AlgebraId,
                        principalTable: "BookObjects",
                        principalColumn: "BookObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollections_BookObjects_BiologyId",
                        column: x => x.BiologyId,
                        principalTable: "BookObjects",
                        principalColumn: "BookObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollections_BookObjects_ChemistryId",
                        column: x => x.ChemistryId,
                        principalTable: "BookObjects",
                        principalColumn: "BookObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollections_BookObjects_EnglishId",
                        column: x => x.EnglishId,
                        principalTable: "BookObjects",
                        principalColumn: "BookObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollections_BookObjects_GeometryId",
                        column: x => x.GeometryId,
                        principalTable: "BookObjects",
                        principalColumn: "BookObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollections_BookObjects_InformaticsId",
                        column: x => x.InformaticsId,
                        principalTable: "BookObjects",
                        principalColumn: "BookObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollections_BookObjects_LiteratureId",
                        column: x => x.LiteratureId,
                        principalTable: "BookObjects",
                        principalColumn: "BookObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollections_BookObjects_MusicId",
                        column: x => x.MusicId,
                        principalTable: "BookObjects",
                        principalColumn: "BookObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollections_BookObjects_RussianId",
                        column: x => x.RussianId,
                        principalTable: "BookObjects",
                        principalColumn: "BookObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollections_BookObjects_UkraineHistoryId",
                        column: x => x.UkraineHistoryId,
                        principalTable: "BookObjects",
                        principalColumn: "BookObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollections_BookObjects_UkrainianId",
                        column: x => x.UkrainianId,
                        principalTable: "BookObjects",
                        principalColumn: "BookObjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCollections_BookObjects_WorldHistoryId",
                        column: x => x.WorldHistoryId,
                        principalTable: "BookObjects",
                        principalColumn: "BookObjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_AlgebraId",
                table: "BookCollections",
                column: "AlgebraId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_BiologyId",
                table: "BookCollections",
                column: "BiologyId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_ChemistryId",
                table: "BookCollections",
                column: "ChemistryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_EnglishId",
                table: "BookCollections",
                column: "EnglishId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_GeometryId",
                table: "BookCollections",
                column: "GeometryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_InformaticsId",
                table: "BookCollections",
                column: "InformaticsId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_LiteratureId",
                table: "BookCollections",
                column: "LiteratureId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_MusicId",
                table: "BookCollections",
                column: "MusicId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_RussianId",
                table: "BookCollections",
                column: "RussianId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_UkraineHistoryId",
                table: "BookCollections",
                column: "UkraineHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_UkrainianId",
                table: "BookCollections",
                column: "UkrainianId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_UserId",
                table: "BookCollections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollections_WorldHistoryId",
                table: "BookCollections",
                column: "WorldHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCollections");
        }
    }
}
