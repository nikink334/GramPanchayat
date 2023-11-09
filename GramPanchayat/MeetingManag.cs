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
    public partial class MeetingManag : Form
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Nikita\Desktop\project_main\Main_DataBase\GramPanchayat.accdb";
        private OleDbConnection conn;
        public MeetingManag()
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
                if (!int.TryParse(txt_meetNo.Text, out int meetNo))
                {
                    MessageBox.Show("Please enter a valid Meeting number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txt_organizerId.Text, out int orgId))
                {
                    MessageBox.Show("Please enter a valid Organizers Id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string time = txt_time.Text;
                if (string.IsNullOrWhiteSpace(time))
                {
                    MessageBox.Show("Please enter a Time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParse(date_meeting.Text, out DateTime meetDate))
                {
                    MessageBox.Show("Please enter a valid registration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string purpose = txt_purpose.Text;
                if (string.IsNullOrWhiteSpace(purpose))
                {
                    MessageBox.Show("Please enter a valid purpose.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string description = txt_description.Text;
                if (string.IsNullOrWhiteSpace(description))
                {
                    MessageBox.Show("Please enter a valid Description.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Use parameterized query to insert data
                cmd.CommandText = "INSERT INTO Meetings (Meet_No, M_Time, Organizer_Id, Meet_Date, Purpose, Description) " +
                                  "VALUES (?, ?, ?, ?, ?, ?)";

                cmd.Parameters.AddWithValue("@p1", meetNo);
                cmd.Parameters.AddWithValue("@p2", time);
                cmd.Parameters.AddWithValue("@p3", orgId);
                cmd.Parameters.AddWithValue("@p4", meetDate);
                cmd.Parameters.AddWithValue("@p5", purpose);
                cmd.Parameters.AddWithValue("@p6", description);

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
            txt_meetNo.Clear();
            txt_time.Clear();
            txt_organizerId.Clear();
            date_meeting.Value = DateTime.Now;
            txt_purpose.Clear();
            txt_description.Clear();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
            Dashboard ds = new Dashboard();
            ds.Show();
        }
    }
}