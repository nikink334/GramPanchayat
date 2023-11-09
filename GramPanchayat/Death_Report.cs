﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.OleDb;


namespace GramPanchayat
{
    public partial class Death_Report : Form
    {
        private string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\GramPanchayat.accdb";
        private OleDbConnection conn;
        private DataTable dt;
        public Death_Report()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
            dt = new DataTable();
        }



        private void Death_Report_Load(object sender, EventArgs e)
        {
       
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            
            dt.Rows.Clear();
            conn.Open();

            // Create an SQL command for the query
            using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM Death_Certificate WHERE Reg_No LIKE ?", conn))
            {
                // Replace ? with the actual parameter marker used in your database
                cmd.Parameters.AddWithValue("?", "%" + txt_search.Text + "%");

                // Execute the query and load the results into the DataTable
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            // Create a ReportDataSource with the DataTable
            ReportDataSource rds = new ReportDataSource("Death", dt);

            // Set the data source for the report viewer
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            // Refresh the report
            reportViewer1.RefreshReport();

            conn.Close();

    }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
            Dashboard ds = new Dashboard(); 
            ds.Show();
        }

        private void btn_reportAll_Click(object sender, EventArgs e)
        {
            dt.Rows.Clear();
            conn.Open();

            // Create an SQL command for the query
            using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM Death_Certificate ", conn))
            {
                // Replace ? with the actual parameter marker used in your database
                //cmd.Parameters.AddWithValue("?", "%" + txt_search.Text + "%");

                // Execute the query and load the results into the DataTable
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            // Create a ReportDataSource with the DataTable
            ReportDataSource rds = new ReportDataSource("Death", dt);

            // Set the data source for the report viewer
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);

            // Refresh the report
            reportViewer1.RefreshReport();

            conn.Close();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_search.Clear();
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.RefreshReport();
        }
    }
}
