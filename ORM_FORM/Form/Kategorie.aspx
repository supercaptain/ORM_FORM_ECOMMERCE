<%@ Page Language="C#"  MasterPageFile="~/Site.Master"   AutoEventWireup="true" CodeBehind="Kategorie.aspx.cs" Inherits="ORM_FORM.Form.Kategorie" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


   <asp:GridView  ID="GridViewCategory" DataKeyNames="idcategory" runat="server" DataSourceID="getCategory" >
      <Columns>
        <asp:CommandField ShowSelectButton="True" ShowDeleteButton="True" />
      </Columns>     
    </asp:GridView>
  <p><br /><br /><br /></p> 
    

   <asp:DetailsView ID="DetailCategory" runat="server" DataKeyNames="idcategory"  DataSourceID="getDetailCategory" OnDataBound="DetailCategory_DataBound"  >
       <Fields>
           <asp:CommandField ShowEditButton="true" ShowInsertButton="true" />
      </Fields>
   </asp:DetailsView>

   <asp:ObjectDataSource
      ID="getCategory" runat="server"  
      TypeName="ORM_ECOMMERCE.DatabaseTable.CategoryTable"  
      SelectMethod="SelectAll"  DeleteMethod="Delete">  
      <DeleteParameters>
          <asp:ControlParameter Type="Int32" Name="idcategory" ControlID="GridViewCategory" />
      </DeleteParameters> 
  </asp:ObjectDataSource> 

  <asp:ObjectDataSource
      ID="getDetailCategory" runat="server"  
      TypeName="ORM_ECOMMERCE.DatabaseTable.CategoryTable"  
      DataObjectTypeName="ORM_ECOMMERCE.Category"
      SelectMethod="Select" UpdateMethod="Update" InsertMethod="Insert">  
      <SelectParameters>
          <asp:ControlParameter Type="Int32" Name="idcategory" DefaultValue="1" ControlID ="GridViewCategory" />
      </SelectParameters>

  </asp:ObjectDataSource> 

      

              
          

         
    
</asp:Content>
