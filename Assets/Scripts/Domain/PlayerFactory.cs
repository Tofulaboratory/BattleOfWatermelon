using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory
{
    public PlayerFactory()
    {
        
    }

    internal PlayerEntity CreateEntity() => new();

    public PlayerEntity Create()
    {
        return CreateEntity();
    }
}
