using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUnit : MonoBehaviour, IPlayerUnit
{
    private readonly ReactiveProperty<IFruitUnit> _heldFruit = new();

    public void Initialize(PlayerEntity entity)
    {
        _heldFruit.Subscribe(value => {

        }).AddTo(this);
    }

    public void HoldFruit(IFruitUnit fruitUnit)
    {
        _heldFruit.Value = fruitUnit;
        _heldFruit.Value.SetHold(true);
    }

    public void ReleaseFruit()
    {
        _heldFruit.Value.SetHold(false);
        _heldFruit.Value = null;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void MovePosition(float direction)
    {
        //TODO 値定義
        transform.position += Vector3.right*direction*0.01f;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
