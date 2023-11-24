using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class TitlePresenter : IDisposable
{
    private readonly CompositeDisposable _disposable = new();

    private ITitleView titleView;

    [Inject]
    public TitlePresenter(ITitleView titleView)
    {
        this.titleView = titleView;
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
