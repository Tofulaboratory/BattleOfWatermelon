using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class GameUsecase : IDisposable
{
    private CompositeDisposable _disposable = new();

    private readonly ITitleView _titleView;

    private readonly GameRegistry _gameRegistry;

    private readonly GameFactory _gameFactory;

    private TitlePresenter titlePresenter;
    private IngamePresenter ingamePresenter;

    [Inject]
    public GameUsecase(
        ITitleView titleView, 
        GameRegistry gameRegistry, 
        GameFactory gameFactory
        )
    {
        _titleView = titleView;
        _gameRegistry = gameRegistry;
        _gameFactory = gameFactory;

        _gameRegistry.CurrentGameEntity.Subscribe(_=>{
            ExecuteGame();
        }).AddTo(_disposable);
    }

    public void ExecuteTitle()
    {
        titlePresenter = new TitlePresenter(_titleView,_gameFactory,_gameRegistry);
    }

    public void ExecuteGame()
    {
        ingamePresenter = new IngamePresenter();
    }

    public void Dispose()
    {
        titlePresenter.Dispose();
        ingamePresenter.Dispose();
        _disposable.Dispose();
    }
}