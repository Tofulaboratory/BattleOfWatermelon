using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UniRx.Triggers;

public class InputEventProvider : SingletonMonoBehaviour<InputEventProvider>
{
    public IObservable<float> GetHorizontal {get; private set;}

    protected override void Awake()
    {
        base.Awake();

        GetHorizontal = this.UpdateAsObservable()
            .Where(_ => Input.GetAxis("Horizontal") != 0)
            .Select(value => Input.GetAxis("Horizontal"));
    }
}
