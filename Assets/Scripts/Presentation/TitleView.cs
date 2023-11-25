using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TitleView : MonoBehaviour, ITitleView
{
    [SerializeField] private Button startButton;

    public IObservable<Unit> OnClickStartButton() => startButton.OnClickAsObservable();

    private void Start()
    {
    }
}
