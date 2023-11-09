using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GramPanchayat
{
    public partial class Property : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Nikita\Desktop\project_main\Main_DataBase\GramPanchayat.accdb";
        private OleDbConnection conn;
        public Property()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Property_Load(object sender, EventArgs e)
        {

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                // Validate and convert input values
                if (!int.TryParse(txt_regNo.Text, out int proNo))
                {
                    MessageBox.Show("Please enter a valid property number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(picker_regDate.Text, out DateTime regDate))
                {
                    MessageBox.Show("Please enter a valid registration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string proOwner = txt_proOwner.Text;
                if (string.IsNullOrWhiteSpace(proOwner))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string proAddress = txt_proAddress.Text;
                if (string.IsNullOrWhiteSpace(proAddress))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string contactNo = txt_contactNo.Text;
                if (string.IsNullOrWhiteSpace(contactNo) || contactNo.Length != 10)
                {
                    MessageBox.Show("Please enter a valid 10-digit Contact number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string voterId = txt_voterId.Text;
                if (string.IsNullOrWhiteSpace(voterId))
                {
                    MessageBox.Show("Please enter a valid voter ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string aadharNo = txt_AadharNo.Text;
                if (string.IsNullOrWhiteSpace(aadharNo) || aadharNo.Length != 12)
                {
                    MessageBox.Show("Please enter a valid 12-digit Aadhar number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(picker_purchaseDate.Text, out DateTime purchaseDate))
                {
                    MessageBox.Show("Please enter a valid  date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string selectedProType = combo_proType.Text;

                string[] validProTypes = { "Empty Land", "Apartment", "Bunglow", "Flats" };

                // Check if the selected text is in the list of valid options
                if (!validProTypes.Contains(selectedProType))
                {
                    MessageBox.Show("Please select a valid Property Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string proArea = txt_proArea.Text;
                if (string.IsNullOrWhiteSpace(proArea))
                {
                    MessageBox.Show("Please enter a valid Area.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string oldOwner = txt_previousOwner.Text;
                if (string.IsNullOrWhiteSpace(oldOwner))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                // Use parameterized query to insert data
                cmd.CommandText = "INSERT INTO NewProperty (Pro_No, Reg_Date, Pro_Owner, Pro_Address, Contact_No, Voter_Id, Aadhar_No, Purchase_Date, Pro_Type, Pro_Area, Old_Owner) " +
                                  "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";


                cmd.Parameters.AddWithValue("@p1", proNo);
                cmd.Parameters.AddWithValue("@p2", regDate);
                cmd.Parameters.AddWithValue("@p3", proOwner);
                cmd.Parameters.AddWithValue("@p4", proAddress);
                cmd.Parameters.AddWithValue("@p5", contactNo);
                cmd.Parameters.AddWithValue("@p6", voterId);
                cmd.Parameters.AddWithValue("@p7", aadharNo);
                cmd.Parameters.AddWithValue("@p8", purchaseDate);
                cmd.Parameters.AddWithValue("@p9", selectedProType);
                cmd.Parameters.AddWithValue("@p10", proArea);
                cmd.Parameters.AddWithValue("@p11", oldOwner);


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

                int proNo;
                if (!int.TryParse(txt_regNo.Text, out proNo))
                {
                    MessageBox.Show("Please enter a valid property number for updating.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Use a parameterized query to update the record
                cmd.CommandText = "UPDATE NewProperty SET Reg_Date=?, Pro_Owner=?, Pro_Address=?, Contact_No=?, Voter_Id=?, Aadhar_No=?, Purchase_Date=?, Pro_Type=?, Pro_Area=?, Old_Owner=? WHERE Pro_No = ?";
                cmd.Parameters.AddWithValue("@p1", picker_regDate.Text);
                cmd.Parameters.AddWithValue("@p2", txt_proOwner.Text);
                cmd.Parameters.AddWithValue("@p3", txt_proAddress.Text);
                cmd.Parameters.AddWithValue("@p4", txt_contactNo.Text);
                cmd.Parameters.AddWithValue("@p5", txt_voterId.Text);
                cmd.Parameters.AddWithValue("@p6", txt_AadharNo.Text);
                cmd.Parameters.AddWithValue("@p7", picker_purchaseDate.Text);
                cmd.Parameters.AddWithValue("@p8", combo_proType.Text);
                cmd.Parameters.AddWithValue("@p9", txt_proArea.Text);
                cmd.Parameters.AddWithValue("@p10", txt_previousOwner.Text);
                cmd.Parameters.AddWithValue("@p11", proNo);

                int rowsUpdated = cmd.ExecuteNonQuery();

                if (rowsUpdated > 0)
                {
                    MessageBox.Show("Record Updated Successfully.", "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else
                {
                    MessageBox.Show("Update failed. Record not found.", "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                // Assuming you want to search based on Pro_No (Property Number)
                int proNo;
                if (!int.TryParse(txt_regNo.Text, out proNo))
                {
                    MessageBox.Show("Please enter a valid property number for searching.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Use a parameterized query to search for the record
                cmd.CommandText = "SELECT * FROM NewProperty WHERE Pro_No = ?";
                cmd.Parameters.AddWithValue("@p1", proNo);

                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Populate the form fields with the retrieved data
                    txt_regNo.Text = reader["Pro_No"].ToString();
                    picker_regDate.Text = reader["Reg_Date"].ToString();
                    txt_proOwner.Text = reader["Pro_Owner"].ToString();
                    txt_proAddress.Text = reader["Pro_Address"].ToString();
                    txt_contactNo.Text = reader["Contact_No"].ToString();
                    txt_voterId.Text = reader["Voter_Id"].ToString();
                    txt_AadharNo.Text = reader["Aadhar_No"].ToString();
                    picker_purchaseDate.Text = reader["Purchase_Date"].ToString();
                    combo_proType.Text = reader["Pro_Type"].ToString();
                    txt_proArea.Text = reader["Pro_Area"].ToString();
                    txt_previousOwner.Text = reader["Old_Owner"].ToString();

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
            // Clear all input fields
            txt_regNo.Clear();
            picker_regDate.Value = DateTime.Now; // Reset the date picker to the current date and time
            txt_proOwner.Clear();
            txt_proAddress.Clear();
            txt_contactNo.Clear();
            txt_voterId.Clear();
            txt_AadharNo.Clear();
            picker_purchaseDate.Value = DateTime.Now;
            combo_proType.Items.Clear();
            txt_proArea.Clear();
            txt_previousOwner.Clear();
        }

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

       
    }
}
