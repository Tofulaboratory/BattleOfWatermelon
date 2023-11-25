using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFactory
{
    public GameFactory()
    {

    }

    internal GameEntity CreateGameEntity(IngameType type) => new(type);

    public GameEntity Create(IngameType type)
    {
        return CreateGameEntity(type);
    }
}
