using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TP1.Models;
using TP1.Views;

namespace TP1.Controllers
{
    /// <summary>
    /// Classe GameController responsable de la gestion de la logique du jeu. Elle g�re les interactions entre la vue (GameView) et le mod�le (GameModel).
    /// </summary>
    public class GameController
    {
        private GameModel model;  // Mod�le du jeu contenant les donn�es et la logique.
        private GameView view;    // Vue du jeu repr�sentant l'interface utilisateur.
        private Label flagsRemainingLabel; // Label affichant le nombre de drapeaux restants.

        /// <summary>
        /// Constructeur qui initialise le contr�leur avec la vue et le mod�le donn�s.
        /// </summary>
        public GameController(GameView gameView, GameModel gameModel)
        {
            this.view = gameView;
            this.model = gameModel;

            // Initialiser les composants n�cessaires � la vue.
            Initialize();
        }

        /// <summary>
        /// M�thode d'initialisation qui attache les �v�nements de clic aux boutons de la grille.
        /// </summary>
        private void Initialize()
        {
            foreach (Button btn in view.Buttons)
            {
                btn.Click += OnButtonClick;  // Associer l'�v�nement de clic gauche
                btn.MouseUp += OnButtonRightClick;  // Associer l'�v�nement de clic droit
            }

            // R�v�ler les bombes
            RevealBombs();
        }

        /// <summary>
        /// D�finit le label affichant le nombre de drapeaux restants et l'initialise.
        /// </summary>
        /// <param name="label">Label � mettre � jour avec le nombre de drapeaux restants.</param>
        public void SetFlagsRemainingLabel(Label label)
        {
            flagsRemainingLabel = label;  // Assigner le label � utiliser
            UpdateFlagsRemaining();  // Initialiser l'affichage du nombre de drapeaux restants
        }

        /// <summary>
        /// Met � jour l'affichage du nombre de drapeaux restants en fonction des bombes plac�es.
        /// </summary>
        private void UpdateFlagsRemaining()
        {
            // Calculer le nombre de drapeaux restants.
            int remainingFlags = model.NumberOfBombs - model.FlagsPlaced;
            flagsRemainingLabel.Text = $"Drapeaux restants : {remainingFlags}";
        }

        /// <summary>
        /// Gestion du clic gauche sur un bouton. R�v�le le contenu de la cellule et v�rifie les conditions de victoire ou de d�faite.
        /// </summary>
        private void OnButtonClick(object sender, EventArgs e)
        {
            CustomButton clickedButton = sender as CustomButton;
            Point position = (Point)clickedButton.Tag;  // Obtenir la position de la case cliqu�e

            // Enregistrer le premier clic pour placer les bombes en �vitant la premi�re case cliqu�e (puis ses voisins).
            model.HandleFirstClick(position.X, position.Y);

            // Si le jeu n'est pas termin�
            if (!model.GameOver)
            {
                // Si une bombe est trouv�e
                if (model.BombGrid[position.X, position.Y] == 1)
                {
                    view.UpdateButtonColor(clickedButton, Color.Red);  // Marquer la case en rouge
                    view.DisplayMessage("Bombe ! Vous avez perdu.", "Game Over");  // Afficher un message de d�faite
                    model.GameOver = true;
                    DisableAllButtons();  // D�sactiver tous les boutons

                    // Afficher le formulaire de fin de partie
                    EndGameForm endGameForm = new EndGameForm("Vous avez perdu. Que voulez-vous faire ?", "Game Over", false);
                    endGameForm.ShowDialog();

                    // G�rer le choix de l'utilisateur � la fin de la partie
                    HandleEndGameResult(endGameForm.UserChoice);
                }
                else
                {
                    // R�v�ler les cellules adjacentes si elles ne contiennent pas de bombe
                    model.RevealEmptyCells(position.X, position.Y, (x, y, adjacentBombs) =>
                    {
                        CustomButton btn = view.Buttons.First(b => ((Point)b.Tag).X == x && ((Point)b.Tag).Y == y) as CustomButton;
                        view.RevealButton(btn, adjacentBombs);  // R�v�ler le bouton correspondant
                    }, true);

                    // Si toutes les cellules s�res sont r�v�l�es, le joueur gagne
                    if (model.CheckForWin())
                    {
                        view.DisplayMessage("F�licitations ! Vous avez gagn�.", "Victoire");
                        model.GameOver = true;
                        DisableAllButtons();  // D�sactiver tous les boutons

                        // Afficher le formulaire de fin de partie
                        EndGameForm endGameForm = new EndGameForm("Vous avez gagn� ! Que voulez-vous faire ?", "Victoire", false);
                        endGameForm.ShowDialog();

                        // G�rer le choix de l'utilisateur � la fin de la partie
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

                // Ne pas placer de drapeau si le jeu est termin�
                if (model.GameOver) return;

                // V�rifier si un drapeau (repr�sent� par "D") est d�j� pr�sent sur la case
                if (clickedButton.Text == "D")
                {
                    clickedButton.Text = "";  // Retirer le drapeau
                    model.FlagsPlaced--;  // Mettre � jour le nombre de drapeaux plac�s
                }
                else if (model.FlagsPlaced < model.NumberOfBombs)  // Limiter le nombre de drapeaux
                {
                    clickedButton.Text = "D";  // Placer un drapeau
                    clickedButton.ForeColor = Color.Red;  // Couleur rouge pour "D"
                    clickedButton.Font = new Font("Arial", 14, FontStyle.Bold);  // Ajuster la taille et le style
                    model.FlagsPlaced++;  // Mettre � jour le nombre de drapeaux plac�s
                }

                // Mettre � jour l'affichage du nombre de drapeaux restants
                UpdateFlagsRemaining();
            }
        }

        /// <summary>
        /// G�re le choix de l'utilisateur � la fin du jeu (rejouer ou quitter).
        /// </summary>
        private void HandleEndGameResult(DialogResult result)
        {
            if (result == DialogResult.Retry)
            {
                // Red�marrer le jeu
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
        /// R�v�le toutes les bombes (non impl�ment� ici, laiss� en commentaire).
        /// </summary>
        private void RevealBombs()
        {
            var bombButtons = view.Buttons
                .Where(b => model.BombGrid[((Point)b.Tag).X, ((Point)b.Tag).Y] == 1);

            // view.ShowBombs(bombButtons);
        }

        /// <summary>
        /// D�sactive tous les boutons du jeu, utilis� lorsque le jeu est termin�.
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
