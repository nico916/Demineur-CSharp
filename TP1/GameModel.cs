using System;
using System.Collections.Generic;

namespace TP1.Models
{
    /// <summary>
    /// Classe repr�sentant le mod�le du jeu, contenant la logique de gestion des bombes, des cellules s�res, des drapeaux et de la victoire ou d�faite.
    /// </summary>
    public class GameModel
    {
        // Propri�t�s publiques pour acc�der � la taille de la grille, au nombre de bombes, et � d'autres donn�es du jeu.
        public int GridSize => gridSize; // Taille de la grille du jeu.
        public int NumberOfBombs => numberOfBombs; // Nombre total de bombes.
        public int[,] BombGrid { get; private set; } // Grille contenant les bombes (1 = bombe, 0 = pas de bombe).
        public int SafeCellsLeft { get; private set; } // Nombre de cellules s�res restantes � r�v�ler.
        public int RevealedCells { get; private set; } // Nombre de cellules d�j� r�v�l�es.
        public bool GameOver { get; set; } // Indique si le jeu est termin� (victoire ou d�faite).
        public bool FirstClick { get; private set; } = true; // Indique si c'est le premier clic du joueur.
        public int FlagsPlaced { get; set; } // Nombre de drapeaux plac�s par le joueur.

        // Variables priv�es pour stocker la taille de la grille, le nombre de bombes, et les cellules d�j� r�v�l�es.
        private int gridSize;
        private int numberOfBombs;
        private HashSet<(int, int)> revealedCellsSet; // Ensemble pour stocker les cellules d�j� r�v�l�es.

        /// <summary>
        /// Constructeur du mod�le de jeu initialisant la grille, les bombes et les cellules s�res.
        /// </summary>
        /// <param name="gridSize">Taille de la grille de jeu (ex: 10x10, 16x16).</param>
        /// <param name="numberOfBombs">Nombre de bombes dans le jeu.</param>
        public GameModel(int gridSize, int numberOfBombs)
        {
            this.gridSize = gridSize;
            this.numberOfBombs = numberOfBombs;
            BombGrid = new int[gridSize, gridSize]; // Initialiser la grille des bombes.
            SafeCellsLeft = gridSize * gridSize - numberOfBombs; // Calculer le nombre de cellules s�res (cellules qui ne sont pas r�v�l�es et qui ne sont pas des bombes.
            RevealedCells = 0;
            GameOver = false;
            revealedCellsSet = new HashSet<(int, int)>();
            FlagsPlaced = 0;
        }

        /// <summary>
        /// Place al�atoirement des bombes sur la grille tout en �vitant les cases adjacentes au premier clic du joueur.
        /// </summary>
        /// <param name="firstClickX">Position X du premier clic du joueur.</param>
        /// <param name="firstClickY">Position Y du premier clic du joueur.</param>
        public void PlaceBombs(int firstClickX, int firstClickY)
        {
            Random random = new Random();
            int bombsPlaced = 0;

            // Liste des cellules � �viter pour placer les bombes (zone autour du premier clic).
            List<(int, int)> cellsToAvoid = new List<(int, int)>
            {
                (firstClickX, firstClickY)
            };

            // Ajouter les cellules autour du premier clic � �viter.
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

            // Placer al�atoirement les bombes sur la grille tout en �vitant les cellules interdites.
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
        /// G�re le premier clic du joueur en lan�ant le placement des bombes.
        /// </summary>
        /// <param name="x">Position X du premier clic.</param>
        /// <param name="y">Position Y du premier clic.</param>
        public void HandleFirstClick(int x, int y)
        {
            if (FirstClick)
            {
                FirstClick = false; // Marquer que le premier clic a �t� effectu�.
                PlaceBombs(x, y); // Placer les bombes en �vitant la zone du premier clic.
            }
        }

        /// <summary>
        /// Compte le nombre de bombes adjacentes � une cellule donn�e.
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
        /// R�v�le les cellules vides adjacentes � la cellule cliqu�e et appelle l'action pour chaque cellule r�v�l�e.
        /// </summary>
        /// <param name="x">Position X de la cellule cliqu�e.</param>
        /// <param name="y">Position Y de la cellule cliqu�e.</param>
        /// <param name="revealCellAction">Action � ex�cuter pour chaque cellule r�v�l�e.</param>
        /// <param name="isClick">Indique si l'action est d�clench�e par un clic de l'utilisateur.</param>
        public void RevealEmptyCells(int x, int y, Action<int, int, int> revealCellAction, bool isClick = false)
        {
            bool[,] visited = new bool[gridSize, gridSize]; // Tableau pour marquer les cellules visit�es.
            int cellsRevealed = RevealEmptyCellsRecursive(x, y, revealCellAction, visited); // R�v�ler les cellules r�cursivement.

            if (isClick)
            {
                SafeCellsLeft -= cellsRevealed; // D�compter les cellules s�res restantes.
            }

            RevealedCells += cellsRevealed; // Ajouter au nombre total de cellules r�v�l�es.
        }

        /// <summary>
        /// M�thode r�cursive pour r�v�ler les cellules vides adjacentes � une cellule donn�e.
        /// </summary>
        /// <param name="x">Position X de la cellule.</param>
        /// <param name="y">Position Y de la cellule.</param>
        /// <param name="revealCellAction">Action � ex�cuter pour chaque cellule r�v�l�e.</param>
        /// <param name="visited">Tableau indiquant si une cellule a d�j� �t� visit�e.</param>
        /// <returns>Le nombre de cellules r�v�l�es.</returns>
        private int RevealEmptyCellsRecursive(int x, int y, Action<int, int, int> revealCellAction, bool[,] visited)
        {
            // V�rifier les limites de la grille et �viter de revisiter les cellules d�j� visit�es.
            if (x < 0 || x >= gridSize || y < 0 || y >= gridSize || visited[x, y] || BombGrid[x, y] == 1)
                return 0;

            // Si la cellule a d�j� �t� r�v�l�e, la sauter.
            if (revealedCellsSet.Contains((x, y)))
                return 0;

            visited[x, y] = true; // Marquer la cellule comme visit�e.
            revealedCellsSet.Add((x, y)); // Ajouter la cellule � l'ensemble des cellules r�v�l�es.

            int adjacentBombs = CountAdjacentBombs(x, y); // Compter les bombes adjacentes.
            int cellsRevealed = 1; // La cellule actuelle est r�v�l�e.

            // Ex�cuter l'action pour r�v�ler la cellule.
            revealCellAction(x, y, adjacentBombs);

            // Si aucune bombe n'est adjacente, r�v�ler les cellules adjacentes r�cursivement.
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

            return cellsRevealed; // Retourner le nombre total de cellules r�v�l�es.
        }

        /// <summary>
        /// V�rifie si le joueur a gagn� en r�v�lant toutes les cellules s�res.
        /// </summary>
        /// <returns>Retourne true si toutes les cellules s�res sont r�v�l�es, sinon false.</returns>
        public bool CheckForWin()
        {
            int totalCells = gridSize * gridSize; // Calculer le nombre total de cellules dans la grille.
            int totalSafeCells = totalCells - numberOfBombs; // Calculer le nombre total de cellules s�res.
            return RevealedCells == totalSafeCells; // V�rifier si toutes les cellules s�res sont r�v�l�es.
        }
    }
}
