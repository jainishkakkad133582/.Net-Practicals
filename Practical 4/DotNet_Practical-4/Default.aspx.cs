using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNet_Practical_4
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cvTerms_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = chkTerms.Checked;
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string skills = "";

                foreach (ListItem item in cblSkills.Items)
                {
                    if (item.Selected)
                    {
                        if (skills != "")
                            skills += ", ";

                        skills += item.Text;
                    }
                }

                if (skills == "")
                    skills = "None";

                lblResult.Text =
                    "<h3>Registration Successful!</h3>" +
                    "<b>Full Name:</b> " + txtName.Text + "<br/><br/>" +
                    "<b>Email:</b> " + txtEmail.Text + "<br/><br/>" +
                    "<b>Mobile:</b> " + txtMobile.Text + "<br/><br/>" +
                    "<b>College:</b> " + txtCollege.Text + "<br/><br/>" +
                    "<b>Department:</b> " + rblDepartment.SelectedValue + "<br/><br/>" +
                    "<b>Event:</b> " + ddlEvent.SelectedValue + "<br/><br/>" +
                    "<b>Gender:</b> " + rblGender.SelectedValue + "<br/><br/>" +
                    "<b>Skills:</b> " + skills + "<br/><br/>" +
                    "<b>Address:</b> " + txtAddress.Text;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtMobile.Text = "";
            txtCollege.Text = "";
            txtAddress.Text = "";

            rblDepartment.ClearSelection();
            rblGender.ClearSelection();
            ddlEvent.SelectedIndex = 0;

            foreach (ListItem item in cblSkills.Items)
            {
                item.Selected = false;
            }

            chkTerms.Checked = false;
            lblResult.Text = "";
        }
    }
}