<%@ Page Title="DepartmentDetails" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepartmentDetails.aspx.cs" Inherits="ASPLesson5.DepartmentDetails" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-offset-2 col-md-8">
                <div class="form-group">
                    <label>Department ID</label>
                    <asp:TextBox CssClass="form-control" ID="DepartmentIDTextbox" runat="server" required="true"/>
                </div>
                <div class="form-group">
                    <label>Department Name</label>
                    <asp:TextBox CssClass="form-control" ID="DeptartmentNameTextbox" runat="server"  required="true" />
                </div>
                <div class="form-group">
                    <label>Department Budget</label>
                    <asp:TextBox CssClass="form-control" ID="DeptartmentBudgetTextbox" runat="server" required="true" />
                </div>
                <div class="form-group">
                    <asp:Button CssClass="btn btn-danger btn-lg" Text="Cancel" ID="CancelButton" runat="server"
                        CausesValidation="False" UseSubmitBehavior="False" OnClick="CancelButton_OnClick" />
                    
                    <asp:Button CssClass="btn btn-primary btn-lg" Text="Save" ID="SubmitButton" runat="server" 
                        OnClick="SubmitButton_OnClick"/>
                </div>
            </div>
        </div>
</div>
</asp:Content>
