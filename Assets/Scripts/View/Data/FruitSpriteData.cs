using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitSpriteData", menuName = "FruitSpriteData", order = 0)]
public class FruitSpriteData : ScriptableObject
{
    [SerializeField] private Sprite[] fruitSprites;

    public Sprite Get(int level) => fruitSprites[level];
}