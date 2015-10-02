using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["Constr"]);
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string fAirport = "MacH INT. (MDI)";
         
        string query = "SELECT PassengerName, CarrierDesignation, FlightNumber,ToAirport,  AirportFullName, SeatNumber FROM BoardingpassDetails WHERE PNRCode ='" +txtSerialNum.Text + "'";
        SqlCommand cmd = new SqlCommand(query, con1);
        con1.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {

            Label Name = new Label();
            Name.Text = "<br/>" + "Passenger : " + dr["PassengerName"].ToString();
            Panel1.Controls.Add(Name);
            Label FlightNo = new Label();
            FlightNo.Text = "<br/>" + "Flight Number : " + dr["CarrierDesignation"] + " " + dr["FlightNumber"].ToString();
            Panel1.Controls.Add(FlightNo);
            Label fromairport = new Label();
            fromairport.Text = "<br/>" + "Departure : "  + fAirport;
            Panel1.Controls.Add(fromairport);
            Label toairport = new Label();
            toairport.Text = "<br/>" + "Destination : " + dr["AirportFullName"] + " " + "(" + dr["ToAirport"]+")".ToString();
            Panel1.Controls.Add(toairport);
            Label Seat = new Label();
            Seat.Text = "<br/>" + "Seat Number : " + dr["SeatNumber"].ToString();
            Panel1.Controls.Add(Seat);
        }
             
        else
        {
            
            ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Invalid Reference!!!')</script>");
        }
        dr.Close();
        con1.Close();
        txtSerialNum.Text = "";

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }
}