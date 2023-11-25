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

    private readonly GameFactory _gameFactory;

    [Inject]
    public TitleController(ITitleView titleView)
    {
        _titleView = titleView;
        _gameFactory = new GameFactory();
    }

    public void Execute()
    {
        var gameEntity = _gameFactory.CreateSingleGame();
        new TitlePresenter(_titleView,gameEntity);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
