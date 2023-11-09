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
    public partial class PropertyTax : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Nikita\Desktop\project_main\Main_DataBase\GramPanchayat.accdb";
        private OleDbConnection conn;
        public PropertyTax()
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
                if (!int.TryParse(txt_proTaxId.Text, out int proTaxId))
                {
                    MessageBox.Show("Please enter a valid property number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txt_regNo.Text, out int proNo))
                {
                    MessageBox.Show("Please enter a valid property number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(date_reg.Text, out DateTime regDate))
                {
                    MessageBox.Show("Please enter a valid registration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string pName = txt_name.Text;
                if (string.IsNullOrWhiteSpace(pName))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string address = txt_address.Text;
                if (string.IsNullOrWhiteSpace(address))
                {
                    MessageBox.Show("Please enter a valid address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string contactNo = txt_contactNo.Text;
                if (string.IsNullOrWhiteSpace(contactNo) || contactNo.Length != 10)
                {
                    MessageBox.Show("Please enter a valid 10-digit Contact number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string propertyArea = txt_proArea.Text;
                if (string.IsNullOrWhiteSpace(propertyArea))
                {
                    MessageBox.Show("Please enter a valid property area.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txt_calculate.Text, out int totalTax))
                {
                    MessageBox.Show("Please enter a valid total tax amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Use parameterized query to insert data into Property_Tax table
                cmd.CommandText = "INSERT INTO PropertyTax (Pro_Tax_Id,Pro_NO, Reg_Date, P_Name, P_Address, Contact_No, Pro_Area, Total_Tax) " +
                                  "VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)";

                cmd.Parameters.AddWithValue("@p1", proTaxId);
                cmd.Parameters.AddWithValue("@p2", proNo);
                cmd.Parameters.AddWithValue("@p3", regDate);
                cmd.Parameters.AddWithValue("@p4", pName);
                cmd.Parameters.AddWithValue("@p5", address);
                cmd.Parameters.AddWithValue("@p6", contactNo);
                cmd.Parameters.AddWithValue("@p7", propertyArea);
                cmd.Parameters.AddWithValue("@p8", totalTax);

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

        private int ParseIntValue(string text)
        {

            int intValue;
            if (int.TryParse(text, out intValue))
            {
                return intValue;
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a valid integer.");
                return 0; // Default to zero if parsing fails
            }

        }
        private void btn_calculate_Click(object sender, EventArgs e)
        {
            int housingTax = ParseIntValue(txt_housingTax.Text);
            int waterTax = ParseIntValue(txt_electricityTax.Text);
            int electricityTax = ParseIntValue(txt_waterTax.Text);
            int educationTax = ParseIntValue(txt_educationTax.Text);
            int penaltyTax = ParseIntValue(txt_waterTax.Text);

            int totalTax = housingTax + electricityTax + waterTax + educationTax + penaltyTax;
            txt_calculate.Text = totalTax.ToString();

            
        }


        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_proTaxId.Clear();
            txt_regNo.Clear();
            date_reg.Value = DateTime.Now;
            txt_name.Clear();
            txt_address.Clear();
            txt_contactNo.Clear();
            txt_proArea.Clear();
            txt_housingTax.Clear();
            txt_electricityTax.Clear();
            txt_waterTax.Clear();
            txt_educationTax.Clear();
            txt_penalty.Clear();
            txt_calculate.Clear();
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

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
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

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
            Dashboard ds = new Dashboard(); 
            ds.Show();
        }

        private void txt_search_Click(object sender, EventArgs e)
        {
            string proNoToSearch = txt_regNo.Text.Trim(); // Get the Pro_No to search for

            // Create a connection to your database
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                // Create a command to retrieve data from the NewProperty table based on Pro_No
                using (OleDbCommand command = new OleDbCommand("SELECT Pro_Owner, Pro_Address, Contact_No, Pro_Area FROM NewProperty WHERE Pro_No = @ProNo", connection))
                {
                    command.Parameters.AddWithValue("@ProNo", proNoToSearch);

                    // Execute the query and retrieve the data
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Populate the fields in your Property Tax form with the retrieved data
                            txt_name.Text = reader["Pro_Owner"].ToString();
                            txt_address.Text = reader["Pro_Address"].ToString();
                            txt_contactNo.Text = reader["Contact_No"].ToString();
                            txt_proArea.Text = reader["Pro_Area"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No record found for the specified Pro_No.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Clear the fields in your Property Tax form
                            txt_name.Text = "";
                            txt_address.Text = "";
                            txt_contactNo.Text = "";
                            txt_proArea.Text = "";
                        }
                    }
                }
            }

        }
    }
}
