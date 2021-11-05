<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Document.aspx.cs" Inherits="DocumentSum.Document" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        ÖZETLENECEK DÖKÜMAN:<br />
        <br />
        <asp:TextBox ID="TextBox1" runat="server" Height="500px" TextMode="MultiLine" Width="600px"></asp:TextBox>
        <asp:ListBox ID="ListBox3" runat="server" Height="500px"></asp:ListBox>
        <asp:ListBox ID="ListBox4" runat="server" Height="500px"></asp:ListBox>
        <asp:ListBox ID="ListBox5" runat="server" Height="500px"></asp:ListBox>
        <asp:ListBox ID="ListBox2" runat="server" Height="500px" Visible="False"></asp:ListBox>
        <br />
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Özetle" Height="28px" Width="177px" />
        <br />
        <br />
        <br />
        <br />
        <asp:ListBox ID="ListBox6" runat="server" Height="500px" Width="100%" Visible="False"></asp:ListBox>
        <br />
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
    </form>
</body>
</html>
