using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ResultView : ViewBase, IResultView
{
    [SerializeField] private Button titleButton;
    [SerializeField] private TMP_Text resultText;

    public IObservable<Unit> OnClickTitleButton() => titleButton.OnClickAsObservable();

    public void ApplyResultText(string text)
    {
        resultText.text = text;
    }

    public void SetActive(bool isActivate)
    {
        base.SetActiveCanvasGroup(isActivate);
    }
}
