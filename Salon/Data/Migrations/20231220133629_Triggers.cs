using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salon.Data.Migrations
{
    /// <inheritdoc />
    public partial class Triggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[Check_Group]
               ON  [dbo].[Groups]
               AFTER INSERT
            AS 
            BEGIN
	            -- SET NOCOUNT ON added to prevent extra result sets from
	            -- interfering with SELECT statements.
	            IF EXISTS (SELECT 1 FROM inserted WHERE GroupName IS NULL)
                    BEGIN
                        RAISERROR('Название события не может быть пустым', 50, 2)
                        ROLLBACK TRANSACTION
                        RETURN
                    END

            END");

            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[AddDelCount]
            ON  [dbo].[Services]
            AFTER INSERT, DELETE
            AS 
            BEGIN
        	    -- SET NOCOUNT ON added to prevent extra result sets from
        	    -- interfering with SELECT statements.
        	    SET NOCOUNT ON;


        	    DECLARE @GroupId int;
                DECLARE @Count int;       

                SELECT TOP 1 @GroupId=[GroupId] FROM INSERTED;
                if (@GroupId IS NULL) BEGIN
                    SELECT TOP 1 @GroupId=[GroupId] FROM DELETED;
                END
                
                SET @Count = (SELECT COUNT(*) FROM [dbo].[Services] WHERE [GroupId]=@GroupId)
                UPDATE [dbo].[Groups] SET [Services_Count]=@Count WHERE [GroupId]=@GroupId
               
            END");


            migrationBuilder.Sql(@"CREATE TRIGGER [dbo].[UpdCount]
            ON  [dbo].[Services]
            AFTER UPDATE
            AS 
            BEGIN
        	    -- SET NOCOUNT ON added to prevent extra result sets from
        	    -- interfering with SELECT statements.
        	    SET NOCOUNT ON;


        	    DECLARE @GroupId int;
                DECLARE @GroupId2 int;
                DECLARE @Count int;       

                SELECT TOP 1 @GroupId=[GroupId] FROM INSERTED;
                SELECT TOP 1 @GroupId2=[GroupId] FROM DELETED;
                
                SET @Count = (SELECT COUNT(*) FROM [dbo].[Services] WHERE [GroupId]=@GroupId)
                UPDATE [dbo].[Groups] SET [Services_Count]=@Count WHERE [GroupId]=@GroupId

                SET @Count = (SELECT COUNT(*) FROM [dbo].[Services] WHERE [GroupId]=@GroupId2)
                UPDATE [dbo].[Groups] SET [Services_Count]=@Count WHERE [GroupId]=@GroupId2 
               
            END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER [dbo].[Check_Group]");

            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS [dbo].[AddDelCount]");
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS [dbo].[UpdCount]");
        }
    }
}
