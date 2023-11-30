using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner
{
    private readonly GameObject _obj;

    public FruitSpawner()
    {
        _obj = Resources.Load<GameObject>("Prefabs/FruitUnit");
    }

    public IFruitUnit Spawn(FruitEntity entity)
    {
        var ret = UnityEngine.Object.Instantiate(_obj);

        var component = ret.GetComponent<IFruitUnit>();
        component?.Initialize(entity);

        return component;
    }
}
