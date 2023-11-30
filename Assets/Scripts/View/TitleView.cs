using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TitleView : ViewBase, ITitleView
{
    [SerializeField] private Button startButton;

    public IObservable<Unit> OnClickStartButton() => startButton.OnClickAsObservable();

    public void SetActive(bool isActivate)
    {
        base.SetActiveCanvasGroup(isActivate);
    }
}
