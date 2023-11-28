using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner
{
    private readonly GameObject _obj;

    public PlayerSpawner(GameObject obj)
    {
        _obj = obj;
    }

    public GameObject Spawn(PlayerEntity entity)
    {
        var ret = UnityEngine.Object.Instantiate(_obj);

        var component = ret.GetComponent<IPlayerUnit>();
        component?.Initialize(entity);

        return ret;
    }
}
