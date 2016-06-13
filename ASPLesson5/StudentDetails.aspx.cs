using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// using statements required for EF DB access
using ASPLesson5.Models;
using System.Web.ModelBinding;

namespace ASPLesson5
{
    public partial class StudentDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                this.GetStudent();
            }
        }

        protected void GetStudent()
        {
            //Populate the form with existing data from the database
            int StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

            using (DefaultConnection db = new DefaultConnection())
            {
                //Connect to the EF db
                Student updateStudent = (from student in db.Students
                                         where student.StudentID == StudentID
                                         select student).FirstOrDefault();

                //map the student properties to the form controls
                if(updateStudent != null)
                {
                    LastNameTextBox.Text = updateStudent.LastName;
                    FirstNameTextBox.Text = updateStudent.FirstMidName;
                    EnrollmentDateTextBox.Text = updateStudent.EnrollmentDate.ToString("yyyy-mm-dd");
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // Redirect back to Students page
            Response.Redirect("~/Students.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // Use EF to connect to the server
            using (DefaultConnection db = new DefaultConnection())
            {
                // use the Student model to create a new student object and
                // save a new record
                Student newStudent = new Student();

                int StudentID = 0;

                if(Request.QueryString.Count > 0) //URL has a StudentID in it
                {
                    //Get the id from the url
                    StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

                    //Get the current student form the EF db
                    newStudent = (from student in db.Students
                                  where student.StudentID == StudentID
                                  select student).FirstOrDefault();
                }

                // add form data to the new student record
                newStudent.LastName = LastNameTextBox.Text;
                newStudent.FirstMidName = FirstNameTextBox.Text;
                newStudent.EnrollmentDate = Convert.ToDateTime(EnrollmentDateTextBox.Text);

                if(StudentID == 0)
                {
                    db.Students.Add(newStudent);
                }
                // use LINQ to ADO.NET to add / insert new student into the database
                db.Students.Add(newStudent);

                // save our changes
                db.SaveChanges();

                // Redirect back to the updated students page
                Response.Redirect("~/Students.aspx");
            }
        }
    }
}