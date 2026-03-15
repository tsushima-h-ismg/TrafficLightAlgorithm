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
            this.lbl_SecOne = new System.Windows.Forms.Label();
            this.btn_Confirm = new System.Windows.Forms.Button();
            this.lbl_SecThr = new System.Windows.Forms.Label();
            this.txt_RedSec = new System.Windows.Forms.TextBox();
            this.lbl_RedSec = new System.Windows.Forms.Label();
            this.lbl_SecTwo = new System.Windows.Forms.Label();
            this.txt_ArrowSec = new System.Windows.Forms.TextBox();
            this.lbl_ArrowSec = new System.Windows.Forms.Label();
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
            // lbl_SecOne
            // 
            this.lbl_SecOne.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lbl_SecOne, "lbl_SecOne");
            this.lbl_SecOne.Name = "lbl_SecOne";
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.BackColor = System.Drawing.Color.Lime;
            resources.ApplyResources(this.btn_Confirm, "btn_Confirm");
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.UseVisualStyleBackColor = false;
            this.btn_Confirm.Click += new System.EventHandler(this.Btn_Confirm_Click);
            // 
            // lbl_SecThr
            // 
            this.lbl_SecThr.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lbl_SecThr, "lbl_SecThr");
            this.lbl_SecThr.Name = "lbl_SecThr";
            // 
            // txt_RedSec
            // 
            this.txt_RedSec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txt_RedSec, "txt_RedSec");
            this.txt_RedSec.Name = "txt_RedSec";
            // 
            // lbl_RedSec
            // 
            this.lbl_RedSec.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lbl_RedSec, "lbl_RedSec");
            this.lbl_RedSec.Name = "lbl_RedSec";
            // 
            // lbl_SecTwo
            // 
            this.lbl_SecTwo.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lbl_SecTwo, "lbl_SecTwo");
            this.lbl_SecTwo.Name = "lbl_SecTwo";
            // 
            // txt_ArrowSec
            // 
            this.txt_ArrowSec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txt_ArrowSec, "txt_ArrowSec");
            this.txt_ArrowSec.Name = "txt_ArrowSec";
            // 
            // lbl_ArrowSec
            // 
            this.lbl_ArrowSec.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lbl_ArrowSec, "lbl_ArrowSec");
            this.lbl_ArrowSec.Name = "lbl_ArrowSec";
            // 
            // F_SetSec
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lbl_SecTwo);
            this.Controls.Add(this.txt_ArrowSec);
            this.Controls.Add(this.lbl_ArrowSec);
            this.Controls.Add(this.lbl_SecThr);
            this.Controls.Add(this.txt_RedSec);
            this.Controls.Add(this.lbl_RedSec);
            this.Controls.Add(this.btn_Confirm);
            this.Controls.Add(this.lbl_SecOne);
            this.Controls.Add(this.txt_SetValue);
            this.Controls.Add(this.lbl_SetValueName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "F_SetSec";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.F_SetSec_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_SetValueName;
        private System.Windows.Forms.TextBox txt_SetValue;
        private System.Windows.Forms.Label lbl_SecOne;
        private System.Windows.Forms.Button btn_Confirm;
        private System.Windows.Forms.Label lbl_SecThr;
        private System.Windows.Forms.TextBox txt_RedSec;
        private System.Windows.Forms.Label lbl_RedSec;
        private System.Windows.Forms.Label lbl_SecTwo;
        private System.Windows.Forms.TextBox txt_ArrowSec;
        private System.Windows.Forms.Label lbl_ArrowSec;
    }
}