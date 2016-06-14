using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//Using statements that are required to connect to EF DB
using ASPLesson5.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

namespace ASPLesson5
{
    public partial class Departments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // if loading the page for the first time, populate the Departments grid
            if (!IsPostBack)
            {
                Session["SortColumn"] = "DepartmentID"; //Default sort column
                Session["SortDirection"] = "ASC";

                // Get the student data
                this.GetDepartment();
            }
        }

        // <summary>
        // This method gets the department data from the DB
        // </summary>
        // 
        // @method GetDepartment
        // @returns {void}
        //
        protected void GetDepartment()
        {
            // connect to EF
            using (DefaultConnection db = new DefaultConnection())
            {
                string SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                // query the Students Table using EF and LINQ
                var Departments = (from allDepartments in db.Departments
                                   select allDepartments);

                // bind the result to the GridView
                DepartmentsGridView.DataSource = Departments.AsQueryable().OrderBy(SortString).ToList();
                DepartmentsGridView.DataBind();
            }
        }

        /// <summary>
        /// This Event Handler deletes a Department from the db using EF
        /// </summary>
        /// 
        /// @method DepartmentsGridView_RowDeleting
        /// @param (object) sender
        /// @params(GridViewDeleteEventArgs) e
        /// @returns void
        /// 
        protected void DepartmentsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Store which row was selected for deletion
            int selectedRow = e.RowIndex;

            //Get the selected DepartmentID using the Grid's DataKey collection
            int DepartmentID = Convert.ToInt32(DepartmentsGridView.DataKeys[selectedRow].Values["DepartmentID"]);

            //Use EF to find the selected Department in the DB and remove it
            using (DefaultConnection db = new DefaultConnection())
            {
                //Create object of the student class and store the query string inside of it
                Department deletedDepartment = (from departmentRecords in db.Departments
                                                where departmentRecords.DepartmentID == DepartmentID
                                                select departmentRecords).FirstOrDefault();

                //Remove the selected student from the db
                db.Departments.Remove(deletedDepartment);

                //Save changes back to the db
                db.SaveChanges();

                //Refresh the grid
                this.GetDepartment();

            }
        }

        /// <summary>
        /// This Event Handler allows pagination to occur for the Departments page
        /// </summary>
        /// 
        /// @method DepartmentsGridView_PageIndexChanging
        /// @param (object) sender
        /// @params(GridViewPageEventArgs) e
        /// @returns void
        protected void DepartmentsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Set the new page number
            DepartmentsGridView.PageIndex = e.NewPageIndex;

            //Refresh the grid
            this.GetDepartment();
        }

        /// <summary>
        /// This Event Handler toggles the sorting of the grid view
        /// </summary>
        /// 
        /// @method DepartmentsGridView_Sorting
        /// @param (object) sender
        /// @params(GridViewSortEventArgs) e
        /// @returns void
        /// 
        protected void DepartmentsGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Get the colunm to sort by
            Session["SortColumn"] = e.SortExpression;

            //refresh the grid
            this.GetDepartment();

            //Toggle the direction
            Session["SortDirection"] = Session["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// 
        protected void DepartmentsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header) //if header row has been clicked
                {
                    LinkButton linkButton = new LinkButton();

                    for (int index = 0; index < DepartmentsGridView.Columns.Count; index++)
                    {
                        if (DepartmentsGridView.Columns[index].SortExpression == Session["SortColumn"].ToString())
                        {
                            if (Session["SortDirection"].ToString() == "ASC")
                            {
                                linkButton.Text = " <i class = 'fa fa-caret-up fa-lg'></i>";
                            }
                            else
                            {
                                linkButton.Text = " <i class = 'fa fa-caret-down fa-lg'></i>";
                            }

                            e.Row.Cells[index].Controls.Add(linkButton);
                        }

                    }
                }
            }
        }
    }
}