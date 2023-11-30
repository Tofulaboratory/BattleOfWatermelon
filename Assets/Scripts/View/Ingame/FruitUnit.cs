using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FruitUnit : MonoBehaviour, IFruitUnit
{
    [SerializeField] private FruitSpriteData fruitSpriteData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Initialize(FruitEntity entity)
    {
        spriteRenderer.sprite = fruitSpriteData.Get(entity.Level.Value);
        this.AddComponent<PolygonCollider2D>();
    }
}