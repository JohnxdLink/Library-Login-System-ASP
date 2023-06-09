﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin-graph.aspx.cs" Inherits="Library_Login_System.Style.Admin_Styles.admin_graph" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="../../Style/Admin-Styles/admin-graph.css" />
    <title>Admin | Graph</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="whole-container">
            <aside class="aside-left-15">
                <div class="logo-img">
                    <asp:ImageButton ID="img_btn_logo" runat="server" ImageUrl="~/Images/Icons/library.png" Height="150" Width="150" />
                </div>

                <div class="button-container">
                    <div class="button-group">
                        <asp:Button ID="btn_home" runat="server" Text="HOME" CssClass="button-design" OnClick="btn_home_Click" />
                        <asp:Button ID="btn_register" runat="server" Text="REGISTERED" CssClass="button-design" OnClick="btn_register_Click" />
                        <asp:Button ID="btn_timelog" runat="server" Text="TIMELOG" CssClass="button-design" OnClick="btn_timelog_Click" />
                        <asp:Button ID="btn_graph" runat="server" Text="GRAPH" CssClass="button-design" OnClick="btn_graph_Click" />
                    </div>

                    <div class="button-group">
                        <asp:Button ID="btn_logout" runat="server" Text="LOGOUT" CssClass="button-design" OnClick="btn_logout_Click" />
                    </div>
                </div>
            </aside>

            <main class="main-60">
            </main>

            <aside class="aside-right-25">
            </aside>

            <footer class="footer-5">
                Under Development: 05/23/2023
            </footer>

        </div>
    </form>
</body>
</html>
