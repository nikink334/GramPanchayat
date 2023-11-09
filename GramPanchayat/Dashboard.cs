using System;
using System.Windows.Forms;

namespace GramPanchayat
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void complaintsToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void noticesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newResidentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Residence residence = new Residence();
            residence.Show();
            this.Close();
        }

        private void newPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Property property = new Property();
            property.Show();
            this.Close();
        }

        private void birthCertificatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BirthCertificate birthCertificate = new BirthCertificate();
            birthCertificate.Show();
            this.Close();
        }

        private void deathCertificatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Death_Certificate death_Certificate = new Death_Certificate();
            death_Certificate.Show();
            this.Close();
        }

        private void hosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PropertyTax propertyTax = new PropertyTax();
            propertyTax.Show();
            this.Close();
        }

        private void ComplaintsToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {

        }

        private void employeeManagementFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeesMang employeesMang = new EmployeesMang();
            employeesMang.Show();
            this.Close();
        }

        private void meetingManagementFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MeetingManag meetingManag = new MeetingManag();
            meetingManag.Show();
            this.Close();
        }

        private void fundsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void complaintsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Complaints complaints = new Complaints();
            complaints.Show();
            this.Close();

        }

        private void gFundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FundsManag funds = new FundsManag();
            funds.Show();
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void deathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Death_Report death_Report = new Death_Report();
            death_Report.Show();
            this.Close();
        }

        private void birthReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Birth_Report birth_report = new Birth_Report();
            birth_report.Show();
            this.Close();
        }

        private void propertyTaxReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaxReprot taxReprot = new TaxReprot();
            taxReprot.Show();
            this.Close();
        }
    }
}
