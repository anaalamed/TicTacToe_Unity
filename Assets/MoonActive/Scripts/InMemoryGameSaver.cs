using System;
using MoonActive.Scripts;

public class InMemoryGameSaver : GameSaver
{
    private PlayerType?[,] board;
    private PlayerType player;


    (PlayerType?[,], PlayerType?) GameSaver.LoadGame()
    {
        if (board == null)
        {
            // TODO:throw
            return (null,null);
        }
        return (board, player);
    }

    void GameSaver.SaveGame(PlayerType?[,] board, PlayerType currentPlayer)
    {
        if (this.board == null)
        {
            this.board = new PlayerType?[board.GetLength(0), board.GetLength(1)];
        }
        player = currentPlayer;

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                this.board[i, j] = board[i, j];
            }
        }
    }
}

