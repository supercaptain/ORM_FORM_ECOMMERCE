<%@ Page Title="Registrace" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registrace.aspx.cs" Inherits="ORM_FORM.Form.Registrace" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

      <div class="form-horizontal">
        <h4>Create a new account.</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Email" CssClass="text-danger" ErrorMessage="The email field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                    CssClass="text-danger" ErrorMessage="The password field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 control-label">Confirm password</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />
            </div>
        </div>

        <div class="form-group">    
            <asp:Label runat="server" AssociatedControlID="FnameTxt" CssClass="col-md-2 control-label">Jmeno</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="FnameTxt" CssClass="form-control" TextMode="SingleLine" />
                 <asp:RequiredFieldValidator runat="server" ControlToValidate="FnameTxt"
                    CssClass="text-danger" ErrorMessage="The FNAME field is required." />               
            </div>
        </div>  

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="LnameTxt" CssClass="col-md-2 control-label">Prijmeni</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="LnameTxt" CssClass="form-control" TextMode="SingleLine" /> 
                <asp:RequiredFieldValidator runat="server" ControlToValidate="LnameTxt" 
                    CssClass="text-danger" ErrorMessage="The LNAME field is required." />                 
            </div>
        </div>  
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PhoneTxt" CssClass="col-md-2 control-label">Telefon</asp:Label>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="PhoneTxt" runat="server" ErrorMessage="Pouze Cisla" ValidationExpression="\d+"></asp:RegularExpressionValidator>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="PhoneTxt" CssClass="form-control" TextMode="Phone" />               
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="CompanyTxt" CssClass="col-md-2 control-label">Firma</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="CompanyTxt" CssClass="form-control" TextMode="SingleLine" />               
            </div>
        </div>  


        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Register" CssClass="btn btn-default" />
            </div>
        </div>


        <div class="form-group">
            <asp:Label runat="server" ID="label1"  ></asp:Label>
         </div>
    </div>
</asp:Content>
