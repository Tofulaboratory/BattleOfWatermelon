using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class TitleController : IDisposable
{
    private CompositeDisposable _disposable = new();

    private readonly ITitleView _titleView;

    [Inject]
    public TitleController(ITitleView titleView)
    {
        _titleView = titleView;
    }

    public void Execute()
    {
        new TitlePresenter(_titleView);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
