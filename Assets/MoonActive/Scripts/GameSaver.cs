/// <summary>
/// This is an interface to store game state. 
/// </summary>
public interface GameSaver
{
    /// <summary>
    /// Saves the game state. 
    /// </summary>
    /// <param name="gameState"></param>
    void SaveGame(GameState gameState);

    /// <summary>
    /// Returns the last saved game state. 
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    GameState LoadGame();
}

