using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitFactory
{
    public FruitFactory()
    {

    }

    internal FruitEntity CreateFruitEntity() => new();

    public FruitEntity Create()
    {
        return CreateFruitEntity();
    }
}
