using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Service;

namespace DV
{
    public partial class DisplayVisits : Form
    {
        AirlineService srv = new AirlineService();
        public readonly string COMPLETED = "Completed";
        public readonly string FAILED = "Failed";
        public readonly string REG_AND_DATE = "RegAndDate";
        public readonly string FLIGHT_NUM = "FlightNum";

        public DisplayVisits()
        {
            InitializeComponent();
        }

        private void btnUploadLeg1_Click(object sender, EventArgs e)
        {
            DialogResult result = uploadLeg1.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblLeg1DefaultText.Visible = true;
                lblLeg1DefaultText.Text = "Leg DataUpload in Process";
                btnUploadLeg1.Enabled = false;
                Task.Run(()=> processData(uploadLeg1.FileName));
            }
            
        }

        private async Task processData(string filePath)
        {
            int result = 0;
            result= await srv.UploadLegData(uploadLeg1.FileName);

            if (result.ToString() == "1")
            {
                SetLegText(COMPLETED);
            }
            else
            {
                SetLegText(FAILED);
            }
        }

        private void SetLegText(string text)
        {
            if (InvokeRequired)
            {
                Invoke((Action<string>)SetLegText, text);
                return;
            }
            lblLeg1DefaultText.Text = text;
            btnUploadLeg1.Enabled = true;
        }

        private async Task GetVisits(string filterBy)
        {
            var dt = await srv.GetVisits(filterBy, "PHBFR", "2017-01-02", "KL0661");

            if (dt.Rows.Count > 0)
            {
                SetVisitText(COMPLETED, dt, filterBy);
            }
            else
            {
                SetVisitText(FAILED, dt, filterBy);
            }
        }

        private void SetVisitText(string text, DataTable dt, string filterBy)
        {
            if (InvokeRequired)
            {
                Invoke((Action<string, DataTable, string>)SetVisitText, text, dt, filterBy);
                return;
            }
            if (dt.Rows.Count > 0)
            {
                if(filterBy==REG_AND_DATE)
                    dgvFilterByDateAndReg.DataSource = dt;
                else if (filterBy == FLIGHT_NUM)
                    dgvFilterByFlightNumber.DataSource = dt;
            }

            if (filterBy == REG_AND_DATE)
            {
                lblVisitsByDateAndReg.Text = text;
                btnDisplayVisitsByDateAndReg.Enabled = true;
            }
            else if (filterBy == FLIGHT_NUM)
            {
                lblGenVisitByFlightNumber.Text = text;
                btnVisitByFlightNum.Enabled = true;
            }
        }

        private void btnDisplayVisitsByDateAndReg_Click(object sender, EventArgs e)
        {
            btnDisplayVisitsByDateAndReg.Enabled = false;
            lblVisitsByDateAndReg.Text = "Generating Aircraft Visits";
            lblVisitsByDateAndReg.Visible = true;
            Task.Run(() => GetVisits(REG_AND_DATE));
        }

        private void btnVisitByFlightNum_Click(object sender, EventArgs e)
        {
            btnVisitByFlightNum.Enabled = false;
            lblGenVisitByFlightNumber.Text = "Generating Aircraft Visits";
            lblGenVisitByFlightNumber.Visible = true;
            Task.Run(() => GetVisits(FLIGHT_NUM));
        }
    }
}
