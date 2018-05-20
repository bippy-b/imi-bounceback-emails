<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="imi_bounceback_emails._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size: medium">
    
        <h2>Please enter the email address which has bounced back to notify the Project Team</h2>
    
    </div>
        <asp:TextBox ID="TextBox1" runat="server" Width="433px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" Width="107px" />
        <br />
        <br />
        <asp:Label ID="lblStatus" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lblIdentity" runat="server"></asp:Label>
    </form>
</body>
</html>
