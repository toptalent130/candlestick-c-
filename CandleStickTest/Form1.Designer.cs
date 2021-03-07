namespace CandleStickTest
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTLine = new System.Windows.Forms.Button();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.btnHLine = new System.Windows.Forms.Button();
            this.btnVLine = new System.Windows.Forms.Button();
            this.lblCurClose = new System.Windows.Forms.Label();
            this.lblCurOpen = new System.Windows.Forms.Label();
            this.lblCurLow = new System.Windows.Forms.Label();
            this.lblCurHigh = new System.Windows.Forms.Label();
            this.lblCurTime = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnTLine);
            this.panel2.Controls.Add(this.checkBox3);
            this.panel2.Controls.Add(this.btnHLine);
            this.panel2.Controls.Add(this.btnVLine);
            this.panel2.Controls.Add(this.lblCurClose);
            this.panel2.Controls.Add(this.lblCurOpen);
            this.panel2.Controls.Add(this.lblCurLow);
            this.panel2.Controls.Add(this.lblCurHigh);
            this.panel2.Controls.Add(this.lblCurTime);
            this.panel2.Controls.Add(this.checkBox2);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(992, 94);
            this.panel2.TabIndex = 1;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(230, 50);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(162, 24);
            this.button4.TabIndex = 17;
            this.button4.Text = "Selected Line Delete";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(834, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Close:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Black;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(740, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Open:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(644, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Low:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(546, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "High:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(398, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Time:";
            // 
            // btnTLine
            // 
            this.btnTLine.Location = new System.Drawing.Point(693, 16);
            this.btnTLine.Name = "btnTLine";
            this.btnTLine.Size = new System.Drawing.Size(125, 24);
            this.btnTLine.TabIndex = 11;
            this.btnTLine.Text = "TrendLine";
            this.btnTLine.UseVisualStyleBackColor = true;
            this.btnTLine.Click += new System.EventHandler(this.Button6_Click);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.ForeColor = System.Drawing.Color.White;
            this.checkBox3.Location = new System.Drawing.Point(824, 21);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(71, 17);
            this.checkBox3.TabIndex = 10;
            this.checkBox3.Text = "DashLine";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.CheckBox3_CheckedChanged);
            // 
            // btnHLine
            // 
            this.btnHLine.Location = new System.Drawing.Point(549, 16);
            this.btnHLine.Name = "btnHLine";
            this.btnHLine.Size = new System.Drawing.Size(125, 24);
            this.btnHLine.TabIndex = 9;
            this.btnHLine.Text = "HorizontalLine";
            this.btnHLine.UseVisualStyleBackColor = true;
            this.btnHLine.Click += new System.EventHandler(this.Button5_Click);
            // 
            // btnVLine
            // 
            this.btnVLine.Location = new System.Drawing.Point(405, 16);
            this.btnVLine.Name = "btnVLine";
            this.btnVLine.Size = new System.Drawing.Size(125, 24);
            this.btnVLine.TabIndex = 8;
            this.btnVLine.Text = "VerticalLine";
            this.btnVLine.UseVisualStyleBackColor = true;
            this.btnVLine.Click += new System.EventHandler(this.Button4_Click);
            // 
            // lblCurClose
            // 
            this.lblCurClose.AutoSize = true;
            this.lblCurClose.BackColor = System.Drawing.Color.Black;
            this.lblCurClose.ForeColor = System.Drawing.Color.White;
            this.lblCurClose.Location = new System.Drawing.Point(873, 55);
            this.lblCurClose.Name = "lblCurClose";
            this.lblCurClose.Size = new System.Drawing.Size(13, 13);
            this.lblCurClose.TabIndex = 7;
            this.lblCurClose.Text = "0";
            // 
            // lblCurOpen
            // 
            this.lblCurOpen.AutoSize = true;
            this.lblCurOpen.BackColor = System.Drawing.Color.Black;
            this.lblCurOpen.ForeColor = System.Drawing.Color.White;
            this.lblCurOpen.Location = new System.Drawing.Point(782, 55);
            this.lblCurOpen.Name = "lblCurOpen";
            this.lblCurOpen.Size = new System.Drawing.Size(13, 13);
            this.lblCurOpen.TabIndex = 6;
            this.lblCurOpen.Text = "0";
            // 
            // lblCurLow
            // 
            this.lblCurLow.AutoSize = true;
            this.lblCurLow.BackColor = System.Drawing.Color.Black;
            this.lblCurLow.ForeColor = System.Drawing.Color.White;
            this.lblCurLow.Location = new System.Drawing.Point(680, 55);
            this.lblCurLow.Name = "lblCurLow";
            this.lblCurLow.Size = new System.Drawing.Size(13, 13);
            this.lblCurLow.TabIndex = 5;
            this.lblCurLow.Text = "0";
            // 
            // lblCurHigh
            // 
            this.lblCurHigh.AutoSize = true;
            this.lblCurHigh.BackColor = System.Drawing.Color.Black;
            this.lblCurHigh.ForeColor = System.Drawing.Color.White;
            this.lblCurHigh.Location = new System.Drawing.Point(584, 56);
            this.lblCurHigh.Name = "lblCurHigh";
            this.lblCurHigh.Size = new System.Drawing.Size(13, 13);
            this.lblCurHigh.TabIndex = 4;
            this.lblCurHigh.Text = "0";
            // 
            // lblCurTime
            // 
            this.lblCurTime.AutoSize = true;
            this.lblCurTime.BackColor = System.Drawing.Color.Black;
            this.lblCurTime.ForeColor = System.Drawing.Color.White;
            this.lblCurTime.Location = new System.Drawing.Point(437, 56);
            this.lblCurTime.Name = "lblCurTime";
            this.lblCurTime.Size = new System.Drawing.Size(31, 13);
            this.lblCurTime.TabIndex = 0;
            this.lblCurTime.Text = "0:0:0";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.BackColor = System.Drawing.Color.Black;
            this.checkBox2.ForeColor = System.Drawing.Color.White;
            this.checkBox2.Location = new System.Drawing.Point(149, 55);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(75, 17);
            this.checkBox2.TabIndex = 3;
            this.checkBox2.Text = "CrossView";
            this.checkBox2.UseVisualStyleBackColor = false;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.CheckBox2_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(254, 16);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(85, 24);
            this.button3.TabIndex = 2;
            this.button3.Text = "ZoomOut";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(145, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 24);
            this.button2.TabIndex = 1;
            this.button2.Text = "ZoomIn";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.Black;
            this.checkBox1.ForeColor = System.Drawing.Color.White;
            this.checkBox1.Location = new System.Drawing.Point(36, 55);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(107, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "TimeAxisSpacing";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(36, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "NewInput";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 94);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(992, 519);
            this.panel1.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(992, 613);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label lblCurTime;
        private System.Windows.Forms.Label lblCurHigh;
        private System.Windows.Forms.Label lblCurLow;
        private System.Windows.Forms.Label lblCurClose;
        private System.Windows.Forms.Label lblCurOpen;
        private System.Windows.Forms.Button btnHLine;
        private System.Windows.Forms.Button btnVLine;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnTLine;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button4;
    }
}

