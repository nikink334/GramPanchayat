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
    public partial class Complaints : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Nikita\Desktop\project_main\Main_DataBase\GramPanchayat.accdb";
        private OleDbConnection conn;
        public Complaints()
        {
            InitializeComponent();
            conn = new OleDbConnection(connectionString);
        }

        private void Complaints_Load(object sender, EventArgs e)
        {

        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_comNo.Clear();
            txt_residentId.Clear();
            txt_compName.Clear();
            txt_contactNo.Clear();
            date_Complaint.Value = DateTime.Now;
            combo_comNature.SelectedIndex = 0;
            txt_description.Clear();
            txt_witnessName.Clear();
            combo_confidential.SelectedIndex = 0;
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

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                // Get values from the form fields
                int complainId = int.Parse(txt_comNo.Text);
                int residentId = int.Parse(txt_residentId.Text);
                string comName = txt_compName.Text;
                string contactNo = txt_contactNo.Text;
                if (!DateTime.TryParse(date_Complaint.Text, out DateTime comDate))
                {
                    MessageBox.Show("Please enter a valid registration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string comNature = combo_comNature.SelectedItem.ToString();
                string description = txt_description.Text;
                string witnessName = txt_witnessName.Text;
                string confidential = combo_confidential.SelectedItem.ToString();

                string insertQuery = "INSERT INTO Complaints (C_Id, Resident_Id, Com_Name, Contact_No, Date_Complaint, Nature_Complaint, Description, Witness_Name, Confidential) " +
                     "VALUES (@complaintId, @ResidentId, @complaintName, @contactNo, @comDate, @complaintNature, @description, @witnessName, @confidential)";

                using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Nikita\\Desktop\\project_main\\Main_DataBase\\GramPanchayat.accdb"))
                using (OleDbCommand insertCommand = new OleDbCommand(insertQuery, connection))
                {
                    // Set parameters for the insert query
                    insertCommand.Parameters.AddWithValue("@ComplaintNo", complainId);
                    insertCommand.Parameters.AddWithValue("@ResidentId", residentId);
                    insertCommand.Parameters.AddWithValue("@ComplaintName", comName);
                    insertCommand.Parameters.AddWithValue("@ContactNo", contactNo);
                    insertCommand.Parameters.AddWithValue("@ReceiveDate", comDate);
                    insertCommand.Parameters.AddWithValue("@ComplaintNature", comNature);
                    insertCommand.Parameters.AddWithValue("@Description", description);
                    insertCommand.Parameters.AddWithValue("@WitnessName", witnessName);
                    insertCommand.Parameters.AddWithValue("@Confidential", confidential);

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
    }
}



