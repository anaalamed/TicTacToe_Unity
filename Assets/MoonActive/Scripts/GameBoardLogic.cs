using System;
using System.Collections.Generic;
using MoonActive.Scripts;
using UnityEngine;

public class GameBoardLogic
{
    private readonly GameView gameView;
    private readonly UserActionEvents userActionEvents;
    private PlayerType currentPlayer;

    // Store the local version of board
    private PlayerType?[,] board;
    private bool isGaveActive = false;

    // Mapping to use the relevant class of GameSaver according to GameStateSource
    private readonly Dictionary<GameStateSource, GameSaver> gameSavers = new()
    {
        { GameStateSource.InMemory, new InMemoryGameSaver()},
        { GameStateSource.PlayerPrefs, new JSONGameSaver() }
    };

    public GameBoardLogic(GameView gameView, UserActionEvents userActionEvents)
    {
        this.gameView = gameView;
        this.userActionEvents = userActionEvents;
        this.currentPlayer = PlayerType.PlayerX;
    }

    public void Initialize(int columns, int rows)
    {
        Debug.Log("Initialize");
        board = new PlayerType?[columns, rows];

        userActionEvents.StartGameClicked += StartGame;
        userActionEvents.TileClicked += ClickTile;
        userActionEvents.SaveStateClicked += SaveState;
        userActionEvents.LoadStateClicked += LoadState;
    }

    public void DeInitialize()
    {
        Debug.Log("DeInitialize");
        userActionEvents.StartGameClicked -= StartGame;
        userActionEvents.TileClicked -= ClickTile;
        userActionEvents.SaveStateClicked -= SaveState;
        userActionEvents.LoadStateClicked -= LoadState;
    }

    /// <summary>
    /// Iniatilize the new game. Resets all relevant fields. 
    /// </summary>
    private void StartGame()
    {
        gameView.StartGame(currentPlayer);
        board = new PlayerType?[board.GetLength(0), board.GetLength(1)];
        isGaveActive = true;
    }

    /// <summary>
    /// Sets a tile sign of current player, checks if game is over and changes
    /// the turn to the next player if not.
    /// </summary>
    /// <param name="boardTilePosition"></param>
    private void ClickTile(BoardTilePosition boardTilePosition)
    {
        // If game started and is not over or the tile has already been set  
        if (!isGaveActive || board[boardTilePosition.Row, boardTilePosition.Column] != null)
        {
            return;
        }

        gameView.SetTileSign(currentPlayer, boardTilePosition);
        board[boardTilePosition.Row, boardTilePosition.Column] = currentPlayer;


        if (CheckIsGameWon())
        {
            gameView.GameWon(currentPlayer);
            isGaveActive = false;
        }
        else if (CheckIsGameTie())
        {
            gameView.GameTie();
            isGaveActive = false;
        }

        // Set the turn to the next player
        if (isGaveActive)
        {
            currentPlayer = currentPlayer == PlayerType.PlayerX ? PlayerType.PlayerO : PlayerType.PlayerX;
            gameView.ChangeTurn(currentPlayer);
        }

    }

    private void SaveState(GameStateSource gameStateSource)
    {
        gameSavers[gameStateSource].SaveGame(new GameState
        {
            Board = board,
            CurrentPlayer = currentPlayer
        });
    }

    private void LoadState(GameStateSource gameStateSource)
    {
        try
        {
            GameState gameState = gameSavers[gameStateSource].LoadGame();

            currentPlayer = gameState.CurrentPlayer;
            gameView.StartGame(currentPlayer);

            // Set the board from the loaded state 
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    PlayerType? currentSign = gameState.Board[i, j];
                    board[i, j] = currentSign;
                    if (currentSign != null)
                    {
                        gameView.SetTileSign((PlayerType)currentSign, new BoardTilePosition(i, j));
                    }
                }
            }
        }
        catch (InvalidOperationException e)
        {
            Debug.LogException(e);
            return;
        }
    }

    /// <summary>
    /// Checks if in one of rows, columns or the diagonals all the signs are the same.  
    /// </summary>
    /// <returns></returns>
    private Boolean CheckIsGameWon()
    {
        // Go over rows
        for (int i = 0; i < board.GetLength(0); i++)
        {
            if (IsWinRow(i))
            {
                return true;
            }
        }

        // Go over columns
        for (int i = 0; i < board.GetLength(1); i++)
        {
            if (IsWinColumn(i))
            {
                return true;
            }
        }

        // Verify thr number of rows and columns is equal
        return board.GetLength(0) == board.GetLength(1) && IsWinDiagonal();
    }

    /// <summary>
    /// Checks if there is still available tile on the board. 
    /// </summary>
    private Boolean CheckIsGameTie()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }


    private Boolean IsWinRow(int row)
    {
        return IsWin(row, 0, 0, 1);
    }

    private Boolean IsWinColumn(int column)
    {
        return IsWin(0, column, 1, 0);
    }

    private Boolean IsWinDiagonal()
    {
        return IsWin(0, 0, 1, 1) || IsWin(0, board.GetLength(0) - 1, 1, -1);
    }

    /// <summary>
    /// Common function to check win. Used to check rows, columns an the diagonals.
    /// </summary>
    /// <param name="iStart">An index to row start</param>
    /// <param name="jStart">An index to column start</param>
    /// <param name="iAdvance">A step to row advance</param>
    /// <param name="jAdvance">A step to column advance</param>
    /// <returns></returns>
    private Boolean IsWin(int iStart, int jStart, int iAdvance, int jAdvance)
    {
        PlayerType? player = board[iStart, jStart];
        if (player == null)
        {
            return false;
        }

        int i = iStart + iAdvance;
        int j = jStart + jAdvance;
        for (; i < board.GetLength(0) && j < board.GetLength(1); i += iAdvance, j += jAdvance)
        {
            if (board[i, j] != player)
            {
                return false;
            }
        }
        return true;
    }
}
