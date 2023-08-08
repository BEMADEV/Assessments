<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RatingSurvey.ascx.cs" Inherits="RockWeb.Plugins.com_bemaservices.Assessments.RatingSurvey" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <Rock:NotificationBox ID="nbMain" runat="server" Visible="false"></Rock:NotificationBox>

        <asp:Panel ID="pnlView" runat="server" CssClass="panel panel-block">
            <div class="panel-body">
                <div style="text-align: center;">
                    <h1>
                        <asp:Literal ID="lTitle" runat="server" /></h1>
                    <p>
                        <asp:Literal ID="lDescription" runat="server" />
                    </p>
                </div>
            </div>
            <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                <ItemTemplate>
                    <div class="row">
                        <div class="col-xs-12">
                            <asp:HiddenField ID="hfQuestionId" runat="server" />
                            <Rock:RockRadioButtonList ID="rblAnswer" runat="server" RepeatDirection="Horizontal" Required="true">
                                <asp:ListItem Text="1" Value="1" />
                                <asp:ListItem Text="2" Value="2" />
                                <asp:ListItem Text="3" Value="3" />
                                <asp:ListItem Text="4" Value="4" />
                                <asp:ListItem Text="5" Value="5" />
                            </Rock:RockRadioButtonList>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div class="actions">
                <asp:LinkButton ID="btnSubmit" runat="server" AccessKey="s" ToolTip="Alt+s" Text="Submit" CssClass="btn btn-primary pull-right" OnClick="btnSubmit_Click" />
            </div>

        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
