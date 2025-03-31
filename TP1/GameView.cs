using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TP1.Models;

namespace TP1.Views
{
    /// <summary>
    /// Classe représentant la vue principale du jeu. Elle gère l'affichage de la grille de jeu et des boutons représentant chaque cellule.
    /// </summary>
    public class GameView : Form
    {
        // Liste des boutons représentant les cellules de la grille.
        public List<Button> Buttons { get; private set; }

        // Panneau de la grille où les boutons sont placés.
        public TableLayoutPanel TablePanel { get; private set; }

        // Dictionnaire pour stocker les couleurs originales des boutons.
        private Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();

        /// <summary>
        /// Constructeur pour initialiser la vue avec une taille de grille et de cellule donnée.
        /// </summary>
        /// <param name="gridSize">Taille de la grille (nombre de lignes et de colonnes).</param>
        /// <param name="cellSize">Taille des cellules (pixels).</param>
        public GameView(int gridSize, int cellSize)
        {
            Buttons = new List<Button>();
            InitializeGameBoard(gridSize, cellSize); // Créer la grille de jeu.
            this.BackColor = Color.FromArgb(28, 28, 28); // Couleur de fond de la fenêtre.
            this.StartPosition = FormStartPosition.CenterScreen; // Positionner la fenêtre au centre de l'écran.
        }

