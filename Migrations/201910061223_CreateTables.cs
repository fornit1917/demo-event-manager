using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager.Migrations {
    [Migration(201910061223)]
    public class CreateTables : Migration {
        public override void Up() {
            var sql = @"
                CREATE TABLE [dbo].[Event](
	                [Id] [int] NOT NULL IDENTITY(1,1),
	                [Name] [nvarchar](500) NOT NULL,
	                [Place] [nvarchar](500) NOT NULL,
	                [Type] [smallint] NOT NULL,
	                [MaxGuests] [int] NOT NULL,
	                [IsArchived] [bit] NOT NULL DEFAULT 0,
	                CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED ([Id] ASC)
                );
                GO

                CREATE INDEX IX_Event_IsArchived ON [Event]([IsArchived]);
                GO

                CREATE TABLE [dbo].[Guest] (
	                [Id] [int] NOT NULL IDENTITY(1,1),
	                [Email] [nvarchar](100) NOT NULL,
	                [Name] [nvarchar](200) NOT NULL,
	                [Comment] [nvarchar](500) NOT NULL,
	                [EventId] [int] NOT NULL,

	                CONSTRAINT [PK_Guest] PRIMARY KEY CLUSTERED ([Id] ASC),
	                CONSTRAINT [FK_Guest_Event] FOREIGN KEY ([EventId]) REFERENCES [dbo].[Event]([Id]) ON DELETE CASCADE ON UPDATE CASCADE
                );
                GO

                CREATE UNIQUE INDEX IX_Guest_EventEmail ON [Guest]([EventId], [Email]);
                GO
            ";

            Execute.Sql(sql);
        }

        public override void Down() {
            Execute.Sql("DROP TABLE [dbo].[Guest]; DROP TABLE [dbo].[Event];");
        }
    }
}
