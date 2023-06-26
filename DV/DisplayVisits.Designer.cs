namespace DV
{
    partial class DisplayVisits
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDisplayVisitsByDateAndReg = new System.Windows.Forms.Button();
            this.uploadLeg1 = new System.Windows.Forms.OpenFileDialog();
            this.btnUploadLeg1 = new System.Windows.Forms.Button();
            this.lblVisitsByDateAndReg = new System.Windows.Forms.Label();
            this.lblLeg1DefaultText = new System.Windows.Forms.Label();
            this.dgvFilterByDateAndReg = new System.Windows.Forms.DataGridView();
            this.dgvFilterByFlightNumber = new System.Windows.Forms.DataGridView();
            this.lblGenVisitByFlightNumber = new System.Windows.Forms.Label();
            this.btnVisitByFlightNum = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterByDateAndReg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterByFlightNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDisplayVisitsByDateAndReg
            // 
            this.btnDisplayVisitsByDateAndReg.Location = new System.Drawing.Point(68, 42);
            this.btnDisplayVisitsByDateAndReg.Name = "btnDisplayVisitsByDateAndReg";
            this.btnDisplayVisitsByDateAndReg.Size = new System.Drawing.Size(210, 23);
            this.btnDisplayVisitsByDateAndReg.TabIndex = 0;
            this.btnDisplayVisitsByDateAndReg.Text = "Display Visits By date and registration";
            this.btnDisplayVisitsByDateAndReg.UseVisualStyleBackColor = true;
            this.btnDisplayVisitsByDateAndReg.Click += new System.EventHandler(this.btnDisplayVisitsByDateAndReg_Click);
            // 
            // uploadLeg1
            // 
            this.uploadLeg1.FileName = "openFileDialog1";
            this.uploadLeg1.Filter = "|*.csv";
            // 
            // btnUploadLeg1
            // 
            this.btnUploadLeg1.Location = new System.Drawing.Point(68, 13);
            this.btnUploadLeg1.Name = "btnUploadLeg1";
            this.btnUploadLeg1.Size = new System.Drawing.Size(144, 23);
            this.btnUploadLeg1.TabIndex = 1;
            this.btnUploadLeg1.Text = "Upload Leg Data";
            this.btnUploadLeg1.UseVisualStyleBackColor = true;
            this.btnUploadLeg1.Click += new System.EventHandler(this.btnUploadLeg1_Click);
            // 
            // lblVisitsByDateAndReg
            // 
            this.lblVisitsByDateAndReg.AutoSize = true;
            this.lblVisitsByDateAndReg.Location = new System.Drawing.Point(376, 52);
            this.lblVisitsByDateAndReg.Name = "lblVisitsByDateAndReg";
            this.lblVisitsByDateAndReg.Size = new System.Drawing.Size(122, 13);
            this.lblVisitsByDateAndReg.TabIndex = 3;
            this.lblVisitsByDateAndReg.Text = "Generating Aircraft Visits";
            this.lblVisitsByDateAndReg.Visible = false;
            // 
            // lblLeg1DefaultText
            // 
            this.lblLeg1DefaultText.AutoSize = true;
            this.lblLeg1DefaultText.Location = new System.Drawing.Point(228, 18);
            this.lblLeg1DefaultText.Name = "lblLeg1DefaultText";
            this.lblLeg1DefaultText.Size = new System.Drawing.Size(137, 13);
            this.lblLeg1DefaultText.TabIndex = 4;
            this.lblLeg1DefaultText.Text = "Leg DataUpload in Process";
            this.lblLeg1DefaultText.Visible = false;
            // 
            // dgvFilterByDateAndReg
            // 
            this.dgvFilterByDateAndReg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilterByDateAndReg.Location = new System.Drawing.Point(68, 71);
            this.dgvFilterByDateAndReg.Name = "dgvFilterByDateAndReg";
            this.dgvFilterByDateAndReg.Size = new System.Drawing.Size(661, 150);
            this.dgvFilterByDateAndReg.TabIndex = 6;
            // 
            // dgvFilterByFlightNumber
            // 
            this.dgvFilterByFlightNumber.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilterByFlightNumber.Location = new System.Drawing.Point(68, 263);
            this.dgvFilterByFlightNumber.Name = "dgvFilterByFlightNumber";
            this.dgvFilterByFlightNumber.Size = new System.Drawing.Size(661, 150);
            this.dgvFilterByFlightNumber.TabIndex = 7;
            // 
            // lblGenVisitByFlightNumber
            // 
            this.lblGenVisitByFlightNumber.AutoSize = true;
            this.lblGenVisitByFlightNumber.Location = new System.Drawing.Point(376, 244);
            this.lblGenVisitByFlightNumber.Name = "lblGenVisitByFlightNumber";
            this.lblGenVisitByFlightNumber.Size = new System.Drawing.Size(122, 13);
            this.lblGenVisitByFlightNumber.TabIndex = 10;
            this.lblGenVisitByFlightNumber.Text = "Generating Aircraft Visits";
            this.lblGenVisitByFlightNumber.Visible = false;
            // 
            // btnVisitByFlightNum
            // 
            this.btnVisitByFlightNum.Location = new System.Drawing.Point(68, 234);
            this.btnVisitByFlightNum.Name = "btnVisitByFlightNum";
            this.btnVisitByFlightNum.Size = new System.Drawing.Size(210, 23);
            this.btnVisitByFlightNum.TabIndex = 11;
            this.btnVisitByFlightNum.Text = "Display Visits By Flight Number";
            this.btnVisitByFlightNum.UseVisualStyleBackColor = true;
            this.btnVisitByFlightNum.Click += new System.EventHandler(this.btnVisitByFlightNum_Click);
            // 
            // DisplayVisits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 534);
            this.Controls.Add(this.btnVisitByFlightNum);
            this.Controls.Add(this.lblGenVisitByFlightNumber);
            this.Controls.Add(this.dgvFilterByFlightNumber);
            this.Controls.Add(this.dgvFilterByDateAndReg);
            this.Controls.Add(this.lblLeg1DefaultText);
            this.Controls.Add(this.lblVisitsByDateAndReg);
            this.Controls.Add(this.btnUploadLeg1);
            this.Controls.Add(this.btnDisplayVisitsByDateAndReg);
            this.Name = "DisplayVisits";
            this.Text = "View Aircraft Visits";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterByDateAndReg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterByFlightNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDisplayVisitsByDateAndReg;
        private System.Windows.Forms.OpenFileDialog uploadLeg1;
        private System.Windows.Forms.Button btnUploadLeg1;
        private System.Windows.Forms.Label lblVisitsByDateAndReg;
        private System.Windows.Forms.Label lblLeg1DefaultText;
        private System.Windows.Forms.DataGridView dgvFilterByDateAndReg;
        private System.Windows.Forms.DataGridView dgvFilterByFlightNumber;
        private System.Windows.Forms.Label lblGenVisitByFlightNumber;
        private System.Windows.Forms.Button btnVisitByFlightNum;
    }
}

