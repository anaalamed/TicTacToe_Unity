using System;
using System.Collections.Generic;
using Codice.CM.Client.Differences;
using MoonActive.Scripts;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

public class JSONGameSaver : GameSaver
{
    private string json;



    (PlayerType?[,], PlayerType?) GameSaver.LoadGame()
    {
        if (json == null) {
            return (null, null);
        }
        GameState gameState =JsonConvert.DeserializeObject<GameState>(json);
        return (gameState.Board,gameState.CurrentPlayer);
    }

    void GameSaver.SaveGame(PlayerType?[,] board, PlayerType currentPlayer)
    {
        GameState gameState = new GameState { Board = board,
        CurrentPlayer = currentPlayer};

        json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
        Debug.Log("aa, " + json);
    }
}

