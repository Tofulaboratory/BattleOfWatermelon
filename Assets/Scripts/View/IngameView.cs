using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameView : MonoBehaviour, IIngameView
{
    [SerializeField] private FruitSpriteData fruitSpriteData;

    [SerializeField] private Image nextFrameImage;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ApplyNextFrame(FruitEntity entity)
    {
        nextFrameImage.sprite = fruitSpriteData.Get(entity.Level.Value);
    }
}
