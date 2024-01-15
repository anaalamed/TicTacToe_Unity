using System;
using MoonActive.Scripts;
using UnityEngine;

public class InMemoryGameSaver : GameSaver
{
    private GameState savedGameState;


    GameState GameSaver.LoadGame()
    {
        if (savedGameState == null)
        {
            Debug.LogWarning("The game state was never saved");
            throw new InvalidOperationException(); 
        }

        return this.savedGameState;
    }

    void GameSaver.SaveGame(GameState gameState)
    {
        // Iniatilize state for the first time
        if (this.savedGameState == null)
        {
            this.savedGameState = new GameState
            {
                Board = new PlayerType?[gameState.Board.GetLength(0), gameState.Board.GetLength(1)],
                CurrentPlayer = gameState.CurrentPlayer
            };
        }

        // TODO consider both assignment 
        this.savedGameState.CurrentPlayer = gameState.CurrentPlayer;
        for (int i = 0; i < gameState.Board.GetLength(0); i++)
        {
            for (int j = 0; j < gameState.Board.GetLength(1); j++)
            {
                this.savedGameState.Board[i, j] = gameState.Board[i, j];
            }
        }
    }
}