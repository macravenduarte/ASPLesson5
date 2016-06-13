using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// using statements that are required to connect to EF DB
using ASPLesson5.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

namespace ASPLesson5
{
    public partial class Students : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // if loading the page for the first time, populate the student grid
            if (!IsPostBack)
            { 
                Session["SortColumn"] = "StudentID"; //Default sort column
                Session["SortDirection"] = "ASC";

                // Get the student data
                this.GetStudents();
            }
        }

        // <summary>
        // This method gets the student data from the DB
        // </summary>
        // 
        // @method GetStudents
        // @returns {void}
        //
        protected void GetStudents()
        {
            // connect to EF
            using (DefaultConnection db = new DefaultConnection())
            {
                string SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();
                
                // query the Students Table using EF and LINQ
                var Students = (from allStudents in db.Students
                                select allStudents);

                // bind the result to the GridView
                StudentsGridView.DataSource = Students.AsQueryable().OrderBy(SortString).ToList();
                StudentsGridView.DataBind();
            }
        }

        /// <summary>
        /// This Event Handler deletes a Student fro the db usinf EF
        /// </summary>
        /// 
        /// @method StudentsGridView_RowDeleting
        /// @param (object) sender
        /// @params(GridViewDeleteEventArgs) e
        /// @returns void
        /// 
        protected void StudentsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Store which row was selected for deletion
            int selectedRow = e.RowIndex;

            //Get the selected StudentID using the Grid's DataKEy collection
            int StudentID = Convert.ToInt32(StudentsGridView.DataKeys[selectedRow].Values["StudentID"]);

            //Use EF to find the selected student in the DB and remove it
            using (DefaultConnection db = new DefaultConnection())
            {
                //Create object of the student class and store the query string inside of it
                Student deletedStudent = (from studentRecords in db.Students
                                          where studentRecords.StudentID == StudentID
                                          select studentRecords).FirstOrDefault();

                //Remove the selected student from the db
                db.Students.Remove(deletedStudent);

                //Save changes back to the db
                db.SaveChanges();

                //Refresh the grid
                this.GetStudents();

            }
        }

        /// <summary>
        /// This Event Handler allows pagination to occur for the Students page
        /// </summary>
        /// 
        /// @method StudentsGridView_PageIndexChanging
        /// @param (object) sender
        /// @params(GridViewPageEventArgs) e
        /// @returns void
        protected void StudentsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Set the new page number
            StudentsGridView.PageIndex = e.NewPageIndex;

            //Refresh the grid
            this.GetStudents();
        }

        /// <summary>
        /// This Event Handler allows groups of 3, 5, an 10 items per page
        /// </summary>
        /// 
        /// @method PageSizeDropDownList_SelectedIndexChanged
        /// @param (object) sender
        /// @params(EventArgs) e
        /// @returns void
        protected void PageSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Set the new page size
            StudentsGridView.PageSize = Convert.ToInt32(PageSizeDropDownList.SelectedValue);

            //Refresh the grid
            this.GetStudents();
        }

        /// <summary>
        /// This Event Handler toggles the sorting of the grid view
        /// </summary>
        /// 
        /// @method StudentsGridView_Sorting
        /// @param (object) sender
        /// @params(GridViewSortEventArgs) e
        /// @returns void
        /// 
        protected void StudentsGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Get the colunm to sort by
            Session["SortColumn"] = e.SortExpression;

            //refresh the grid
            this.GetStudents();

            //Toggle the direction
            Session["SortDirection"] = Session["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// 
        protected void StudentsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header) //if header row has been clicked
                {
                    LinkButton linkButton = new LinkButton();

                    for(int index = 0; index < StudentsGridView.Columns.Count; index++)
                    {
                        if(StudentsGridView.Columns[index].SortExpression == Session["SortColumn"].ToString())
                        {
                            if(Session["SortDirection"].ToString() == "ASC")
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