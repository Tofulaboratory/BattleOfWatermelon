using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class IngamePresenter : IDisposable
{
    private readonly CompositeDisposable _disposable = new();

    private readonly IIngameView _ingameView;
    private readonly FruitFactory _fruitFactory;
    private readonly FruitSpawner _fruitSpawner;
    private readonly GameRegistry _gameRegistry;

    public IngamePresenter(IIngameView ingameView, FruitFactory fruitFactory, FruitSpawner fruitSpawner, GameRegistry gameRegistry)
    {
        _ingameView = ingameView;
        _fruitSpawner = fruitSpawner;
        _gameRegistry = gameRegistry;
        _fruitFactory = fruitFactory;

        Initialize();
    }

    private void Initialize()
    {
        var gameEntity = _gameRegistry.CurrentGameEntity;

        gameEntity?.Value.GameBoardEntity.InNextFruitEntity.Where(item => item != null).Subscribe(item =>
        {
            _ingameView.ApplyNextFrame(item);
        }).AddTo(_disposable);
        gameEntity?.Value.GameBoardEntity.InHoldFruitEntity.Where(item => item != null).Subscribe(item =>
        {
            _fruitSpawner.Spawn(item);
        }).AddTo(_disposable);

        gameEntity?.Value.CurrentGameState.Subscribe(state =>
        {
            switch (state)
            {
                case IngameState.READY:
                    gameEntity?.Value.GameBoardEntity.Initialize(
                        _fruitFactory.Create(),
                        _fruitFactory.Create()
                    );

                    break;
                case IngameState.BEGIN:
                    break;
                case IngameState.PROGRESS:
                    break;
                case IngameState.JUDGE:
                    break;
                case IngameState.CHANGE_PLAYER:
                    break;
                case IngameState.RESULT:
                    break;
                case IngameState.END:
                    break;
                default:
                    break;
            }
        }).AddTo(_disposable);
    }


    public void Dispose()
    {
        _disposable.Dispose();
    }
}
