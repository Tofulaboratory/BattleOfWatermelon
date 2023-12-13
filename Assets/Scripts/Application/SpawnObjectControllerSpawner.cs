using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectControllerSpawner
{
    private readonly GameObject _obj;

    public SpawnObjectControllerSpawner()
    {
        _obj = Resources.Load<GameObject>("Prefabs/SpawnObjectController");
    }

    public SpawnObjectController Spawn()
    {
        var ret = UnityEngine.Object.Instantiate(_obj);

        var component = ret.GetComponent<SpawnObjectController>();
        return component;
    }
}
