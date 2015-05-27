

<%@ Page Title="Kosik" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Kosik.aspx.cs" Inherits="ORM_FORM.Form.Kosik" %>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <asp:Label ID="lbl" runat="server" Text="Label"></asp:Label>

    <asp:Repeater ID="productCartRepeater" runat="server" EnableViewState="false" >
     <HeaderTemplate>
     <table border="1" cellpadding=""5" cellspacing="10" class="table-bordered"> 
     <thead>
      <tr>                            
         <th>ID</th>
         <th>Name</th>
         <th>Pocet</th>
      </tr>
      </thead>
    <tbody>
    </HeaderTemplate>

    <ItemTemplate>
    <tr>                      
        <th><%# Eval(" ID  ") %></th>
        <th><%# Eval("Name ") %></th> 
        <th><asp:TextBox ID="TxtBox" runat="server"  Text="1" EnableViewState="false"></asp:TextBox></th>       
            
    </tr>    
     </ItemTemplate>     
     <FooterTemplate>
         </tbody>
       </table>
     </FooterTemplate>
    </asp:Repeater>

  <p>&nbsp;</p>
     <asp:Label ID="label" runat="server" Text="Label"></asp:Label>

    <asp:DropDownList 
        ID="DeliveryList" runat="server" DataSourceID ="getDelivery" DataTextField="Name" DataValueField="idDelivery" >
    </asp:DropDownList>
        
     <asp:ObjectDataSource 
         ID="getDelivery" runat="server"  TypeName="ORM_FORM.TypeDeliveryTable" SelectMethod="SelectAll">
    </asp:ObjectDataSource>

     <div class="form-horizontal">
        <h4>Udaje k objednavce</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Adresa" CssClass="col-md-2 control-label">Adresa</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Adresa" CssClass="form-control" TextMode="SingleLine" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Adresa" CssClass="text-danger" ErrorMessage="Adresa je pozadovana" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Mesto" CssClass="col-md-2 control-label">Mesto</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Mesto" TextMode="SingleLine" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Mesto" CssClass="text-danger" ErrorMessage="Mesto je pozadovana" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="PSC" CssClass="col-md-2 control-label">PSC</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="PSC" TextMode="SingleLine" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="PSC" CssClass="text-danger" Display="Dynamic" ErrorMessage="PSC je pozadovana" />
               
            </div>
        </div>
 </div>

    <p> 
       <asp:Button ID="btnCollection" runat="server" onclick="btnCollection_Click"  Text="Odeslat" />
       
   </p>

  </asp:Content>