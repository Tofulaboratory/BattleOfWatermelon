using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class GameUsecase : IDisposable
{
    private CompositeDisposable _disposable = new();

    private readonly ReactiveProperty<OutgameState> _outgameState = new();
    public void ChangeOutgameState(OutgameState state) => _outgameState.Value = state;

    private readonly ITitleView _titleView;

    private readonly GameRegistry _gameRegistry;

    private readonly GameFactory _gameFactory;
    private readonly FruitFactory _fruitFactory;

    private readonly FruitSpawner _fruitSpawner;

    private TitlePresenter titlePresenter;
    private IngamePresenter ingamePresenter;

    [Inject]
    public GameUsecase(
        ITitleView titleView,
        GameRegistry gameRegistry,
        GameFactory gameFactory,
        FruitFactory fruitFactory,
        FruitSpawner fruitSpawner
        )
    {
        _titleView = titleView;
        _gameRegistry = gameRegistry;
        _gameFactory = gameFactory;
        _fruitFactory = fruitFactory;
        _fruitSpawner = fruitSpawner;

        _outgameState.Subscribe(state =>
        {
            Debug.Log(state);
            switch (state)
            {
                case OutgameState.TITLE:
                    ExecuteTitle();
                    break;

                case OutgameState.MATCHING:
                    ChangeOutgameState(OutgameState.INGAME);
                    break;

                case OutgameState.INGAME:
                    ExecuteIngame();
                    break;

                case OutgameState.RESULT:
                    break;

                default:
                    break;
            }
        }).AddTo(_disposable);
    }

    private void ExecuteTitle()
    {
        titlePresenter = new TitlePresenter(
            _titleView,
            _gameFactory,
            _gameRegistry,
            () => ChangeOutgameState(OutgameState.MATCHING)
            );
    }

    private void ExecuteIngame()
    {
        ingamePresenter = new IngamePresenter(
            _fruitFactory,
            _fruitSpawner,
            _gameRegistry
            );
    }

    public void Dispose()
    {
        titlePresenter.Dispose();
        ingamePresenter.Dispose();
        _disposable.Dispose();
    }
}