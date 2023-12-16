using System;
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

    public PlayerEntity[] PlayerEntities { get; private set; }
    private readonly ReactiveProperty<int> _turn = new();
    public IReadOnlyReactiveProperty<int> Turn => _turn;
    private void ProgressTurn() => _turn.Value++;
    private int GetTurnIndex() => _turn.Value % PlayerEntities.Length;
    public string GetCurrentTurnPlayerID() => PlayerEntities[GetTurnIndex()].ID;

    public GameBoardEntity(PlayerEntity[] playerEntities)
    {
        this.PlayerEntities = playerEntities;
    }

    public void Initialize(FruitEntity inHoldFruit, FruitEntity inNextFruit)
    {
        this.PlayerEntities[GetTurnIndex()].HoldFruit(inHoldFruit);
        this.PlayerEntities[GetTurnIndex()].SetTurn(true);
        _inNextFruitEntity.Value = inNextFruit;
    }

    public void ReleaseFruit()
    {
        _inBoardFruitEntities.Add(this.PlayerEntities[GetTurnIndex()].HeldFruit.Value);
        this.PlayerEntities[GetTurnIndex()].ReleaseFruit();
    }

    public void MoveTurn(FruitEntity entity)
    {
        ProgressTurn();

        var playerEntity = this.PlayerEntities[GetTurnIndex()];
        playerEntity.HoldFruit(_inNextFruitEntity.Value);
        for(int i = 0;i<this.PlayerEntities.Length;i++)
        {
            playerEntity.SetTurn(false);    
        }
        playerEntity.SetTurn(true);
        _inNextFruitEntity.Value = entity;
    }

    public bool IsExistUnsafeFruit()
    {
        foreach (var i in _inBoardFruitEntities)
        {
            if (!i.IsSafe()) return true;
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
                    var position = Vector3.Lerp(_hervestFruitEntities[i].Position, _hervestFruitEntities[j].Position, 0.5f);
                    _hervestFruitEntities.RemoveAt(j);
                    _hervestFruitEntities.RemoveAt(i);
                    this.PlayerEntities[GetTurnIndex()].AddScore((int)Math.Pow(2, level));
                    return (level, position);
                }
            }
        }

        return (-1, Vector2.zero);
    }
}