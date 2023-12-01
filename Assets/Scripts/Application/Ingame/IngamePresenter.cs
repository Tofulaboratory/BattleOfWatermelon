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
    private readonly PlayerSpawner _playerSpawner;
    private readonly GameRegistry _gameRegistry;

    private readonly List<IPlayerUnit> _playerUnitList = new();

    public IngamePresenter(
        IIngameView ingameView,
        FruitFactory fruitFactory,
        FruitSpawner fruitSpawner,
        PlayerSpawner playerSpawner,
        GameRegistry gameRegistry
        )
    {
        _ingameView = ingameView;
        _fruitSpawner = fruitSpawner;
        _playerSpawner = playerSpawner;
        _gameRegistry = gameRegistry;
        _fruitFactory = fruitFactory;

        Bind();
    }

    private void Bind()
    {
        var gameEntity = _gameRegistry.CurrentGameEntity;

        //TODO 複数人対応
        var playerEntity = gameEntity?.Value.GameBoardEntity.PlayerEntity;
        if (playerEntity != null) _playerUnitList.Add(_playerSpawner.Spawn(playerEntity));

        gameEntity?.Value.GameBoardEntity.InNextFruitEntity.Where(item => item != null).Subscribe(item =>
        {
            _ingameView.ApplyNextFrame(item);
        }).AddTo(_disposable);
        gameEntity?.Value.GameBoardEntity.PlayerEntity.HeldFruit.Where(item => item != null).Subscribe(item =>
        {
            //TODO 複数人対応
            var fruit = _fruitSpawner.Spawn(item);
            _playerUnitList[0].HoldFruit(fruit);
            fruit.SetVisible(true);
            fruit.SetPosition(_playerUnitList[0].GetPosition());
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

        InputEventProvider.Instance.MoveDirectionX.Where(_ =>
            gameEntity?.Value.CurrentGameState.Value == IngameState.PROGRESS ||
            gameEntity?.Value.CurrentGameState.Value == IngameState.JUDGE
            ).Subscribe(value =>
            {
                //TODO プレイヤー移動
            }).AddTo(_disposable);
    }

    public void Initialize()
    {
        _ingameView.SetActive(true);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
