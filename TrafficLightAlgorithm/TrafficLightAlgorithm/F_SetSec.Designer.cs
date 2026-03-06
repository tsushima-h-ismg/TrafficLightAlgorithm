namespace TrafficLightAlgorithm
{
    partial class F_SetSec
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_SetSec));
            this.lbl_SetValueName = new System.Windows.Forms.Label();
            this.txt_SetValue = new System.Windows.Forms.TextBox();
            this.lbl_Sec = new System.Windows.Forms.Label();
            this.btn_Confirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_SetValueName
            // 
            this.lbl_SetValueName.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lbl_SetValueName, "lbl_SetValueName");
            this.lbl_SetValueName.Name = "lbl_SetValueName";
            // 
            // txt_SetValue
            // 
            this.txt_SetValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txt_SetValue, "txt_SetValue");
            this.txt_SetValue.Name = "txt_SetValue";
            // 
            // lbl_Sec
            // 
            this.lbl_Sec.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lbl_Sec, "lbl_Sec");
            this.lbl_Sec.Name = "lbl_Sec";
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.BackColor = System.Drawing.Color.Lime;
            resources.ApplyResources(this.btn_Confirm, "btn_Confirm");
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.UseVisualStyleBackColor = false;
            this.btn_Confirm.Click += new System.EventHandler(this.Btn_Confirm_Click);
            // 
            // F_SetSec
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btn_Confirm);
            this.Controls.Add(this.lbl_Sec);
            this.Controls.Add(this.txt_SetValue);
            this.Controls.Add(this.lbl_SetValueName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "F_SetSec";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_SetValueName;
        private System.Windows.Forms.TextBox txt_SetValue;
        private System.Windows.Forms.Label lbl_Sec;
        private System.Windows.Forms.Button btn_Confirm;
    }
}