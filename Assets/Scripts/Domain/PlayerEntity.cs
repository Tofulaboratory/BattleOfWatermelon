using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerEntity
{
    public string ID { get; private set; }

    public string Name {get; private set;}

    private readonly ReactiveProperty<FruitEntity> _heldFruit = new();
    public IReadOnlyReactiveProperty<FruitEntity> HeldFruit => _heldFruit;

    private readonly ReactiveProperty<bool> _isMyTurn = new();
    public IReadOnlyReactiveProperty<bool> IsMyTurn => _isMyTurn;

    private readonly ReactiveProperty<int> _score = new();
    public IReadOnlyReactiveProperty<int> Score => _score;
    public void AddScore(int value) => _score.Value += value;

    public PlayerEntity(string name)
    {
        ID = Guid.NewGuid().ToString();
        Name = name;

        _score.Value = 0;
    }

    public void SetTurn(bool isMyturn)
    {
        _isMyTurn.Value = isMyturn;
    }

    public void HoldFruit(FruitEntity fruitEntity)
    {
        _heldFruit.Value = fruitEntity;
    }

    public void ReleaseFruit()
    {
        _heldFruit.Value = null;
    }
}
