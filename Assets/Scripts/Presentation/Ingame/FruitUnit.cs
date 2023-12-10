using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;

public class FruitUnit : MonoBehaviour, IFruitUnit
{
    [SerializeField] private FruitSpriteData fruitSpriteData;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidbody2D;

    private Subject<string> _onRemove = new();
    public IObservable<string> OnRemove() => _onRemove;

    private FruitEntity _fruitEntity;
    public int GetFruitLevel() => _fruitEntity.Level.Value;

    public void Initialize(FruitEntity entity)
    {
        _fruitEntity = entity;
        spriteRenderer.sprite = fruitSpriteData.Get(entity.Level.Value);

        //当たり判定の生成のため
        this.AddComponent<PolygonCollider2D>();

        var onCollidion = this.OnCollisionEnter2DAsObservable();
        onCollidion.Subscribe(col =>
        {
            var collidedFruit = col.gameObject.GetComponent<IFruitUnit>();
            if (collidedFruit == null) return;
            if (collidedFruit.GetFruitLevel() != GetFruitLevel()) return;
            _onRemove.OnNext(entity.ID);

            Destroy(col.gameObject);
        }).AddTo(this);

        _fruitEntity.State.Subscribe(value =>
        {
            rigidbody2D.simulated = value != FruitState.HOLD;

            switch (value)
            {
                case FruitState.HOLD:
                    break;
                case FruitState.FALL:
                    SetParent(null);
                    break;
                default:
                    break;
            }
        }).AddTo(this);
    }

    public void SetHold(bool value)
    {
        _fruitEntity.SetHold(value);
    }

    public void SetVisible(bool isVisible)
    {
        spriteRenderer.enabled = isVisible;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }
}