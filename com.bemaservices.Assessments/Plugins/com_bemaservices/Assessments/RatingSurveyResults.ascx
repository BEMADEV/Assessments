<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RatingSurveyResults.ascx.cs" Inherits="RockWeb.Plugins.com_bemaservices.Assessments.RatingSurveyResults" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlView" runat="server" CssClass="panel panel-block">

            <asp:Literal ID="lResult" runat="server"></asp:Literal>
            <div class="actions margin-t-md">
                <asp:LinkButton ID="btnRetakeTest" runat="server" CssClass="btn btn-primary" OnClick="btnRetakeTest_Click">Retake Test</asp:LinkButton>
            </div>
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
