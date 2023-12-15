using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TitleView : ViewBase, ITitleView
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button multiStartButton;

    public IObservable<Unit> OnClickStartButton() => startButton.OnClickAsObservable();
    public IObservable<Unit> OnClickMultiStartButton() => multiStartButton.OnClickAsObservable();

    public void SetActive(bool isActivate)
    {
        base.SetActiveCanvasGroup(isActivate);
    }
}
