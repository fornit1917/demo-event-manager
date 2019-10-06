using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager.Migrations {
    [Migration(201910061315)]
    public class FillTestData : Migration {
        public override void Up() {
            string sql = @"
                DECLARE @i int = 1
                WHILE @i <= 20
                BEGIN
	                INSERT INTO [Event] ([Name], [Place], [MaxGuests], [EventDate], [Type])
                    VALUES (CONCAT('Event ', @i), CONCAT('Place ', @i), 30, '2019-10-10', @i % 3);

	                DECLARE @eventId int = SCOPE_IDENTITY();

	                DECLARE @j int = 1;
	                WHILE @j <= 20	
	                BEGIN
		                INSERT INTO [Guest] ([Email], [Name], [Comment], [EventId])
		                VALUES (CONCAT('guest.', @j, '@test.com'), CONCAT('Guest ', @j), CONCAT('Comment ', @j), @eventId);

		                SET @j = @j + 1;
	                END

	                SET @i = @i + 1;
                END
            ";

            Execute.Sql(sql);
        }

        public override void Down() {
            throw new NotImplementedException();
        }
    }
}
