using System;
using System.Collections.Generic;
using MoonActive.Scripts;
using UnityEngine;

public class GameBoardLogic
{
    private readonly GameView gameView;
    private readonly UserActionEvents userActionEvents;
    private PlayerType currentPlayer;
    private PlayerType?[,] board;
    private bool isGaveOver = false;

    private readonly Dictionary<GameStateSource, GameSaver> gameSavers = new Dictionary<GameStateSource, GameSaver>
    {
        { GameStateSource.InMemory, new InMemoryGameSaver()},
        { GameStateSource.PlayerPrefs, new JSONGameSaver() }
    };



    public GameBoardLogic(GameView gameView, UserActionEvents userActionEvents)
    {
        this.gameView = gameView;
        this.userActionEvents = userActionEvents;

        // TODO: make random 
        this.currentPlayer = PlayerType.PlayerX;
    }

    public void Initialize(int columns, int rows)
    {
        Debug.Log("Initialize");
        board = new PlayerType?[columns,rows];

        userActionEvents.StartGameClicked += StartGame;
        userActionEvents.TileClicked += ClickTile;
        userActionEvents.SaveStateClicked += SaveState;
        userActionEvents.LoadStateClicked += LoadState;
    }
    
    public void DeInitialize()
    {
        Debug.Log("DeInitialize");
        userActionEvents.StartGameClicked += StartGame;
        userActionEvents.TileClicked += ClickTile;
        userActionEvents.SaveStateClicked += SaveState;
        userActionEvents.LoadStateClicked += LoadState;
    }

    private void StartGame()
    {
        gameView.StartGame(currentPlayer);
        board = new PlayerType?[board.GetLength(0), board.GetLength(1)];
        isGaveOver = false;
    }

    private void ClickTile(BoardTilePosition boardTilePosition)
    {
        // If game is over or the tile is not available - do nothing 
        if (isGaveOver || board[boardTilePosition.Row, boardTilePosition.Column] != null)
        {
            return;
        }

        gameView.SetTileSign(currentPlayer, boardTilePosition);
        board[boardTilePosition.Row, boardTilePosition.Column] = currentPlayer;


        if (CheckIsGameWon())
        {
            gameView.GameWon(currentPlayer);
            isGaveOver = true;
        }

        if (CheckIsGameTie())
        {
            gameView.GameTie();
            isGaveOver = true;
        }

        // Set next player
        if (!isGaveOver)
        {
            currentPlayer = currentPlayer == PlayerType.PlayerX ? PlayerType.PlayerO : PlayerType.PlayerX;
            gameView.ChangeTurn(currentPlayer);
        }

    }

    private void SaveState(GameStateSource gameStateSource)
    {
        gameSavers[gameStateSource].SaveGame(new GameState {
            Board = board,
            CurrentPlayer = currentPlayer
        });
    }

    private void LoadState(GameStateSource gameStateSource)
    {
        GameState gameState = gameSavers[gameStateSource].LoadGame();

        if (gameState.Board == null)
        {
            return;
        }

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

        return board.GetLength(0) == board.GetLength(1) && IsWinDiagonal();
    }

    private Boolean CheckIsGameTie()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j=0; j< board.GetLength(1); j++)
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

    private Boolean IsWin(int iStart, int jStart, int iAdvance, int jAdvance)
    {
        PlayerType? player = board[iStart, jStart];
        if (player == null)
        {
            return false;
        }
        int i = iStart + iAdvance;
        int j = jStart + jAdvance;
        for (; i < board.GetLength(0) && j < board.GetLength(1); i += iAdvance, j += jAdvance )
        {
            if (board[i,j] != player)
            {
                return false;
            }
        }
        return true;
    }
}
