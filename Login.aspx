<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
           <div class="loginbox3">
<div class="headinglogin3">Login</div>
 <table class="loginfildes3" style="font-weight: bold; color: #375880;">
 <tr>
     <td width="50%" align="center" style="height: 29px">
                  Username :</td>
     <td style="height: 29px">

     <asp:TextBox ID="txtUsername" runat="server" CssClass="textfield1" AutoCompleteType="Disabled" Height="18px" Width="170px" ></asp:TextBox>
 
     </td>
 </tr>
     <tr>
     <td align="center" width="50%">

         <br />

         Password :</td>
     <td>

         <br />

     <asp:TextBox ID="txtPassword" runat="server" CssClass="textfield1" TextMode="Password" AutoCompleteType="Disabled" Height="18px" Width="170px"></asp:TextBox>
 
     </td>
 </tr>
     <tr>
     <td colspan="2" align="center" style="height: 52px">

             <asp:Label ID="Label1" runat="server" Font-Bold="False" ForeColor="Red"></asp:Label>

             <br />

             <asp:Button ID="btnSearch" runat="server" Text="Login" Font-Bold="True" Height="30px" Width="90px" OnClick="btnSearch_Click" Font-Size="Small" />

     </td>
 </tr>
 </table>
         
 </div>
 
</asp:Content>

