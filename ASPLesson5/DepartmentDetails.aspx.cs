using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// using statements required for EF DB access
using ASPLesson5.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

namespace ASPLesson5
{
    public partial class DepartmentDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && (Request.QueryString.Count > 0) )
                this.GetDepartments();
        }

        protected void GetDepartments()
        {
            var DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

            using (DefaultConnection db = new DefaultConnection())
            {
                var updatedDepartment = (from deptartment in db.Departments
                                        where deptartment.DepartmentID == DepartmentID
                                        select deptartment).FirstOrDefault();

                if (updatedDepartment != null)
                {
                  // DepartmentIDTextbox.Text = updatedDepartment.DepartmentID.ToString();
                   DeptartmentNameTextbox.Text = updatedDepartment.DepartmentName;
                   DeptartmentBudgetTextbox.Text = updatedDepartment.Budget.ToString();
                }
            }
        }

        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Departments.aspx");
        }

        protected void SubmitButton_OnClick(object sender, EventArgs e)
        {
            //connect to database and save the contents of the form
            using (DefaultConnection db = new DefaultConnection())
            {
                var newDepartment = new Department();

                newDepartment.DepartmentName = DeptartmentNameTextbox.Text;
                newDepartment.Budget = Convert.ToDecimal(DeptartmentBudgetTextbox.Text);

                db.Departments.Add(newDepartment);
                db.SaveChanges();

                Response.Redirect("~/Departments.aspx");
            }
        }
    }
}