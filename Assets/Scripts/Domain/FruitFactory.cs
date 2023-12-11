using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitFactory
{
    public FruitFactory()
    {

    }

    internal FruitEntity CreateEntity(int level, FruitState fruitState) => new(level, fruitState);

    public FruitEntity Create()
    {
        return CreateEntity(-1, FruitState.HOLD);
    }

    public FruitEntity Create(int level, FruitState fruitState)
    {
        return CreateEntity(level, fruitState);
    }

    public List<FruitEntity> CreateList(int num)
    {
        var ret = new List<FruitEntity>();
        for (int i = 0; i < num; i++)
        {
            var entity = CreateEntity(-1, FruitState.STANDBY);
            ret.Add(entity);
        }

        return ret;
    }
}
