﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;

namespace Library_Login_System.Views.Admins
{
    public partial class admin_home : System.Web.UI.Page
    {

        //Sql Connection
        SqlConnection db_con = new SqlConnection("Data Source=ECCLESIASTES\\SQLEXPRESS;Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Calls the method to update the time and date
                Update_time();
            }
            catch (Exception ex)
            {
                Response.Write("Error: " + ex);
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                // Calls the method to update the time and date
                Update_time();
            }
            catch (Exception ex)
            {
                Response.Write("Error" + ex);
            }
        }

        /*
         Timer1_Tick updates time & date by calling UpdateTime method, which formats the current time using ToString method, and refreshes the labels.
         */
        private void Update_time()
        {
            var now = DateTime.Now;
            var time = now.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
            var date = now.ToString("dddd, MMMM dd, yyyy", CultureInfo.InvariantCulture).ToUpper();
            var ampm = time.Substring(time.Length - 2);
            ampm = ampm.ToLowerInvariant().ToUpper()[0] + ampm.ToUpperInvariant().Substring(1);
            time = time.Substring(0, time.Length - 2) + ampm;

            Lbl_current_time.Text = time;
            Lbl_current_date.Text = date;

            bool isOnline = IsSystemOnline(now);

            // Enable/disable the login and register buttons based on the time
            if (isOnline && IsWithinWorkingHours(now))
            {
                Lbl_library_status.Text = "SYSTEM ONLINE";
                Lbl_library_status.ForeColor = System.Drawing.ColorTranslator.FromHtml("#00ff00");
            }
            else
            {
                Lbl_library_status.Text = "SYSTEM OFFLINE";
                Lbl_library_status.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");
            }
        }

        private bool IsSystemOnline(DateTime now)
        {
            // Implement your logic to determine if the system is online.
            // You can check connectivity or any other criteria specific to your system.
            // For demonstration purposes, let's assume the system is always online.
            return true;
        }

        private bool IsWithinWorkingHours(DateTime now)
        {
            // Define the working hours (7am to 5pm)
            TimeSpan startWorkingHours = new TimeSpan(7, 0, 0);
            TimeSpan endWorkingHours = new TimeSpan(17, 0, 0);

            // Check if the current time is within the working hours
            TimeSpan currentTime = now.TimeOfDay;
            return currentTime >= startWorkingHours && currentTime <= endWorkingHours;
        }

        protected void btn_home_Click(object sender, EventArgs e)
        {
            Response.Redirect("admin-home.aspx");
        }

        protected void Img_browse_id_Click(object sender, ImageClickEventArgs e)
        {
            // SQL query to retrieve student information
            string sqlQuery = "SELECT Id_no, Last_name, First_name, Course, School_year, Major FROM registration WHERE Id_no = @Id_no;";

            string sqltimelogid = "SELECT COUNT(Time_out) FROM timelog WHERE Id_no = @Id_no";

            string sqlshowtbl = "SELECT Timelog_id, Time_in, Time_out FROM timelog WHERE Id_no = @Id_no";

            SqlCommand cmd = new SqlCommand(sqlQuery, db_con);
            cmd.Parameters.Add("@Id_no", SqlDbType.NVarChar, 50).Value = Txb_search_id.Text;

            SqlCommand sqltimelog = new SqlCommand(sqltimelogid, db_con);
            sqltimelog.Parameters.Add("@Id_no", SqlDbType.NVarChar, 50).Value = Txb_search_id.Text;

            SqlCommand sqltable = new SqlCommand(sqlshowtbl, db_con);
            sqltable.Parameters.Add("@Id_no", SqlDbType.NVarChar, 50).Value = Txb_search_id.Text;

            try
            {
                // Open the database connection
                db_con.Open();
                db_con.ChangeDatabase("Library_Login_Db");
                SqlDataReader sqlreader = cmd.ExecuteReader();

                if (sqlreader.Read())
                {
                    Img_student.ImageUrl = "~/Images/Student Images/" + sqlreader["Id_no"].ToString() + ".JPG";

                    Lbl_std_id.Text = sqlreader["Id_no"].ToString();
                    Lbl_std_id.ForeColor = System.Drawing.ColorTranslator.FromHtml("#f5d7db");
                    Lbl_std_name.Text = sqlreader["Last_name"].ToString() + " " + sqlreader["First_name"].ToString();
                    Lbl_std_course.Text = sqlreader["Course"].ToString();

                    // Convert the school year value to a displayable format
                    string schoolYear = sqlreader["School_year"].ToString();
                    string yearDisplay;
                    switch (schoolYear)
                    {
                        case "1":
                            yearDisplay = "1st Year";
                            break;
                        case "2":
                            yearDisplay = "2nd Year";
                            break;
                        case "3":
                            yearDisplay = "3rd Year";
                            break;
                        default:
                            yearDisplay = schoolYear + "th Year";
                            break;
                    }

                    Lbl_std_year.Text = yearDisplay;

                    Lbl_std_major.Text = sqlreader["Major"].ToString();

                    // Close the SqlDataReader
                    sqlreader.Close();

                    object usertimelog = sqltimelog.ExecuteScalar();

                    if (usertimelog != null)
                    {
                        int count = Convert.ToInt32(usertimelog);
                        Lbl_std_timelog.Text = count.ToString();
                    }

                    sqltimelog.Dispose();

                    SqlDataReader dt_reader = null;

                    try
                    {
                        dt_reader = sqltable.ExecuteReader();

                        DataTable dataTable = new DataTable();
                        dataTable.Load(dt_reader);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Access the data in each row
                            string timeIn = row["Time_in"].ToString();
                            string timeOut = row["Time_out"].ToString();

                            // Perform any necessary operations with the data
                            // For example, you can display it or manipulate it further
                        }

                        GridView1.DataSource = dataTable;
                        GridView1.DataBind();
                    }
                    finally
                    {
                        // Close the SqlDataReader and SqlConnection in the finally block
                        if (dt_reader != null)
                        {
                            dt_reader.Close();
                        }

                        if (db_con.State != ConnectionState.Closed)
                        {
                            db_con.Close();
                        }
                    }

                }
                else
                {
                    Lbl_std_id.Text = "STUDENT DOES NOT EXISTS";
                    Lbl_std_id.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");

                    Lbl_std_name.Text = "";
                    Lbl_std_course.Text = "";
                    Lbl_std_year.Text = "";
                    Lbl_std_major.Text = "";

                    Lbl_std_timelog.Text = "";

                    sqlreader.Close();

                }

                // Close the database connection
                db_con.Close();

            }
            catch (SqlException sqlex)
            {
                Response.Write(sqlex);
            }

            finally
            {
                // Dispose the SqlCommand
                cmd.Dispose();
                sqltimelog.Dispose();

                if (db_con.State != System.Data.ConnectionState.Closed)
                {
                    db_con.Close(); // Close the database connection if it is still open
                }
                db_con.Dispose(); // Dispose the SqlConnection
            }


        }

        protected void btn_register_Click(object sender, EventArgs e)
        {
            Response.Redirect("admin-register.aspx");
        }

        protected void btn_timelog_Click(object sender, EventArgs e)
        {
            Response.Redirect("admin-timelog.aspx");
        }

        protected void btn_graph_Click(object sender, EventArgs e)
        {
            Response.Redirect("admin-graph.aspx");
        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/library-home.aspx");
        }


    }
}