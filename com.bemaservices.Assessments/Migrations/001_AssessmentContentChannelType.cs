using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rock;
using Rock.Model;
using Rock.Plugin;

namespace com.bemaservices.Assessments.Migrations
{
    [MigrationNumber( 1, "1.11.2" )]
    class AssessmentContentChannelType : Migration
    {
        public override void Up()
        {
            Sql( @"
            INSERT INTO [dbo].[ContentChannelType]
                       ([IsSystem]
                       ,[Name]
                       ,[DateRangeType]
                       ,[Guid]
                       ,[DisablePriority]
                       ,[IncludeTime]
                       ,[DisableContentField]
                       ,[DisableStatus]
                       ,[ShowInChannelList])
                 VALUES
                       (1
                       ,'Assessments'
                       ,3
                       ,'3E42C0B9-9ABC-4136-BE22-140349A65A86'
                       ,1
                       ,0
                       ,0
                       ,1
                       ,1)" );
        }

        public override void Down()
        {

        }
    }
}
