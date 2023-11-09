using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GramPanchayat
{
    public partial class FundsManag : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Nikita\Desktop\project_main\Main_DataBase\GramPanchayat.accdb";
        private OleDbConnection conn;
        public FundsManag()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string recNoToSearch = txt_receiveNo.Text.Trim();

            // Perform a database query to retrieve the record with the specified Reg_No
            string query = "SELECT * FROM Funds WHERE Rec_No = @RecNo";

            using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Nikita\\Desktop\\project_main\\Main_DataBase\\GramPanchayat.accdb"))
            using (OleDbCommand command = new OleDbCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RegNo", recNoToSearch);

                try
                {
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Populate the form fields with data from the database
                        txt_receiveNo.Text = reader["Rec_No"].ToString();
                        date_fundReceive.Value = DateTime.Parse(reader["Rec_Date"].ToString());
                        string provideFromDatabase = reader["Provided_By"].ToString();
                        comboProvide.SelectedItem = provideFromDatabase;
                        txt_receivedBy.Text = reader["Receive_By"].ToString();
                        txt_fundAmount.Text = reader["Fund_Amount"].ToString();
                        txt_fundPurpose.Text = reader["Purpose"].ToString();
                    }
                    else
                    {
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
                // Get values from the form fields
                string receiveNo = txt_receiveNo.Text;
                DateTime receiveDate = date_fundReceive.Value;
                string provideBy = comboProvide.SelectedItem.ToString();
                string receivedBy = txt_receivedBy.Text;
                double fundAmount = double.Parse(txt_fundAmount.Text);
                string fundPurpose = txt_fundPurpose.Text;

                // Construct the insert SQL query
                string insertQuery = "INSERT INTO Funds (Rec_No, Rec_Date, Provided_By, Receive_By, Fund_Amount, Purpose) " +
                                     "VALUES (@RecNo, @RecDate, @ProvideBy, @ReceiveBy, @FundAmount, @Purpose)";

                using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Nikita\\Desktop\\project_main\\Main_DataBase\\GramPanchayat.accdb"))
                using (OleDbCommand insertCommand = new OleDbCommand(insertQuery, connection))
                {
                    // Set parameters for the insert query
                    insertCommand.Parameters.AddWithValue("@RecNo", receiveNo);
                    insertCommand.Parameters.AddWithValue("@RecDate", receiveDate);
                    insertCommand.Parameters.AddWithValue("@ProvideBy", provideBy);
                    insertCommand.Parameters.AddWithValue("@ReceiveBy", receivedBy);
                    insertCommand.Parameters.AddWithValue("@FundAmount", fundAmount);
                    insertCommand.Parameters.AddWithValue("@Purpose", fundPurpose);

                    connection.Open();
                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record Inserted Successfully.", "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert the record.", "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;

                // Get values from the form fields
                string receiveNo = txt_receiveNo.Text;
                DateTime receiveDate = date_fundReceive.Value;
                string provideBy = comboProvide.SelectedItem.ToString();
                string receivedBy = txt_receivedBy.Text;
                double fundAmount = double.Parse(txt_fundAmount.Text);
                string fundPurpose = txt_fundPurpose.Text;

                // Use parameterized query to update data
                cmd.CommandText = "UPDATE Funds SET Rec_Date = ?, Provided_By = ?, Receive_By = ?, Fund_Amount = ?, Purpose = ? WHERE Rec_No = ?";

                cmd.Parameters.AddWithValue("@p1", receiveDate);
                cmd.Parameters.AddWithValue("@p2", provideBy);
                cmd.Parameters.AddWithValue("@p3", receivedBy);
                cmd.Parameters.AddWithValue("@p4", fundAmount);
                cmd.Parameters.AddWithValue("@p5", fundPurpose);
                cmd.Parameters.AddWithValue("@p6", receiveNo);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Record Updated Successfully.", "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No matching record found for the provided Rec_No.", "Access Connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_receiveNo.Clear();
            date_fundReceive.Value = DateTime.Now;
            comboProvide.Items.Clear();
            txt_receivedBy.Clear();
            txt_fundAmount.Clear();
            txt_fundPurpose.Clear();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
            Dashboard ds = new Dashboard();
            ds.Show();
        }
    }
}
