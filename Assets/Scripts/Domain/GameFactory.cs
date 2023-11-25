using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFactory
{
    public GameFactory()
    {

    }

    internal GameEntity CreateGameEntity() => new();

    public GameEntity CreateSingleGame()
    {
        return CreateGameEntity();
    }
}
