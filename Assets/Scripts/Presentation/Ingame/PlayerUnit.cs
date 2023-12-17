using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerUnit : MonoBehaviour, IPlayerUnit
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private readonly ReactiveProperty<IFruitUnit> _heldFruit = new();

    public void Initialize(PlayerEntity entity)
    {
        _heldFruit.Subscribe(value =>
        {

        }).AddTo(this);
    }

    public void HoldFruit(IFruitUnit fruitUnit)
    {
        _heldFruit.Value = fruitUnit;
    }

    public void ReleaseFruit()
    {
        _heldFruit.Value.Release();
        _heldFruit.Value = null;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void MovePosition(float direction)
    {
        transform.position += Vector3.right * direction * ValueDefines.Player_MOVE_SPEED;
        var maxLimitX = ValueDefines.SAFE_ZONE_MAX_X - ValueDefines.MOVE_LIMIT_X;
        if (transform.position.x >= maxLimitX)
        {
            transform.position = new Vector3(maxLimitX, transform.position.y, transform.position.z);
        }

        var minLimitX = ValueDefines.SAFE_ZONE_MIN_X + ValueDefines.MOVE_LIMIT_X;
        if (transform.position.x <= minLimitX)
        {
            transform.position = new Vector3(minLimitX, transform.position.y, transform.position.z);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public GameObject GetObj()
    {
        return gameObject;
    }

    public void ChangeColor(int index)
    {
        //TODO 雑なので直す
        if (index == 0)
        {
            spriteRenderer.color = new Color(1, 0.5f, 0.5f);
        }
        else if (index == 1)
        {
            spriteRenderer.color = new Color(0.5f, 0.5f, 1);
        }
    }
}
