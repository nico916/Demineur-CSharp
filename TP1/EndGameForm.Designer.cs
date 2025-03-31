namespace TP1
{
    partial class EndGameForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnPlayAgain;
        private System.Windows.Forms.Button btnReturnToMenu;
        private System.Windows.Forms.Button btnExit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnPlayAgain = new System.Windows.Forms.Button();
            this.btnReturnToMenu = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblMessage
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblMessage.Location = new System.Drawing.Point(12, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(80, 22);
            this.lblMessage.TabIndex = 0;

            // btnPlayAgain
            this.btnPlayAgain.Location = new System.Drawing.Point(90, 65);
            this.btnPlayAgain.Name = "btnPlayAgain";
            this.btnPlayAgain.Size = new System.Drawing.Size(100, 40);
            this.btnPlayAgain.TabIndex = 1;
            this.btnPlayAgain.Text = "Rejouer";
            this.btnPlayAgain.BackColor = System.Drawing.Color.FromArgb(34, 139, 34);
            this.btnPlayAgain.ForeColor = System.Drawing.Color.White;
            this.btnPlayAgain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlayAgain.FlatAppearance.BorderSize = 0;
            this.btnPlayAgain.Click += new System.EventHandler(this.BtnPlayAgain_Click);

            // btnExit
            this.btnExit.Location = new System.Drawing.Point(235, 65);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 40);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Quitter";
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(255, 69, 58);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);

            this.ClientSize = new System.Drawing.Size(350, 100);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnReturnToMenu);
            this.Controls.Add(this.btnPlayAgain);
            this.Controls.Add(this.lblMessage);
            this.Name = "EndGameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
