<%@ Page Title="Departments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Departments.aspx.cs" Inherits="ASPLesson5.Departments" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-offset-2 col-md-8">
                <h1>Student List</h1>
                <!--Add button for testing department crud -->
                <a href="DepartmentDetails.aspx" class="btn btn-success btn-sm"><i class="fa fa-plus"></i> Add Department</a>

                 <asp:GridView runat="server" CssClass="table table-bordered table-striped table-hover" 
                    ID="DepartmentsGridView" AutoGenerateColumns="false" DataKeyNames="DepartmentID" 
                    OnRowDeleting="DepartmentsGridView_RowDeleting" AllowPaging="true"
                    OnPageIndexChanging="DepartmentsGridView_PageIndexChanging" AllowSorting="true" 
                    OnSorting="DepartmentsGridView_Sorting" OnRowDataBound="DepartmentsGridView_RowDataBound" >

                    <Columns>
                        <%-- DepartmentID --%>
                        <asp:BoundField DataField="DepartmentID" HeaderText="Department ID" Visible="true" SortExpression="DepartmentID"/>
                        <%-- Department Name --%>
                        <asp:BoundField DataField="Name" HeaderText="Department Name" Visible="true" SortExpression="Name" />
                        <%-- Department Budget --%>
                        <asp:BoundField DataField="Budget" HeaderText="Budget" Visible="true" SortExpression="Budget" DataFormatString="{0:C}"/>
                        <%-- Edit --%>
                        <asp:HyperLinkField HeaderText="Edit" Text="<i class='fa fa-pencil-square-o fa-lg'></i> Edit" 
                            NavigateUrl="~/DepartmentDetails.aspx" ControlStyle-CssClass="btn btn-primary btn-sm"
                            runat="server" DataNavigateUrlFields="DepartmentID" DataNavigateUrlFormatString="DepartmentDetails.aspx?DepartmentID={0}" />
                        <%-- Delete --%>
                        <asp:CommandField HeaderText="Delete" DeleteText="<i class='fa fa-trash-o fa-lg'></i> Delete"
                            ShowDeleteButton="true" ButtonType="Link" ControlStyle-CssClass="btn btn-danger btn-sm" />

                    </Columns>
                 </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