        /// <summary>
        /// Méthode pour initialiser la grille de jeu en plaçant les boutons représentant les cellules.
        /// </summary>
        /// <param name="gridSize">Taille de la grille (nombre de lignes et de colonnes).</param>
        /// <param name="cellSize">Taille des cellules (pixels).</param>
        private void InitializeGameBoard(int gridSize, int cellSize)
        {
            // Créer le panneau contenant la grille de jeu.
            TablePanel = new TableLayoutPanel
            {
                RowCount = gridSize,
                ColumnCount = gridSize,
                Size = new Size(gridSize * cellSize, gridSize * cellSize), // Taille du panneau en fonction de la taille de la grille.
                Location = new Point(20, 20), // Position du panneau dans la fenêtre.
                BackColor = Color.FromArgb(50, 50, 50) // Couleur de fond du panneau.
            };

            // Ajouter des colonnes et des lignes de la taille spécifiée.
            for (int i = 0; i < gridSize; i++)
            {
                TablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, cellSize));
                TablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, cellSize));
            }

            // Boucle pour ajouter des boutons pour chaque cellule de la grille.
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    // Créer un bouton personnalisé pour chaque cellule.
                    CustomButton btn = new CustomButton
                    {
                        Size = new Size(cellSize, cellSize), // Taille du bouton.
                        Margin = new Padding(1), // Ajouter une marge autour du bouton.
                        ForeColor = Color.White,
                        Font = new Font("Arial", 12, FontStyle.Bold),
                        FlatStyle = FlatStyle.Flat,
                        Tag = new Point(i, j) // Stocker les coordonnées de la cellule dans la propriété Tag.
                    };

                    btn.FlatAppearance.BorderSize = 0; // Pas de bordure visible.

                    // Déterminer la couleur originale en fonction de la position de la cellule (alterner une couleur sur 2).
                    Color originalColor = (i + j) % 2 == 0 ? Color.PaleTurquoise : Color.SteelBlue;
                    btn.BackColor = originalColor;

                    // Stocker la couleur originale du bouton dans le dictionnaire.
                    originalColors[btn] = originalColor;

                    // Ajouter le bouton à la liste des boutons et à la grille.
                    Buttons.Add(btn);
                    TablePanel.Controls.Add(btn, j, i); // Ajouter le bouton au bon emplacement dans la grille.
                }
            }

            // Ajouter le panneau contenant la grille à la fenêtre.
            this.Controls.Add(TablePanel);
        }

        /// <summary>
        /// Méthode pour mettre à jour la couleur d'un bouton donné.
        /// </summary>
        /// <param name="btn">Bouton dont la couleur doit être modifiée.</param>
        /// <param name="color">Nouvelle couleur à appliquer au bouton.</param>
        public void UpdateButtonColor(Button btn, Color color)
        {
            btn.BackColor = color; // Appliquer la nouvelle couleur au bouton.
        }

        /// <summary>
        /// Affiche un message de victoire ou de défaite dans un formulaire stylisé.
        /// </summary>
        /// <param name="message">Le texte du message à afficher.</param>
        /// <param name="title">Le titre de la fenêtre de message.</param>
        public void DisplayMessage(string message, string title)
        {
            // Créer un nouveau formulaire pour afficher le message.
            Form messageForm = new Form
            {
                Text = title,
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterScreen, // Centrer la fenêtre sur l'écran.
                BackColor = Color.FromArgb(34, 34, 34), // Couleur de fond sombre.
                FormBorderStyle = FormBorderStyle.FixedDialog, // Bordure fixe sans agrandissement possible.
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Créer un label pour afficher le message de victoire ou de défaite.
            Label lblMessage = new Label
            {
                Text = message,
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true, // Ajuster automatiquement la taille du label.
                TextAlign = ContentAlignment.MiddleCenter, // Centrer le texte.
                Location = new Point(20, 30), // Position du label dans la fenêtre.
                Size = new Size(360, 60) // Taille du label.
            };
            messageForm.Controls.Add(lblMessage); // Ajouter le label au formulaire.

            // Créer un bouton "Suivant" pour fermer la fenêtre.
            Button btnSuivant = new Button
            {
                Text = "Suivant",
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK // Action pour fermer la fenêtre au clic.
            };
            btnSuivant.FlatAppearance.BorderSize = 0; // Pas de bordure visible.

            // Ajuster la position du bouton pour le centrer dans la fenêtre.
            btnSuivant.Location = new Point((messageForm.ClientSize.Width - btnSuivant.Width) / 2, 100);
            messageForm.Controls.Add(btnSuivant); // Ajouter le bouton au formulaire.

            // Configurer le bouton "Suivant" comme bouton de validation (ferme la fenêtre sur appui d'Entrée).
            messageForm.AcceptButton = btnSuivant;

            // Afficher la fenêtre de message en mode dialogue.
            messageForm.ShowDialog();
        }

        /// <summary>
        /// Révèle une cellule du jeu en affichant le nombre de bombes adjacentes.
        /// </summary>
        /// <param name="btn">Bouton représentant la cellule à révéler.</param>
        /// <param name="adjacentBombs">Nombre de bombes adjacentes à cette cellule.</param>
        public void RevealButton(CustomButton btn, int adjacentBombs)
        {
            btn.BombCount = adjacentBombs; // Enregistrer le nombre de bombes adjacentes.
            btn.Enabled = false; // Désactiver le bouton une fois révélé.

            // Restaurer la couleur d'origine du bouton en fonction de la couleur de base.
            if (originalColors[btn] == Color.PaleTurquoise)
            {
                btn.BackColor = Color.DarkOrange; // Changer la couleur du bouton révélé.
            }
            else if (originalColors[btn] == Color.SteelBlue)
            {
                btn.BackColor = Color.Moccasin; // Changer la couleur du bouton révélé.
            }

            // Ajouter une bordure colorée pour indiquer que le bouton a été révélé.
            btn.FlatAppearance.BorderColor = Color.FromArgb(64, 224, 208);
        }

        /// <summary>
        /// Définit la couleur du texte affiché sur le bouton en fonction du nombre de bombes adjacentes.
        /// </summary>
        /// <param name="button">Le bouton dont la couleur de texte doit être modifiée.</param>
        /// <param name="bombCount">Le nombre de bombes adjacentes à la cellule.</param>
        private void SetButtonTextColor(Button button, int bombCount)
        {
            // Définir la couleur du texte en fonction du nombre de bombes adjacentes.
            Color textColor;
            switch (bombCount)
            {
                case 1:
                    textColor = Color.FromArgb(0, 0, 255); // Bleu pour 1 bombe adjacente.
                    break;
                case 2:
                    textColor = Color.FromArgb(0, 128, 0); // Vert pour 2 bombes adjacentes.
                    break;
                case 3:
                    textColor = Color.FromArgb(255, 0, 0); // Rouge pour 3 bombes adjacentes.
                    break;
                case 4:
                    textColor = Color.FromArgb(128, 0, 128); // Violet pour 4 bombes adjacentes.
                    break;
                default:
                    textColor = Color.FromArgb(255, 105, 180); // Rose pour plus de 4 bombes adjacentes.
                    break;
            }

            // Appliquer la couleur du texte au bouton.
            button.ForeColor = textColor;
        }
    }
}
