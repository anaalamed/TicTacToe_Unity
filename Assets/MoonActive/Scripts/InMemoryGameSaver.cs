using System;
using MoonActive.Scripts;
using UnityEngine;

/// <summary>
/// Class handling storing game state in memory
/// </summary>
public class InMemoryGameSaver : GameSaver
{
    private GameState savedGameState;

    GameState GameSaver.LoadGame()
    {
        if (savedGameState == null)
        {
            throw new InvalidOperationException("The game state wasn't saved");
        }

        return this.savedGameState;
    }

    void GameSaver.SaveGame(GameState gameState)
    {
        var board = new PlayerType?[gameState.Board.GetLength(0), gameState.Board.GetLength(1)];
        for (int i = 0; i < gameState.Board.GetLength(0); i++)
        {
            for (int j = 0; j < gameState.Board.GetLength(1); j++)
            {
               board[i, j] = gameState.Board[i, j];
            }
        }

        this.savedGameState = new GameState
        {
            Board = board,
            CurrentPlayer = gameState.CurrentPlayer
        };
    }
}