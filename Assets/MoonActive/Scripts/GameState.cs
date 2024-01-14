using System;
using MoonActive.Scripts;

public class GameState
{
    public PlayerType?[,] Board { get; set; }
    public PlayerType CurrentPlayer { get; set; }
}

