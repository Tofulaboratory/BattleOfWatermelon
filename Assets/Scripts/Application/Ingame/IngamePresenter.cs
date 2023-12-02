using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        var gameEntity = _gameRegistry.CurrentGameEntity?.Value;

        //TODO 複数人対応
        var playerEntity = gameEntity?.GameBoardEntity.PlayerEntity;
        if (playerEntity != null) _playerUnitList.Add(_playerSpawner.Spawn(playerEntity));

        gameEntity?.GameBoardEntity.InNextFruitEntity.Where(item => item != null).Subscribe(item =>
        {
            _ingameView.ApplyNextFrame(item);
        }).AddTo(_disposable);

        gameEntity?.GameBoardEntity.PlayerEntity.HeldFruit.Where(item => item != null).Subscribe(item =>
        {
            //TODO 複数人対応
            var fruit = _fruitSpawner.Spawn(item);
            var player = _playerUnitList[0];
            player.HoldFruit(fruit);
            fruit.SetVisible(true);
            fruit.SetPosition(player.GetPosition());
            fruit.SetParent(player.GetTransform());
        }).AddTo(_disposable);

        gameEntity?.CurrentGameState.Subscribe(async state =>
        {
            Debug.Log(state);
            switch (state)
            {
                case IngameState.READY:
                    await ExecuteREADY(gameEntity);
                    break;
                case IngameState.BEGIN:
                    await ExecuteBEGIN(gameEntity);
                    break;
                case IngameState.PROGRESS:
                    break;
                case IngameState.JUDGE:
                    await ExecuteJUDGE(gameEntity);
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

        InputEventProvider.Instance.GetHorizontalObservable.Where(_ =>
            gameEntity?.CurrentGameState.Value == IngameState.PROGRESS ||
            gameEntity?.CurrentGameState.Value == IngameState.JUDGE
            ).Subscribe(value =>
            {
                _playerUnitList[0].MovePosition(value);
            }).AddTo(_disposable);

        InputEventProvider.Instance.GetKeyDownSpaceObservable.Where(_ =>
            gameEntity?.CurrentGameState.Value == IngameState.PROGRESS
            ).Subscribe(value =>
            {
                _playerUnitList[0].ReleaseFruit();
                gameEntity?.Judge();
            }).AddTo(_disposable);
    }

    private async UniTask ExecuteREADY(GameEntity entity)
    {
        await UniTask.Delay(1);

        entity?.Initialize(
            _fruitFactory.Create(),
            _fruitFactory.Create()
        );
    }

    private async UniTask ExecuteBEGIN(GameEntity entity)
    {
        //TODO 表示待ち
        await UniTask.Delay(500);

        entity?.ChangeGameState(IngameState.PROGRESS);
    }

    private async UniTask ExecuteJUDGE(GameEntity entity)
    {
        //TODO 判定処理
        await UniTask.Delay(500);

        //TODO GameEntityに集約
        entity?.GameBoardEntity.MoveTurn(_fruitFactory.Create());
        entity?.ChangeGameState(IngameState.PROGRESS);
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
