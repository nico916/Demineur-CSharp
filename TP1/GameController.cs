using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TP1.Models;
using TP1.Views;

namespace TP1.Controllers
{
    /// <summary>
    /// Classe GameController responsable de la gestion de la logique du jeu. Elle gère les interactions entre la vue (GameView) et le modèle (GameModel).
    /// </summary>
    public class GameController
    {
        private GameModel model;  // Modèle du jeu contenant les données et la logique.
        private GameView view;    // Vue du jeu représentant l'interface utilisateur.
        private Label flagsRemainingLabel; // Label affichant le nombre de drapeaux restants.

        /// <summary>
        /// Constructeur qui initialise le contrôleur avec la vue et le modèle donnés.
        /// </summary>
        public GameController(GameView gameView, GameModel gameModel)
        {
            this.view = gameView;
            this.model = gameModel;

            // Initialiser les composants nécessaires à la vue.
            Initialize();
        }

        /// <summary>
        /// Méthode d'initialisation qui attache les événements de clic aux boutons de la grille.
        /// </summary>
        private void Initialize()
        {
            foreach (Button btn in view.Buttons)
            {
                btn.Click += OnButtonClick;  // Associer l'événement de clic gauche
                btn.MouseUp += OnButtonRightClick;  // Associer l'événement de clic droit
            }

            // Révéler les bombes
            RevealBombs();
        }

        /// <summary>
        /// Définit le label affichant le nombre de drapeaux restants et l'initialise.
        /// </summary>
        /// <param name="label">Label à mettre à jour avec le nombre de drapeaux restants.</param>
        public void SetFlagsRemainingLabel(Label label)
        {
            flagsRemainingLabel = label;  // Assigner le label à utiliser
            UpdateFlagsRemaining();  // Initialiser l'affichage du nombre de drapeaux restants
        }

        /// <summary>
        /// Met à jour l'affichage du nombre de drapeaux restants en fonction des bombes placées.
        /// </summary>
        private void UpdateFlagsRemaining()
        {
            // Calculer le nombre de drapeaux restants.
            int remainingFlags = model.NumberOfBombs - model.FlagsPlaced;
            flagsRemainingLabel.Text = $"Drapeaux restants : {remainingFlags}";
        }

        /// <summary>
        /// Gestion du clic gauche sur un bouton. Révèle le contenu de la cellule et vérifie les conditions de victoire ou de défaite.
        /// </summary>
        private void OnButtonClick(object sender, EventArgs e)
        {
            CustomButton clickedButton = sender as CustomButton;
            Point position = (Point)clickedButton.Tag;  // Obtenir la position de la case cliquée

            // Enregistrer le premier clic pour placer les bombes en évitant la première case cliquée (puis ses voisins).
            model.HandleFirstClick(position.X, position.Y);

            // Si le jeu n'est pas terminé
            if (!model.GameOver)
            {
                // Si une bombe est trouvée
                if (model.BombGrid[position.X, position.Y] == 1)
                {
                    view.UpdateButtonColor(clickedButton, Color.Red);  // Marquer la case en rouge
                    view.DisplayMessage("Bombe ! Vous avez perdu.", "Game Over");  // Afficher un message de défaite
                    model.GameOver = true;
                    DisableAllButtons();  // Désactiver tous les boutons

                    // Afficher le formulaire de fin de partie
                    EndGameForm endGameForm = new EndGameForm("Vous avez perdu. Que voulez-vous faire ?", "Game Over", false);
                    endGameForm.ShowDialog();

                    // Gérer le choix de l'utilisateur à la fin de la partie
                    HandleEndGameResult(endGameForm.UserChoice);
                }
                else
                {
                    // Révéler les cellules adjacentes si elles ne contiennent pas de bombe
                    model.RevealEmptyCells(position.X, position.Y, (x, y, adjacentBombs) =>
                    {
                        CustomButton btn = view.Buttons.First(b => ((Point)b.Tag).X == x && ((Point)b.Tag).Y == y) as CustomButton;
                        view.RevealButton(btn, adjacentBombs);  // Révéler le bouton correspondant
                    }, true);

                    // Si toutes les cellules sûres sont révélées, le joueur gagne
                    if (model.CheckForWin())
                    {
                        view.DisplayMessage("Félicitations ! Vous avez gagné.", "Victoire");
                        model.GameOver = true;
                        DisableAllButtons();  // Désactiver tous les boutons

                        // Afficher le formulaire de fin de partie
                        EndGameForm endGameForm = new EndGameForm("Vous avez gagné ! Que voulez-vous faire ?", "Victoire", false);
                        endGameForm.ShowDialog();

                        // Gérer le choix de l'utilisateur à la fin de la partie
                        HandleEndGameResult(endGameForm.UserChoice);
                    }
                }
            }
        }

        /// <summary>
        /// Gestion du clic droit sur un bouton. Permet de placer ou retirer un drapeau (affichage de "D").
        /// </summary>
        private void OnButtonRightClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CustomButton clickedButton = sender as CustomButton;
                Point position = (Point)clickedButton.Tag;

                // Ne pas placer de drapeau si le jeu est terminé
                if (model.GameOver) return;

                // Vérifier si un drapeau (représenté par "D") est déjà présent sur la case
                if (clickedButton.Text == "D")
                {
                    clickedButton.Text = "";  // Retirer le drapeau
                    model.FlagsPlaced--;  // Mettre à jour le nombre de drapeaux placés
                }
                else if (model.FlagsPlaced < model.NumberOfBombs)  // Limiter le nombre de drapeaux
                {
                    clickedButton.Text = "D";  // Placer un drapeau
                    clickedButton.ForeColor = Color.Red;  // Couleur rouge pour "D"
                    clickedButton.Font = new Font("Arial", 14, FontStyle.Bold);  // Ajuster la taille et le style
                    model.FlagsPlaced++;  // Mettre à jour le nombre de drapeaux placés
                }

                // Mettre à jour l'affichage du nombre de drapeaux restants
                UpdateFlagsRemaining();
            }
        }

        /// <summary>
        /// Gère le choix de l'utilisateur à la fin du jeu (rejouer ou quitter).
        /// </summary>
        private void HandleEndGameResult(DialogResult result)
        {
            if (result == DialogResult.Retry)
            {
                // Redémarrer le jeu
                Application.Restart();
            }
            else if (result == DialogResult.Yes)
            {
                // Retourner au menu principal
                Application.OpenForms[0].Show();
                Application.OpenForms[1].Close();  // Fermer le jeu
            }
            else if (result == DialogResult.Cancel)
            {
                // Quitter l'application
                Application.Exit();
            }
        }

        /// <summary>
        /// Révèle toutes les bombes (non implémenté ici, laissé en commentaire).
        /// </summary>
        private void RevealBombs()
        {
            var bombButtons = view.Buttons
                .Where(b => model.BombGrid[((Point)b.Tag).X, ((Point)b.Tag).Y] == 1);

            // view.ShowBombs(bombButtons);
        }

        /// <summary>
        /// Désactive tous les boutons du jeu, utilisé lorsque le jeu est terminé.
        /// </summary>
        private void DisableAllButtons()
        {
            foreach (Button btn in view.Buttons)
            {
                btn.Enabled = false;
            }
        }
    }
}
