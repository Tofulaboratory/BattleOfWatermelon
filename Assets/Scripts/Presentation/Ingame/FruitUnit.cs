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

    private Subject<string> _onCollide = new();
    public IObservable<string> OnCollide() => _onCollide;

    private FruitEntity _fruitEntity;
    public int GetFruitLevel() => _fruitEntity.Level.Value;
    public string GetFruitID() => _fruitEntity.ID;

    //TODO 修正する
    private Action _onFall = null;
    public void RegisterOnFall(Action onFall) => _onFall = onFall;

    public void Initialize(FruitEntity entity)
    {
        _fruitEntity = entity;

        spriteRenderer.sprite = fruitSpriteData.Get(entity.Level.Value);
        ApplySize(GetFruitLevel());

        //当たり判定の生成のため
        this.AddComponent<PolygonCollider2D>();

        var onCollidion = this.OnCollisionEnter2DAsObservable();
        onCollidion.Subscribe(col =>
        {
            if (entity.State.Value == FruitState.FALL) _onCollide.OnNext(entity.ID);

            var collidedFruit = col.gameObject.GetComponent<IFruitUnit>();
            var isStandby = collidedFruit == null || collidedFruit.GetFruitLevel() != GetFruitLevel();
            if (isStandby)
            {
                entity.StandBy(new Vector2(transform.position.x, transform.position.y));
                return;
            }

            entity.Harvest(new Vector2(transform.position.x, transform.position.y));
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
                    _onFall.Invoke();
                    break;
                default:
                    break;
            }
        }).AddTo(this);
    }

    public void Release()
    {
        _fruitEntity.Release();
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

    public GameObject GetObj() => this.gameObject;

    public void Remove()
    {
        if (this.gameObject == null) return;
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        _onRemove.OnNext(_fruitEntity.ID);
    }

    private void ApplySize(int level)
    {
        this.transform.localScale = Vector3.one * (ValueDefines.FRUIT_SIZE_BASE + ValueDefines.FRUIT_SIZE_RATE * level);
    }
}