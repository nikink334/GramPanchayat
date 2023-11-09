using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Printing;


namespace GramPanchayat
{
    public partial class Residence : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Nikita\Desktop\project_main\Main_DataBase\GramPanchayat.accdb";
        private OleDbConnection conn;

        public Residence()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
        }

        private void Residence_Load(object sender, EventArgs e)
        {

        }

        // SAVE BUTTTON

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                // Validate and convert input values
                if (!int.TryParse(txt_registrastionNo.Text, out int regNo))
                {
                    MessageBox.Show("Please enter a valid registration number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(dateTimePicker1.Text, out DateTime regDate))
                {
                    MessageBox.Show("Please enter a valid registration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string regName = txt_name.Text;
                if (string.IsNullOrWhiteSpace(regName))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate and convert input values
                string aadharNo = txt_aadharNo.Text;
                if (string.IsNullOrWhiteSpace(aadharNo) || aadharNo.Length != 12)
                {
                    MessageBox.Show("Please enter a valid 12-digit Aadhar number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string voterId = txt_voterId.Text;
                if (string.IsNullOrWhiteSpace(voterId))
                {
                    MessageBox.Show("Please enter a valid voter ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string regAddress = txt_address.Text;

                // Use parameterized query to insert data
                cmd.CommandText = "INSERT INTO New_Residence (Reg_No, Reg_Date, Reg_Name, Aadhar_No, Voter_Id, Reg_Address) " +
                                  "VALUES (?, ?, ?, ?, ?, ?)";

                cmd.Parameters.AddWithValue("@p1", regNo);
                cmd.Parameters.AddWithValue("@p2", regDate);
                cmd.Parameters.AddWithValue("@p3", regName);
                cmd.Parameters.AddWithValue("@p4", aadharNo);
                cmd.Parameters.AddWithValue("@p5", voterId);
                cmd.Parameters.AddWithValue("@p6", regAddress);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Record Saved Successfully.", "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

        }

        // UPDATE BUTTTON

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                // Validate and convert input values
                if (!int.TryParse(txt_registrastionNo.Text, out int regNo))
                {
                    MessageBox.Show("Please enter a valid registration number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(dateTimePicker1.Text, out DateTime regDate))
                {
                    MessageBox.Show("Please enter a valid registration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string regName = txt_name.Text;
                if (string.IsNullOrWhiteSpace(regName))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string aadharNo = txt_aadharNo.Text;
                if (string.IsNullOrWhiteSpace(aadharNo) || aadharNo.Length != 12)
                {
                    MessageBox.Show("Please enter a valid 12-digit Aadhar number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string voterId = txt_voterId.Text;
                if (string.IsNullOrWhiteSpace(voterId))
                {
                    MessageBox.Show("Please enter a valid voter ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string regAddress = txt_address.Text;

                // Use parameterized query to update data
                cmd.CommandText = "UPDATE New_Residence SET Reg_Date = ?, Reg_Name = ?, Aadhar_No = ?, Voter_Id = ?, Reg_Address = ? WHERE Reg_No = ?";

                cmd.Parameters.AddWithValue("@p1", regDate);
                cmd.Parameters.AddWithValue("@p2", regName);
                cmd.Parameters.AddWithValue("@p3", aadharNo);
                cmd.Parameters.AddWithValue("@p4", voterId);
                cmd.Parameters.AddWithValue("@p5", regAddress);
                cmd.Parameters.AddWithValue("@p6", regNo);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Record Updated Successfully.", "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No matching record found for the provided Registration Number.", "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        // RESET BUTTTON
        private void btn_clear_Click(object sender, EventArgs e)
        {
            // Clear all input fields
            txt_registrastionNo.Clear();
            dateTimePicker1.Value = DateTime.Now; // Reset the date picker to the current date and time
            txt_name.Clear();
            txt_aadharNo.Clear();
            txt_voterId.Clear();
            txt_address.Clear();
        }

        // EXIT BUTTTON
        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Create a Bitmap to capture the form's content
            Bitmap bitmap = new Bitmap(this.Width, this.Height);

            // Draw the form onto the Bitmap
            this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));

            // Create a Graphics object for printing
            Graphics printGraphics = e.Graphics;

            // Specify the position and size for printing
            Rectangle printArea = e.MarginBounds;

            // Draw the captured content on the printed page
            printGraphics.DrawImage(bitmap, printArea);

            // Dispose of the Bitmap
            bitmap.Dispose();
        }

        // PRINT BUTTTON
        private void button4_Click(object sender, EventArgs e)
        {
            // Create a PrintDocument object
            PrintDocument printDoc = new PrintDocument();

            // Add an event handler to print the form's content
            printDoc.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

            // Display the PrintDialog to configure and initiate the printing
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDoc;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }

        // SEARCH BUTTTON
        private void btn_search_Click(object sender, EventArgs e)
        {
            // Retrieve the registration number entered by the user
            string regNoToSearch = txt_registrastionNo.Text.Trim();

            // Perform a database query to retrieve the record with the specified Reg_No
            string query = "SELECT * FROM New_Residence WHERE Reg_No = @RegNo";

            using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Nikita\\Desktop\\project_main\\Main_DataBase\\GramPanchayat.accdb"))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RegNo", regNoToSearch);

                try
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Populate the form fields with data from the database
                        txt_registrastionNo.Text = reader["Reg_No"].ToString();
                        dateTimePicker1.Value = DateTime.Parse(reader["Reg_Date"].ToString());
                        txt_name.Text = reader["Reg_Name"].ToString();
                        txt_aadharNo.Text = reader["Aadhar_No"].ToString();
                        txt_voterId.Text = reader["Voter_Id"].ToString();
                        txt_address.Text = reader["Reg_Address"].ToString();

                        // Additional actions you may want to perform after a successful search
                    }
                    else
                    {
                        // Handle the case where no matching record was found
                        MessageBox.Show("No record found for the specified Reg_No.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

