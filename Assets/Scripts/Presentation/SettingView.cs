using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : ViewBase, ISettingView
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Slider soundVolumeSlider;

    void Awake()
    {
        soundVolumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
        closeButton.OnClickAsObservable().Subscribe(_=>{
            SetActive(false);
            AudioManager.Instance.PlaySE("button01");
        }).AddTo(this);
    }

    public void SetActive(bool isActivate)
    {
        base.SetActiveCanvasGroup(isActivate);
    }
}
