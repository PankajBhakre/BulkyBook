using Microsoft.EntityFrameworkCore.Migrations;

namespace BulkyBook.DataAccess.Migrations
{
    public partial class addStoredProcforCovertype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC usp_GetCoverTypes 
                                    AS 
                                    BEGIN 
                                     SELECT * FROM   dbo.CoverTypes 
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_GetCoverType 
                                    @ID int 
                                    AS 
                                    BEGIN 
                                     SELECT * FROM   dbo.CoverTypes  WHERE  (ID = @ID) 
                                    END ");

            migrationBuilder.Sql(@"CREATE PROC usp_UpdateCoverType
	                                @ID int,
	                                @Name varchar(100)
                                    AS 
                                    BEGIN 
                                     UPDATE dbo.CoverTypes
                                     SET  NAME = @NAME
                                     WHERE  ID = @ID
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_DeleteCoverType
	                                @ID int
                                    AS 
                                    BEGIN 
                                     DELETE FROM dbo.CoverTypes
                                     WHERE  ID = @ID
                                    END");

            migrationBuilder.Sql(@"CREATE PROC usp_CreateCoverType
                                   @NAME varchar(100)
                                   AS 
                                   BEGIN 
                                    INSERT INTO dbo.CoverTypes(NAME)
                                    VALUES (@NAME)
                                   END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE usp_GetCoverTypes");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_GetCoverType");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_UpdateCoverType");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_DeleteCoverType");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_CreateCoverType");
        }
    }
}
