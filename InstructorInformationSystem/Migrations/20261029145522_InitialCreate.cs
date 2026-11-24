using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstructorInformationSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Some existing local databases were created before migrations were enabled.
            // Preserve those tables and create only tables that are missing.
            migrationBuilder.Sql(
                """
                CREATE TABLE IF NOT EXISTS "Instructors" (
                    "Id" INTEGER NOT NULL CONSTRAINT "PK_Instructors" PRIMARY KEY AUTOINCREMENT,
                    "FullName" TEXT NOT NULL,
                    "Age" INTEGER NOT NULL,
                    "Address" TEXT NOT NULL,
                    "Designation" TEXT NOT NULL,
                    "Gender" TEXT NOT NULL,
                    "ContactNumber" TEXT NOT NULL,
                    "Majorship" TEXT NOT NULL,
                    "BachelorDegree" TEXT NOT NULL,
                    "EmailAddress" TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS "UserAccounts" (
                    "Id" INTEGER NOT NULL CONSTRAINT "PK_UserAccounts" PRIMARY KEY AUTOINCREMENT,
                    "CreatedAt" TEXT NOT NULL DEFAULT '1970-01-01 00:00:00',
                    "AdminId" TEXT NOT NULL,
                    "FirstName" TEXT NOT NULL,
                    "FullName" TEXT NOT NULL,
                    "LastName" TEXT NOT NULL,
                    "Email" TEXT NOT NULL,
                    "PasswordHash" TEXT NOT NULL
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}
