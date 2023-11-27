using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameBoardFactory
{
    private readonly FruitFactory _fruitFactory;

    public GameBoardFactory(FruitFactory fruitFactory)
    {
        _fruitFactory = fruitFactory;
    }

    internal GameBoardEntity CreateEntity(List<FruitEntity> fruitEntities) => new(fruitEntities);

    public GameBoardEntity Create()
    {
        return CreateEntity(_fruitFactory.CreateList(ValueDefines.FRUIT_STOCK_NUM));
    }
}
