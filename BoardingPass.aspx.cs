using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Text;
using ZXing.Common;
using ZXing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;

public partial class Default2 : System.Web.UI.Page
{
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["Constr"]);
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindGridview();
        }
    }

    public void BindGridview()
    {
        con1.Open();
        SqlCommand cmd = new SqlCommand("select  TOP(100) ag.DepartDateTime, ag.FlightNo, ag.FromAirport, ag.ToAirport,  ag.CarrierDesignator, ai.Name  as Airline , a.Name from SmartAccessGate.AccessGateFlightInfo ag  inner join Airport A  on  ag.ToAirport=A.IATACode inner join Airline ai  on  ag.CarrierDesignator=ai.IATADesignator   where  ag.DepartDateTime between dateadd(hh, 0, convert(datetime, convert(datetime, getdate())))  and  dateadd(hh, 24, convert(datetime, convert(datetime, dateadd(dd, 1, getdate())))) order by DepartDateTime asc", con1);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        gvFlightDetails.DataSource = ds;
        gvFlightDetails.DataBind();
        con1.Close();
    }

    protected void btnLogin_Click1(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }
 
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
       
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var stringChars = new char[7];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        Label1.Text = new String(stringChars);
    
        string date = Session["date"].ToString();
        string passengername = txtFirstname.Text + " " + txtLastName.Text;
        string pnrcode = Label1.Text;
        string jDate = Session["jDate"].ToString();
        string flightno = Session["flightno"].ToString();
        string fromairport = Session["fromairport"].ToString();
        string toairport = Session["toairport"].ToString();
        string carrierdesignator = Session["carrierdesignator"].ToString();
        string airline = Session["airline"].ToString();
        string fullname = Session["fullname"].ToString();
        string fname = fullname.Split(' ').FirstOrDefault();
        string createddate = DateTime.Now.ToString("dd/MM/yyyy");
        string createdby = Session["username"].ToString();

        SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["Constr"]);
        string latestCheckin = "Select TOP(1) CheckInSequenceNumber from BoardingpassDetails where  JulianDate = '" + Convert.ToDateTime(date).DayOfYear + "' order by CheckInSequenceNumber desc";
        con1.Open();
        SqlCommand cmd = new SqlCommand(latestCheckin, con1);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adp.Fill(dt);
        string nextval = "";
        if (dt.Rows.Count > 0)
        {
            var c = dt.Rows[0]["CheckInSequenceNumber"].ToString();
            int last = Convert.ToInt32(c);
            int next = last + 1;

            nextval = next.ToString().PadLeft(4, '0');

        }
        else nextval = "0001";

        SqlConnection conString = new SqlConnection(ConfigurationManager.AppSettings["ConStr"]);
        string seatno = "select TOP(1) SeatNumber from BoardingpassDetails where FlightNumber='"+flightno+"' and JulianDate = '" + Convert.ToDateTime(date).DayOfYear + "' order by SeatNumber desc";
        conString.Open();
        SqlCommand cmd2 = new SqlCommand(seatno, conString);
        SqlDataAdapter adp1 = new SqlDataAdapter(cmd2);
        DataTable dt1 = new DataTable();
        adp1.Fill(dt1);
        string SeatId = "";

        if (dt1.Rows.Count > 0)
        {
            var nextSeat = "";

            var s = dt1.Rows[0]["SeatNumber"].ToString();
            string splitno = s.Substring(0, 1);
            char c = s[1];
            c++;
            int splitletter = (int)c;
            if (splitletter <= 70)
            {
                nextSeat += (Convert.ToInt32(splitno)).ToString("0");
                nextSeat += (char)splitletter;
                nextSeat.PadLeft(1, '0');
            }
            else
            {
                nextSeat += (Convert.ToInt32(splitno) + 1).ToString("0");
                nextSeat += (char)65;
                nextSeat.PadLeft(1, '0');
            }

            SeatId = nextSeat;
        }

        else
        {
            SeatId = "1A";
        }
       
        string cat = DropDownList1.SelectedItem.Text;
        SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["Constr"]);
        string sql = "SET ANSI_WARNINGS  OFF; insert into BoardingpassDetails(PassengerName,PNRCode, FlightNumber, FromAirport, ToAirport,AirportFullName, CarrierDesignation,SeatNumber, CheckInSequenceNumber, JulianDate,  CreatedDate, CreatedBy) values ('" + cat + " " + passengername + "','" + Label1.Text + "','" + flightno + "','" + fromairport + "','" + toairport + "','" + fname + "','" + carrierdesignator + "','" + SeatId + "','" + nextval + "','" + jDate + "','" + Convert.ToDateTime(createddate) + "','" + createdby + "')";
        Label2.Text = sql;
        try
        {
            
            con2.Open();
            SqlCommand cmd1 = new SqlCommand(sql, con2);
            cmd1.ExecuteNonQuery();
            Label2.Text = "Inserted";
           
            string formatcode = "M1";
            string eticket = "E";
            string Firstclass = "F";
            string Fieldsize = "100";
            string PassengerStatus = "1";
            Label5.Text = formatcode + (cat + passengername).PadRight(20, ' ') + eticket + pnrcode + fromairport + toairport + carrierdesignator.PadRight(3, ' ') + flightno.PadRight(5, ' ') + jDate + Firstclass + SeatId.PadRight(4, ' ') + nextval + PassengerStatus + Fieldsize;

            string barcodestring = Label5.Text;
            var barcodeWriter = new BarcodeWriter {
                Format = BarcodeFormat.PDF_417,
                Options = { Margin = 2 }
            };
            //var result = barcodeWriter.Write(barcodestring);
            //var barcodeBitmap = new Bitmap(result);
            //var s = new MemoryStream();
            //barcodeBitmap.Save(s, ImageFormat.Png);
            

            Document pdfDoc = new Document(PageSize.A4, 20, 20, 40, 40);
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
          
            pdfDoc.Open();
            PdfPCell cell = null;
            PdfPTable table = null;

            iTextSharp.text.Font[] fonts = new iTextSharp.text.Font[10];
            var titleFont = FontFactory.GetFont("Arial", 13, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(921964));
            var subTitleFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(18303));
            var boldTableFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.NORMAL);
            var endingMessageFont = FontFactory.GetFont("Arial", 13, iTextSharp.text.Font.BOLDITALIC, new iTextSharp.text.BaseColor(18303));
            var bodyFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(4868682));
            var quote = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);

            PdfPTable table2 = null;
            table2 = new PdfPTable(4);
            table2.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table2.SpacingBefore = 5f;
            table2.SpacingAfter = 5f;
            
            float[] sglTblHdWidths = new float[4];
            sglTblHdWidths[0] = 70f;
            sglTblHdWidths[1] = 110f;
            sglTblHdWidths[2] = 1f;
            sglTblHdWidths[3] = 90f;
            table2.SetWidths(sglTblHdWidths);
  
            cell = new PdfPCell(new Phrase("", titleFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = iTextSharp.text.BaseColor.BLACK;
            cell.BorderColor = iTextSharp.text.BaseColor.BLACK;
            cell.Colspan = 4;
            cell.FixedHeight = 2f;
            table2.AddCell(cell);
            cell = new PdfPCell(new Phrase("", titleFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
            cell.Colspan = 4;
            cell.FixedHeight = 4f;
            table2.AddCell(cell);
            cell = new PdfPCell(new Phrase("", titleFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.Colspan = 4;
            cell.FixedHeight = 1f;
            table2.AddCell(cell);
            cell = new PdfPCell(new Phrase("BOARDING PASS", titleFont));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            table2.AddCell(cell);
            string airplane = Server.MapPath(".") + "/images/plane-icon3.png";
            iTextSharp.text.Image png1 = iTextSharp.text.Image.GetInstance(airplane);
            png1.ScaleToFit(40f, 40f);
            cell = new PdfPCell(png1);
            cell.HorizontalAlignment =0;            
            cell.Border = PdfPCell.BOTTOM_BORDER;
            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
            table2.AddCell(cell);
            table2.AddCell(PhraseCell(new Phrase("", bodyFont), PdfPCell.ALIGN_LEFT));
            string imageFilePath = Server.MapPath(".") + "/images/MHATitle.png";
            iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageFilePath);
            png.ScaleToFit(140f, 140f);
            cell = new PdfPCell(png);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.HorizontalAlignment = 1;
            cell.Border = PdfPCell.BOTTOM_BORDER;
            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
            table2.AddCell(cell);
            cell = new PdfPCell(new Phrase("", titleFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = iTextSharp.text.BaseColor.BLACK;
            cell.BorderColor = iTextSharp.text.BaseColor.BLACK;
            cell.Colspan = 4;
            cell.FixedHeight = 1f;
            table2.AddCell(cell);
            cell = new PdfPCell(new Phrase("", titleFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.Colspan = 4;
            cell.FixedHeight = 2f;
            table2.AddCell(cell);
            var result = barcodeWriter.Write(barcodestring);
            var barcodeBitmap = new Bitmap(result);
            var s = new MemoryStream();
            barcodeBitmap.Save(s, ImageFormat.Png);
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(s.ToArray());
            //return img;
            iTextSharp.text.Image barcodeimg = iTextSharp.text.Image.GetInstance(img);
           
            barcodeimg.ScaleToFit(130f, 130f);
            cell = new PdfPCell(barcodeimg);
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.BOTTOM_BORDER;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            table2.AddCell(cell);
            table2.AddCell(new Phrase("      "+cat + " " + passengername, endingMessageFont));
            table2.AddCell(PhraseCell(new Phrase("", bodyFont), PdfPCell.ALIGN_LEFT));
            string date1 = Session["date"].ToString();
            DateTime time = DateTime.Parse(date);
            string tim= time.ToString("HH:mm");          
            DateTime time1 = Convert.ToDateTime(tim).AddMinutes(-30);          
            string gateclose = time1.ToString("HH:mm");                            
            string date2 = Convert.ToDateTime(date1).ToString("dd MMM yyyy");
            var RefTextFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(4868682));
            var RefFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD,new iTextSharp.text.BaseColor(18303));
            var tChunk = new Chunk("Booking Reference"+"\n", RefTextFont);
            var dChunk = new Chunk(Label1.Text, RefFont);
            var phrase = new Phrase(tChunk);
            phrase.Add(dChunk);
            cell=new PdfPCell(phrase);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            table2.AddCell(cell);
        
            cell = new PdfPCell(new Phrase("", titleFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.Colspan = 4;
            cell.FixedHeight = 4f;
            table2.AddCell(cell);
            table2.AddCell(new Phrase(toairport, subTitleFont));
            table2.AddCell(new Phrase(SeatId, subTitleFont));
            table2.AddCell("");
            pdfDoc.Add(table2);
         
            table = new PdfPTable(4);
            table.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            
            float[] sglTblHdWidth = new float[4];
            sglTblHdWidth[0] = 50f;
            sglTblHdWidth[1] = 50f;
            sglTblHdWidth[2] = 50f;
            sglTblHdWidth[3] = 50f;
            table.SetWidths(sglTblHdWidth);

            table.AddCell(PhraseCell(new Phrase("Flight", bodyFont), PdfPCell.ALIGN_CENTER));
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            table.AddCell(PhraseCell(new Phrase("Departure", bodyFont), PdfPCell.ALIGN_CENTER));
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            table.AddCell(PhraseCell(new Phrase("Destination", bodyFont), PdfPCell.ALIGN_CENTER));
            table.AddCell(PhraseCell(new Phrase("Seat", bodyFont), PdfPCell.ALIGN_CENTER));
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            table.AddCell(PhraseCell(new Phrase(carrierdesignator + flightno, subTitleFont), PdfPCell.ALIGN_CENTER));
            table.AddCell(PhraseCell(new Phrase("MacH INT. (MDI)", subTitleFont), PdfPCell.ALIGN_CENTER));
            table.AddCell(PhraseCell(new Phrase(fname+" "+"("+toairport+")", subTitleFont), PdfPCell.ALIGN_CENTER));
            table.AddCell(PhraseCell(new Phrase(SeatId, subTitleFont), PdfPCell.ALIGN_CENTER));
            cell = new PdfPCell(new Phrase("", titleFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.Colspan = 4;
            cell.FixedHeight = 15f;
            table.AddCell(cell);
            table.AddCell(PhraseCell(new Phrase("Date", bodyFont), PdfPCell.ALIGN_CENTER));
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            table.AddCell(PhraseCell(new Phrase("Departure time", bodyFont), PdfPCell.ALIGN_CENTER));
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            table.AddCell(PhraseCell(new Phrase("Airline", bodyFont), PdfPCell.ALIGN_CENTER));
            table.AddCell(PhraseCell(new Phrase("Class", bodyFont), PdfPCell.ALIGN_CENTER));
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            table.AddCell(PhraseCell(new Phrase(date2, subTitleFont), PdfPCell.ALIGN_CENTER));
            table.AddCell(PhraseCell(new Phrase(tim, subTitleFont), PdfPCell.ALIGN_CENTER));
            table.AddCell(PhraseCell(new Phrase(airline, subTitleFont), PdfPCell.ALIGN_CENTER));
            table.AddCell(PhraseCell(new Phrase("First", subTitleFont), PdfPCell.ALIGN_CENTER));
            table.AddCell(PhraseCell(new Phrase("", bodyFont), PdfPCell.ALIGN_LEFT));
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            table.AddCell(PhraseCell(new Phrase("", bodyFont), PdfPCell.ALIGN_LEFT));
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            table.AddCell(PhraseCell(new Phrase("", bodyFont), PdfPCell.ALIGN_LEFT));
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell = new PdfPCell(new Phrase("", titleFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.Colspan = 4;
            cell.FixedHeight = 10f;
            table.AddCell(cell);
            var blackListTextFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD,iTextSharp.text.BaseColor.RED);
            var redListTextFont = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(393370));
            var titleChunk = new Chunk("PLEASE BE AT THE BOARDING GATE BEFORE CLOSE TIME ", blackListTextFont);
            var descriptionChunk = new Chunk(gateclose, redListTextFont);
            cell = new PdfPCell(new Phrase(titleChunk));
            cell.Colspan = 3;
            cell.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.HorizontalAlignment = 2;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(descriptionChunk));
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.HorizontalAlignment = 0;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(""));
            cell.Colspan = 4;
            cell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
            cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell.FixedHeight = 2f;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", titleFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
            cell.BorderColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
            cell.Colspan = 4;
            cell.FixedHeight = 3f;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", titleFont));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = iTextSharp.text.BaseColor.BLACK;
            cell.BorderColor = iTextSharp.text.BaseColor.BLACK;
            cell.Colspan = 4;
            cell.FixedHeight = 1f;
            table.AddCell(cell);
            pdfDoc.Add(table);

            pdfDoc.Close();
            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=boardingpass.pdf");
            Response.ContentType = "application/pdf";
            Response.Buffer = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(bytes);
            Response.End();
            Response.Close();
            txtFirstname.Text = "";
            txtLastName.Text = "";
            CleartextBoxes(this);
        }

        catch (Exception ex)
        {
            Label2.Text = ex.Message;
        }

        finally
        {
            con1.Close();
            con2.Close();
            BindGridview();
           
        }

    }
    public void CleartextBoxes(Control parent)
    {

        foreach (Control c in parent.Controls)
        {
            if ((c.GetType() == typeof(TextBox)))
            {

                ((TextBox)(c)).Text = "";
            }

            if (c.HasControls())
            {
                CleartextBoxes(c);
            }
        }
    }  
    private static PdfPCell PhraseCell(Phrase phrase, int align)
    {
        PdfPCell cell = new PdfPCell(phrase);
        cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
        cell.HorizontalAlignment = align;
        cell.PaddingBottom = 2f;
        cell.PaddingTop = 0f;
        return cell;
    }
    private static PdfPCell ImageCell(string path, float scale, int align)
    {
        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path));
        image.ScalePercent(scale);
        PdfPCell cell = new PdfPCell(image);
        cell.HorizontalAlignment = 2;
        cell.BorderColor = iTextSharp.text.BaseColor.WHITE;
        cell.PaddingBottom = 10f;
        cell.PaddingTop = 0f;
        return cell;
    }

    protected void cbSelect_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in gvFlightDetails.Rows)
        {
            
            CheckBox chk = (CheckBox)gvrow.FindControl("cbSelect");
            if (chk.Checked == true)
            {

                string date = gvrow.Cells[0].Text;
                DateTime dt = DateTime.Parse(date);
                int jDate = dt.DayOfYear;
                string flightno = gvrow.Cells[1].Text;
                string fromairport = gvrow.Cells[2].Text;
                string toairport = gvrow.Cells[3].Text;
                string carrierdesignator = gvrow.Cells[4].Text;
                string airline = gvrow.Cells[5].Text;
                string fullname = gvrow.Cells[6].Text;

                Session["gvrow.Cells[5].Text"] = airline.ToString();
                Session["date"] = date.ToString();
                Session["jDate"] = jDate.ToString();
                Session["flightno"] = flightno.ToString();
                Session["fromairport"] = fromairport.ToString();
                Session["toairport"] = toairport.ToString();
                Session["carrierdesignator"] = carrierdesignator.ToString();
                Session["airline"] = airline.ToString();
                Session["fullname"] = fullname.ToString();
            }
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        txtFirstname.Text = "";
        txtLastName.Text = "";
        BindGridview();
    }
}