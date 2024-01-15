using Codice.CM.Client.Differences;
using MoonActive.Scripts;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

public class JSONGameSaver : GameSaver
{
    private string json;


    GameState GameSaver.LoadGame()
    {
        if (json == null) {
            // TODO throw 
            //return (null, PlayerType.PlayerX);
        }

        GameState gameState =JsonConvert.DeserializeObject<GameState>(json);
        return new GameState
        {
            Board = gameState.Board,
            CurrentPlayer = gameState.CurrentPlayer
        };
    }

    void GameSaver.SaveGame(GameState gameState)
    {
        json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
        Debug.Log("json object: , " + json);
    }
}

