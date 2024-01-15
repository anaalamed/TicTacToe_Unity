public interface GameSaver
{
    void SaveGame(GameState gameState);

    GameState LoadGame();
}

