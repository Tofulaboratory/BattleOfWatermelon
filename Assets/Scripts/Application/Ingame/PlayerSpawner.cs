using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner
{
    private readonly GameObject _obj;

    public PlayerSpawner()
    {
         _obj = Resources.Load<GameObject>("Prefabs/PlayerUnit");
    }

    public IPlayerUnit Spawn(PlayerEntity entity)
    {
        var ret = UnityEngine.Object.Instantiate(_obj);

        var component = ret.GetComponent<IPlayerUnit>();
        component?.Initialize(entity);

        return component;
    }
}
