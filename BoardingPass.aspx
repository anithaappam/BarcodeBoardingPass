
<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="BoardingPass.aspx.cs" Inherits="Default2" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

 
<asp:Content ID="head" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <script type="text/javascript"> 
   
       function CheckSingleCheckbox(ob) {
           var grid = ob.parentNode.parentNode.parentNode;
           var inputs = grid.getElementsByTagName("input");
           for (var i = 0; i < inputs.length; i++) {
               if (inputs[i].type == "checkbox") {
                   if (ob.checked && inputs[i] != ob && inputs[i].checked) {
                       inputs[i].checked = false;
                   }
               }
           }
       }
       function onlyAlphabets(evt) {
           var charCode;
           if (window.event)
               charCode = window.event.keyCode;
           else
               charCode = evt.which;
           if (charCode == 32)
               return true;
           if (charCode > 31 && charCode < 65) 
               return false;
           if (charCode > 90 && charCode < 97)
               return false;
           if (charCode > 122)
               return false;
           return true;
       }
       function clearTextBox() {
           document.getElementById('txtFirstname').value = '';
       }
       function clearTextBox() {
           var elements = [];
           elements = document.getElementsByClassName("form-control"); 

           for (var i = 0; i < elements.length ; i++) {
               elements[i].value = "";
           }
       }

        </script>
   <style>
        .hiddencol

  {
    display: none;
   }
       .auto-style1 {
           height: 29px;
       }
       .auto-style2 {
           width: 245px;
           height: 29px;
       }
   </style>

    <table style="width: 100%">
        <tr>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td align="right">
                
                <br />
                
                <br />
                <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small" PostBackUrl="~/Login.aspx" ForeColor="White">Logout</asp:LinkButton>
            </td>
        </tr>
    </table>
 
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
           
     <div class="loginbox">
<div class="headinglogin">Boarding Pass</div>
 <table class="loginfildes" style="font-weight: bold; color: #375880;">

<tr>
<td class="auto-style1">
  Firstname:
</td>
<td class="auto-style1"  >
      <br />
      <asp:DropDownList ID="DropDownList1" runat="server" Height="23px">
         <asp:ListItem>Ms</asp:ListItem>
         <asp:ListItem>Mr</asp:ListItem>
         <asp:ListItem>Mrs</asp:ListItem>
     </asp:DropDownList>
 
      <asp:TextBox ID="txtFirstname" ClientIDMode="Static" runat="server" CssClass="textfield1" Width="192px" class="form-control" AutoCompleteType="Disabled" onkeypress="return onlyAlphabets(event);"   EnableViewState="False" Height="18px"></asp:TextBox>
      <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Group1" ForeColor="Red" ControlToValidate="txtFirstname" ErrorMessage="Firstname Required!" Font-Bold="False"></asp:RequiredFieldValidator>
   <td class="auto-style1">
     Lastname:
        
  </td>
<td class="auto-style2">
    
     <br />
    
     <asp:TextBox ID="txtLastName" runat="server" ClientIDMode="Static" CssClass="textfield1" Width="180px" AutoCompleteType="Disabled" class="form-control" onkeypress="return onlyAlphabets(event);" Height="18px" ></asp:TextBox>
     <br />
    &nbsp;
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Group1" ForeColor="Red" ControlToValidate="txtLastName" ErrorMessage="Lastname Required!" Font-Bold="False"></asp:RequiredFieldValidator>
</td>
</tr>
     <tr>
         <td colspan="4" align="center">
            
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            
                                        <asp:Button ID="btnGenerate" runat="server" ValidationGroup="Group1" Text="Generate" Font-Bold="True" Height="30px" Width="75px" OnClick="btnGenerate_Click" />
                 &nbsp; 
                                        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" Font-Bold="True" Height="30px" Width="75px" OnClick="btnRefresh_Click"  />

                                        &nbsp;<asp:CustomValidator ID="CustomValidator1" runat="server" ForeColor="Red" ValidationGroup="Group1" ErrorMessage="Please Check the flight!" ClientValidationFunction="Validate" Font-Bold="False" ></asp:CustomValidator>

                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <br />
                 <asp:Label ID="Label5" runat="server"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="Label2" runat="server"></asp:Label>
             <asp:Label ID="Label3" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="Label4" runat="server" Visible="False"></asp:Label>
             
                     </td>
   
     </tr>

