using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class GameController : IDisposable
{
    private CompositeDisposable _disposable = new();

    private readonly ITitleView _titleView;

    private readonly GameFactory _gameFactory;

    private readonly GameRepository _gameRepository;

    private TitlePresenter titlePresenter;

    [Inject]
    public GameController(ITitleView titleView, GameRepository gameRepository)
    {
        _titleView = titleView;
        _gameRepository = gameRepository;
        _gameFactory = new GameFactory();
    }

    public void ExecuteTitle()
    {
        titlePresenter = new TitlePresenter(_titleView,_gameFactory,_gameRepository);
    }

    public void Dispose()
    {
        titlePresenter.Dispose();
        _disposable.Dispose();
    }
}