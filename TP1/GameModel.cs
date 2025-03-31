using System;
using System.Collections.Generic;

namespace TP1.Models
{
    /// <summary>
    /// Classe représentant le modèle du jeu, contenant la logique de gestion des bombes, des cellules sûres, des drapeaux et de la victoire ou défaite.
    /// </summary>
    public class GameModel
    {
        // Propriétés publiques pour accéder à la taille de la grille, au nombre de bombes, et à d'autres données du jeu.
        public int GridSize => gridSize; // Taille de la grille du jeu.
        public int NumberOfBombs => numberOfBombs; // Nombre total de bombes.
        public int[,] BombGrid { get; private set; } // Grille contenant les bombes (1 = bombe, 0 = pas de bombe).
        public int SafeCellsLeft { get; private set; } // Nombre de cellules sûres restantes à révéler.
        public int RevealedCells { get; private set; } // Nombre de cellules déjà révélées.
        public bool GameOver { get; set; } // Indique si le jeu est terminé (victoire ou défaite).
        public bool FirstClick { get; private set; } = true; // Indique si c'est le premier clic du joueur.
        public int FlagsPlaced { get; set; } // Nombre de drapeaux placés par le joueur.

        // Variables privées pour stocker la taille de la grille, le nombre de bombes, et les cellules déjà révélées.
        private int gridSize;
        private int numberOfBombs;
        private HashSet<(int, int)> revealedCellsSet; // Ensemble pour stocker les cellules déjà révélées.

        /// <summary>
        /// Constructeur du modèle de jeu initialisant la grille, les bombes et les cellules sûres.
        /// </summary>
        /// <param name="gridSize">Taille de la grille de jeu (ex: 10x10, 16x16).</param>
        /// <param name="numberOfBombs">Nombre de bombes dans le jeu.</param>
        public GameModel(int gridSize, int numberOfBombs)
        {
            this.gridSize = gridSize;
            this.numberOfBombs = numberOfBombs;
            BombGrid = new int[gridSize, gridSize]; // Initialiser la grille des bombes.
            SafeCellsLeft = gridSize * gridSize - numberOfBombs; // Calculer le nombre de cellules sûres (cellules qui ne sont pas révélées et qui ne sont pas des bombes.
            RevealedCells = 0;
            GameOver = false;
            revealedCellsSet = new HashSet<(int, int)>();
            FlagsPlaced = 0;
        }

        /// <summary>
        /// Place aléatoirement des bombes sur la grille tout en évitant les cases adjacentes au premier clic du joueur.
        /// </summary>
        /// <param name="firstClickX">Position X du premier clic du joueur.</param>
        /// <param name="firstClickY">Position Y du premier clic du joueur.</param>
        public void PlaceBombs(int firstClickX, int firstClickY)
        {
            Random random = new Random();
            int bombsPlaced = 0;

            // Liste des cellules à éviter pour placer les bombes (zone autour du premier clic).
            List<(int, int)> cellsToAvoid = new List<(int, int)>
            {
                (firstClickX, firstClickY)
            };

            // Ajouter les cellules autour du premier clic à éviter.
            for (int i = firstClickX - 1; i <= firstClickX + 1; i++)
            {
                for (int j = firstClickY - 1; j <= firstClickY + 1; j++)
                {
                    if (i >= 0 && i < gridSize && j >= 0 && j < gridSize)
                    {
                        cellsToAvoid.Add((i, j));
                    }
                }
            }

            // Placer aléatoirement les bombes sur la grille tout en évitant les cellules interdites.
            while (bombsPlaced < numberOfBombs)
            {
                int x = random.Next(0, gridSize);
                int y = random.Next(0, gridSize);

                if (!cellsToAvoid.Contains((x, y)) && BombGrid[x, y] == 0)
                {
                    BombGrid[x, y] = 1; // Placer une bombe.
                    bombsPlaced++;
                }
            }
        }

        /// <summary>
        /// Gère le premier clic du joueur en lançant le placement des bombes.
        /// </summary>
        /// <param name="x">Position X du premier clic.</param>
        /// <param name="y">Position Y du premier clic.</param>
        public void HandleFirstClick(int x, int y)
        {
            if (FirstClick)
            {
                FirstClick = false; // Marquer que le premier clic a été effectué.
                PlaceBombs(x, y); // Placer les bombes en évitant la zone du premier clic.
            }
        }

