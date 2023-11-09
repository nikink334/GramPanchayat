using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GramPanchayat
{
    
    public partial class EmployeesMang : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Nikita\Desktop\project_main\Main_DataBase\GramPanchayat.accdb";
        private OleDbConnection conn;
        public EmployeesMang()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                // Validate and convert input values
                if (!int.TryParse(txt_regNo.Text, out int regNo))
                {
                    MessageBox.Show("Please enter a valid registration number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                string Ename = txt_name.Text;
                if (string.IsNullOrWhiteSpace(Ename))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



                if (!DateTime.TryParse(date_DOB.Text, out DateTime dobDate))
                {
                    MessageBox.Show("Please enter a valid DOB date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string designation = txt_designation.Text;
                if (string.IsNullOrWhiteSpace(designation))
                {
                    MessageBox.Show("Please enter a valid Designation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txt_salery.Text, out int salery))
                {
                    MessageBox.Show("Please enter a valid Salery.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string address = txt_address.Text;

                string contactNo = txt_contactNo.Text;
                if (string.IsNullOrWhiteSpace(contactNo) || contactNo.Length != 10)
                {
                    MessageBox.Show("Please enter a valid 10-digit Contact number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


                // Use parameterized query to insert data
                cmd.CommandText = "INSERT INTO Employee (Reg_No, Emp_Name, DOB, Designation, Salery, Address, Contact_No, Aadhar_No, Voter_Id ) " +
                                  "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

                cmd.Parameters.AddWithValue("@p1", regNo);
                cmd.Parameters.AddWithValue("@p2", Ename);
                cmd.Parameters.AddWithValue("@p3", dobDate);
                cmd.Parameters.AddWithValue("@p4", designation);
                cmd.Parameters.AddWithValue("@p5", salery);
                cmd.Parameters.AddWithValue("@p6", address);
                cmd.Parameters.AddWithValue("@p7", contactNo);
                cmd.Parameters.AddWithValue("@p8", aadharNo);
                cmd.Parameters.AddWithValue("@p9", voterId);

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

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                // Validate and convert input values
                if (!int.TryParse(txt_regNo.Text, out int regNo))
                {
                    MessageBox.Show("Please enter a valid registration number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string Ename = txt_name.Text;
                if (string.IsNullOrWhiteSpace(Ename))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(date_DOB.Text, out DateTime dobDate))
                {
                    MessageBox.Show("Please enter a valid DOB date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string designation = txt_designation.Text;
                if (string.IsNullOrWhiteSpace(designation))
                {
                    MessageBox.Show("Please enter a valid Designation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txt_salery.Text, out int salery))
                {
                    MessageBox.Show("Please enter a valid Salary.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string address = txt_address.Text;

                string contactNo = txt_contactNo.Text;
                if (string.IsNullOrWhiteSpace(contactNo) || contactNo.Length != 10)
                {
                    MessageBox.Show("Please enter a valid 10-digit Contact number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // Use parameterized query to update data
                cmd.CommandText = "UPDATE Employee SET Emp_Name = ?, DOB = ?, Designation = ?, Salery = ?, Address = ?, Contact_No = ?, Aadhar_No = ?, Voter_Id = ? WHERE Reg_No = ?";

                cmd.Parameters.AddWithValue("@p1", Ename);
                cmd.Parameters.AddWithValue("@p2", dobDate);
                cmd.Parameters.AddWithValue("@p3", designation);
                cmd.Parameters.AddWithValue("@p4", salery);
                cmd.Parameters.AddWithValue("@p5", address);
                cmd.Parameters.AddWithValue("@p6", contactNo);
                cmd.Parameters.AddWithValue("@p7", aadharNo);
                cmd.Parameters.AddWithValue("@p8", voterId);
                cmd.Parameters.AddWithValue("@p9", regNo);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Record Updated Successfully.", "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btn_print_Click(object sender, EventArgs e)
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

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                // Validate and convert search input (registration number)
                if (!int.TryParse(txt_regNo.Text, out int searchRegNo))

                {
                    MessageBox.Show("Please enter a valid registration number to search.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Use parameterized query to search for a specific record
                cmd.CommandText = "SELECT * FROM Employee WHERE Reg_No = ?";
                cmd.Parameters.AddWithValue("@p1", searchRegNo);

                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Fill the textboxes with the retrieved data
                    txt_name.Text = reader["Emp_Name"].ToString();
                    date_DOB.Text = reader["DOB"].ToString();
                    txt_designation.Text = reader["Designation"].ToString();
                    txt_salery.Text = reader["Salery"].ToString();
                    txt_address.Text = reader["Address"].ToString();
                    txt_contactNo.Text = reader["Contact_No"].ToString();
                    txt_aadharNo.Text = reader["Aadhar_No"].ToString();
                    txt_voterId.Text = reader["Voter_Id"].ToString();
                }
                else
                {
                    MessageBox.Show("Record not found.", "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                reader.Close();
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

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_regNo.Clear();
            txt_name.Clear();
            date_DOB.Value = DateTime.Now;
            txt_designation.Clear();
            txt_salery.Clear();
            txt_address.Clear();
            txt_contactNo.Clear();
            txt_aadharNo.Clear();
            txt_voterId.Clear();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
            Dashboard ds = new Dashboard(); 
            ds.Show();
        }
    }
}
