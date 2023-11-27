using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameBoardEntity
{
    private readonly ReactiveCollection<FruitEntity> _fruitEntitiesInBoard = new();
    public IReadOnlyReactiveCollection<FruitEntity> FruitEntitiesInBoard => _fruitEntitiesInBoard;

    public GameBoardEntity(List<FruitEntity> fruitEntities)
    {
        fruitEntities.ForEach(item=>_fruitEntitiesInBoard.Add(item));
    }
}