</table>
         </div>
   
    <div style=" height: 300px; width:1442px">
        <table class="fonts_styles2">
            <tr>
                <td align="center" style="width: 1442px; height: 217px;">
                    &nbsp;
                    <br />
                    &nbsp;<asp:GridView ID="gvFlightDetails" runat="server" BackColor="#F1F6FF"  CellPadding="4" ShowFooter="True" HorizontalAlign="Center" AutoGenerateColumns="False"
        AllowSorting="True" GridLines="None" ForeColor="#3A31F7" Font-Bold="True" >

                      <RowStyle  BackColor="#E4E4E4"/>

                <Columns> 
                        <asp:BoundField DataField="DepartDateTime"  HeaderText="Date" >
                <HeaderStyle Font-Bold="true" />
                <ItemStyle HorizontalAlign="Center"
                    Width="140px"/>
            </asp:BoundField>
                          <asp:BoundField  DataField="FlightNo" HeaderText="Flight No" >
                           <HeaderStyle Font-Bold="true" />
                <FooterStyle HorizontalAlign="Center" Font-Bold="true" />
                <ItemStyle HorizontalAlign="Center" Width="90px" />
                     </asp:BoundField>
                     
                    
                          <asp:BoundField DataField="FromAirport" HeaderText="From Airport">
                          <HeaderStyle  Font-Bold="true"/>
                <FooterStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center"
                    Width="110px" />
                     </asp:BoundField>
                       
                       <asp:BoundField DataField="ToAirport" HeaderText="To Airport" >
                          <HeaderStyle Font-Bold="true" />
                <FooterStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center"
                    Width="110px" />
                     </asp:BoundField>
         <asp:BoundField DataField="CarrierDesignator" HeaderText="Carrier Designator" >
                          <HeaderStyle Font-Bold="true"/>
                <FooterStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center"
                    Width="120px" />
                     </asp:BoundField>
                    <asp:BoundField DataField="Airline" HeaderText="Airline" >
                          <HeaderStyle Font-Bold="true" />
                <FooterStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center"
                    Width="120px" />
                     </asp:BoundField>
        
                       <asp:BoundField DataField="Name" HeaderText="Airportname" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                          <HeaderStyle  Font-Bold="true"/>
                <FooterStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center"
                     />
                     </asp:BoundField>
                <asp:templatefield HeaderText="Select" HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center">
    <itemtemplate>
        <asp:checkbox ID="cbSelect"  runat="server" OnCheckedChanged="cbSelect_CheckedChanged" AutoPostBack="True"  onclick ="CheckSingleCheckbox(this)" ></asp:checkbox>

         </itemtemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:templatefield>
            
         </Columns>
          
      <FooterStyle Font-Bold="True"  BackColor="#A1ABB8" />
         <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#2461BF" Font-Bold="True" />
         <HeaderStyle Font-Bold="True" Font-Size="Small"  />
            <AlternatingRowStyle BackColor="White"/>
                        </asp:GridView>
  <script type = "text/javascript">
      function Validate(sender, args) {
          var gridView = document.getElementById("<%=gvFlightDetails.ClientID %>");
          var checkBoxes = gridView.getElementsByTagName("input");
         
         
          args.IsValid = true;
          for (var i = 0; i < checkBoxes.length; i++) {
              if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                  
                  return;
              } 
          }
        //  alert("Check atleast one record!");
          args.IsValid = false;
          
      }
 
  </script>
                </td>
            </tr>

        </table>
    </div>

</asp:Content>

