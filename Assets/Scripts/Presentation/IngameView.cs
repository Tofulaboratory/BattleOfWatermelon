using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameView : ViewBase, IIngameView
{
    [SerializeField] private TMP_Text turnIndicaterText;
    [SerializeField] private FruitSpriteData fruitSpriteData;

    [SerializeField] private Image nextFrameImage;

    [SerializeField] private TMP_Text scoreText;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ApplyNextFrame(FruitEntity entity)
    {
        nextFrameImage.sprite = fruitSpriteData.Get(entity.Level.Value);
    }

    public void ApplyScoreText(int score)
    {
        scoreText.text = $"{score}";
    }

    public void ApplyTurnIndicator(string name)
    {
        turnIndicaterText.text = $"{name}のターン";
    }

    public void SetActive(bool isActivate)
    {
        base.SetActiveCanvasGroup(isActivate);
    }
}