        /// <summary>
        /// Compte le nombre de bombes adjacentes à une cellule donnée.
        /// </summary>
        /// <param name="x">Position X de la cellule.</param>
        /// <param name="y">Position Y de la cellule.</param>
        /// <returns>Le nombre de bombes adjacentes.</returns>
        public int CountAdjacentBombs(int x, int y)
        {
            int bombCount = 0;

            // Parcourir les cellules adjacentes et compter les bombes.
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && i < gridSize && j >= 0 && j < gridSize && BombGrid[i, j] == 1)
                    {
                        bombCount++;
                    }
                }
            }
            return bombCount; // Retourner le nombre de bombes adjacentes.
        }

        /// <summary>
        /// Révèle les cellules vides adjacentes à la cellule cliquée et appelle l'action pour chaque cellule révélée.
        /// </summary>
        /// <param name="x">Position X de la cellule cliquée.</param>
        /// <param name="y">Position Y de la cellule cliquée.</param>
        /// <param name="revealCellAction">Action à exécuter pour chaque cellule révélée.</param>
        /// <param name="isClick">Indique si l'action est déclenchée par un clic de l'utilisateur.</param>
        public void RevealEmptyCells(int x, int y, Action<int, int, int> revealCellAction, bool isClick = false)
        {
            bool[,] visited = new bool[gridSize, gridSize]; // Tableau pour marquer les cellules visitées.
            int cellsRevealed = RevealEmptyCellsRecursive(x, y, revealCellAction, visited); // Révéler les cellules récursivement.

            if (isClick)
            {
                SafeCellsLeft -= cellsRevealed; // Décompter les cellules sûres restantes.
            }

            RevealedCells += cellsRevealed; // Ajouter au nombre total de cellules révélées.
        }

        /// <summary>
        /// Méthode récursive pour révéler les cellules vides adjacentes à une cellule donnée.
        /// </summary>
        /// <param name="x">Position X de la cellule.</param>
        /// <param name="y">Position Y de la cellule.</param>
        /// <param name="revealCellAction">Action à exécuter pour chaque cellule révélée.</param>
        /// <param name="visited">Tableau indiquant si une cellule a déjà été visitée.</param>
        /// <returns>Le nombre de cellules révélées.</returns>
        private int RevealEmptyCellsRecursive(int x, int y, Action<int, int, int> revealCellAction, bool[,] visited)
        {
            // Vérifier les limites de la grille et éviter de revisiter les cellules déjà visitées.
            if (x < 0 || x >= gridSize || y < 0 || y >= gridSize || visited[x, y] || BombGrid[x, y] == 1)
                return 0;

            // Si la cellule a déjà été révélée, la sauter.
            if (revealedCellsSet.Contains((x, y)))
                return 0;

            visited[x, y] = true; // Marquer la cellule comme visitée.
            revealedCellsSet.Add((x, y)); // Ajouter la cellule à l'ensemble des cellules révélées.

            int adjacentBombs = CountAdjacentBombs(x, y); // Compter les bombes adjacentes.
            int cellsRevealed = 1; // La cellule actuelle est révélée.

            // Exécuter l'action pour révéler la cellule.
            revealCellAction(x, y, adjacentBombs);

            // Si aucune bombe n'est adjacente, révéler les cellules adjacentes récursivement.
            if (adjacentBombs == 0)
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (i != x || j != y)
                        {
                            cellsRevealed += RevealEmptyCellsRecursive(i, j, revealCellAction, visited);
                        }
                    }
                }
            }

            return cellsRevealed; // Retourner le nombre total de cellules révélées.
        }

        /// <summary>
        /// Vérifie si le joueur a gagné en révélant toutes les cellules sûres.
        /// </summary>
        /// <returns>Retourne true si toutes les cellules sûres sont révélées, sinon false.</returns>
        public bool CheckForWin()
        {
            int totalCells = gridSize * gridSize; // Calculer le nombre total de cellules dans la grille.
            int totalSafeCells = totalCells - numberOfBombs; // Calculer le nombre total de cellules sûres.
            return RevealedCells == totalSafeCells; // Vérifier si toutes les cellules sûres sont révélées.
        }
    }
}
