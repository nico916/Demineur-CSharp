using System;
using System.Drawing;
using System.Windows.Forms;

namespace TP1
{
    /// <summary>
    /// Formulaire personnalisable pour afficher un message avec un bouton OK.
    /// </summary>
    public class CustomMessageForm : Form
    {
        public CustomMessageForm(string message, string title)
        {
            InitializeComponent(message, title);
        }

        // Initialisation des composants du formulaire.
        private void InitializeComponent(string message, string title)
        {
            this.Text = title;
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(50, 50, 50);

            // Texte du message.
            Label lblMessage = new Label
            {
                Text = message,
                ForeColor = Color.White,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Size = new Size(360, 60),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 30)
            };
            this.Controls.Add(lblMessage);

            // Bouton Suivant pour fermer le formulaire.
            Button btnOk = new Button
            {
                Text = "Suivant",
                Size = new Size(100, 40),
                Location = new Point(150, 100),
                BackColor = Color.FromArgb(34, 139, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOk.Click += (sender, e) => this.Close();
            this.Controls.Add(btnOk);

            // Bordure stylisée autour du formulaire.
            this.Paint += (sender, e) =>
            {
                using (Pen pen = new Pen(Color.Turquoise, 5))
                {
                    e.Graphics.DrawRectangle(pen, this.DisplayRectangle);
                }
            };
        }
    }
}
