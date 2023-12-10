using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitFactory
{
    public FruitFactory()
    {

    }

    internal FruitEntity CreateEntity(int level) => new(level);

    public FruitEntity Create()
    {
        return CreateEntity(-1);
    }

    public FruitEntity Create(int level)
    {
        return CreateEntity(level);
    }

    public List<FruitEntity> CreateList(int num)
    {
        var ret = new List<FruitEntity>();
        for(int i=0;i<num;i++){
            var entity = CreateEntity(-1);
            ret.Add(entity);
        }

        return ret;
    }
}
