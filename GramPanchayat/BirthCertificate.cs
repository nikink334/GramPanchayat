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
    
    public partial class BirthCertificate : Form
   {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Nikita\Desktop\project_main\Main_DataBase\GramPanchayat.accdb";
        private OleDbConnection conn;
        public BirthCertificate()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void BirthCertificate_Load(object sender, EventArgs e)
        {

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            // Retrieve the registration number entered by the user
            string regNoToSearch = txt_registrastionNo.Text.Trim();

            // Perform a database query to retrieve the record with the specified Reg_No
            string query = "SELECT * FROM Birth_Certificate WHERE Reg_No = @RegNo";

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
                        date_registrastionDate.Value = DateTime.Parse(reader["Reg_Date"].ToString());
                        txt_childName.Text = reader["Child_Name"].ToString();
                        date_dobChild.Value = DateTime.Parse(reader["Reg_Date"].ToString());
                        string genderFromDatabase = reader["Gender"].ToString();
                        comboGender.SelectedItem = genderFromDatabase;
                        txt_birthPlace.Text = reader["City_Birth"].ToString();
                        txt_fatherName.Text = reader["Father_Name"].ToString();
                        txt_motherName.Text = reader["Mother_Name"].ToString();
                        txt_address.Text = reader["Parents_Address"].ToString();

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
                if (!int.TryParse(txt_registrastionNo.Text, out int regNo))
                {
                    MessageBox.Show("Please enter a valid registration number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(date_registrastionDate.Text, out DateTime regDate))
                {
                    MessageBox.Show("Please enter a valid registration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string childName = txt_childName.Text;
                if (string.IsNullOrWhiteSpace(childName))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(date_dobChild.Text, out DateTime birthDate))
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

                string cityBirth = txt_birthPlace.Text;
                if (string.IsNullOrWhiteSpace(cityBirth))
                {
                    MessageBox.Show("Please enter a valid information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string fatherName = txt_fatherName.Text;
                if (string.IsNullOrWhiteSpace(fatherName))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string motherName = txt_motherName.Text;
                if (string.IsNullOrWhiteSpace(motherName))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string parentsAddress = txt_address.Text;

                // Use parameterized query to insert data
                cmd.CommandText = "INSERT INTO Birth_Certificate (Reg_No, Reg_Date, Child_Name, DOB, Gender, City_Birth, Father_Name, Mother_Name, Parents_Address) " +
                                  "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

                cmd.Parameters.AddWithValue("@p1", regNo);
                cmd.Parameters.AddWithValue("@p2", regDate);
                cmd.Parameters.AddWithValue("@p3", childName);
                cmd.Parameters.AddWithValue("@p4", birthDate);
                cmd.Parameters.AddWithValue("@p5", selectedGender); 
                cmd.Parameters.AddWithValue("@p6", cityBirth);
                cmd.Parameters.AddWithValue("@p7", fatherName);
                cmd.Parameters.AddWithValue("@p8", motherName);
                cmd.Parameters.AddWithValue("@p9", parentsAddress);

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
                if (!int.TryParse(txt_registrastionNo.Text, out int regNo))
                {
                    MessageBox.Show("Please enter a valid registration number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(date_registrastionDate.Text, out DateTime regDate))
                {
                    MessageBox.Show("Please enter a valid registration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string childName = txt_childName.Text;
                if (string.IsNullOrWhiteSpace(childName))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(date_dobChild.Text, out DateTime birthDate))
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

                string cityBirth = txt_birthPlace.Text;
                if (string.IsNullOrWhiteSpace(cityBirth))
                {
                    MessageBox.Show("Please enter a valid information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string fatherName = txt_fatherName.Text;
                if (string.IsNullOrWhiteSpace(fatherName))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string motherName = txt_motherName.Text;
                if (string.IsNullOrWhiteSpace(motherName))
                {
                    MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string parentsAddress = txt_address.Text;

                // Use parameterized query to update data
                cmd.CommandText = "UPDATE Birth_Certificate SET Reg_Date = ?, Child_Name = ?, DOB = ?, Gender = ?, City_Birth = ?, Father_Name = ?, Mother_Name = ?, Parents_Address = ? WHERE Reg_No = ?";

                cmd.Parameters.AddWithValue("@p1", regDate);
                cmd.Parameters.AddWithValue("@p2", childName);
                cmd.Parameters.AddWithValue("@p3", birthDate);
                cmd.Parameters.AddWithValue("@p4", selectedGender);
                cmd.Parameters.AddWithValue("@p5", cityBirth);
                cmd.Parameters.AddWithValue("@p6", fatherName);
                cmd.Parameters.AddWithValue("@p7", motherName);
                cmd.Parameters.AddWithValue("@p8", parentsAddress);
                cmd.Parameters.AddWithValue("@p9", regNo);

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

        private void btn_clear_Click(object sender, EventArgs e)
        {
            // Clear all input fields
            txt_registrastionNo.Clear();
            date_registrastionDate.Value = DateTime.Now; // Reset the date picker to the current date and time
            txt_childName.Clear();
            date_dobChild.Value =  DateTime.Now;
            comboGender.Items.Clear();
            txt_birthPlace.Clear();
            txt_fatherName.Clear();
            txt_motherName.Clear();
            txt_address.Clear();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
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
