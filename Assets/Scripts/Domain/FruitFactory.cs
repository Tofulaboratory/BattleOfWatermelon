using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitFactory
{
    public FruitFactory()
    {

    }

    internal FruitEntity CreateEntity() => new();

    public FruitEntity Create()
    {
        return CreateEntity();
    }

    public List<FruitEntity> CreateList(int num)
    {
        var ret = new List<FruitEntity>();
        for(int i=0;i<num;i++){
            ret.Add(CreateEntity());
        }

        return ret;
    }
}
