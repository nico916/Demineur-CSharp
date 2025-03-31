using System;
using System.Drawing;
using System.Windows.Forms;

namespace TP1
{
    /// <summary>
    /// Formulaire affich� lorsque la partie est termin�e (victoire ou d�faite). Propose les options de rejouer ou quitter.
    /// </summary>
    public partial class EndGameForm : Form
    {
        public EndGameForm(string message, string title, bool showMenuButton)
        {
            InitializeComponent();
            this.Text = title;
            lblMessage.Text = message;
            this.BackColor = Color.FromArgb(28, 28, 28); // Fond sombre
            this.Width = 440;
            this.Height = 170;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Afficher ou masquer le bouton Menu en fonction du param�tre
            btnReturnToMenu.Visible = showMenuButton;
        }

        /// <summary>
        /// Stocke le choix de l'utilisateur sur ce formulaire (Rejouer,Quitter).
        /// </summary>
        public DialogResult UserChoice { get; private set; }

        // G�re le clic sur le bouton Rejouer.
        private void BtnPlayAgain_Click(object sender, EventArgs e)
        {
            UserChoice = DialogResult.Retry;
            this.Close();
        }

        // G�re le clic sur le bouton Quitter.
        private void BtnExit_Click(object sender, EventArgs e)
        {
            UserChoice = DialogResult.Cancel;
            this.Close();
        }
    }
}
