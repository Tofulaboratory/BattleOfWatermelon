using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GameBoardEntity
{
    private readonly ReactiveCollection<FruitEntity> _inBoardFruitEntities = new();
    public IReadOnlyReactiveCollection<FruitEntity> InBoardFruitEntities => _inBoardFruitEntities;

    private readonly ReactiveProperty<FruitEntity> _inHoldFruitEntity = new();
    public IReadOnlyReactiveProperty<FruitEntity> InHoldFruitEntity => _inHoldFruitEntity;

    private readonly ReactiveProperty<FruitEntity> _inNextFruitEntity = new();
    public IReadOnlyReactiveProperty<FruitEntity> InNextFruitEntity => _inNextFruitEntity;

    public GameBoardEntity()
    {
    }

    public void Initialize(FruitEntity inHoldFruit,FruitEntity inNextFruit)
    {
        _inHoldFruitEntity.Value = inHoldFruit;
        _inNextFruitEntity.Value = inNextFruit;
    }

    public void MoveTurn(FruitEntity entity)
    {
        _inBoardFruitEntities.Add(_inHoldFruitEntity.Value);
        _inHoldFruitEntity.Value = _inNextFruitEntity.Value;
        _inNextFruitEntity.Value = entity;
    }

    public void RemoveInBoardFruitIndex(string id)
    {
        var entities = _inBoardFruitEntities.Where(item=>item.ID==id).ToList();
        if(entities.Count>=1)
        {
            _inBoardFruitEntities.Remove(entities[0]);
        }
    }
}
