using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerEntity
{
    private readonly ReactiveProperty<FruitEntity> _heldFruit = new();
    public IReadOnlyReactiveProperty<FruitEntity> HeldFruit => _heldFruit;

    private readonly ReactiveProperty<int> _score = new();
    public IReadOnlyReactiveProperty<int> Score => _score;
    public void AddScore(int value) => _score.Value += value;

    public PlayerEntity()
    {
        _score.Value = 0;

        _score.Subscribe(v=>Debug.Log(v));
    }

    public void HoldFruit(FruitEntity fruitEntity)
    {
        _heldFruit.Value = fruitEntity;
    }
}
