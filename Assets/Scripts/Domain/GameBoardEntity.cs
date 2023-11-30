using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GameBoardEntity
{
    private readonly ReactiveCollection<FruitEntity> _inBoardFruitEntities = new();
    public IReadOnlyReactiveCollection<FruitEntity> InBoardFruitEntities => _inBoardFruitEntities;

    private readonly ReactiveProperty<FruitEntity> _inNextFruitEntity = new();
    public IReadOnlyReactiveProperty<FruitEntity> InNextFruitEntity => _inNextFruitEntity;

    public PlayerEntity PlayerEntity {get; private set;}

    public GameBoardEntity(PlayerEntity playerEntity)
    {
        this.PlayerEntity = playerEntity;
    }

    public void Initialize(FruitEntity inHoldFruit,FruitEntity inNextFruit)
    {
        this.PlayerEntity.HoldFruit(inHoldFruit);
        _inNextFruitEntity.Value = inNextFruit;
    }

    public void MoveTurn(FruitEntity entity)
    {
        _inBoardFruitEntities.Add(this.PlayerEntity.HeldFruit.Value);
        this.PlayerEntity.HoldFruit(_inNextFruitEntity.Value);
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
