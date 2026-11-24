using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstructorInformationSystem.Migrations
{
    /// <inheritdoc />
    public partial class PersistSubjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubjectCode = table.Column<string>(type: "TEXT", nullable: false),
                    SubjectName = table.Column<string>(type: "TEXT", nullable: false),
                    Units = table.Column<int>(type: "INTEGER", nullable: false),
                    InstructorName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectId);
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "SubjectId", "SubjectCode", "SubjectName", "Units", "InstructorName" },
                values: new object[,]
                {
                    { 1, "IT312", "Systems Integration and Architecture", 3, "Prof. Dela Cruz" },
                    { 2, "IT313", "Capstone Project 1", 3, "Prof. Santos" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
