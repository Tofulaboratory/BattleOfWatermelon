using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UniRx.Triggers;

public class InputEventProvider : SingletonMonoBehaviour<InputEventProvider>
{
    public IObservable<float> GetHorizontalObservable {get; private set;}
    public IObservable<bool> GetKeyDownSpaceObservable {get; private set;}

    protected override void Awake()
    {
        base.Awake();

        GetHorizontalObservable = this.UpdateAsObservable()
            .Where(_ => Input.GetAxis("Horizontal") != 0)
            .Select(value => Input.GetAxis("Horizontal"));

        GetKeyDownSpaceObservable = this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Select(value => Input.GetKeyDown(KeyCode.Space));
    }
}
