namespace Minesweeper
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
            this.tmrGame = new System.Windows.Forms.Timer(this.components);
            this.lblFlagsRemaining = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblAntiFlagsRemaining = new System.Windows.Forms.Label();
            this.txtRecords = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // tmrGame
            // 
            this.tmrGame.Interval = 1000;
            this.tmrGame.Tick += new System.EventHandler(this.tmrGame_Tick);
            // 
            // lblFlagsRemaining
            // 
            this.lblFlagsRemaining.AutoSize = true;
            this.lblFlagsRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlagsRemaining.Location = new System.Drawing.Point(266, 22);
            this.lblFlagsRemaining.Name = "lblFlagsRemaining";
            this.lblFlagsRemaining.Size = new System.Drawing.Size(73, 29);
            this.lblFlagsRemaining.TabIndex = 1;
            this.lblFlagsRemaining.Text = "Flags";
            this.lblFlagsRemaining.Visible = false;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(690, 22);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(69, 29);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "Time";
            this.lblTime.Visible = false;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(378, 799);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(242, 16);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.Text = "Safe First Click: False, Anti Mines: False";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAntiFlagsRemaining
            // 
            this.lblAntiFlagsRemaining.AutoSize = true;
            this.lblAntiFlagsRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAntiFlagsRemaining.Location = new System.Drawing.Point(456, 22);
            this.lblAntiFlagsRemaining.Name = "lblAntiFlagsRemaining";
            this.lblAntiFlagsRemaining.Size = new System.Drawing.Size(119, 29);
            this.lblAntiFlagsRemaining.TabIndex = 5;
            this.lblAntiFlagsRemaining.Text = "Anti Flags";
            this.lblAntiFlagsRemaining.Visible = false;
            // 
            // txtRecords
            // 
            this.txtRecords.BackColor = System.Drawing.SystemColors.Desktop;
            this.txtRecords.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRecords.ForeColor = System.Drawing.SystemColors.Control;
            this.txtRecords.Location = new System.Drawing.Point(63, 289);
            this.txtRecords.Name = "txtRecords";
            this.txtRecords.ReadOnly = true;
            this.txtRecords.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtRecords.Size = new System.Drawing.Size(855, 491);
            this.txtRecords.TabIndex = 8;
            this.txtRecords.Text = "";
            this.txtRecords.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.ClientSize = new System.Drawing.Size(982, 853);
            this.Controls.Add(this.txtRecords);
            this.Controls.Add(this.lblAntiFlagsRemaining);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblFlagsRemaining);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MineSweeper";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrGame;
        private System.Windows.Forms.Label lblFlagsRemaining;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblAntiFlagsRemaining;
        private System.Windows.Forms.RichTextBox txtRecords;
    }
}

