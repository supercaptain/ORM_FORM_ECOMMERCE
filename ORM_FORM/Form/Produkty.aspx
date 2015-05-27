
<%@ Page Title="Produkty" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Produkty.aspx.cs" Inherits="ORM_FORM.Form.Produkty" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:GridView ID="GridViewProduct" runat="server" DataSourceID="getProduct" Width="800px" AutoGenerateColumns ="False" 
         onrowcommand="GridViewProduct_RowCommand">
      <Columns>
          <asp:BoundField DataField="idProduct" HeaderText="ID" />
          <asp:BoundField DataField="name" HeaderText="Popis" />
          <asp:BoundField DataField="Category.Name" HeaderText="Kategorie" />
          <asp:BoundField DataField="CurrenCost" HeaderText="Cena" />        
      </Columns>     
      <Columns ><asp:ButtonField  runat="server" Text="Do kosiku" CommandName="AddToCart" /></Columns>

    </asp:GridView>

    <asp:Button ID="btnCollection" runat="server" onclick="btnCollection_Click"  Text="Zobraz Kosik" />
     <asp:Label ID="lbl" runat="server" Text="Label"></asp:Label>

    <asp:ObjectDataSource ID="getProduct" runat="server"  TypeName="ORM_FORM.ProductTable" SelectMethod="SelectAll">
   
   </asp:ObjectDataSource> 



</asp:Content>
