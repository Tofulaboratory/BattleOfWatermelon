using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner
{
    private readonly GameObject _obj;

    public FruitSpawner(GameObject obj)
    {
        _obj = obj;
    }

    public GameObject Spawn(FruitEntity entity)
    {
        var ret = UnityEngine.Object.Instantiate(_obj);

        var component = ret.GetComponent<IFruitUnit>();
        component?.Initialize(entity);

        return ret;
    }
}
