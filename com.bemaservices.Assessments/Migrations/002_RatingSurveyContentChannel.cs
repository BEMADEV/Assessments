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
    [MigrationNumber( 2, "1.11.2" )]
    class RatingSurveyContentChannel : Migration
    {
        public override void Up()
        {
            // Setup Attribute Matrix: Assessment Questions
            Sql( @"
            DECLARE @TemplateGuid uniqueidentifier= 'CA0119FD-AC77-48C1-890F-34E177CED793'
            DECLARE @FieldOneAttributeGuid uniqueidentifier = '8F6D8239-7D4D-45E4-8F54-F15E29A96362'
            DECLARE @FieldTwoAttributeGuid uniqueidentifier = '182C4242-2B90-464D-8F43-7F2586932956'
                        
            DECLARE @AttributeMatrixItemEntityTypeId INT = (SELECT TOP 1 Id FROM EntityType WHERE [Name] = 'Rock.Model.AttributeMatrixItem');
            DECLARE @AttributeFieldTypeId INT = (SELECT TOP 1 Id FROM FieldType WHERE [Guid] = TRY_CAST('99B090AA-4D7E-46D8-B393-BF945EA1BA8B' AS uniqueidentifier));
            DECLARE @TextFieldTypeId INT = (SELECT TOP 1 Id FROM FieldType WHERE [Guid] = TRY_CAST('9C204CD0-1233-41C5-818A-C5DA439445AA' AS uniqueidentifier));

            INSERT INTO [dbo].[AttributeMatrixTemplate] (
                [Name]
                ,[Description]
                ,[IsActive]
                ,[FormattedLava]
                ,[Guid]
            )
            VALUES (
                'Assessment Questions'
                ,'A System Template used by the Assessments Plugin. Do Not Delete.'
                ,1
                ,'{% if AttributeMatrixItems != empty %}  <table class=''grid-table table table-condensed table-light''> <thead> <tr> {% for itemAttribute in ItemAttributes %}     <th>{{ itemAttribute.Name }}</th> {% endfor %} </tr> </thead> <tbody> {% for attributeMatrixItem in AttributeMatrixItems %} <tr>     {% for itemAttribute in ItemAttributes %}         <td>{{ attributeMatrixItem | Attribute:itemAttribute.Key }}</td>     {% endfor %} </tr> {% endfor %} </tbody> </table>  {% endif %}'
                ,@TemplateGuid
            )

            DECLARE @AttributeMatrixTemplateId INT = (SELECT TOP 1 Id FROM AttributeMatrixTemplate WHERE [Guid] = @TemplateGuid);

            INSERT INTO [dbo].[Attribute] (
	                [IsSystem]
	            ,[FieldTypeId]
	            ,[EntityTypeId]
	            ,[EntityTypeQualifierColumn]
	            ,[EntityTypeQualifierValue]
	            ,[Key]
	            ,[Name]
	            ,[Description]
	            ,[Order]
	            ,[IsGridColumn]
	            ,[IsMultiValue]
	            ,[IsRequired]
	            ,[Guid]
	            ,[AllowSearch]
	            ,[IsIndexEnabled]
	            ,[IsAnalytic]
	            ,[IsAnalyticHistory]
                ,[IsActive]
                ,[EnableHistory]
            )
            VALUES
            (1, @TextFieldTypeId, @AttributeMatrixItemEntityTypeId, 'AttributeMatrixTemplateId', convert(nvarchar(max),@AttributeMatrixTemplateId), 'QuestionText', 'Question Text', '', 0, 0, 0, 1, @FieldOneAttributeGuid, 0, 0, 0, 0, 1, 1 ),
            (1, @AttributeFieldTypeId, @AttributeMatrixItemEntityTypeId, 'AttributeMatrixTemplateId', convert(nvarchar(max),@AttributeMatrixTemplateId), 'PersonAttribute', 'Person Attribute', '', 1, 0, 0, 0, @FieldTwoAttributeGuid, 0, 0, 0, 0, 1, 1 )
 
             DECLARE @AttributeAttributeId INT = (SELECT TOP 1 Id FROM Attribute WHERE [Guid] = @FieldTwoAttributeGuid);

            INSERT INTO [dbo].[AttributeQualifier]
                       ([IsSystem]
                       ,[AttributeId]
                       ,[Key]
                       ,[Value]
                       ,[Guid])
                 VALUES
                       (1
                       ,@AttributeAttributeId
                       ,'entitytype'
                       ,'72657ed8-d16e-492e-ac12-144c5e7567e7'
                       ,'98220C92-C141-4390-A96E-C3275A90266B')
             " );

            // Setup Attribute Matrix: Assessment Results
            Sql( @"
            DECLARE @TemplateGuid uniqueidentifier= '584788CE-EB34-4C39-87E8-51361BCF7C9F'
            DECLARE @FieldOneAttributeGuid uniqueidentifier = '95A38C87-6F11-48F0-96BC-71524841691B'
            DECLARE @FieldTwoAttributeGuid uniqueidentifier = 'E8B5DB6C-AE3E-4292-9B15-F6870AFEA70D'
            DECLARE @FieldThreeAttributeGuid uniqueidentifier = '4774704B-CB7E-4A4E-B4E7-106865F3BECB'
                        
            DECLARE @AttributeMatrixItemEntityTypeId INT = (SELECT TOP 1 Id FROM EntityType WHERE [Name] = 'Rock.Model.AttributeMatrixItem');
            DECLARE @AttributeFieldTypeId INT = (SELECT TOP 1 Id FROM FieldType WHERE [Guid] = TRY_CAST('99B090AA-4D7E-46D8-B393-BF945EA1BA8B' AS uniqueidentifier));
            DECLARE @HtmlFieldTypeId INT = (SELECT TOP 1 Id FROM FieldType WHERE [Guid] = TRY_CAST('DD7ED4C0-A9E0-434F-ACFE-BD4F56B043DF' AS uniqueidentifier));

            INSERT INTO [dbo].[AttributeMatrixTemplate] (
                [Name]
                ,[Description]
                ,[IsActive]
                ,[FormattedLava]
                ,[Guid]
            )
            VALUES (
                'Assessment Results'
                ,'A System Template used by the Assessments Plugin. Do Not Delete.'
                ,1
                ,'{% if AttributeMatrixItems != empty %}  <table class=''grid-table table table-condensed table-light''> <thead> <tr> {% for itemAttribute in ItemAttributes %}     <th>{{ itemAttribute.Name }}</th> {% endfor %} </tr> </thead> <tbody> {% for attributeMatrixItem in AttributeMatrixItems %} <tr>     {% for itemAttribute in ItemAttributes %}         <td>{{ attributeMatrixItem | Attribute:itemAttribute.Key }}</td>     {% endfor %} </tr> {% endfor %} </tbody> </table>  {% endif %}'
                ,@TemplateGuid
            )

            DECLARE @AttributeMatrixTemplateId INT = (SELECT TOP 1 Id FROM AttributeMatrixTemplate WHERE [Guid] = @TemplateGuid);

            INSERT INTO [dbo].[Attribute] (
	                [IsSystem]
	            ,[FieldTypeId]
	            ,[EntityTypeId]
	            ,[EntityTypeQualifierColumn]
	            ,[EntityTypeQualifierValue]
	            ,[Key]
	            ,[Name]
	            ,[Description]
	            ,[Order]
	            ,[IsGridColumn]
	            ,[IsMultiValue]
	            ,[IsRequired]
	            ,[Guid]
	            ,[AllowSearch]
	            ,[IsIndexEnabled]
	            ,[IsAnalytic]
	            ,[IsAnalyticHistory]
                ,[IsActive]
                ,[EnableHistory]
            )
            VALUES
            (1, @AttributeFieldTypeId, @AttributeMatrixItemEntityTypeId, 'AttributeMatrixTemplateId', convert(nvarchar(max),@AttributeMatrixTemplateId), 'ResultAttribute', 'Result Attribute', '', 0, 0, 0, 1, @FieldOneAttributeGuid, 0, 0, 0, 0, 1, 1 ),
            (1, @HtmlFieldTypeId, @AttributeMatrixItemEntityTypeId, 'AttributeMatrixTemplateId', convert(nvarchar(max),@AttributeMatrixTemplateId), 'PrimaryDescription', 'Primary Description', '', 1, 0, 0, 0, @FieldTwoAttributeGuid, 0, 0, 0, 0, 1, 1 ),
            (1, @HtmlFieldTypeId, @AttributeMatrixItemEntityTypeId, 'AttributeMatrixTemplateId', convert(nvarchar(max),@AttributeMatrixTemplateId), 'SecondaryDescription', 'Secondary Description', '', 2, 0, 0, 0, @FieldThreeAttributeGuid, 0, 0, 0, 0, 1, 1 )
 
                DECLARE @AttributeAttributeId INT = (SELECT TOP 1 Id FROM Attribute WHERE [Guid] = @FieldOneAttributeGuid);

            INSERT INTO [dbo].[AttributeQualifier]
                        ([IsSystem]
                        ,[AttributeId]
                        ,[Key]
                        ,[Value]
                        ,[Guid])
                    VALUES
                        (1
                        ,@AttributeAttributeId
                        ,'entitytype'
                        ,'72657ed8-d16e-492e-ac12-144c5e7567e7'
                        ,'5DFB1814-6028-46C6-9E8A-F2BAC2631B45')
             " );

            // Add Content Channel
            Sql( @"DECLARE @ContentChannelTypeId INT = (SELECT TOP 1 Id FROM ContentChannelType WHERE [Guid] = '3E42C0B9-9ABC-4136-BE22-140349A65A86');

                INSERT INTO [dbo].[ContentChannel]
                           ([ContentChannelTypeId]
                           ,[Name]
                           ,[Description]
                           ,[RequiresApproval]
                           ,[EnableRss]
                           ,[Guid]
                           ,[ContentControlType]
                           ,[ItemsManuallyOrdered]
                           ,[ChildItemsManuallyOrdered]
                           ,[IsIndexEnabled]
                           ,[IsTaggingEnabled]
                           ,[IsStructuredContent])
                     VALUES
                           (@ContentChannelTypeId
                           ,'Rating Survey'
                           ,'A simple assessment with ""Rate on a scale from 1 - 5"" questions that a user can fill out.'
                           , 0
                           ,0
                           ,'DF49BACD-60E0-4D45-9092-5E18290DAC3B'
                           ,1
                           ,0
                           ,0
                           ,0
                           ,0
                           ,0)" );


            var contentChannelId = SqlScalar( "Select Top 1 [Id] From ContentChannel Where Guid = 'DF49BACD-60E0-4D45-9092-5E18290DAC3B'" ).ToString();
            var questionMatrixTemplateId = SqlScalar( "Select Top 1 [Id] From AttributeMatrixTemplate Where Guid = 'CA0119FD-AC77-48C1-890F-34E177CED793'" ).ToString();
            var resultMatrixTemplateId = SqlScalar( "Select Top 1 [Id] From AttributeMatrixTemplate Where Guid = '584788CE-EB34-4C39-87E8-51361BCF7C9F'" ).ToString();

            // Entity: Rock.Model.ContentChannelItem Attribute: Assessment Results
            RockMigrationHelper.AddOrUpdateEntityAttribute( "Rock.Model.ContentChannelItem", "F16FC460-DC1E-4821-9012-5F21F974C677", "ContentChannelId", contentChannelId, "Assessment Results", "Assessment Results", @"", 1000, @"", "128793E9-B628-41A9-98C7-80ED2EC8AFCE", "AssessmentResults" );
            RockMigrationHelper.UpdateAttributeQualifier( "128793E9-B628-41A9-98C7-80ED2EC8AFCE", "attributematrixtemplate", resultMatrixTemplateId, "3E32514A-943A-4BC4-9497-6B479C071C32" );

            // Entity: Rock.Model.ContentChannelItem Attribute: Assessment Questions
            RockMigrationHelper.AddOrUpdateEntityAttribute( "Rock.Model.ContentChannelItem", "F16FC460-DC1E-4821-9012-5F21F974C677", "ContentChannelId", contentChannelId, "Assessment Questions", "Assessment Questions", @"", 1001, @"", "3DBFC388-B15E-44D2-8FB1-FC0075D721F4", "AssessmentQuestions" );
            RockMigrationHelper.UpdateAttributeQualifier( "3DBFC388-B15E-44D2-8FB1-FC0075D721F4", "attributematrixtemplate", questionMatrixTemplateId, "3CC2690C-852F-4F25-8507-E6A9170321FA" );

            // Entity: Rock.Model.ContentChannelItem Attribute: Submission Date Attribute
            RockMigrationHelper.AddOrUpdateEntityAttribute( "Rock.Model.ContentChannelItem", "99B090AA-4D7E-46D8-B393-BF945EA1BA8B", "ContentChannelId", contentChannelId, "Submission Date Attribute", "Submission Date Attribute", @"To what Person Attribute should we save the date this assessment was taken?", 1002, @"", "DF4F40AD-45FE-4726-AAB2-44C3F4FDE786", "SubmissionDateAttribute" );
            RockMigrationHelper.UpdateAttributeQualifier( "DF4F40AD-45FE-4726-AAB2-44C3F4FDE786", "allowmultiple", @"False", "55B5C1AE-143B-4A14-A25A-1E5819995E33" );
            RockMigrationHelper.UpdateAttributeQualifier( "DF4F40AD-45FE-4726-AAB2-44C3F4FDE786", "entitytype", @"72657ed8-d16e-492e-ac12-144c5e7567e7", "0A68C8B4-B339-4DCC-A207-B3EB02B291FE" );
            RockMigrationHelper.UpdateAttributeQualifier( "DF4F40AD-45FE-4726-AAB2-44C3F4FDE786", "qualifierColumn", @"", "BEB9CBE7-0B5B-479E-B906-B80C497912E5" );
            RockMigrationHelper.UpdateAttributeQualifier( "DF4F40AD-45FE-4726-AAB2-44C3F4FDE786", "qualifierValue", @"", "114AA143-5EA9-45BA-93FE-16B793F2FFAF" );

            // Entity: Rock.Model.ContentChannelItem Attribute: Result Lava
            RockMigrationHelper.AddOrUpdateEntityAttribute( "Rock.Model.ContentChannelItem", "27718256-C1EB-4B1F-9B4B-AC53249F78DF", "ContentChannelId", contentChannelId, "Result Lava", "Result Lava", @"", 1003, @"", "8CB41BDE-477B-479F-BA6D-36A910E838AF", "ResultLava" );
            RockMigrationHelper.UpdateAttributeQualifier( "8CB41BDE-477B-479F-BA6D-36A910E838AF", "editorHeight", @"", "DAF41F46-86CE-4C89-9EFA-3E6D5947AC54" );
            RockMigrationHelper.UpdateAttributeQualifier( "8CB41BDE-477B-479F-BA6D-36A910E838AF", "editorMode", @"0", "EEEACF95-28A4-4D14-9719-455C0DD5EF8B" );

        }

        public override void Down()
        {
            RockMigrationHelper.DeleteAttribute( "128793E9-B628-41A9-98C7-80ED2EC8AFCE" ); // Rock.Model.ContentChannelItem: Assessment Results  
            RockMigrationHelper.DeleteAttribute( "3DBFC388-B15E-44D2-8FB1-FC0075D721F4" ); // Rock.Model.ContentChannelItem: Assessment Questions  
            RockMigrationHelper.DeleteAttribute( "DF4F40AD-45FE-4726-AAB2-44C3F4FDE786" ); // Rock.Model.ContentChannelItem: Submission Date Attribute  

        }
    }
}
