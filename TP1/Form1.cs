using System;
using System.Drawing;
using System.Windows.Forms;
using TP1.Controllers;
using TP1.Models;
using TP1.Views;

namespace TP1
{
    /// <summary>
    /// Formulaire principal où se déroule la partie. Gère l'interface et l'affichage des drapeaux restants.
    /// </summary>
    public partial class Form1 : Form
    {
        private GameController controller;
        private Label difficultyLabel;
        private Label flagsRemainingLabel;

        public Form1(int gridSize, int numberOfBombs)
        {
            InitializeComponent();
            StartGame(gridSize, numberOfBombs);

            // Empêcher la redimension de la fenêtre.
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Empêcher la redimension.
            this.MaximizeBox = false; // Désactiver le bouton d'agrandissement.
            this.MinimizeBox = false; // Désactiver le bouton de minimisation.
            this.StartPosition = FormStartPosition.CenterScreen; // Centrer la fenêtre à l'ouverture.
            this.SizeGripStyle = SizeGripStyle.Hide; // Masquer la zone de redimensionnement.
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void StartGame(int gridSize, int numberOfBombs)
        {
            GameModel model = new GameModel(gridSize, numberOfBombs);

            // Taille des cases selon la taille de la grille.
            int cellSize = gridSize == 22 ? 30 : 40;
            GameView view = new GameView(gridSize, cellSize);

            controller = new GameController(view, model);

            this.Controls.Add(view.TablePanel);

            // Ajuster la taille de la fenêtre.
            this.Size = new Size(view.TablePanel.Width + 200, view.TablePanel.Height + 100);

            // Ajouter le label pour afficher le niveau de difficulté.
            difficultyLabel = new Label
            {
                Text = gridSize == 22 ? "Difficile" : gridSize == 16 ? "Moyen" : "Facile",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(34, 34, 34),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(180, 50),
                Location = new Point(view.TablePanel.Width + 10, 20)
            };
            this.Controls.Add(difficultyLabel);

            // Label pour afficher le nombre de drapeaux restants.
            flagsRemainingLabel = new Label
            {
                Text = $"Drapeaux restants : {numberOfBombs}",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Yellow,
                BackColor = Color.FromArgb(34, 34, 34),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(180, 50),
                Location = new Point(view.TablePanel.Width + 10, 80)
            };
            this.Controls.Add(flagsRemainingLabel);

            controller.SetFlagsRemainingLabel(flagsRemainingLabel);
        }
    }
}
