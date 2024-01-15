using System;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

public class JSONGameSaver : GameSaver
{
    private readonly string fileName = "gameState.json";

    GameState GameSaver.LoadGame()
    {
        try
        {
            Debug.Log("Reading json state game from file: " + fileName);
            string json = File.ReadAllText(fileName);
            GameState gameState = JsonConvert.DeserializeObject<GameState>(json);
            return gameState;
        }
        catch (FileNotFoundException)
        {
            Debug.LogWarning("The game state was never saved");
            throw new InvalidOperationException();
        }
    }

    void GameSaver.SaveGame(GameState gameState)
    {
        string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
        Debug.Log("Saving json state game to file: " + fileName);
        File.WriteAllText(fileName, json);
    }
}

