<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="_Default" %>

 
<asp:Content ID="head" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
  
    <table style="width: 100%">
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td align="right">
                <br />
                <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small" PostBackUrl="~/Login.aspx" ForeColor="#F0F0F0">Login</asp:LinkButton>
                <br />
            
            </td>
        </tr>
    </table>
 
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
   
  
       <div class="login_box">
<div class="heading_login">Search</div>
 <table class="login_fildes" style="font-weight: bold; color: #375880;">
 <tr>
 <td align="center" colspan="2" style="width: 522px; line-height: 19px; height: 47px;">
 
     Booking Reference Number 
     <br />
     <br />
     <asp:TextBox ID="txtSerialNum" runat="server" CssClass="textfield1" AutoCompleteType="Disabled" Height="19px" Width="195px"></asp:TextBox>
     <br />
 </td>
 
 </tr>
     <tr>
         <td align="center" colspan="2" style="width: 522px; height: 67px; vertical-align: top; " >
            
             <br />
            
             <asp:Button ID="btnSearch" runat="server" Text="Search" Font-Bold="True" Height="32px" Width="95px" OnClick="btnSearch_Click" Font-Size="Small" />
          
             <br />
          
     <asp:Panel ID="Panel1" runat="server"  Width="500px" HorizontalAlign="Center" BorderColor="Black" CssClass="panel" ForeColor="#660033" Font-Bold="False">
     </asp:Panel>
   
         </td>
     </tr >
        
    
 </table>
         
 </div>
   
</asp:Content>
 

