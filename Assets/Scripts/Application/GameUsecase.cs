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
    private readonly IMatchingView _matchingView;
    private readonly IIngameView _ingameView;
    private readonly IResultView _resultView;

    private readonly GameRegistry _gameRegistry;

    private readonly GameFactory _gameFactory;
    private readonly FruitFactory _fruitFactory;
    private readonly FruitSpawner _fruitSpawner;
    private readonly PlayerSpawner _playerSpawner;
    private readonly SpawnObjectControllerSpawner _spawnObjectControllerSpawner;

    private TitlePresenter titlePresenter;
    private MatchingPresenter matchingPresenter;
    private IngamePresenter ingamePresenter;

    [Inject]
    public GameUsecase(
        ITitleView titleView,
        IMatchingView matchingView,
        IIngameView ingameView,
        IResultView resultView,
        GameRegistry gameRegistry,
        GameFactory gameFactory,
        FruitFactory fruitFactory
        )
    {
        _titleView = titleView;
        _matchingView = matchingView;
        _ingameView = ingameView;
        _resultView = resultView;
        _gameRegistry = gameRegistry;
        _gameFactory = gameFactory;
        _fruitFactory = fruitFactory;

        _fruitSpawner = new FruitSpawner();
        _playerSpawner = new PlayerSpawner();
        _spawnObjectControllerSpawner = new SpawnObjectControllerSpawner();

        _outgameState.Subscribe(state =>
        {
            //Debug.Log(state);
            switch (state)
            {
                case OutgameState.TITLE:
                    InitializeTitle();
                    break;

                case OutgameState.MATCHING:
                    InitializeMatching();
                    break;

                case OutgameState.INGAME:
                    InitializeIngame();
                    break;

                default:
                    break;
            }
        }).AddTo(_disposable);
    }

    ~GameUsecase()
    {
        Dispose();
    }

    private void InitializeTitle()
    {
        titlePresenter?.Dispose();
        titlePresenter = new TitlePresenter(
            _titleView,
            _gameFactory,
            _gameRegistry,
            () => ChangeOutgameState(OutgameState.MATCHING)
        );
        titlePresenter.Initialize();
    }

    private void InitializeMatching()
    {
        matchingPresenter?.Dispose();
        matchingPresenter = new MatchingPresenter(
            _matchingView,
            _gameRegistry,
            () => ChangeOutgameState(OutgameState.INGAME)
        );
        matchingPresenter.Initialize();
        matchingPresenter.Match();
    }

    private void InitializeIngame()
    {
        ingamePresenter?.Dispose();
        ingamePresenter = new IngamePresenter(
            _ingameView,
            _resultView,
            _fruitFactory,
            _fruitSpawner,
            _playerSpawner,
            _spawnObjectControllerSpawner,
            _gameRegistry,
            () => ChangeOutgameState(OutgameState.TITLE)
        );
        ingamePresenter.Initialize();
    }

    public void Dispose()
    {
        titlePresenter.Dispose();
        matchingPresenter.Dispose();
        ingamePresenter.Dispose();
        _disposable.Dispose();

        GC.SuppressFinalize(this);
    }
}