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
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;
using Rock.Attribute;
using Rock.Store;
using Rock.Utility;
using Rock.VersionInfo;
using System.Net;
using System.Text.RegularExpressions;
using System.IO.Compression;
using Microsoft.Web.XmlTransform;
using Rock.Security;
using RestSharp;
using System.Web;
using Newtonsoft.Json;
using Rock.Web.UI;
using Rock.Web;

namespace RockWeb.Plugins.com_bemaservices.Assessments
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Rating Survey" )]
    [Category( "BEMA Services > Assessments" )]
    [Description( "Allows you to take a Rating Survey test and saves your Rating Survey score." )]

    #region Block Attributes

    [TextField( "Assessment Id Param",
        Key = AttributeKey.AssessmentIdParam,
        Description = "The Page Parameter that will be used to set the AssessmentId value",
        IsRequired = false,
        DefaultValue = "Assessment",
        Order = 0 )]

    [LinkedPage( "Results Page", "The page a user will be taken to to view their results. If none is listed, the user will be taken to the parent page.", false, "", "", 1, AttributeKey.ResultsPage )]

    #endregion Block Attributes
    public partial class RatingSurvey : Rock.Web.UI.RockBlock
    {

        #region Attribute Keys

        private static class AttributeKey
        {
            public const string AssessmentIdParam = "AssessmentIdParam";
            public const string ResultsPage = "ResultsPage";
        }

        #endregion Attribute Keys

        #region PageParameterKeys

        private static class PageParameterKey
        {
            public const string StarkId = "StarkId";
        }

        #endregion PageParameterKeys

        #region Fields

        // used for private variables

        #endregion

        #region Properties

        // used for public / protected properties

        #endregion

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
                if ( CurrentPerson != null )
                {
                    var assessmentContentTypeGuid = "DF49BACD-60E0-4D45-9092-5E18290DAC3B".AsGuid();
                    ContentChannelItem contentChannelItem = GetContentChannelItem();

                    if ( contentChannelItem != null &&
                        contentChannelItem.ContentChannel.Guid == assessmentContentTypeGuid &&
                        contentChannelItem.IsAuthorized( Rock.Security.Authorization.VIEW, CurrentPerson ) )
                    {
                        ShowDetails( contentChannelItem );
                    }
                    else
                    {
                        ShowWarning( "Sorry", "The selected Survey could not be found or is no longer active." );
                    }
                }
                else
                {
                    var site = RockPage.Site;
                    if ( site.LoginPageId.HasValue )
                    {
                        site.RedirectToLoginPage( true );
                    }
                    else
                    {
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    }
                }

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
                breadCrumbs.Add( new BreadCrumb( contentChannelItem.Title, pageReference ) );

                RockPage.BrowserTitle = contentChannelItem.Title;
                RockPage.PageTitle = contentChannelItem.Title;
                RockPage.Header.Title = contentChannelItem.Title;
            }
            else
            {
                breadCrumbs.Add( new BreadCrumb( "Rating Survey", pageReference ) );
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

        protected void btnSubmit_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            var attributeMatrixService = new AttributeMatrixService( rockContext );
            var attributeMatrixItemService = new AttributeMatrixItemService( rockContext );
            var personService = new PersonService( rockContext );

            ContentChannelItem contentChannelItem = GetContentChannelItem();
            contentChannelItem.LoadAttributes();

            var resultAttributeMapping = new Dictionary<Guid, int>();

            AttributeMatrix resultMatrix = null;
            var resultMatrixGuid = contentChannelItem.GetAttributeValue( "AssessmentResults" ).AsGuidOrNull();
            if ( resultMatrixGuid.HasValue )
            {
                resultMatrix = attributeMatrixService.Get( resultMatrixGuid.Value );
            }

            if ( resultMatrix != null )
            {
                foreach ( var resultItem in resultMatrix.AttributeMatrixItems )
                {
                    resultItem.LoadAttributes();
                    var attributeGuid = resultItem.GetAttributeValue( "ResultAttribute" ).AsGuid();
                    resultAttributeMapping.Add( attributeGuid, 0 );
                }
            }

            foreach ( RepeaterItem rItem in rptQuestions.Items )
            {
                RockRadioButtonList rblAnswer = rItem.FindControl( "rblAnswer" ) as RockRadioButtonList;
                HiddenField hfQuestionId = rItem.FindControl( "hfQuestionId" ) as HiddenField;
                var questionItem = attributeMatrixItemService.Get( hfQuestionId.ValueAsInt() );
                if ( questionItem != null )
                {
                    questionItem.LoadAttributes();
                    var attributeGuid = questionItem.GetAttributeValue( "PersonAttribute" ).AsGuid();
                    if ( resultAttributeMapping.ContainsKey( attributeGuid ) )
                    {
                        resultAttributeMapping[attributeGuid] += rblAnswer.SelectedValue.AsInteger();
                    }
                }
            }

            var person = personService.Get( CurrentPerson.Id );
            person.LoadAttributes();
            foreach ( var resultMap in resultAttributeMapping )
            {
                var attributeCache = AttributeCache.Get( resultMap.Key );
                if ( attributeCache != null )
                {
                    person.SetAttributeValue( attributeCache.Key, resultMap.Value );
                }
            }

            var submissionDateAttributeGuid = contentChannelItem.GetAttributeValue( "SubmissionDateAttribute" ).AsGuid();
            var submissionDateAttributeCache = AttributeCache.Get( submissionDateAttributeGuid );
            if ( submissionDateAttributeCache != null )
            {
                person.SetAttributeValue( submissionDateAttributeCache.Key, RockDateTime.Now.ToShortDateTimeString() );
            }

            person.SaveAttributeValues( rockContext );

            var qryString = new Dictionary<string, string>();
            var assessmentIdParam = GetAttributeValue( AttributeKey.AssessmentIdParam );
            qryString.Add( assessmentIdParam, PageParameter( assessmentIdParam ) );

            var resultsPage = GetAttributeValue( AttributeKey.ResultsPage ).AsGuidOrNull();
            if ( resultsPage.HasValue )
            {
                NavigateToLinkedPage( AttributeKey.ResultsPage, qryString );
            }
            else
            {
                NavigateToParentPage( qryString );
            }

        }

        protected void rptQuestions_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            var attributeMatrixItem = e.Item.DataItem as AttributeMatrixItem;
            attributeMatrixItem.LoadAttributes();
            var hfQuestionId = e.Item.FindControl( "hfQuestionId" ) as HiddenField;
            var rblAnswer = e.Item.FindControl( "rblAnswer" ) as RockRadioButtonList;
            hfQuestionId.Value = attributeMatrixItem.Id.ToString();
            rblAnswer.Label = attributeMatrixItem.GetAttributeValue( "QuestionText" );
        }

        #endregion

        #region Methods


        private void ShowDetails( ContentChannelItem contentChannelItem )
        {
            RockContext rockContext = new RockContext();
            var attributeMatrixService = new AttributeMatrixService( rockContext );
            lTitle.Text = contentChannelItem.Title;
            lDescription.Text = contentChannelItem.Content;

            contentChannelItem.LoadAttributes();

            var questionMatrixGuid = contentChannelItem.GetAttributeValue( "AssessmentQuestions" ).AsGuidOrNull();
            if ( questionMatrixGuid.HasValue )
            {
                var attributeMatrix = attributeMatrixService.Get( questionMatrixGuid.Value );
                if ( attributeMatrix != null )
                {
                    rptQuestions.DataSource = attributeMatrix.AttributeMatrixItems.OrderBy( ami => ami.Order );
                    rptQuestions.DataBind();
                }
            }
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

        /// <summary>
        /// Shows a warning message.
        /// </summary>
        /// <param name="heading">The heading.</param>
        /// <param name="text">The text.</param>
        private void ShowWarning( string heading, string text )
        {
            nbMain.Heading = heading;
            nbMain.Text = string.Format( "<p>{0}</p>", text );
            nbMain.NotificationBoxType = NotificationBoxType.Warning;
            nbMain.Visible = true;
        }

        /// <summary>
        /// Shows an error message.
        /// </summary>
        /// <param name="heading">The heading.</param>
        /// <param name="text">The text.</param>
        private void ShowError( string heading, string text )
        {
            nbMain.Heading = heading;
            nbMain.Text = string.Format( "<p>{0}</p>", text );
            nbMain.NotificationBoxType = NotificationBoxType.Danger;
            nbMain.Visible = true;
        }


        #endregion
    }
}