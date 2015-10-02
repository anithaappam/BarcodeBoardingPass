using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["Constr"]);
        con.Open();
        Session["username"] = txtUsername.Text;
        SqlCommand cmd = new SqlCommand("select * from SEMS.Users where UserName =@username and Password=@password", con);
        cmd.Parameters.AddWithValue("@username", txtUsername.Text);
        cmd.Parameters.AddWithValue("@password", txtPassword.Text);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            Response.Redirect("BoardingPass.aspx");
            con.Close();
            txtPassword.Text = "";
            txtUsername.Text = "";
        }
        else
        {
            Label1.Text = "Invalid Username or Password";
        }
    }

}