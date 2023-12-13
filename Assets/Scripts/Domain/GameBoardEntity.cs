using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GameBoardEntity
{
    private readonly ReactiveCollection<FruitEntity> _inBoardFruitEntities = new();
    public IReadOnlyReactiveCollection<FruitEntity> InBoardFruitEntities => _inBoardFruitEntities;

    private readonly ReactiveCollection<FruitEntity> _hervestFruitEntities = new();
    public IReadOnlyReactiveCollection<FruitEntity> HervestFruitEntities => _hervestFruitEntities;

    private readonly ReactiveProperty<FruitEntity> _inNextFruitEntity = new();
    public IReadOnlyReactiveProperty<FruitEntity> InNextFruitEntity => _inNextFruitEntity;

    public PlayerEntity PlayerEntity { get; private set; }

    public GameBoardEntity(PlayerEntity playerEntity)
    {
        this.PlayerEntity = playerEntity;
    }

    public void Initialize(FruitEntity inHoldFruit, FruitEntity inNextFruit)
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

    public bool IsExistUnsafeFruit()
    {
        foreach(var i in _inBoardFruitEntities)
        {
            if(!i.IsSafe()) return true;
        }

        return false;
    }

    public void HervestFruits(string id)
    {
        HervestInBoardFruitIndex(id);
    }

    public void InsertFruit(FruitEntity entity)
    {
        _inBoardFruitEntities.Add(entity);
    }

    private void RemoveInBoardFruitIndex(string id)
    {
        var entity = FindInBoardFruitIndex(id);
        if (entity != null) _inBoardFruitEntities.Remove(entity);
    }

    private void HervestInBoardFruitIndex(string id)
    {
        var entity = FindInBoardFruitIndex(id);
        if (entity != null)
        {
            _hervestFruitEntities.Add(entity);
            _inBoardFruitEntities.Remove(entity);
        }
    }

    private FruitEntity FindInBoardFruitIndex(string id)
    {
        var entities = _inBoardFruitEntities.Where(item => item.ID == id).ToList();
        if (entities.Count >= 1)
        {
            return entities[0];
        }

        return null;
    }

    public (int, Vector2) MergeFruit()
    {
        for (var i = 0; i < _hervestFruitEntities.Count; i++)
        {
            for (var j = i + 1; j < _hervestFruitEntities.Count; j++)
            {
                if (_hervestFruitEntities[i].Level.Value == _hervestFruitEntities[j].Level.Value)
                {
                    var level = _hervestFruitEntities[i].Level.Value;
                    var position = Vector3.Lerp(_hervestFruitEntities[i].Position,_hervestFruitEntities[j].Position,0.5f);
                    _hervestFruitEntities.RemoveAt(j);
                    _hervestFruitEntities.RemoveAt(i);
                    return (level, position);
                }
            }
        }

        return (-1, Vector2.zero);
    }
}