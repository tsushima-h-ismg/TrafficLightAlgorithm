namespace TrafficLightAlgorithm
{
    partial class F_Version
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Version));
            this.lbl_FormTitle = new System.Windows.Forms.Label();
            this.lbl_companyName = new System.Windows.Forms.Label();
            this.lbl_department = new System.Windows.Forms.Label();
            this.lbl_SoftTitle = new System.Windows.Forms.Label();
            this.lbl_Version = new System.Windows.Forms.Label();
            this.lbl_verInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_FormTitle
            // 
            this.lbl_FormTitle.BackColor = System.Drawing.Color.Lime;
            this.lbl_FormTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_FormTitle.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_FormTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbl_FormTitle.Location = new System.Drawing.Point(-1, 10);
            this.lbl_FormTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_FormTitle.Name = "lbl_FormTitle";
            this.lbl_FormTitle.Size = new System.Drawing.Size(466, 30);
            this.lbl_FormTitle.TabIndex = 0;
            this.lbl_FormTitle.Text = "ー　バージョン情報　ー";
            this.lbl_FormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_companyName
            // 
            this.lbl_companyName.BackColor = System.Drawing.Color.Transparent;
            this.lbl_companyName.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_companyName.Location = new System.Drawing.Point(10, 40);
            this.lbl_companyName.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_companyName.Name = "lbl_companyName";
            this.lbl_companyName.Size = new System.Drawing.Size(444, 35);
            this.lbl_companyName.TabIndex = 1;
            this.lbl_companyName.Text = "(c) 2026 Takashin Co., Ltd.";
            this.lbl_companyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_department
            // 
            this.lbl_department.BackColor = System.Drawing.Color.Transparent;
            this.lbl_department.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_department.Location = new System.Drawing.Point(10, 135);
            this.lbl_department.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_department.Name = "lbl_department";
            this.lbl_department.Size = new System.Drawing.Size(444, 35);
            this.lbl_department.TabIndex = 3;
            this.lbl_department.Text = "情報システム管理G";
            this.lbl_department.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SoftTitle
            // 
            this.lbl_SoftTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbl_SoftTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_SoftTitle.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SoftTitle.Location = new System.Drawing.Point(157, 80);
            this.lbl_SoftTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SoftTitle.Name = "lbl_SoftTitle";
            this.lbl_SoftTitle.Size = new System.Drawing.Size(150, 20);
            this.lbl_SoftTitle.TabIndex = 4;
            this.lbl_SoftTitle.Text = "SoftTitle";
            this.lbl_SoftTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_SoftTitle.Click += new System.EventHandler(this.Lbl_verInfo_Click);
            // 
            // lbl_Version
            // 
            this.lbl_Version.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Version.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_Version.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_Version.Location = new System.Drawing.Point(157, 108);
            this.lbl_Version.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_Version.Name = "lbl_Version";
            this.lbl_Version.Size = new System.Drawing.Size(150, 20);
            this.lbl_Version.TabIndex = 5;
            this.lbl_Version.Text = "Version";
            this.lbl_Version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Version.Click += new System.EventHandler(this.Lbl_verInfo_Click);
            // 
            // lbl_verInfo
            // 
            this.lbl_verInfo.BackColor = System.Drawing.Color.Transparent;
            this.lbl_verInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_verInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_verInfo.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_verInfo.Location = new System.Drawing.Point(10, 75);
            this.lbl_verInfo.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_verInfo.Name = "lbl_verInfo";
            this.lbl_verInfo.Size = new System.Drawing.Size(444, 60);
            this.lbl_verInfo.TabIndex = 2;
            this.lbl_verInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_verInfo.Click += new System.EventHandler(this.Lbl_verInfo_Click);
            // 
            // F_Version
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(464, 171);
            this.Controls.Add(this.lbl_Version);
            this.Controls.Add(this.lbl_SoftTitle);
            this.Controls.Add(this.lbl_department);
            this.Controls.Add(this.lbl_verInfo);
            this.Controls.Add(this.lbl_companyName);
            this.Controls.Add(this.lbl_FormTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(480, 210);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(480, 210);
            this.Name = "F_Version";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "信号機プログラム Program";
            this.Load += new System.EventHandler(this.F_Version_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_FormTitle;
        private System.Windows.Forms.Label lbl_companyName;
        private System.Windows.Forms.Label lbl_department;
        private System.Windows.Forms.Label lbl_SoftTitle;
        private System.Windows.Forms.Label lbl_Version;
        private System.Windows.Forms.Label lbl_verInfo;
    }
}