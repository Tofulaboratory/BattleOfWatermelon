using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ResultView : ViewBase, IResultView
{
    [SerializeField] private Button titleButton;

    public IObservable<Unit> OnClickTitleButton() => titleButton.OnClickAsObservable();
    public void SetActive(bool isActivate)
    {
        base.SetActiveCanvasGroup(isActivate);
    }
}
