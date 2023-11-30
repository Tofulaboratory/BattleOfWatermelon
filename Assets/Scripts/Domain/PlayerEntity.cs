using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerEntity
{
    private readonly ReactiveProperty<FruitEntity> _heldFruit = new();
    public IReadOnlyReactiveProperty<FruitEntity> HeldFruit => _heldFruit;

    public PlayerEntity()
    {

    }

    public void HoldFruit(FruitEntity entity)
    {
        _heldFruit.Value = entity;
    }
}
