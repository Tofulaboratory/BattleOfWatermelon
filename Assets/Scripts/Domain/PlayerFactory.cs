using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFactory
{
    public PlayerFactory()
    {

    }

    internal PlayerEntity CreateEntity(string name) => new(name);

    public PlayerEntity[] CreateSingle()
    {
        var ret = new PlayerEntity[1];
        ret[0] = CreateEntity("たかし");
        return ret;
    }

    public PlayerEntity[] CreateMulti()
    {
        var ret = new PlayerEntity[2];
        ret[0] = CreateEntity("たかし1");
        ret[1] = CreateEntity("たかし2");
        return ret;
    }
}