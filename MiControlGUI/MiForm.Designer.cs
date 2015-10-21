namespace MiControlGUI
{
    partial class MiForm
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
        	this.groupBox1 = new System.Windows.Forms.GroupBox();
        	this.trackBrightness1 = new System.Windows.Forms.TrackBar();
        	this.btnOff1 = new System.Windows.Forms.Button();
        	this.btnOn1 = new System.Windows.Forms.Button();
        	this.groupBox2 = new System.Windows.Forms.GroupBox();
        	this.trackBrightness2 = new System.Windows.Forms.TrackBar();
        	this.btnOff2 = new System.Windows.Forms.Button();
        	this.btnOn2 = new System.Windows.Forms.Button();
        	this.groupBox3 = new System.Windows.Forms.GroupBox();
        	this.trackBrightness3 = new System.Windows.Forms.TrackBar();
        	this.btnOff3 = new System.Windows.Forms.Button();
        	this.btnOn3 = new System.Windows.Forms.Button();
        	this.groupBox4 = new System.Windows.Forms.GroupBox();
        	this.trackBrightness4 = new System.Windows.Forms.TrackBar();
        	this.btnOff4 = new System.Windows.Forms.Button();
        	this.btnOn4 = new System.Windows.Forms.Button();
        	this.groupAmbi = new System.Windows.Forms.GroupBox();
        	this.btnAmbi = new System.Windows.Forms.Button();
        	this.bwAmbi = new System.ComponentModel.BackgroundWorker();
        	this.cmbGroup = new System.Windows.Forms.ComboBox();
        	this.groupBox1.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.trackBrightness1)).BeginInit();
        	this.groupBox2.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.trackBrightness2)).BeginInit();
        	this.groupBox3.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.trackBrightness3)).BeginInit();
        	this.groupBox4.SuspendLayout();
        	((System.ComponentModel.ISupportInitialize)(this.trackBrightness4)).BeginInit();
        	this.groupAmbi.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// groupBox1
        	// 
        	this.groupBox1.Controls.Add(this.trackBrightness1);
        	this.groupBox1.Controls.Add(this.btnOff1);
        	this.groupBox1.Controls.Add(this.btnOn1);
        	this.groupBox1.Location = new System.Drawing.Point(12, 12);
        	this.groupBox1.Name = "groupBox1";
        	this.groupBox1.Size = new System.Drawing.Size(60, 237);
        	this.groupBox1.TabIndex = 0;
        	this.groupBox1.TabStop = false;
        	this.groupBox1.Text = "Group 1";
        	// 
        	// trackBrightness1
        	// 
        	this.trackBrightness1.Location = new System.Drawing.Point(9, 77);
        	this.trackBrightness1.Maximum = 100;
        	this.trackBrightness1.Name = "trackBrightness1";
        	this.trackBrightness1.Orientation = System.Windows.Forms.Orientation.Vertical;
        	this.trackBrightness1.Size = new System.Drawing.Size(45, 154);
        	this.trackBrightness1.SmallChange = 10;
        	this.trackBrightness1.TabIndex = 1;
        	this.trackBrightness1.TickFrequency = 10;
        	this.trackBrightness1.TickStyle = System.Windows.Forms.TickStyle.Both;
        	this.trackBrightness1.Scroll += new System.EventHandler(this.trackBrightness1_Scroll);
        	// 
        	// btnOff1
        	// 
        	this.btnOff1.Location = new System.Drawing.Point(6, 48);
        	this.btnOff1.Name = "btnOff1";
        	this.btnOff1.Size = new System.Drawing.Size(50, 23);
        	this.btnOff1.TabIndex = 1;
        	this.btnOff1.Text = "OFF";
        	this.btnOff1.UseVisualStyleBackColor = true;
        	this.btnOff1.Click += new System.EventHandler(this.btnOff1_Click);
        	// 
        	// btnOn1
        	// 
        	this.btnOn1.Location = new System.Drawing.Point(6, 19);
        	this.btnOn1.Name = "btnOn1";
        	this.btnOn1.Size = new System.Drawing.Size(50, 23);
        	this.btnOn1.TabIndex = 0;
        	this.btnOn1.Text = "ON";
        	this.btnOn1.UseVisualStyleBackColor = true;
        	this.btnOn1.Click += new System.EventHandler(this.btnOn1_Click);
        	// 
        	// groupBox2
        	// 
        	this.groupBox2.Controls.Add(this.trackBrightness2);
        	this.groupBox2.Controls.Add(this.btnOff2);
        	this.groupBox2.Controls.Add(this.btnOn2);
        	this.groupBox2.Location = new System.Drawing.Point(78, 12);
        	this.groupBox2.Name = "groupBox2";
        	this.groupBox2.Size = new System.Drawing.Size(60, 237);
        	this.groupBox2.TabIndex = 2;
        	this.groupBox2.TabStop = false;
        	this.groupBox2.Text = "Group 2";
        	// 
        	// trackBrightness2
        	// 
        	this.trackBrightness2.Location = new System.Drawing.Point(9, 77);
        	this.trackBrightness2.Maximum = 100;
        	this.trackBrightness2.Name = "trackBrightness2";
        	this.trackBrightness2.Orientation = System.Windows.Forms.Orientation.Vertical;
        	this.trackBrightness2.Size = new System.Drawing.Size(45, 154);
        	this.trackBrightness2.SmallChange = 10;
        	this.trackBrightness2.TabIndex = 1;
        	this.trackBrightness2.TickFrequency = 10;
        	this.trackBrightness2.TickStyle = System.Windows.Forms.TickStyle.Both;
        	this.trackBrightness2.Scroll += new System.EventHandler(this.trackBrightness2_Scroll);
        	// 
        	// btnOff2
        	// 
        	this.btnOff2.Location = new System.Drawing.Point(6, 48);
        	this.btnOff2.Name = "btnOff2";
        	this.btnOff2.Size = new System.Drawing.Size(50, 23);
        	this.btnOff2.TabIndex = 1;
        	this.btnOff2.Text = "OFF";
        	this.btnOff2.UseVisualStyleBackColor = true;
        	this.btnOff2.Click += new System.EventHandler(this.btnOff2_Click);
        	// 
        	// btnOn2
        	// 
        	this.btnOn2.Location = new System.Drawing.Point(6, 19);
        	this.btnOn2.Name = "btnOn2";
        	this.btnOn2.Size = new System.Drawing.Size(50, 23);
        	this.btnOn2.TabIndex = 0;
        	this.btnOn2.Text = "ON";
        	this.btnOn2.UseVisualStyleBackColor = true;
        	this.btnOn2.Click += new System.EventHandler(this.btnOn2_Click);
        	// 
        	// groupBox3
        	// 
        	this.groupBox3.Controls.Add(this.trackBrightness3);
        	this.groupBox3.Controls.Add(this.btnOff3);
        	this.groupBox3.Controls.Add(this.btnOn3);
        	this.groupBox3.Location = new System.Drawing.Point(144, 12);
        	this.groupBox3.Name = "groupBox3";
        	this.groupBox3.Size = new System.Drawing.Size(60, 237);
        	this.groupBox3.TabIndex = 2;
        	this.groupBox3.TabStop = false;
        	this.groupBox3.Text = "Group 3";
        	// 
        	// trackBrightness3
        	// 
        	this.trackBrightness3.Location = new System.Drawing.Point(9, 77);
        	this.trackBrightness3.Maximum = 100;
        	this.trackBrightness3.Name = "trackBrightness3";
        	this.trackBrightness3.Orientation = System.Windows.Forms.Orientation.Vertical;
        	this.trackBrightness3.Size = new System.Drawing.Size(45, 154);
        	this.trackBrightness3.SmallChange = 10;
        	this.trackBrightness3.TabIndex = 1;
        	this.trackBrightness3.TickFrequency = 10;
        	this.trackBrightness3.TickStyle = System.Windows.Forms.TickStyle.Both;
        	this.trackBrightness3.Scroll += new System.EventHandler(this.trackBrightness3_Scroll);
        	// 
        	// btnOff3
        	// 
        	this.btnOff3.Location = new System.Drawing.Point(6, 48);
        	this.btnOff3.Name = "btnOff3";
        	this.btnOff3.Size = new System.Drawing.Size(50, 23);
        	this.btnOff3.TabIndex = 1;
        	this.btnOff3.Text = "OFF";
        	this.btnOff3.UseVisualStyleBackColor = true;
        	this.btnOff3.Click += new System.EventHandler(this.btnOff3_Click);
        	// 
        	// btnOn3
        	// 
        	this.btnOn3.Location = new System.Drawing.Point(6, 19);
        	this.btnOn3.Name = "btnOn3";
        	this.btnOn3.Size = new System.Drawing.Size(50, 23);
        	this.btnOn3.TabIndex = 0;
        	this.btnOn3.Text = "ON";
        	this.btnOn3.UseVisualStyleBackColor = true;
        	this.btnOn3.Click += new System.EventHandler(this.btnOn3_Click);
        	// 
        	// groupBox4
        	// 
        	this.groupBox4.Controls.Add(this.trackBrightness4);
        	this.groupBox4.Controls.Add(this.btnOff4);
        	this.groupBox4.Controls.Add(this.btnOn4);
        	this.groupBox4.Location = new System.Drawing.Point(212, 12);
        	this.groupBox4.Name = "groupBox4";
        	this.groupBox4.Size = new System.Drawing.Size(60, 237);
        	this.groupBox4.TabIndex = 2;
        	this.groupBox4.TabStop = false;
        	this.groupBox4.Text = "Group 4";
        	// 
        	// trackBrightness4
        	// 
        	this.trackBrightness4.Location = new System.Drawing.Point(9, 77);
        	this.trackBrightness4.Maximum = 100;
        	this.trackBrightness4.Name = "trackBrightness4";
        	this.trackBrightness4.Orientation = System.Windows.Forms.Orientation.Vertical;
        	this.trackBrightness4.Size = new System.Drawing.Size(45, 154);
        	this.trackBrightness4.SmallChange = 10;
        	this.trackBrightness4.TabIndex = 1;
        	this.trackBrightness4.TickFrequency = 10;
        	this.trackBrightness4.TickStyle = System.Windows.Forms.TickStyle.Both;
        	this.trackBrightness4.Scroll += new System.EventHandler(this.trackBrightness4_Scroll);
        	// 
        	// btnOff4
        	// 
        	this.btnOff4.Location = new System.Drawing.Point(6, 48);
        	this.btnOff4.Name = "btnOff4";
        	this.btnOff4.Size = new System.Drawing.Size(50, 23);
        	this.btnOff4.TabIndex = 1;
        	this.btnOff4.Text = "OFF";
        	this.btnOff4.UseVisualStyleBackColor = true;
        	this.btnOff4.Click += new System.EventHandler(this.btnOff4_Click);
        	// 
        	// btnOn4
        	// 
        	this.btnOn4.Location = new System.Drawing.Point(6, 19);
        	this.btnOn4.Name = "btnOn4";
        	this.btnOn4.Size = new System.Drawing.Size(50, 23);
        	this.btnOn4.TabIndex = 0;
        	this.btnOn4.Text = "ON";
        	this.btnOn4.UseVisualStyleBackColor = true;
        	this.btnOn4.Click += new System.EventHandler(this.btnOn4_Click);
        	// 
        	// groupAmbi
        	// 
        	this.groupAmbi.Controls.Add(this.cmbGroup);
        	this.groupAmbi.Controls.Add(this.btnAmbi);
        	this.groupAmbi.Location = new System.Drawing.Point(12, 257);
        	this.groupAmbi.Name = "groupAmbi";
        	this.groupAmbi.Size = new System.Drawing.Size(260, 144);
        	this.groupAmbi.TabIndex = 3;
        	this.groupAmbi.TabStop = false;
        	this.groupAmbi.Text = "Ambilight";
        	// 
        	// btnAmbi
        	// 
        	this.btnAmbi.Location = new System.Drawing.Point(6, 46);
        	this.btnAmbi.Name = "btnAmbi";
        	this.btnAmbi.Size = new System.Drawing.Size(248, 86);
        	this.btnAmbi.TabIndex = 4;
        	this.btnAmbi.Text = "Ambi";
        	this.btnAmbi.UseVisualStyleBackColor = true;
        	this.btnAmbi.Click += new System.EventHandler(this.btnAmbi_Click);
        	// 
        	// bwAmbi
        	// 
        	this.bwAmbi.WorkerSupportsCancellation = true;
        	this.bwAmbi.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwAmbi_DoWork);
        	// 
        	// cmbGroup
        	// 
        	this.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.cmbGroup.FormattingEnabled = true;
        	this.cmbGroup.Items.AddRange(new object[] {
			"All",
			"Group 1",
			"Group 2",
			"Group 3",
			"Group 4"});
        	this.cmbGroup.Location = new System.Drawing.Point(6, 19);
        	this.cmbGroup.Name = "cmbGroup";
        	this.cmbGroup.Size = new System.Drawing.Size(248, 21);
        	this.cmbGroup.TabIndex = 5;
        	// 
        	// MiForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(284, 411);
        	this.Controls.Add(this.groupAmbi);
        	this.Controls.Add(this.groupBox4);
        	this.Controls.Add(this.groupBox3);
        	this.Controls.Add(this.groupBox2);
        	this.Controls.Add(this.groupBox1);
        	this.Name = "MiForm";
        	this.Text = "MiForm";
        	this.Load += new System.EventHandler(this.MiFormLoad);
        	this.groupBox1.ResumeLayout(false);
        	this.groupBox1.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.trackBrightness1)).EndInit();
        	this.groupBox2.ResumeLayout(false);
        	this.groupBox2.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.trackBrightness2)).EndInit();
        	this.groupBox3.ResumeLayout(false);
        	this.groupBox3.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.trackBrightness3)).EndInit();
        	this.groupBox4.ResumeLayout(false);
        	this.groupBox4.PerformLayout();
        	((System.ComponentModel.ISupportInitialize)(this.trackBrightness4)).EndInit();
        	this.groupAmbi.ResumeLayout(false);
        	this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar trackBrightness1;
        private System.Windows.Forms.Button btnOff1;
        private System.Windows.Forms.Button btnOn1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TrackBar trackBrightness2;
        private System.Windows.Forms.Button btnOff2;
        private System.Windows.Forms.Button btnOn2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TrackBar trackBrightness3;
        private System.Windows.Forms.Button btnOff3;
        private System.Windows.Forms.Button btnOn3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TrackBar trackBrightness4;
        private System.Windows.Forms.Button btnOff4;
        private System.Windows.Forms.Button btnOn4;
        private System.Windows.Forms.GroupBox groupAmbi;
        private System.Windows.Forms.Button btnAmbi;
        private System.ComponentModel.BackgroundWorker bwAmbi;
        private System.Windows.Forms.ComboBox cmbGroup;
    }
}