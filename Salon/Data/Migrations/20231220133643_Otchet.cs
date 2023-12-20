using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salon.Data.Migrations
{
    /// <inheritdoc />
    public partial class Otchet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE Otchet
	        --@id int
            AS
            BEGIN
	            -- SET NOCOUNT ON added to prevent extra result sets from
	            -- interfering with SELECT statements.
	            SET NOCOUNT ON;

                -- Insert statements for procedure here
	            SELECT N.[ServiceId] id,[ServiceName] nm, COUNT(P.VisitId) kol FROM [dbo].[Services] N
		            JOIN [dbo].[Visits] P ON N.ServiceId=P.ServiceId
			            --WHERE VisitId=@id
	            --GROUP BY [ServiceName]
                GROUP BY N.[ServiceId],[ServiceName]
            END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE Otchet");
        }
    }
}
