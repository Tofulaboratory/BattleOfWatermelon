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

    [SerializeField] private TMP_Text[] scoreText;

    [SerializeField] private GameObject score2Obj;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ApplyNextFrame(FruitEntity entity)
    {
        nextFrameImage.sprite = fruitSpriteData.Get(entity.Level.Value);
    }

    public void ApplyScoreText(int index,int score)
    {
        scoreText[index].text = $"{score}";
    }

    public void ApplyTurnIndicator(string name)
    {
        turnIndicaterText.text = $"{name}のターン";
    }

    public void SetActiveScore2(bool isActivate)
    {
        score2Obj.SetActive(isActivate);
    }

    public void SetActive(bool isActivate)
    {
        base.SetActiveCanvasGroup(isActivate);
    }
}
