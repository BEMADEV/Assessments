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
    [MigrationNumber( 3, "1.11.2" )]
    class RatingSurveyPages : Migration
    {
        public override void Up()
        {
            // Page: Rating Survey
            RockMigrationHelper.AddPage( "FCF44690-D74C-4FB7-A01B-0EFCA6EA9E1E", "5FEAF34C-7FB6-4A11-8A1E-C452EC7849BD", "Rating Survey", "", "D112CF16-533D-464A-B5F8-E0682AD9A73F", "" ); // Site:External Website
            RockMigrationHelper.UpdateBlockType( "Rating Survey", "Allows you to take a Rating Survey test and saves your Rating Survey score.", "~/Plugins/com_bemaservices/Assessments/RatingSurvey.ascx", "BEMA Services > Assessments", "03459C30-F239-4C2C-B9FA-6A71EB0CDC46" );
            RockMigrationHelper.UpdatePageBreadcrumb( "D112CF16-533D-464A-B5F8-E0682AD9A73F", false );

            // Page: Rating Survey Results
            RockMigrationHelper.AddPage( "D112CF16-533D-464A-B5F8-E0682AD9A73F", "5FEAF34C-7FB6-4A11-8A1E-C452EC7849BD", "Rating Survey Results", "", "3BA5F5A4-A7C2-473A-857E-4B6577B34B17", "" ); // Site:External Website
            RockMigrationHelper.UpdateBlockType( "Rating Survey Results", "View the results of a Rating Survey.", "~/Plugins/com_bemaservices/Assessments/RatingSurveyResults.ascx", "BEMA Services > Assessments", "BC0A3D28-E8BE-4AD9-96A5-D42B2C3F705D" );
            RockMigrationHelper.UpdatePageBreadcrumb( "3BA5F5A4-A7C2-473A-857E-4B6577B34B17", false );

            // Add Block to Page: Rating Survey, Site: External Website
            RockMigrationHelper.AddBlock( true, "D112CF16-533D-464A-B5F8-E0682AD9A73F", "", "03459C30-F239-4C2C-B9FA-6A71EB0CDC46", "Rating Survey", "Main", "", "", 0, "402B40CC-7BE9-4CF0-BD58-E246192C5D19" );
            // Attrib for BlockType: Rating Survey:Assessment Id Param
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "03459C30-F239-4C2C-B9FA-6A71EB0CDC46", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Assessment Id Param", "AssessmentIdParam", "Assessment Id Param", @"The Page Parameter that will be used to set the AssessmentId value", 0, @"Assessment", "9C58F87D-B185-4D90-B606-95C6142C2DC3" );
            // Attrib for BlockType: Rating Survey:Results Page
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "03459C30-F239-4C2C-B9FA-6A71EB0CDC46", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Results Page", "ResultsPage", "Results Page", @"The page a user will be taken to to view their results. If none is listed, the user will be taken to the parent page.", 1, @"", "7ABDD3EC-B4F7-4F8F-A6F2-F2CA9EECD3F7" );
            // Attrib Value for Block:Rating Survey, Attribute:Assessment Id Param Page: Rating Survey, Site: External Website
            RockMigrationHelper.AddBlockAttributeValue( "402B40CC-7BE9-4CF0-BD58-E246192C5D19", "9C58F87D-B185-4D90-B606-95C6142C2DC3", @"Assessment" );
            // Attrib Value for Block:Rating Survey, Attribute:Results Page Page: Rating Survey, Site: External Website
            RockMigrationHelper.AddBlockAttributeValue( "402B40CC-7BE9-4CF0-BD58-E246192C5D19", "7ABDD3EC-B4F7-4F8F-A6F2-F2CA9EECD3F7", @"3ba5f5a4-a7c2-473a-857e-4b6577b34b17" );

            // Add Block to Page: Rating Survey Results, Site: External Website
            RockMigrationHelper.AddBlock( true, "3BA5F5A4-A7C2-473A-857E-4B6577B34B17", "", "BC0A3D28-E8BE-4AD9-96A5-D42B2C3F705D", "Rating Survey Results", "Main", "", "", 0, "FD1B5723-9000-4890-B7BD-8CF2212641FE" );
            // Attrib for BlockType: Rating Survey Results:Entry Page
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "BC0A3D28-E8BE-4AD9-96A5-D42B2C3F705D", "BD53F9C9-EBA9-4D3F-82EA-DE5DD34A8108", "Entry Page", "EntryPage", "Entry Page", @"The page a user will be taken to to retake the survey. ", 0, @"", "2930C377-6119-4425-9937-D9FCCA9F25BF" );
            // Attrib Value for Block:Rating Survey Results, Attribute:Entry Page Page: Rating Survey Results, Site: External Website
            RockMigrationHelper.AddBlockAttributeValue( "FD1B5723-9000-4890-B7BD-8CF2212641FE", "2930C377-6119-4425-9937-D9FCCA9F25BF", @"d112cf16-533d-464a-b5f8-e0682ad9a73f" );

            // Page: Survey List
            RockMigrationHelper.AddPage( "C0854F84-2E8B-479C-A3FB-6B47BE89B795", "5FEAF34C-7FB6-4A11-8A1E-C452EC7849BD", "Survey List", "", "7FFC5F22-B177-44AB-94C4-2AE3A16BCC37", "" ); // Site:External Website
            // Add Block to Page: Survey List, Site: External Website
            RockMigrationHelper.AddBlock( true, "7FFC5F22-B177-44AB-94C4-2AE3A16BCC37", "", "19B61D65-37E3-459F-A44F-DEF0089118A3", "HTML Content", "Main", "", "", 0, "B0A06BE9-1BBF-4A82-A0D7-EFCD05C5DCA8" );

            // Attrib Value for Block:HTML Content, Attribute:Cache Duration Page: Survey List, Site: External Website
            RockMigrationHelper.AddBlockAttributeValue( "B0A06BE9-1BBF-4A82-A0D7-EFCD05C5DCA8", "4DFDB295-6D0F-40A1-BEF9-7B70C56F66C4", @"0" );
            // Attrib Value for Block:HTML Content, Attribute:Require Approval Page: Survey List, Site: External Website
            RockMigrationHelper.AddBlockAttributeValue( "B0A06BE9-1BBF-4A82-A0D7-EFCD05C5DCA8", "EC2B701B-4C1D-4F3F-9C77-A73C75D7FF7A", @"False" );
            // Attrib Value for Block:HTML Content, Attribute:Enable Versioning Page: Survey List, Site: External Website
            RockMigrationHelper.AddBlockAttributeValue( "B0A06BE9-1BBF-4A82-A0D7-EFCD05C5DCA8", "7C1CE199-86CF-4EAE-8AB3-848416A72C58", @"False" );
            // Attrib Value for Block:HTML Content, Attribute:Start in Code Editor mode Page: Survey List, Site: External Website
            RockMigrationHelper.AddBlockAttributeValue( "B0A06BE9-1BBF-4A82-A0D7-EFCD05C5DCA8", "0673E015-F8DD-4A52-B380-C758011331B2", @"True" );
            // Attrib Value for Block:HTML Content, Attribute:Image Root Folder Page: Survey List, Site: External Website
            RockMigrationHelper.AddBlockAttributeValue( "B0A06BE9-1BBF-4A82-A0D7-EFCD05C5DCA8", "26F3AFC6-C05B-44A4-8593-AFE1D9969B0E", @"~/Content" );
            // Attrib Value for Block:HTML Content, Attribute:User Specific Folders Page: Survey List, Site: External Website
            RockMigrationHelper.AddBlockAttributeValue( "B0A06BE9-1BBF-4A82-A0D7-EFCD05C5DCA8", "9D3E4ED9-1BEF-4547-B6B0-CE29FE3835EE", @"False" );
            // Attrib Value for Block:HTML Content, Attribute:Document Root Folder Page: Survey List, Site: External Website
            RockMigrationHelper.AddBlockAttributeValue( "B0A06BE9-1BBF-4A82-A0D7-EFCD05C5DCA8", "3BDB8AED-32C5-4879-B1CB-8FC7C8336534", @"~/Content" );
            // Attrib Value for Block:HTML Content, Attribute:Enabled Lava Commands Page: Survey List, Site: External Website
            RockMigrationHelper.AddBlockAttributeValue( "B0A06BE9-1BBF-4A82-A0D7-EFCD05C5DCA8", "7146AC24-9250-4FC4-9DF2-9803B9A84299", @"RockEntity" );
            // Attrib Value for Block:HTML Content, Attribute:Is Secondary Block Page: Survey List, Site: External Website
            RockMigrationHelper.AddBlockAttributeValue( "B0A06BE9-1BBF-4A82-A0D7-EFCD05C5DCA8", "04C15DC1-DFB6-4D63-A7BC-0507D0E33EF4", @"False" );

            RockMigrationHelper.UpdateHtmlContentBlock( "B0A06BE9-1BBF-4A82-A0D7-EFCD05C5DCA8", @"{% page where:'Guid == ""D112CF16-533D-464A-B5F8-E0682AD9A73F""'%}
  {% for page in pageItems %}
    {% assign entryPage = page %}
  {% endfor %}
{% endpage %}

{% page where:'Guid == ""3BA5F5A4-A7C2-473A-857E-4B6577B34B17""'%}
  {% for page in pageItems %}
    {% assign resultPage = page %}
  {% endfor %}
{% endpage %}

<div class='panel panel-default'>
    <div class='panel-heading'>My Surveys</div>
    <div class='panel-body'>
        {% contentchanneltype where:'Guid == ""3E42C0B9-9ABC-4136-BE22-140349A65A86""' %}
          {% for contentChannelType in contentchanneltypeItems %}
            {% for contentChannel in contentchanneltype.Channels %}
              {% for contentChannelItem in contentChannel.Items %}
                {% contentchannelitem id:'{{contentChannelItem.Id}}' %}
                    {% if contentchannelitem.Id > 0 %}
                        {% assign submittedAttribute = contentChannelItem | Attribute:'SubmissionDateAttribute','Object' %}
                        {% assign submissionDate = CurrentPerson | Attribute:submittedAttribute.Key %}
                        {% if submissionDate != '' %}
                          <div class='panel panel-success'>
                            <div class='panel-heading'>{{ contentChannelItem.Title }}<br />
                                Completed: {{ submissionDate| Date:'M/d/yyyy'}} <br />
                                <a href='/page/{{resultPage.Id}}?Assessment={{contentChannelItem.Id}}'>View Results</a><br/>
                                <a href='/page/{{entryPage.Id}}?Assessment={{contentChannelItem.Id}}'>Retake Assessment</a>
                            </div>
                          </div>
                        {% else %}
                          <div class='panel panel-default'>
                              <div class='panel-heading'> {{ contentChannelItem.Title }}<br />
                                  Available<br />
                                  <a href='/page/{{entryPage.Id}}?Assessment={{contentChannelItem.Id}}'>Start Assessment</a>
                              </div>
                          </div>
                        {% endif %}
                    {% endif %}
                {% endcontentchannelitem %}
              {% endfor %}
            {% endfor %}
          {% endfor %}
        {% endcontentchanneltype %}
    </div>
</div>", "7C5E4D49-637C-4C95-B5BF-D6016483E6AB" );
        }

        public override void Down()
        {
            RockMigrationHelper.DeleteBlock( "B0A06BE9-1BBF-4A82-A0D7-EFCD05C5DCA8" );
            RockMigrationHelper.DeletePage( "7FFC5F22-B177-44AB-94C4-2AE3A16BCC37" ); //  Page: Survey List

            RockMigrationHelper.DeleteAttribute( "2930C377-6119-4425-9937-D9FCCA9F25BF" );
            RockMigrationHelper.DeleteBlock( "FD1B5723-9000-4890-B7BD-8CF2212641FE" );
            RockMigrationHelper.DeleteBlockType( "BC0A3D28-E8BE-4AD9-96A5-D42B2C3F705D" );
            RockMigrationHelper.DeletePage( "3BA5F5A4-A7C2-473A-857E-4B6577B34B17" ); //  Page: Rating Survey Results

            RockMigrationHelper.DeleteAttribute( "7ABDD3EC-B4F7-4F8F-A6F2-F2CA9EECD3F7" );
            RockMigrationHelper.DeleteAttribute( "9C58F87D-B185-4D90-B606-95C6142C2DC3" );
            RockMigrationHelper.DeleteBlock( "402B40CC-7BE9-4CF0-BD58-E246192C5D19" );
            RockMigrationHelper.DeleteBlockType( "03459C30-F239-4C2C-B9FA-6A71EB0CDC46" );
            RockMigrationHelper.DeletePage( "D112CF16-533D-464A-B5F8-E0682AD9A73F" ); //  Page: Rating Survey

        }
    }
}
