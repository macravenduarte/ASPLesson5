using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// using statements that are required to connect to EF DB
using ASPLesson5.Models;
using System.Web.ModelBinding;

namespace ASPLesson5
{
    public partial class Students : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // if loading the page for the first time, populate the student grid
            if (!IsPostBack)
            {
                // Get the student data
                this.GetStudents();
            }
        }

        /**
         * <summary>
         * This method gets the student data from the DB
         * </summary>
         * 
         * @method GetStudents
         * @returns {void}
         */
        protected void GetStudents()
        {
            // connect to EF
            using (DefaultConnection db = new DefaultConnection())
            {
                // query the Students Table using EF and LINQ
                var Students = (from allStudents in db.Students
                                select allStudents);

                // bind the result to the GridView
                StudentsGridView.DataSource = Students.ToList();
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
            //Stre whish row was clicked
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
        /// This Event Handler allows pagination to occur for the Students page        ///
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
    }
}