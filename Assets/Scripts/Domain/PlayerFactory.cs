using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFactory
{
    public PlayerFactory()
    {

    }

    internal PlayerEntity CreateEntity() => new();

    public PlayerEntity[] CreateSingle()
    {
        var ret = new PlayerEntity[1];
        ret[0] = CreateEntity();
        return ret;
    }

    public PlayerEntity[] CreateMulti()
    {
        var ret = new PlayerEntity[2];
        ret[0] = CreateEntity();
        ret[1] = CreateEntity();
        return ret;
    }
}