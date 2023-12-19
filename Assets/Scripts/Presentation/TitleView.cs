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
    [SerializeField] private Button settingButton;

    public IObservable<Unit> OnClickStartButton() => startButton.OnClickAsObservable();
    public IObservable<Unit> OnClickMultiStartButton() => multiStartButton.OnClickAsObservable();
    public IObservable<Unit> OnClickSettingButton() => settingButton.OnClickAsObservable();

    public void SetActive(bool isActivate)
    {
        base.SetActiveCanvasGroup(isActivate);
    }
}
