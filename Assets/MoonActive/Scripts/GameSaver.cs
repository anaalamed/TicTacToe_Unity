using System;
using MoonActive.Scripts;

public interface GameSaver
{

    void SaveGame(PlayerType?[,] board, PlayerType currentPlayer);

    (PlayerType?[,], PlayerType?) LoadGame();
}

