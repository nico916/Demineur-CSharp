# Minesweeper in C# with Windows Forms

![Language](https://img.shields.io/badge/language-C%23-9B4F96?style=flat-square)
![Platform](https://img.shields.io/badge/platform-Windows-0078D6?style=flat-square)
![Framework](https://img.shields.io/badge/framework-WinForms-blue?style=flat-square)
![IDE](https://img.shields.io/badge/IDE-Visual%20Studio-5C2D91?style=flat-square)

A classic implementation of the Minesweeper game, developed in C# using Windows Forms. This project was created as an academic exercise to apply and demonstrate fundamental software development principles, including Object-Oriented Programming and the Model-View-Controller (MVC) architectural pattern.

## Table of Contents

- [About The Project](#about-the-project)
- [Key Features](#key-features)
- [Built With](#built-with)
- [Getting Started](#getting-started)
- [Technical Deep Dive](#technical-deep-dive)
- [Screenshots](#screenshots)
- [Future Improvements](#future-improvements)
- [License](#license)

## About The Project

This project is a fully functional desktop version of Minesweeper, built from the ground up in C#. The primary goal was to create a clean, well-structured application within a one-week timeframe, focusing on separating the game's logic from its user interface.

## Key Features

-   **Graphical User Interface**: Built with the native Windows Forms framework.
-   **Multiple Difficulty Levels**: Includes Easy, Medium, and Hard modes, each with a different grid size and bomb count.
-   **Flagging System**: Players can right-click to place flags on suspected bomb locations.
-   **Safe First Click**: The game ensures that the player's first click is never on a bomb.
-   **Win/Loss Detection**: Clear end-game states with appropriate messages.
-   **Color-Coded Number Display**: Adjacent bomb counts are colored for better readability.

## Built With

-   **C#**
-   **.NET Framework** (with Windows Forms)
-   **Visual Studio**

## Getting Started

To run this project, you will need Visual Studio with the .NET desktop development workload installed.

1.  **Clone the repository:**
    ```sh
    git clone https://github.com/nico916/Demineur-CSharp.git
    ```
2.  **Open the solution:**
    Navigate to the project folder and open the `.sln` file with Visual Studio.
3.  **Run the application:**
    Press `F5` or click the "Start" button in Visual Studio to compile and run the project.

## Technical Deep Dive

The project's architecture is based on the **Model-View-Controller (MVC)** pattern to ensure a clean separation of concerns:

-   **Model**: Contains all the core game logic, such as the grid state, bomb placement, and cell status. It has no knowledge of the user interface.
-   **View** (`GameView.cs`, `MenuForm.cs`): Manages the entire graphical interface using Windows Forms. It displays the game board and updates based on data from the Model.
-   **Controller**: Acts as the intermediary. It handles user input (e.g., button clicks), updates the Model accordingly, and then instructs the View to refresh its display.

This structure makes the code more organized, easier to debug, and more scalable for future enhancements.

## Future Improvements

Given more time, the following features could be added to enhance the game:

-   A timer to track and score game sessions.
-   A high-score system to save the best times for each difficulty.
-   Visual themes to customize the look of the game board.

## License

Distributed under the MIT License. See `LICENSE` file for more information.
