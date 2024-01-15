using System;
using MoonActive.Scripts;

public class InMemoryGameSaver : GameSaver
{
    private PlayerType?[,] board;
    private PlayerType player;


    GameState GameSaver.LoadGame()
    {
        if (board == null)
        {
            // TODO:throw
            return new GameState{
            Board = null,
            CurrentPlayer = player}; 
        }

        return new GameState{
            Board = board,
            CurrentPlayer = player
        };
    }

    void GameSaver.SaveGame(GameState gameState)
    {
        if (this.board == null)
        {
            this.board = new PlayerType?[gameState.Board.GetLength(0), gameState.Board.GetLength(1)];
        }

        this.player = gameState.CurrentPlayer;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                this.board[i, j] = gameState.Board[i, j];
            }
        }
    }
}

