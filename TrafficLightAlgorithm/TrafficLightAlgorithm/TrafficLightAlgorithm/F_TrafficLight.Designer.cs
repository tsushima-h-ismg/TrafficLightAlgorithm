namespace TrafficLightAlgorithm
{
    partial class F_TrafficLight
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_TrafficLight));
            this.btn_Start = new System.Windows.Forms.Button();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.btn_InterruptResume = new System.Windows.Forms.Button();
            this.lbl_NLightOnSec = new System.Windows.Forms.Label();
            this.lbl_SLightOnSec = new System.Windows.Forms.Label();
            this.lbl_ELightOnSec = new System.Windows.Forms.Label();
            this.lbl_WLightOnSec = new System.Windows.Forms.Label();
            this.lbl_PrepareSec = new System.Windows.Forms.Label();
            this.tlp_InputSecField = new System.Windows.Forms.TableLayoutPanel();
            this.txt_PrepareSec = new System.Windows.Forms.TextBox();
            this.txt_WLightOnSec = new System.Windows.Forms.TextBox();
            this.txt_ELightOnSec = new System.Windows.Forms.TextBox();
            this.txt_SLightOnSec = new System.Windows.Forms.TextBox();
            this.txt_NLightOnSec = new System.Windows.Forms.TextBox();
            this.lbl_SecHead = new System.Windows.Forms.Label();
            this.lbl_NameHead = new System.Windows.Forms.Label();
            this.pnl_Traffic = new System.Windows.Forms.Panel();
            this.lbl_SouthStopLine = new System.Windows.Forms.Label();
            this.lbl_NorthStopLine = new System.Windows.Forms.Label();
            this.lbl_WestStopLine = new System.Windows.Forms.Label();
            this.lbl_EastStopLine = new System.Windows.Forms.Label();
            this.lbl_SouthCenterLine = new System.Windows.Forms.Label();
            this.lbl_WestCenterLine = new System.Windows.Forms.Label();
            this.lbl_NorthCenterLine = new System.Windows.Forms.Label();
            this.lbl_EastCenterLine = new System.Windows.Forms.Label();
            this.lbl_SouthCrossWalkSix = new System.Windows.Forms.Label();
            this.lbl_SouthCrossWalkFive = new System.Windows.Forms.Label();
            this.lbl_SouthCrossWalkFour = new System.Windows.Forms.Label();
            this.lbl_SouthCrossWalkThree = new System.Windows.Forms.Label();
            this.lbl_SouthCrossWalkTwo = new System.Windows.Forms.Label();
            this.lbl_SouthCrossWalkOne = new System.Windows.Forms.Label();
            this.lbl_NorthCrossWalkSix = new System.Windows.Forms.Label();
            this.lbl_NorthCrossWalkFive = new System.Windows.Forms.Label();
            this.lbl_NorthCrossWalkFour = new System.Windows.Forms.Label();
            this.lbl_NorthCrossWalkThree = new System.Windows.Forms.Label();
            this.lbl_NorthCrossWalkTwo = new System.Windows.Forms.Label();
            this.lbl_NorthCrossWalkOne = new System.Windows.Forms.Label();
            this.lbl_EastCrossWalkSix = new System.Windows.Forms.Label();
            this.lbl_EastCrossWalkFive = new System.Windows.Forms.Label();
            this.lbl_EastCrossWalkFour = new System.Windows.Forms.Label();
            this.lbl_EastCrossWalkThree = new System.Windows.Forms.Label();
            this.lbl_EastCrossWalkTwo = new System.Windows.Forms.Label();
            this.lbl_EastCrossWalkOne = new System.Windows.Forms.Label();
            this.lbl_WestCrossWalkSix = new System.Windows.Forms.Label();
            this.lbl_WestCrossWalkOne = new System.Windows.Forms.Label();
            this.lbl_WestCrossWalkFive = new System.Windows.Forms.Label();
            this.lbl_WestCrossWalkFour = new System.Windows.Forms.Label();
            this.lbl_WestCrossWalkThree = new System.Windows.Forms.Label();
            this.lbl_WestCrossWalkTwo = new System.Windows.Forms.Label();
            this.lbl_SouthGreen = new System.Windows.Forms.Label();
            this.lbl_NorthRed = new System.Windows.Forms.Label();
            this.lbl_SouthYellow = new System.Windows.Forms.Label();
            this.lbl_EastRed = new System.Windows.Forms.Label();
            this.lbl_SouthRed = new System.Windows.Forms.Label();
            this.lbl_NorthYellow = new System.Windows.Forms.Label();
            this.lbl_WestGreen = new System.Windows.Forms.Label();
            this.lbl_NorthGreen = new System.Windows.Forms.Label();
            this.lbl_EastYellow = new System.Windows.Forms.Label();
            this.lbl_WestYellow = new System.Windows.Forms.Label();
            this.lbl_EastGreen = new System.Windows.Forms.Label();
            this.lbl_WestRed = new System.Windows.Forms.Label();
            this.lbl_SNRoadway = new System.Windows.Forms.Label();
            this.lbl_EWRoadway = new System.Windows.Forms.Label();
            this.lbl_NorthDirection = new System.Windows.Forms.Label();
            this.lbl_SouthDirection = new System.Windows.Forms.Label();
            this.lbl_WestDirection = new System.Windows.Forms.Label();
            this.lbl_EastDirection = new System.Windows.Forms.Label();
            this.Timer_Traffic = new System.Windows.Forms.Timer(this.components);
            this.tlp_InputSecField.SuspendLayout();
            this.pnl_Traffic.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Start
            // 
            this.btn_Start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Start.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_Start.Location = new System.Drawing.Point(480, 50);
            this.btn_Start.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(100, 50);
            this.btn_Start.TabIndex = 0;
            this.btn_Start.Text = "開始";
            this.btn_Start.UseVisualStyleBackColor = false;
            this.btn_Start.Click += new System.EventHandler(this.Btn_Start_Click);
            // 
            // btn_Stop
            // 
            this.btn_Stop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Stop.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_Stop.Location = new System.Drawing.Point(740, 50);
            this.btn_Stop.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(100, 50);
            this.btn_Stop.TabIndex = 2;
            this.btn_Stop.Text = "終了";
            this.btn_Stop.UseVisualStyleBackColor = false;
            this.btn_Stop.Click += new System.EventHandler(this.Btn_Stop_Click);
            // 
            // btn_InterruptResume
            // 
            this.btn_InterruptResume.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_InterruptResume.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_InterruptResume.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_InterruptResume.Location = new System.Drawing.Point(610, 50);
            this.btn_InterruptResume.Margin = new System.Windows.Forms.Padding(0);
            this.btn_InterruptResume.Name = "btn_InterruptResume";
            this.btn_InterruptResume.Size = new System.Drawing.Size(100, 50);
            this.btn_InterruptResume.TabIndex = 1;
            this.btn_InterruptResume.Text = "中断";
            this.btn_InterruptResume.UseVisualStyleBackColor = false;
            this.btn_InterruptResume.Click += new System.EventHandler(this.Btn_InterruptResume_Click);
            // 
            // lbl_NLightOnSec
            // 
            this.lbl_NLightOnSec.BackColor = System.Drawing.Color.White;
            this.lbl_NLightOnSec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_NLightOnSec.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NLightOnSec.Location = new System.Drawing.Point(1, 42);
            this.lbl_NLightOnSec.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NLightOnSec.Name = "lbl_NLightOnSec";
            this.lbl_NLightOnSec.Size = new System.Drawing.Size(232, 40);
            this.lbl_NLightOnSec.TabIndex = 6;
            this.lbl_NLightOnSec.Text = "北信号機の青色灯火時間";
            this.lbl_NLightOnSec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SLightOnSec
            // 
            this.lbl_SLightOnSec.BackColor = System.Drawing.Color.White;
            this.lbl_SLightOnSec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_SLightOnSec.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SLightOnSec.Location = new System.Drawing.Point(1, 83);
            this.lbl_SLightOnSec.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SLightOnSec.Name = "lbl_SLightOnSec";
            this.lbl_SLightOnSec.Size = new System.Drawing.Size(232, 40);
            this.lbl_SLightOnSec.TabIndex = 7;
            this.lbl_SLightOnSec.Text = "南信号機の青色灯火時間";
            this.lbl_SLightOnSec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ELightOnSec
            // 
            this.lbl_ELightOnSec.BackColor = System.Drawing.Color.White;
            this.lbl_ELightOnSec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_ELightOnSec.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_ELightOnSec.Location = new System.Drawing.Point(1, 124);
            this.lbl_ELightOnSec.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_ELightOnSec.Name = "lbl_ELightOnSec";
            this.lbl_ELightOnSec.Size = new System.Drawing.Size(232, 40);
            this.lbl_ELightOnSec.TabIndex = 8;
            this.lbl_ELightOnSec.Text = "東信号機の青色灯火時間";
            this.lbl_ELightOnSec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WLightOnSec
            // 
            this.lbl_WLightOnSec.BackColor = System.Drawing.Color.White;
            this.lbl_WLightOnSec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_WLightOnSec.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WLightOnSec.Location = new System.Drawing.Point(1, 165);
            this.lbl_WLightOnSec.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WLightOnSec.Name = "lbl_WLightOnSec";
            this.lbl_WLightOnSec.Size = new System.Drawing.Size(232, 40);
            this.lbl_WLightOnSec.TabIndex = 9;
            this.lbl_WLightOnSec.Text = "西信号機の青色灯火時間";
            this.lbl_WLightOnSec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_PrepareSec
            // 
            this.lbl_PrepareSec.BackColor = System.Drawing.Color.White;
            this.lbl_PrepareSec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_PrepareSec.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_PrepareSec.Location = new System.Drawing.Point(1, 206);
            this.lbl_PrepareSec.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_PrepareSec.Name = "lbl_PrepareSec";
            this.lbl_PrepareSec.Size = new System.Drawing.Size(232, 40);
            this.lbl_PrepareSec.TabIndex = 10;
            this.lbl_PrepareSec.Text = "進行方向切り替え準備時間";
            this.lbl_PrepareSec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlp_InputSecField
            // 
            this.tlp_InputSecField.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlp_InputSecField.ColumnCount = 2;
            this.tlp_InputSecField.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tlp_InputSecField.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlp_InputSecField.Controls.Add(this.txt_PrepareSec, 1, 5);
            this.tlp_InputSecField.Controls.Add(this.txt_WLightOnSec, 1, 4);
            this.tlp_InputSecField.Controls.Add(this.txt_ELightOnSec, 1, 3);
            this.tlp_InputSecField.Controls.Add(this.txt_SLightOnSec, 1, 2);
            this.tlp_InputSecField.Controls.Add(this.txt_NLightOnSec, 1, 1);
            this.tlp_InputSecField.Controls.Add(this.lbl_SecHead, 1, 0);
            this.tlp_InputSecField.Controls.Add(this.lbl_NameHead, 0, 0);
            this.tlp_InputSecField.Controls.Add(this.lbl_PrepareSec, 0, 5);
            this.tlp_InputSecField.Controls.Add(this.lbl_NLightOnSec, 0, 1);
            this.tlp_InputSecField.Controls.Add(this.lbl_SLightOnSec, 0, 2);
            this.tlp_InputSecField.Controls.Add(this.lbl_ELightOnSec, 0, 3);
            this.tlp_InputSecField.Controls.Add(this.lbl_WLightOnSec, 0, 4);
            this.tlp_InputSecField.Location = new System.Drawing.Point(480, 140);
            this.tlp_InputSecField.Margin = new System.Windows.Forms.Padding(0);
            this.tlp_InputSecField.Name = "tlp_InputSecField";
            this.tlp_InputSecField.RowCount = 6;
            this.tlp_InputSecField.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlp_InputSecField.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlp_InputSecField.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlp_InputSecField.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlp_InputSecField.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlp_InputSecField.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlp_InputSecField.Size = new System.Drawing.Size(360, 247);
            this.tlp_InputSecField.TabIndex = 4;
            // 
            // txt_PrepareSec
            // 
            this.txt_PrepareSec.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_PrepareSec.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txt_PrepareSec.Location = new System.Drawing.Point(234, 216);
            this.txt_PrepareSec.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.txt_PrepareSec.MaxLength = 2;
            this.txt_PrepareSec.Name = "txt_PrepareSec";
            this.txt_PrepareSec.Size = new System.Drawing.Size(125, 20);
            this.txt_PrepareSec.TabIndex = 7;
            this.txt_PrepareSec.Text = "1";
            this.txt_PrepareSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_WLightOnSec
            // 
            this.txt_WLightOnSec.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_WLightOnSec.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txt_WLightOnSec.Location = new System.Drawing.Point(234, 175);
            this.txt_WLightOnSec.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.txt_WLightOnSec.MaxLength = 2;
            this.txt_WLightOnSec.Name = "txt_WLightOnSec";
            this.txt_WLightOnSec.Size = new System.Drawing.Size(125, 20);
            this.txt_WLightOnSec.TabIndex = 6;
            this.txt_WLightOnSec.Text = "5";
            this.txt_WLightOnSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_ELightOnSec
            // 
            this.txt_ELightOnSec.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_ELightOnSec.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txt_ELightOnSec.Location = new System.Drawing.Point(234, 134);
            this.txt_ELightOnSec.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.txt_ELightOnSec.MaxLength = 2;
            this.txt_ELightOnSec.Name = "txt_ELightOnSec";
            this.txt_ELightOnSec.Size = new System.Drawing.Size(125, 20);
            this.txt_ELightOnSec.TabIndex = 5;
            this.txt_ELightOnSec.Text = "5";
            this.txt_ELightOnSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_SLightOnSec
            // 
            this.txt_SLightOnSec.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_SLightOnSec.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txt_SLightOnSec.Location = new System.Drawing.Point(234, 93);
            this.txt_SLightOnSec.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.txt_SLightOnSec.MaxLength = 2;
            this.txt_SLightOnSec.Name = "txt_SLightOnSec";
            this.txt_SLightOnSec.Size = new System.Drawing.Size(125, 20);
            this.txt_SLightOnSec.TabIndex = 4;
            this.txt_SLightOnSec.Text = "5";
            this.txt_SLightOnSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_NLightOnSec
            // 
            this.txt_NLightOnSec.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_NLightOnSec.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txt_NLightOnSec.Location = new System.Drawing.Point(234, 52);
            this.txt_NLightOnSec.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.txt_NLightOnSec.MaxLength = 2;
            this.txt_NLightOnSec.Name = "txt_NLightOnSec";
            this.txt_NLightOnSec.Size = new System.Drawing.Size(125, 20);
            this.txt_NLightOnSec.TabIndex = 3;
            this.txt_NLightOnSec.Text = "5";
            this.txt_NLightOnSec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbl_SecHead
            // 
            this.lbl_SecHead.BackColor = System.Drawing.Color.SkyBlue;
            this.lbl_SecHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_SecHead.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SecHead.Location = new System.Drawing.Point(234, 1);
            this.lbl_SecHead.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SecHead.Name = "lbl_SecHead";
            this.lbl_SecHead.Size = new System.Drawing.Size(125, 40);
            this.lbl_SecHead.TabIndex = 12;
            this.lbl_SecHead.Text = "時間（秒）";
            this.lbl_SecHead.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NameHead
            // 
            this.lbl_NameHead.BackColor = System.Drawing.Color.SkyBlue;
            this.lbl_NameHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_NameHead.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NameHead.Location = new System.Drawing.Point(1, 1);
            this.lbl_NameHead.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NameHead.Name = "lbl_NameHead";
            this.lbl_NameHead.Size = new System.Drawing.Size(232, 40);
            this.lbl_NameHead.TabIndex = 5;
            this.lbl_NameHead.Text = "名称";
            this.lbl_NameHead.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnl_Traffic
            // 
            this.pnl_Traffic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Traffic.Controls.Add(this.lbl_SouthStopLine);
            this.pnl_Traffic.Controls.Add(this.lbl_NorthStopLine);
            this.pnl_Traffic.Controls.Add(this.lbl_WestStopLine);
            this.pnl_Traffic.Controls.Add(this.lbl_EastStopLine);
            this.pnl_Traffic.Controls.Add(this.lbl_SouthCenterLine);
            this.pnl_Traffic.Controls.Add(this.lbl_WestCenterLine);
            this.pnl_Traffic.Controls.Add(this.lbl_NorthCenterLine);
            this.pnl_Traffic.Controls.Add(this.lbl_EastCenterLine);
            this.pnl_Traffic.Controls.Add(this.lbl_SouthCrossWalkSix);
            this.pnl_Traffic.Controls.Add(this.lbl_SouthCrossWalkFive);
            this.pnl_Traffic.Controls.Add(this.lbl_SouthCrossWalkFour);
            this.pnl_Traffic.Controls.Add(this.lbl_SouthCrossWalkThree);
            this.pnl_Traffic.Controls.Add(this.lbl_SouthCrossWalkTwo);
            this.pnl_Traffic.Controls.Add(this.lbl_SouthCrossWalkOne);
            this.pnl_Traffic.Controls.Add(this.lbl_NorthCrossWalkSix);
            this.pnl_Traffic.Controls.Add(this.lbl_NorthCrossWalkFive);
            this.pnl_Traffic.Controls.Add(this.lbl_NorthCrossWalkFour);
            this.pnl_Traffic.Controls.Add(this.lbl_NorthCrossWalkThree);
            this.pnl_Traffic.Controls.Add(this.lbl_NorthCrossWalkTwo);
            this.pnl_Traffic.Controls.Add(this.lbl_NorthCrossWalkOne);
            this.pnl_Traffic.Controls.Add(this.lbl_EastCrossWalkSix);
            this.pnl_Traffic.Controls.Add(this.lbl_EastCrossWalkFive);
            this.pnl_Traffic.Controls.Add(this.lbl_EastCrossWalkFour);
            this.pnl_Traffic.Controls.Add(this.lbl_EastCrossWalkThree);
            this.pnl_Traffic.Controls.Add(this.lbl_EastCrossWalkTwo);
            this.pnl_Traffic.Controls.Add(this.lbl_EastCrossWalkOne);
            this.pnl_Traffic.Controls.Add(this.lbl_WestCrossWalkSix);
            this.pnl_Traffic.Controls.Add(this.lbl_WestCrossWalkOne);
            this.pnl_Traffic.Controls.Add(this.lbl_WestCrossWalkFive);
            this.pnl_Traffic.Controls.Add(this.lbl_WestCrossWalkFour);
            this.pnl_Traffic.Controls.Add(this.lbl_WestCrossWalkThree);
            this.pnl_Traffic.Controls.Add(this.lbl_WestCrossWalkTwo);
            this.pnl_Traffic.Controls.Add(this.lbl_SouthGreen);
            this.pnl_Traffic.Controls.Add(this.lbl_NorthRed);
            this.pnl_Traffic.Controls.Add(this.lbl_SouthYellow);
            this.pnl_Traffic.Controls.Add(this.lbl_EastRed);
            this.pnl_Traffic.Controls.Add(this.lbl_SouthRed);
            this.pnl_Traffic.Controls.Add(this.lbl_NorthYellow);
            this.pnl_Traffic.Controls.Add(this.lbl_WestGreen);
            this.pnl_Traffic.Controls.Add(this.lbl_NorthGreen);
            this.pnl_Traffic.Controls.Add(this.lbl_EastYellow);
            this.pnl_Traffic.Controls.Add(this.lbl_WestYellow);
            this.pnl_Traffic.Controls.Add(this.lbl_EastGreen);
            this.pnl_Traffic.Controls.Add(this.lbl_WestRed);
            this.pnl_Traffic.Controls.Add(this.lbl_SNRoadway);
            this.pnl_Traffic.Controls.Add(this.lbl_EWRoadway);
            this.pnl_Traffic.Location = new System.Drawing.Point(50, 50);
            this.pnl_Traffic.Margin = new System.Windows.Forms.Padding(0);
            this.pnl_Traffic.Name = "pnl_Traffic";
            this.pnl_Traffic.Size = new System.Drawing.Size(380, 380);
            this.pnl_Traffic.TabIndex = 5;
            // 
            // lbl_SouthStopLine
            // 
            this.lbl_SouthStopLine.BackColor = System.Drawing.Color.White;
            this.lbl_SouthStopLine.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SouthStopLine.Location = new System.Drawing.Point(151, 330);
            this.lbl_SouthStopLine.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SouthStopLine.Name = "lbl_SouthStopLine";
            this.lbl_SouthStopLine.Size = new System.Drawing.Size(36, 10);
            this.lbl_SouthStopLine.TabIndex = 38;
            this.lbl_SouthStopLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NorthStopLine
            // 
            this.lbl_NorthStopLine.BackColor = System.Drawing.Color.White;
            this.lbl_NorthStopLine.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NorthStopLine.Location = new System.Drawing.Point(193, 41);
            this.lbl_NorthStopLine.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NorthStopLine.Name = "lbl_NorthStopLine";
            this.lbl_NorthStopLine.Size = new System.Drawing.Size(36, 10);
            this.lbl_NorthStopLine.TabIndex = 37;
            this.lbl_NorthStopLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WestStopLine
            // 
            this.lbl_WestStopLine.BackColor = System.Drawing.Color.White;
            this.lbl_WestStopLine.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WestStopLine.Location = new System.Drawing.Point(40, 151);
            this.lbl_WestStopLine.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WestStopLine.Name = "lbl_WestStopLine";
            this.lbl_WestStopLine.Size = new System.Drawing.Size(10, 36);
            this.lbl_WestStopLine.TabIndex = 36;
            this.lbl_WestStopLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EastStopLine
            // 
            this.lbl_EastStopLine.BackColor = System.Drawing.Color.White;
            this.lbl_EastStopLine.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EastStopLine.Location = new System.Drawing.Point(330, 192);
            this.lbl_EastStopLine.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EastStopLine.Name = "lbl_EastStopLine";
            this.lbl_EastStopLine.Size = new System.Drawing.Size(10, 36);
            this.lbl_EastStopLine.TabIndex = 35;
            this.lbl_EastStopLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SouthCenterLine
            // 
            this.lbl_SouthCenterLine.BackColor = System.Drawing.Color.White;
            this.lbl_SouthCenterLine.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SouthCenterLine.Location = new System.Drawing.Point(187, 330);
            this.lbl_SouthCenterLine.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SouthCenterLine.Name = "lbl_SouthCenterLine";
            this.lbl_SouthCenterLine.Size = new System.Drawing.Size(6, 50);
            this.lbl_SouthCenterLine.TabIndex = 35;
            this.lbl_SouthCenterLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WestCenterLine
            // 
            this.lbl_WestCenterLine.BackColor = System.Drawing.Color.White;
            this.lbl_WestCenterLine.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WestCenterLine.Location = new System.Drawing.Point(0, 187);
            this.lbl_WestCenterLine.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WestCenterLine.Name = "lbl_WestCenterLine";
            this.lbl_WestCenterLine.Size = new System.Drawing.Size(50, 6);
            this.lbl_WestCenterLine.TabIndex = 34;
            this.lbl_WestCenterLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NorthCenterLine
            // 
            this.lbl_NorthCenterLine.BackColor = System.Drawing.Color.White;
            this.lbl_NorthCenterLine.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NorthCenterLine.Location = new System.Drawing.Point(187, 0);
            this.lbl_NorthCenterLine.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NorthCenterLine.Name = "lbl_NorthCenterLine";
            this.lbl_NorthCenterLine.Size = new System.Drawing.Size(6, 51);
            this.lbl_NorthCenterLine.TabIndex = 34;
            this.lbl_NorthCenterLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EastCenterLine
            // 
            this.lbl_EastCenterLine.BackColor = System.Drawing.Color.White;
            this.lbl_EastCenterLine.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EastCenterLine.Location = new System.Drawing.Point(330, 186);
            this.lbl_EastCenterLine.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EastCenterLine.Name = "lbl_EastCenterLine";
            this.lbl_EastCenterLine.Size = new System.Drawing.Size(50, 6);
            this.lbl_EastCenterLine.TabIndex = 32;
            this.lbl_EastCenterLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SouthCrossWalkSix
            // 
            this.lbl_SouthCrossWalkSix.BackColor = System.Drawing.Color.White;
            this.lbl_SouthCrossWalkSix.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SouthCrossWalkSix.Location = new System.Drawing.Point(221, 290);
            this.lbl_SouthCrossWalkSix.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SouthCrossWalkSix.Name = "lbl_SouthCrossWalkSix";
            this.lbl_SouthCrossWalkSix.Size = new System.Drawing.Size(8, 30);
            this.lbl_SouthCrossWalkSix.TabIndex = 31;
            this.lbl_SouthCrossWalkSix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SouthCrossWalkFive
            // 
            this.lbl_SouthCrossWalkFive.BackColor = System.Drawing.Color.White;
            this.lbl_SouthCrossWalkFive.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SouthCrossWalkFive.Location = new System.Drawing.Point(207, 290);
            this.lbl_SouthCrossWalkFive.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SouthCrossWalkFive.Name = "lbl_SouthCrossWalkFive";
            this.lbl_SouthCrossWalkFive.Size = new System.Drawing.Size(8, 30);
            this.lbl_SouthCrossWalkFive.TabIndex = 30;
            this.lbl_SouthCrossWalkFive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SouthCrossWalkFour
            // 
            this.lbl_SouthCrossWalkFour.BackColor = System.Drawing.Color.White;
            this.lbl_SouthCrossWalkFour.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SouthCrossWalkFour.Location = new System.Drawing.Point(193, 290);
            this.lbl_SouthCrossWalkFour.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SouthCrossWalkFour.Name = "lbl_SouthCrossWalkFour";
            this.lbl_SouthCrossWalkFour.Size = new System.Drawing.Size(8, 30);
            this.lbl_SouthCrossWalkFour.TabIndex = 29;
            this.lbl_SouthCrossWalkFour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SouthCrossWalkThree
            // 
            this.lbl_SouthCrossWalkThree.BackColor = System.Drawing.Color.White;
            this.lbl_SouthCrossWalkThree.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SouthCrossWalkThree.Location = new System.Drawing.Point(179, 290);
            this.lbl_SouthCrossWalkThree.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SouthCrossWalkThree.Name = "lbl_SouthCrossWalkThree";
            this.lbl_SouthCrossWalkThree.Size = new System.Drawing.Size(8, 30);
            this.lbl_SouthCrossWalkThree.TabIndex = 28;
            this.lbl_SouthCrossWalkThree.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SouthCrossWalkTwo
            // 
            this.lbl_SouthCrossWalkTwo.BackColor = System.Drawing.Color.White;
            this.lbl_SouthCrossWalkTwo.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SouthCrossWalkTwo.Location = new System.Drawing.Point(165, 290);
            this.lbl_SouthCrossWalkTwo.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SouthCrossWalkTwo.Name = "lbl_SouthCrossWalkTwo";
            this.lbl_SouthCrossWalkTwo.Size = new System.Drawing.Size(8, 30);
            this.lbl_SouthCrossWalkTwo.TabIndex = 27;
            this.lbl_SouthCrossWalkTwo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SouthCrossWalkOne
            // 
            this.lbl_SouthCrossWalkOne.BackColor = System.Drawing.Color.White;
            this.lbl_SouthCrossWalkOne.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SouthCrossWalkOne.Location = new System.Drawing.Point(151, 290);
            this.lbl_SouthCrossWalkOne.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SouthCrossWalkOne.Name = "lbl_SouthCrossWalkOne";
            this.lbl_SouthCrossWalkOne.Size = new System.Drawing.Size(8, 30);
            this.lbl_SouthCrossWalkOne.TabIndex = 26;
            this.lbl_SouthCrossWalkOne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NorthCrossWalkSix
            // 
            this.lbl_NorthCrossWalkSix.BackColor = System.Drawing.Color.White;
            this.lbl_NorthCrossWalkSix.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NorthCrossWalkSix.Location = new System.Drawing.Point(221, 60);
            this.lbl_NorthCrossWalkSix.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NorthCrossWalkSix.Name = "lbl_NorthCrossWalkSix";
            this.lbl_NorthCrossWalkSix.Size = new System.Drawing.Size(8, 30);
            this.lbl_NorthCrossWalkSix.TabIndex = 25;
            this.lbl_NorthCrossWalkSix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NorthCrossWalkFive
            // 
            this.lbl_NorthCrossWalkFive.BackColor = System.Drawing.Color.White;
            this.lbl_NorthCrossWalkFive.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NorthCrossWalkFive.Location = new System.Drawing.Point(207, 60);
            this.lbl_NorthCrossWalkFive.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NorthCrossWalkFive.Name = "lbl_NorthCrossWalkFive";
            this.lbl_NorthCrossWalkFive.Size = new System.Drawing.Size(8, 30);
            this.lbl_NorthCrossWalkFive.TabIndex = 24;
            this.lbl_NorthCrossWalkFive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NorthCrossWalkFour
            // 
            this.lbl_NorthCrossWalkFour.BackColor = System.Drawing.Color.White;
            this.lbl_NorthCrossWalkFour.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NorthCrossWalkFour.Location = new System.Drawing.Point(193, 60);
            this.lbl_NorthCrossWalkFour.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NorthCrossWalkFour.Name = "lbl_NorthCrossWalkFour";
            this.lbl_NorthCrossWalkFour.Size = new System.Drawing.Size(8, 30);
            this.lbl_NorthCrossWalkFour.TabIndex = 23;
            this.lbl_NorthCrossWalkFour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NorthCrossWalkThree
            // 
            this.lbl_NorthCrossWalkThree.BackColor = System.Drawing.Color.White;
            this.lbl_NorthCrossWalkThree.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NorthCrossWalkThree.Location = new System.Drawing.Point(179, 60);
            this.lbl_NorthCrossWalkThree.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NorthCrossWalkThree.Name = "lbl_NorthCrossWalkThree";
            this.lbl_NorthCrossWalkThree.Size = new System.Drawing.Size(8, 30);
            this.lbl_NorthCrossWalkThree.TabIndex = 22;
            this.lbl_NorthCrossWalkThree.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NorthCrossWalkTwo
            // 
            this.lbl_NorthCrossWalkTwo.BackColor = System.Drawing.Color.White;
            this.lbl_NorthCrossWalkTwo.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NorthCrossWalkTwo.Location = new System.Drawing.Point(165, 60);
            this.lbl_NorthCrossWalkTwo.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NorthCrossWalkTwo.Name = "lbl_NorthCrossWalkTwo";
            this.lbl_NorthCrossWalkTwo.Size = new System.Drawing.Size(8, 30);
            this.lbl_NorthCrossWalkTwo.TabIndex = 21;
            this.lbl_NorthCrossWalkTwo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NorthCrossWalkOne
            // 
            this.lbl_NorthCrossWalkOne.BackColor = System.Drawing.Color.White;
            this.lbl_NorthCrossWalkOne.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NorthCrossWalkOne.Location = new System.Drawing.Point(151, 60);
            this.lbl_NorthCrossWalkOne.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NorthCrossWalkOne.Name = "lbl_NorthCrossWalkOne";
            this.lbl_NorthCrossWalkOne.Size = new System.Drawing.Size(8, 30);
            this.lbl_NorthCrossWalkOne.TabIndex = 16;
            this.lbl_NorthCrossWalkOne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EastCrossWalkSix
            // 
            this.lbl_EastCrossWalkSix.BackColor = System.Drawing.Color.White;
            this.lbl_EastCrossWalkSix.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EastCrossWalkSix.Location = new System.Drawing.Point(290, 220);
            this.lbl_EastCrossWalkSix.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EastCrossWalkSix.Name = "lbl_EastCrossWalkSix";
            this.lbl_EastCrossWalkSix.Size = new System.Drawing.Size(30, 8);
            this.lbl_EastCrossWalkSix.TabIndex = 20;
            this.lbl_EastCrossWalkSix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EastCrossWalkFive
            // 
            this.lbl_EastCrossWalkFive.BackColor = System.Drawing.Color.White;
            this.lbl_EastCrossWalkFive.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EastCrossWalkFive.Location = new System.Drawing.Point(290, 206);
            this.lbl_EastCrossWalkFive.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EastCrossWalkFive.Name = "lbl_EastCrossWalkFive";
            this.lbl_EastCrossWalkFive.Size = new System.Drawing.Size(30, 8);
            this.lbl_EastCrossWalkFive.TabIndex = 17;
            this.lbl_EastCrossWalkFive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EastCrossWalkFour
            // 
            this.lbl_EastCrossWalkFour.BackColor = System.Drawing.Color.White;
            this.lbl_EastCrossWalkFour.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EastCrossWalkFour.Location = new System.Drawing.Point(290, 192);
            this.lbl_EastCrossWalkFour.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EastCrossWalkFour.Name = "lbl_EastCrossWalkFour";
            this.lbl_EastCrossWalkFour.Size = new System.Drawing.Size(30, 8);
            this.lbl_EastCrossWalkFour.TabIndex = 16;
            this.lbl_EastCrossWalkFour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EastCrossWalkThree
            // 
            this.lbl_EastCrossWalkThree.BackColor = System.Drawing.Color.White;
            this.lbl_EastCrossWalkThree.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EastCrossWalkThree.Location = new System.Drawing.Point(290, 178);
            this.lbl_EastCrossWalkThree.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EastCrossWalkThree.Name = "lbl_EastCrossWalkThree";
            this.lbl_EastCrossWalkThree.Size = new System.Drawing.Size(30, 8);
            this.lbl_EastCrossWalkThree.TabIndex = 16;
            this.lbl_EastCrossWalkThree.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EastCrossWalkTwo
            // 
            this.lbl_EastCrossWalkTwo.BackColor = System.Drawing.Color.White;
            this.lbl_EastCrossWalkTwo.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EastCrossWalkTwo.Location = new System.Drawing.Point(290, 164);
            this.lbl_EastCrossWalkTwo.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EastCrossWalkTwo.Name = "lbl_EastCrossWalkTwo";
            this.lbl_EastCrossWalkTwo.Size = new System.Drawing.Size(30, 8);
            this.lbl_EastCrossWalkTwo.TabIndex = 16;
            this.lbl_EastCrossWalkTwo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EastCrossWalkOne
            // 
            this.lbl_EastCrossWalkOne.BackColor = System.Drawing.Color.White;
            this.lbl_EastCrossWalkOne.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EastCrossWalkOne.Location = new System.Drawing.Point(290, 151);
            this.lbl_EastCrossWalkOne.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EastCrossWalkOne.Name = "lbl_EastCrossWalkOne";
            this.lbl_EastCrossWalkOne.Size = new System.Drawing.Size(30, 8);
            this.lbl_EastCrossWalkOne.TabIndex = 15;
            this.lbl_EastCrossWalkOne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WestCrossWalkSix
            // 
            this.lbl_WestCrossWalkSix.BackColor = System.Drawing.Color.White;
            this.lbl_WestCrossWalkSix.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WestCrossWalkSix.Location = new System.Drawing.Point(60, 221);
            this.lbl_WestCrossWalkSix.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WestCrossWalkSix.Name = "lbl_WestCrossWalkSix";
            this.lbl_WestCrossWalkSix.Size = new System.Drawing.Size(30, 8);
            this.lbl_WestCrossWalkSix.TabIndex = 19;
            this.lbl_WestCrossWalkSix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WestCrossWalkOne
            // 
            this.lbl_WestCrossWalkOne.BackColor = System.Drawing.Color.White;
            this.lbl_WestCrossWalkOne.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WestCrossWalkOne.Location = new System.Drawing.Point(60, 151);
            this.lbl_WestCrossWalkOne.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WestCrossWalkOne.Name = "lbl_WestCrossWalkOne";
            this.lbl_WestCrossWalkOne.Size = new System.Drawing.Size(30, 8);
            this.lbl_WestCrossWalkOne.TabIndex = 14;
            this.lbl_WestCrossWalkOne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WestCrossWalkFive
            // 
            this.lbl_WestCrossWalkFive.BackColor = System.Drawing.Color.White;
            this.lbl_WestCrossWalkFive.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WestCrossWalkFive.Location = new System.Drawing.Point(60, 207);
            this.lbl_WestCrossWalkFive.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WestCrossWalkFive.Name = "lbl_WestCrossWalkFive";
            this.lbl_WestCrossWalkFive.Size = new System.Drawing.Size(30, 8);
            this.lbl_WestCrossWalkFive.TabIndex = 18;
            this.lbl_WestCrossWalkFive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WestCrossWalkFour
            // 
            this.lbl_WestCrossWalkFour.BackColor = System.Drawing.Color.White;
            this.lbl_WestCrossWalkFour.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WestCrossWalkFour.Location = new System.Drawing.Point(60, 193);
            this.lbl_WestCrossWalkFour.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WestCrossWalkFour.Name = "lbl_WestCrossWalkFour";
            this.lbl_WestCrossWalkFour.Size = new System.Drawing.Size(30, 8);
            this.lbl_WestCrossWalkFour.TabIndex = 17;
            this.lbl_WestCrossWalkFour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WestCrossWalkThree
            // 
            this.lbl_WestCrossWalkThree.BackColor = System.Drawing.Color.White;
            this.lbl_WestCrossWalkThree.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WestCrossWalkThree.Location = new System.Drawing.Point(60, 179);
            this.lbl_WestCrossWalkThree.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WestCrossWalkThree.Name = "lbl_WestCrossWalkThree";
            this.lbl_WestCrossWalkThree.Size = new System.Drawing.Size(30, 8);
            this.lbl_WestCrossWalkThree.TabIndex = 16;
            this.lbl_WestCrossWalkThree.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WestCrossWalkTwo
            // 
            this.lbl_WestCrossWalkTwo.BackColor = System.Drawing.Color.White;
            this.lbl_WestCrossWalkTwo.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WestCrossWalkTwo.Location = new System.Drawing.Point(60, 165);
            this.lbl_WestCrossWalkTwo.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WestCrossWalkTwo.Name = "lbl_WestCrossWalkTwo";
            this.lbl_WestCrossWalkTwo.Size = new System.Drawing.Size(30, 8);
            this.lbl_WestCrossWalkTwo.TabIndex = 15;
            this.lbl_WestCrossWalkTwo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SouthGreen
            // 
            this.lbl_SouthGreen.BackColor = System.Drawing.Color.White;
            this.lbl_SouthGreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_SouthGreen.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SouthGreen.Location = new System.Drawing.Point(205, 250);
            this.lbl_SouthGreen.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SouthGreen.Name = "lbl_SouthGreen";
            this.lbl_SouthGreen.Size = new System.Drawing.Size(30, 30);
            this.lbl_SouthGreen.TabIndex = 16;
            this.lbl_SouthGreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NorthRed
            // 
            this.lbl_NorthRed.BackColor = System.Drawing.Color.White;
            this.lbl_NorthRed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_NorthRed.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NorthRed.Location = new System.Drawing.Point(205, 100);
            this.lbl_NorthRed.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NorthRed.Name = "lbl_NorthRed";
            this.lbl_NorthRed.Size = new System.Drawing.Size(30, 30);
            this.lbl_NorthRed.TabIndex = 13;
            this.lbl_NorthRed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SouthYellow
            // 
            this.lbl_SouthYellow.BackColor = System.Drawing.Color.White;
            this.lbl_SouthYellow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_SouthYellow.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SouthYellow.Location = new System.Drawing.Point(174, 250);
            this.lbl_SouthYellow.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SouthYellow.Name = "lbl_SouthYellow";
            this.lbl_SouthYellow.Size = new System.Drawing.Size(32, 30);
            this.lbl_SouthYellow.TabIndex = 15;
            this.lbl_SouthYellow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EastRed
            // 
            this.lbl_EastRed.BackColor = System.Drawing.Color.White;
            this.lbl_EastRed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_EastRed.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EastRed.Location = new System.Drawing.Point(250, 205);
            this.lbl_EastRed.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EastRed.Name = "lbl_EastRed";
            this.lbl_EastRed.Size = new System.Drawing.Size(30, 30);
            this.lbl_EastRed.TabIndex = 13;
            this.lbl_EastRed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SouthRed
            // 
            this.lbl_SouthRed.BackColor = System.Drawing.Color.White;
            this.lbl_SouthRed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_SouthRed.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SouthRed.Location = new System.Drawing.Point(145, 250);
            this.lbl_SouthRed.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SouthRed.Name = "lbl_SouthRed";
            this.lbl_SouthRed.Size = new System.Drawing.Size(30, 30);
            this.lbl_SouthRed.TabIndex = 14;
            this.lbl_SouthRed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NorthYellow
            // 
            this.lbl_NorthYellow.BackColor = System.Drawing.Color.White;
            this.lbl_NorthYellow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_NorthYellow.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NorthYellow.Location = new System.Drawing.Point(174, 100);
            this.lbl_NorthYellow.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NorthYellow.Name = "lbl_NorthYellow";
            this.lbl_NorthYellow.Size = new System.Drawing.Size(32, 30);
            this.lbl_NorthYellow.TabIndex = 12;
            this.lbl_NorthYellow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WestGreen
            // 
            this.lbl_WestGreen.BackColor = System.Drawing.Color.White;
            this.lbl_WestGreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_WestGreen.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WestGreen.Location = new System.Drawing.Point(100, 205);
            this.lbl_WestGreen.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WestGreen.Name = "lbl_WestGreen";
            this.lbl_WestGreen.Size = new System.Drawing.Size(30, 30);
            this.lbl_WestGreen.TabIndex = 10;
            this.lbl_WestGreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NorthGreen
            // 
            this.lbl_NorthGreen.BackColor = System.Drawing.Color.White;
            this.lbl_NorthGreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_NorthGreen.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NorthGreen.Location = new System.Drawing.Point(145, 100);
            this.lbl_NorthGreen.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NorthGreen.Name = "lbl_NorthGreen";
            this.lbl_NorthGreen.Size = new System.Drawing.Size(30, 30);
            this.lbl_NorthGreen.TabIndex = 11;
            this.lbl_NorthGreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EastYellow
            // 
            this.lbl_EastYellow.BackColor = System.Drawing.Color.White;
            this.lbl_EastYellow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_EastYellow.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EastYellow.Location = new System.Drawing.Point(250, 174);
            this.lbl_EastYellow.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EastYellow.Name = "lbl_EastYellow";
            this.lbl_EastYellow.Size = new System.Drawing.Size(30, 32);
            this.lbl_EastYellow.TabIndex = 12;
            this.lbl_EastYellow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WestYellow
            // 
            this.lbl_WestYellow.BackColor = System.Drawing.Color.White;
            this.lbl_WestYellow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_WestYellow.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WestYellow.Location = new System.Drawing.Point(100, 174);
            this.lbl_WestYellow.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WestYellow.Name = "lbl_WestYellow";
            this.lbl_WestYellow.Size = new System.Drawing.Size(30, 32);
            this.lbl_WestYellow.TabIndex = 9;
            this.lbl_WestYellow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EastGreen
            // 
            this.lbl_EastGreen.BackColor = System.Drawing.Color.White;
            this.lbl_EastGreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_EastGreen.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EastGreen.Location = new System.Drawing.Point(250, 145);
            this.lbl_EastGreen.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EastGreen.Name = "lbl_EastGreen";
            this.lbl_EastGreen.Size = new System.Drawing.Size(30, 30);
            this.lbl_EastGreen.TabIndex = 11;
            this.lbl_EastGreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WestRed
            // 
            this.lbl_WestRed.BackColor = System.Drawing.Color.White;
            this.lbl_WestRed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_WestRed.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WestRed.Location = new System.Drawing.Point(100, 145);
            this.lbl_WestRed.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WestRed.Name = "lbl_WestRed";
            this.lbl_WestRed.Size = new System.Drawing.Size(30, 30);
            this.lbl_WestRed.TabIndex = 8;
            this.lbl_WestRed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SNRoadway
            // 
            this.lbl_SNRoadway.BackColor = System.Drawing.Color.Silver;
            this.lbl_SNRoadway.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SNRoadway.Location = new System.Drawing.Point(145, 0);
            this.lbl_SNRoadway.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SNRoadway.Name = "lbl_SNRoadway";
            this.lbl_SNRoadway.Size = new System.Drawing.Size(90, 380);
            this.lbl_SNRoadway.TabIndex = 8;
            this.lbl_SNRoadway.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EWRoadway
            // 
            this.lbl_EWRoadway.BackColor = System.Drawing.Color.Silver;
            this.lbl_EWRoadway.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EWRoadway.Location = new System.Drawing.Point(0, 145);
            this.lbl_EWRoadway.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EWRoadway.Name = "lbl_EWRoadway";
            this.lbl_EWRoadway.Size = new System.Drawing.Size(380, 90);
            this.lbl_EWRoadway.TabIndex = 7;
            this.lbl_EWRoadway.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_NorthDirection
            // 
            this.lbl_NorthDirection.BackColor = System.Drawing.Color.White;
            this.lbl_NorthDirection.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_NorthDirection.Location = new System.Drawing.Point(196, 20);
            this.lbl_NorthDirection.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_NorthDirection.Name = "lbl_NorthDirection";
            this.lbl_NorthDirection.Size = new System.Drawing.Size(90, 30);
            this.lbl_NorthDirection.TabIndex = 7;
            this.lbl_NorthDirection.Text = "北方向";
            this.lbl_NorthDirection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SouthDirection
            // 
            this.lbl_SouthDirection.BackColor = System.Drawing.Color.White;
            this.lbl_SouthDirection.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SouthDirection.Location = new System.Drawing.Point(196, 431);
            this.lbl_SouthDirection.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_SouthDirection.Name = "lbl_SouthDirection";
            this.lbl_SouthDirection.Size = new System.Drawing.Size(90, 30);
            this.lbl_SouthDirection.TabIndex = 8;
            this.lbl_SouthDirection.Text = "南方向";
            this.lbl_SouthDirection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_WestDirection
            // 
            this.lbl_WestDirection.BackColor = System.Drawing.Color.White;
            this.lbl_WestDirection.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_WestDirection.Location = new System.Drawing.Point(20, 196);
            this.lbl_WestDirection.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_WestDirection.Name = "lbl_WestDirection";
            this.lbl_WestDirection.Size = new System.Drawing.Size(30, 90);
            this.lbl_WestDirection.TabIndex = 9;
            this.lbl_WestDirection.Text = "西\r\n方\r\n向";
            this.lbl_WestDirection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_EastDirection
            // 
            this.lbl_EastDirection.BackColor = System.Drawing.Color.White;
            this.lbl_EastDirection.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_EastDirection.Location = new System.Drawing.Point(431, 196);
            this.lbl_EastDirection.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_EastDirection.Name = "lbl_EastDirection";
            this.lbl_EastDirection.Size = new System.Drawing.Size(30, 90);
            this.lbl_EastDirection.TabIndex = 10;
            this.lbl_EastDirection.Text = "東\r\n方\r\n向";
            this.lbl_EastDirection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Timer_Traffic
            // 
            this.Timer_Traffic.Tick += new System.EventHandler(this.Timer_Traffic_Tick);
            // 
            // F_TrafficLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(874, 481);
            this.Controls.Add(this.lbl_EastDirection);
            this.Controls.Add(this.lbl_WestDirection);
            this.Controls.Add(this.lbl_SouthDirection);
            this.Controls.Add(this.lbl_NorthDirection);
            this.Controls.Add(this.pnl_Traffic);
            this.Controls.Add(this.tlp_InputSecField);
            this.Controls.Add(this.btn_InterruptResume);
            this.Controls.Add(this.btn_Stop);
            this.Controls.Add(this.btn_Start);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(890, 520);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(890, 520);
            this.Name = "F_TrafficLight";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "信号機プログラム";
            this.Load += new System.EventHandler(this.F_TrafficLight_Load);
            this.tlp_InputSecField.ResumeLayout(false);
            this.tlp_InputSecField.PerformLayout();
            this.pnl_Traffic.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.Button btn_InterruptResume;
        private System.Windows.Forms.Label lbl_NLightOnSec;
        private System.Windows.Forms.Label lbl_SLightOnSec;
        private System.Windows.Forms.Label lbl_ELightOnSec;
        private System.Windows.Forms.Label lbl_WLightOnSec;
        private System.Windows.Forms.Label lbl_PrepareSec;
        private System.Windows.Forms.TableLayoutPanel tlp_InputSecField;
        private System.Windows.Forms.Label lbl_NameHead;
        private System.Windows.Forms.Panel pnl_Traffic;
        private System.Windows.Forms.Label lbl_SNRoadway;
        private System.Windows.Forms.Label lbl_WestRed;
        private System.Windows.Forms.Label lbl_EWRoadway;
        private System.Windows.Forms.Label lbl_NorthRed;
        private System.Windows.Forms.Label lbl_EastRed;
        private System.Windows.Forms.Label lbl_NorthYellow;
        private System.Windows.Forms.Label lbl_WestGreen;
        private System.Windows.Forms.Label lbl_NorthGreen;
        private System.Windows.Forms.Label lbl_EastYellow;
        private System.Windows.Forms.Label lbl_WestYellow;
        private System.Windows.Forms.Label lbl_EastGreen;
        private System.Windows.Forms.Label lbl_SouthGreen;
        private System.Windows.Forms.Label lbl_SouthYellow;
        private System.Windows.Forms.Label lbl_SouthRed;
        private System.Windows.Forms.Label lbl_WestCrossWalkOne;
        private System.Windows.Forms.Label lbl_WestCrossWalkSix;
        private System.Windows.Forms.Label lbl_WestCrossWalkFive;
        private System.Windows.Forms.Label lbl_WestCrossWalkFour;
        private System.Windows.Forms.Label lbl_WestCrossWalkThree;
        private System.Windows.Forms.Label lbl_WestCrossWalkTwo;
        private System.Windows.Forms.Label lbl_EastCrossWalkSix;
        private System.Windows.Forms.Label lbl_EastCrossWalkFive;
        private System.Windows.Forms.Label lbl_EastCrossWalkFour;
        private System.Windows.Forms.Label lbl_EastCrossWalkThree;
        private System.Windows.Forms.Label lbl_EastCrossWalkTwo;
        private System.Windows.Forms.Label lbl_EastCrossWalkOne;
        private System.Windows.Forms.Label lbl_NorthCrossWalkThree;
        private System.Windows.Forms.Label lbl_NorthCrossWalkTwo;
        private System.Windows.Forms.Label lbl_NorthCrossWalkOne;
        private System.Windows.Forms.Label lbl_NorthCrossWalkSix;
        private System.Windows.Forms.Label lbl_NorthCrossWalkFive;
        private System.Windows.Forms.Label lbl_NorthCrossWalkFour;
        private System.Windows.Forms.Label lbl_SouthCrossWalkThree;
        private System.Windows.Forms.Label lbl_SouthCrossWalkTwo;
        private System.Windows.Forms.Label lbl_SouthCrossWalkOne;
        private System.Windows.Forms.Label lbl_SouthCrossWalkSix;
        private System.Windows.Forms.Label lbl_SouthCrossWalkFive;
        private System.Windows.Forms.Label lbl_SouthCrossWalkFour;
        private System.Windows.Forms.Label lbl_EastCenterLine;
        private System.Windows.Forms.Label lbl_WestCenterLine;
        private System.Windows.Forms.Label lbl_WestStopLine;
        private System.Windows.Forms.Label lbl_EastStopLine;
        private System.Windows.Forms.Label lbl_SouthCenterLine;
        private System.Windows.Forms.Label lbl_NorthCenterLine;
        private System.Windows.Forms.Label lbl_SouthStopLine;
        private System.Windows.Forms.Label lbl_NorthStopLine;
        private System.Windows.Forms.Label lbl_WestDirection;
        private System.Windows.Forms.Label lbl_NorthDirection;
        private System.Windows.Forms.Label lbl_SouthDirection;
        private System.Windows.Forms.Label lbl_EastDirection;
        private System.Windows.Forms.TextBox txt_PrepareSec;
        private System.Windows.Forms.TextBox txt_WLightOnSec;
        private System.Windows.Forms.TextBox txt_ELightOnSec;
        private System.Windows.Forms.TextBox txt_SLightOnSec;
        private System.Windows.Forms.TextBox txt_NLightOnSec;
        private System.Windows.Forms.Label lbl_SecHead;
        private System.Windows.Forms.Timer Timer_Traffic;
    }
}

