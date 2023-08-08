// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web;
using Rock.Web.Cache;
using Rock.Web.UI;

namespace RockWeb.Plugins.com_bemaservices.Assessments
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Rating Survey Results" )]
    [Category( "BEMA Services > Assessments" )]
    [Description( "View the results of a Rating Survey." )]

    #region Block Attributes

    [TextField( "Assessment Id Param",
        Key = AttributeKey.AssessmentIdParam,
        Description = "The Page Parameter that will be used to set the AssessmentId value",
        IsRequired = false,
        DefaultValue = "Assessment",
        Order = 0 )]

    [LinkedPage( "Entry Page", "The page a user will be taken to to retake the survey. ", true, "", "", 1, AttributeKey.EntryPage )]

    #endregion Block Attributes
    public partial class RatingSurveyResults : Rock.Web.UI.RockBlock
    {

        #region Attribute Keys

        private static class AttributeKey
        {
            public const string EntryPage = "EntryPage";
            public const string AssessmentIdParam = "AssessmentIdParam";
        }

        #endregion Attribute Keys

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );


            if ( !Page.IsPostBack )
            {
                ShowDetails();
            }
        }



        /// <summary>
        /// Returns breadcrumbs specific to the block that should be added to navigation
        /// based on the current page reference.  This function is called during the page's
        /// oninit to load any initial breadcrumbs
        /// </summary>
        /// <param name="pageReference">The page reference.</param>
        /// <returns></returns>
        public override List<BreadCrumb> GetBreadCrumbs( PageReference pageReference )
        {
            var breadCrumbs = new List<BreadCrumb>();
            ContentChannelItem contentChannelItem = GetContentChannelItem();
            if ( contentChannelItem != null )
            {
                breadCrumbs.Add( new BreadCrumb( string.Format( "{0} Results", contentChannelItem.Title ), pageReference ) );

                RockPage.BrowserTitle = string.Format("{0} Results",contentChannelItem.Title);
                RockPage.PageTitle = string.Format( "{0} Results", contentChannelItem.Title );
                RockPage.Header.Title = string.Format( "{0} Results", contentChannelItem.Title );
            }
            else
            {
                breadCrumbs.Add( new BreadCrumb( "Rating Survey Results", pageReference ) );
            }

            return breadCrumbs;
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        protected void btnRetakeTest_Click( object sender, EventArgs e )
        {
            var qryString = new Dictionary<string, string>();
            var assessmentIdParam = GetAttributeValue( AttributeKey.AssessmentIdParam );
            qryString.Add( assessmentIdParam, PageParameter( assessmentIdParam ) );

            var resultsPage = GetAttributeValue( AttributeKey.EntryPage ).AsGuidOrNull();
            if ( resultsPage.HasValue )
            {
                NavigateToLinkedPage( AttributeKey.EntryPage, qryString );
            }
        }

        #endregion

        #region Methods
        private void ShowDetails()
        {
            var rockContext = new RockContext();
            var attributeMatrixService = new AttributeMatrixService( rockContext );
            var attributeMatrixItemService = new AttributeMatrixItemService( rockContext );
            var personService = new PersonService( rockContext );

            ContentChannelItem contentChannelItem = GetContentChannelItem();
            contentChannelItem.LoadAttributes();

            CurrentPerson.LoadAttributes();

            AttributeMatrix resultMatrix = null;
            var resultMatrixGuid = contentChannelItem.GetAttributeValue( "AssessmentResults" ).AsGuidOrNull();
            if ( resultMatrixGuid.HasValue )
            {
                resultMatrix = attributeMatrixService.Get( resultMatrixGuid.Value );
            }

            var attributeSummaryList = new List<AttributeSummary>();
            if ( resultMatrix != null )
            {
                foreach ( var resultItem in resultMatrix.AttributeMatrixItems )
                {
                    resultItem.LoadAttributes();
                    var attributeGuid = resultItem.GetAttributeValue( "ResultAttribute" ).AsGuid();
                    var attributeCache = AttributeCache.Get( attributeGuid );
                    if ( attributeCache != null )
                    {
                        var attributeSummary = new AttributeSummary();
                        attributeSummary.PersonAttribute = attributeCache;
                        attributeSummary.PrimaryDescription = resultItem.GetAttributeValue( "PrimaryDescription" );
                        attributeSummary.SecondaryDescription = resultItem.GetAttributeValue( "SecondaryDescription" );
                        attributeSummary.Value = CurrentPerson.GetAttributeValue( attributeCache.Key ).AsInteger();
                        attributeSummaryList.Add( attributeSummary );
                    }
                }
            }

            var submissionDateAttributeGuid = contentChannelItem.GetAttributeValue( "SubmissionDateAttribute" ).AsGuid();
            var submissionDateAttributeCache = AttributeCache.Get( submissionDateAttributeGuid );
            var submissionDate = CurrentPerson.GetAttributeValue( submissionDateAttributeCache.Key ).AsDateTime();

            var resultLava = contentChannelItem.GetAttributeValue( "ResultLava" );


            // Resolve the text field merge fields
            var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( this.RockPage, CurrentPerson );
            if ( CurrentPerson != null )
            {
                CurrentPerson.LoadAttributes();
                mergeFields.Add( "Person", CurrentPerson );
                mergeFields.Add( "Scores", attributeSummaryList.OrderByDescending( a => a.Value ).ToList() );
                mergeFields.Add( "SubmissionDate", submissionDate );
                mergeFields.Add( "Assessment", contentChannelItem );

            }

            lResult.Text =  resultLava.ResolveMergeFields( mergeFields );
        }

        private ContentChannelItem GetContentChannelItem()
        {
            var rockContext = new RockContext();
            var contentChannelItemService = new ContentChannelItemService( rockContext );
            ContentChannelItem contentChannelItem = null;
            var assessmentIdParam = GetAttributeValue( AttributeKey.AssessmentIdParam );
            var contentChannelItemId = PageParameter( assessmentIdParam ).AsIntegerOrNull();
            if ( contentChannelItemId.HasValue )
            {
                contentChannelItem = contentChannelItemService.Get( contentChannelItemId.Value );
            }

            return contentChannelItem;
        }

        #endregion
        [DotLiquid.LiquidType( "PersonAttribute", "Value", "PrimaryDescription", "SecondaryDescription" )]

        public class AttributeSummary
        {
            public AttributeCache PersonAttribute { get; set; }
            public int Value { get; set; }
            public String PrimaryDescription { get; set; }
            public String SecondaryDescription { get; set; }
        }
    }
}