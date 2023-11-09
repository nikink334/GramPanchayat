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
    public partial class Death_Certificate : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Nikita\Desktop\project_main\Main_DataBase\GramPanchayat.accdb";
        private OleDbConnection conn;
        public Death_Certificate()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            // Retrieve the registration number entered by the user
            string regNoToSearch = txt_regNo.Text.Trim();

            // Perform a database query to retrieve the record with the specified Reg_No
            string query = "SELECT * FROM Death_Certificate WHERE Reg_No = @RegNo";

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
                        txt_regNo.Text = reader["Reg_No"].ToString();
                        date_regDate.Value = DateTime.Parse(reader["Reg_Date"].ToString());
                        txt_informantName.Text = reader["Informant_Name"].ToString();
                        txt_informantAddress.Text = reader["Informant_Address"].ToString();
                        txt_relationship.Text = reader["Relationship"].ToString();
                        txt_deceasedName.Text = reader["Deceased_Name"].ToString();
                        date_deathDate.Value = DateTime.Parse(reader["Death_Date"].ToString());
                        string genderFromDatabase = reader["Gender"].ToString();
                        comboGender.SelectedItem = genderFromDatabase;
                        txt_ageDeath.Text = reader["Age_Death"].ToString();
                        txt_cause.Text = reader["Cause_Death"].ToString();
                        txt_placeDeath.Text = reader["Place_Death"].ToString();

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

                if (!DateTime.TryParse(date_regDate.Text, out DateTime regDate))
                {
                    MessageBox.Show("Please enter a valid registration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string informantName = txt_informantName.Text;
                if (string.IsNullOrWhiteSpace(informantName))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string informantAddress = txt_informantAddress.Text;
                if (string.IsNullOrWhiteSpace(informantAddress))
                {
                    MessageBox.Show("Please enter a valid information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string relationship = txt_relationship.Text;
                if (string.IsNullOrWhiteSpace(relationship))
                {
                    MessageBox.Show("Please enter a valid information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string deceasedName = txt_deceasedName.Text;
                if (string.IsNullOrWhiteSpace(deceasedName))
                {
                    MessageBox.Show("Please enter a valid information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(date_deathDate.Text, out DateTime deathDate))
                {
                    MessageBox.Show("Please enter a valid registration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get the selected text from the combo box
                string selectedGender = comboGender.Text;

                // Define an array or list of valid options for the combo box
                string[] validGenders = { "Male", "Female" };

                // Check if the selected text is in the list of valid options
                if (!validGenders.Contains(selectedGender))
                {
                    MessageBox.Show("Please select a valid gender.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txt_ageDeath.Text, out int ageDeath))
                {
                    MessageBox.Show("Please enter a valid registration number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string causeDeath = txt_cause.Text;
                if (string.IsNullOrWhiteSpace(causeDeath))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string placeDeath = txt_placeDeath.Text;
                if (string.IsNullOrWhiteSpace(placeDeath))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                // Use parameterized query to insert data
                cmd.CommandText = "INSERT INTO Death_Certificate (Reg_No, Reg_Date, Informant_Name, Informant_Address, Relationship, Deceased_Name, Death_Date, Gender, Age_Death, Cause_Death, Place_Death ) " +
                                  "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                cmd.Parameters.AddWithValue("@p1", regNo);
                cmd.Parameters.AddWithValue("@p2", regDate);
                cmd.Parameters.AddWithValue("@p3", informantName);
                cmd.Parameters.AddWithValue("@p4", informantAddress);
                cmd.Parameters.AddWithValue("@p5", relationship);
                cmd.Parameters.AddWithValue("@p6", deceasedName);
                cmd.Parameters.AddWithValue("@p7", deathDate);
                cmd.Parameters.AddWithValue("@p8", selectedGender);
                cmd.Parameters.AddWithValue("@p9", ageDeath);
                cmd.Parameters.AddWithValue("@p10", causeDeath);
                cmd.Parameters.AddWithValue("@p11", placeDeath);


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

                // Debugging: Display the regNo to check if it's correct
                MessageBox.Show("Updating record with Reg_No: " + regNo, "Debugging", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (!DateTime.TryParse(date_regDate.Text, out DateTime regDate))
                {
                    MessageBox.Show("Please enter a valid registration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string informantName = txt_informantName.Text;
                if (string.IsNullOrWhiteSpace(informantName))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string informantAddress = txt_informantAddress.Text;
                if (string.IsNullOrWhiteSpace(informantAddress))
                {
                    MessageBox.Show("Please enter a valid information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string relationship = txt_relationship.Text;
                if (string.IsNullOrWhiteSpace(relationship))
                {
                    MessageBox.Show("Please enter a valid information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string deceasedName = txt_deceasedName.Text;
                if (string.IsNullOrWhiteSpace(deceasedName))
                {
                    MessageBox.Show("Please enter a valid information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(date_deathDate.Text, out DateTime deathDate))
                {
                    MessageBox.Show("Please enter a valid registration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get the selected text from the combo box
                string selectedGender = comboGender.Text;

                // Define an array or list of valid options for the combo box
                string[] validGenders = { "Male", "Female" };

                // Check if the selected text is in the list of valid options
                if (!validGenders.Contains(selectedGender))
                {
                    MessageBox.Show("Please select a valid gender.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txt_ageDeath.Text, out int ageDeath))
                {
                    MessageBox.Show("Please enter a valid registration number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string causeDeath = txt_cause.Text;
                if (string.IsNullOrWhiteSpace(causeDeath))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string placeDeath = txt_placeDeath.Text;
                if (string.IsNullOrWhiteSpace(placeDeath))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Use parameterized query to update data
                cmd.CommandText = "UPDATE Death_Certificate SET Reg_Date = ?, Informant_Name = ?, Informant_Address = ?, Relationship = ?, Deceased_Name = ?, Death_Date = ?,  Gender = ?, Age_Death = ?, Cause_Death = ?, Place_Death = ? WHERE Reg_No = ?";

                cmd.Parameters.AddWithValue("@p1", regDate);
                cmd.Parameters.AddWithValue("@p2", informantName);
                cmd.Parameters.AddWithValue("@p3", informantAddress);
                cmd.Parameters.AddWithValue("@p4", relationship);
                cmd.Parameters.AddWithValue("@p5", deceasedName);
                cmd.Parameters.AddWithValue("@p6", deathDate);
                cmd.Parameters.AddWithValue("@p7", selectedGender);
                cmd.Parameters.AddWithValue("@p8", ageDeath);
                cmd.Parameters.AddWithValue("@p9", causeDeath);
                cmd.Parameters.AddWithValue("@p10", placeDeath);
                cmd.Parameters.AddWithValue("@p11", regNo); // Use the regNo here for WHERE clause

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

        private void btn_clear_Click(object sender, EventArgs e)
        {
            // Clear all input fields
            txt_regNo.Clear();
            date_regDate.Value = DateTime.Now; // Reset the date picker to the current date and time
            txt_informantName.Clear();
            txt_informantAddress.Clear();
            txt_relationship.Clear();
            txt_deceasedName.Clear();
            date_deathDate.Value = DateTime.Now;
            comboGender.Items.Clear();
            txt_ageDeath.Clear();
            txt_cause.Clear();
            txt_placeDeath.Clear();

        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }
    }
}

