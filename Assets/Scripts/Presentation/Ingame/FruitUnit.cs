using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class FruitUnit : MonoBehaviour, IFruitUnit
{
    [SerializeField] private FruitSpriteData fruitSpriteData;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidbody2D;

    private readonly ReactiveProperty<bool> _isHold = new();

    public void Initialize(FruitEntity entity)
    {
        spriteRenderer.sprite = fruitSpriteData.Get(entity.Level.Value);

        //当たり判定の生成のため
        this.AddComponent<PolygonCollider2D>();

        _isHold.Subscribe(value =>
        {
            rigidbody2D.simulated = value;
        }).AddTo(this);
    }

    public void SetVisible(bool isVisible)
    {
        spriteRenderer.enabled = isVisible;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}