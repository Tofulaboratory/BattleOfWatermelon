using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class InputEventProvider : SingletonMonoBehaviour<InputEventProvider>
{
    private readonly ReactiveProperty<float> moveDirectionX = new();
    public IReadOnlyReactiveProperty<float> MoveDirectionX => moveDirectionX;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        moveDirectionX.Value = Input.GetAxis("Horizontal");
    }
}
