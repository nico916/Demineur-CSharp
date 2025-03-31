using System;
using System.Drawing;
using System.Windows.Forms;

namespace TP1
{
    /// <summary>
    /// Classe repr�sentant le formulaire du menu principal du jeu D�mineur.
    /// Cette interface permet � l'utilisateur de choisir un niveau de difficult� (et donc puis de jouer) ou de quitter le jeu.
    /// </summary>
    public partial class MenuForm : Form
    {
        // D�claration des contr�les du formulaire
        private Label lblTitle; // Titre du jeu
        private Button btnFacile;
        private Button btnMoyen;
        private Button btnDifficile;
        private Button btnQuitter;
        private TableLayoutPanel tableLayoutPanel; // Disposition des contr�les dans une grille

        /// <summary>
        /// Constructeur du formulaire MenuForm qui initialise les composants.
        /// </summary>
        public MenuForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// M�thode qui initialise tous les composants de l'interface du menu.
        /// Cr�e les boutons, le titre, et les place dans une disposition organis�e.
        /// </summary>
        private void InitializeComponent()
        {
            // D�finir les propri�t�s de la fen�tre principale du menu
            this.Text = "Menu Principal";
            this.Size = new System.Drawing.Size(800, 600); // Taille de la fen�tre
            this.StartPosition = FormStartPosition.CenterScreen; // Positionner au centre de l'�cran
            this.BackColor = Color.FromArgb(34, 34, 34); // Couleur de fond

            // Cr�er une table pour organiser les contr�les (titre et boutons)
            tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill, // Remplir toute la fen�tre
                RowCount = 3, // Trois lignes (titre, boutons, espace)
                ColumnCount = 1, // Une seule colonne
                BackColor = Color.FromArgb(34, 34, 34),
                Padding = new Padding(20) // Espacement autour du contenu
            };

            // Ajouter des lignes avec des proportions pour organiser les sections
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20)); // Titre
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60)); // Boutons
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20)); // Espace vide

            // Titre du jeu
            lblTitle = new Label
            {
                Text = "Jeu de D�mineur",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill, // Remplir l'espace assign�
                TextAlign = ContentAlignment.MiddleCenter // Centrer le texte
            };
            tableLayoutPanel.Controls.Add(lblTitle, 0, 0); // Ajouter le titre � la premi�re ligne

            // Panneau pour contenir les boutons de s�lection de difficult� et le bouton Quitter
            TableLayoutPanel buttonPanel = new TableLayoutPanel
            {
                ColumnCount = 1, // Une seule colonne pour les boutons
                RowCount = 4, // Quatre lignes pour les boutons (Facile, Moyen, Difficile, Quitter)
                Dock = DockStyle.Fill, // Remplir l'espace attribu�
                Padding = new Padding(10) // Ajouter de l'espace autour des boutons
            };

            // Ajuster les proportions des lignes pour donner plus d'espace aux boutons
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25)); // Facile
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25)); // Moyen
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25)); // Difficile
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25)); // Quitter

            // Ajouter des marges pour espacer davantage les boutons
            Padding buttonMargin = new Padding(0, 10, 0, 10);

            // Bouton pour d�marrer le niveau Facile
            btnFacile = new Button
            {
                Text = "Facile",
                Font = new Font("Arial", 16, FontStyle.Bold),
                BackColor = Color.FromArgb(50, 150, 50), // Couleur de fond verte
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill, // Remplir l'espace assign�
                Margin = buttonMargin // Ajouter de la marge
            };
            btnFacile.FlatAppearance.BorderSize = 0; // Supprimer les bordures
            btnFacile.Click += (sender, e) => D�marrerJeu(10, 10); // Associer l'�v�nement de clic
            buttonPanel.Controls.Add(btnFacile, 0, 0); // Ajouter le bouton au panneau

            // Bouton pour d�marrer le niveau Moyen
            btnMoyen = new Button
            {
                Text = "Moyen",
                Font = new Font("Arial", 16, FontStyle.Bold),
                BackColor = Color.FromArgb(50, 150, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill, // Remplir l'espace assign�
                Margin = buttonMargin // Ajouter de la marge
            };
            btnMoyen.FlatAppearance.BorderSize = 0; // Supprimer les bordures
            btnMoyen.Click += (sender, e) => D�marrerJeu(16, 40); // Associer l'�v�nement de clic
            buttonPanel.Controls.Add(btnMoyen, 0, 1); // Ajouter le bouton au panneau

            // Bouton pour d�marrer le niveau Difficile
            btnDifficile = new Button
            {
                Text = "Difficile",
                Font = new Font("Arial", 16, FontStyle.Bold),
                BackColor = Color.FromArgb(50, 150, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill, // Remplir l'espace assign�
                Margin = buttonMargin // Ajouter de la marge
            };
            btnDifficile.FlatAppearance.BorderSize = 0; // Supprimer les bordures
            btnDifficile.Click += (sender, e) => D�marrerJeu(22, 99); // Associer l'�v�nement de clic
            buttonPanel.Controls.Add(btnDifficile, 0, 2); // Ajouter le bouton au panneau

            // Bouton pour quitter le jeu
            btnQuitter = new Button
            {
                Text = "Quitter",
                Font = new Font("Arial", 16, FontStyle.Bold),
                BackColor = Color.FromArgb(200, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill, // Remplir l'espace assign�
                Margin = buttonMargin // Ajouter de la marge
            };
            btnQuitter.FlatAppearance.BorderSize = 0; // Supprimer les bordures
            btnQuitter.Click += new EventHandler(BtnQuitter_Click); // Associer l'�v�nement de clic pour quitter
            buttonPanel.Controls.Add(btnQuitter, 0, 3); // Ajouter le bouton au panneau

            // Ajouter le panneau des boutons � la deuxi�me ligne de la table principale
            tableLayoutPanel.Controls.Add(buttonPanel, 0, 1);

            // Ajouter la table principale � la fen�tre
            this.Controls.Add(tableLayoutPanel);
        }

        /// <summary>
        /// M�thode pour d�marrer une nouvelle partie selon la difficult� choisie.
        /// Cache le menu et ouvre le formulaire de jeu.
        /// </summary>
        /// <param name="gridSize">Taille de la grille du jeu (10, 16, ou 22)</param>
        /// <param name="numberOfBombs">Nombre de bombes dans le jeu selon la difficult�</param>
        private void D�marrerJeu(int gridSize, int numberOfBombs)
        {
            this.Hide(); // Masquer le menu
            Form1 gameForm = new Form1(gridSize, numberOfBombs); // Cr�er le formulaire de jeu avec la taille et le nombre de bombes
            gameForm.ShowDialog(); // Afficher le jeu
            this.Close(); // Fermer le menu apr�s la fin du jeu
        }

        /// <summary>
        /// M�thode pour fermer l'application lorsque le bouton Quitter est cliqu�.
        /// </summary>
        private void BtnQuitter_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Quitter l'application
        }
    }
}
