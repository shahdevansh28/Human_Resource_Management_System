﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace HR_Management_System
{
    public partial class AddDepartment : System.Web.UI.Page
    {
        DataSet ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            mesPanelAddDepErr.Visible = false;
            mesPanelAddDepSucc.Visible = false;
            addDepPanel.Visible = true;
            if (Session["userName"] == null)
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        protected void btnAddEmp_Click(object sender, EventArgs e)
        {
            string conStr = WebConfigurationManager.ConnectionStrings["employeeConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select * from Department";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            SqlCommandBuilder builder = new SqlCommandBuilder(da);

            try
            {
                using (con)
                {
                    con.Open();
                    da.Fill(ds, "Department");
                    con.Close();

                    bool flag = true;
                    foreach (DataRow row in ds.Tables["Department"].Rows)
                    {
                        if ((((string)row["department_name"]) != Page.Request.Form["depName"]))
                        {
                            continue;
                        }
                        else
                        {
                            flag = false;
                            lbErrorAddDep.Text = "Department name is already taken.";
                            mesPanelAddDepErr.Visible = true;
                            break;
                        }
                    }

                    if (flag)
                    {
                        DataTable dt = ds.Tables["Department"];
                        DataRow dr = dt.NewRow();
                        dr["department_name"] = Page.Request.Form["depName"];
                        dr["department_des"] = Page.Request.Form["depDesc"];
                        dt.Rows.Add(dr);
                        da.Update(ds, "Department");
                        lbSuccAddDep.Text = "Department added succeessfully";
                        mesPanelAddDepSucc.Visible = true;
                        addDepPanel.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lbErrorAddDep.Text = "Error" + ex.Message;
                mesPanelAddDepErr.Visible = true;
                //Response.Write("Error1 : " + ex.Message);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminHome.aspx", false);
        }

    }
}