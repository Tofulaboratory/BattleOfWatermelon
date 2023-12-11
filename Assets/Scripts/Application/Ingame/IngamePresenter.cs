using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class IngamePresenter : IDisposable
{
    private readonly CompositeDisposable _disposable = new();
    private CancellationTokenSource _commonCts = new();

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
            var player = _playerUnitList[0];
            var fruitUnit = SpawnFruit(item, gameEntity, player.GetPosition(), player.GetTransform());
            player.HoldFruit(fruitUnit);
        }).AddTo(_disposable);

        gameEntity?.CurrentGameState.Subscribe(async state =>
        {
            Debug.Log(state);
            switch (state)
            {
                case IngameState.READY:
                    await ExecuteReady(gameEntity, _commonCts);
                    break;
                case IngameState.BEGIN:
                    await ExecuteBegin(gameEntity, _commonCts);
                    break;
                case IngameState.PROGRESS:
                    break;
                case IngameState.WAIT_FRUITS:
                    ExecuteWaitFruits(gameEntity, _commonCts);
                    break;
                case IngameState.JUDGE:
                    await ExecuteJudge(gameEntity, _commonCts);
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

        gameEntity?.GameBoardEntity.HervestFruitEntities.ObserveAdd().Subscribe(value =>
        {
            var data = gameEntity.TryMergeFruits();
            if (data.Item1 < 0) return;

            var fruitEntity = _fruitFactory.Create(data.Item1 + 1, FruitState.STANDBY);
            gameEntity?.GameBoardEntity.InsertFruit(fruitEntity);

            SpawnFruit(fruitEntity, gameEntity, data.Item2);
        }).AddTo(_disposable);

        InputEventProvider.Instance.GetHorizontalObservable.Where(_ =>
            gameEntity?.CurrentGameState.Value == IngameState.PROGRESS ||
            gameEntity?.CurrentGameState.Value == IngameState.WAIT_FRUITS ||
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
                gameEntity?.ChangeGameState(IngameState.WAIT_FRUITS);
            }).AddTo(_disposable);
    }

    private async UniTask ExecuteReady(GameEntity entity, CancellationTokenSource cts)
    {
        await UniTask.Delay(1);

        entity?.Initialize(
            _fruitFactory.Create(),
            _fruitFactory.Create()
        );
    }

    private async UniTask ExecuteBegin(GameEntity entity, CancellationTokenSource cts)
    {
        //TODO 表示待ち
        await UniTask.Delay(500);

        entity?.ChangeGameState(IngameState.PROGRESS);
    }

    private void ExecuteWaitFruits(GameEntity entity, CancellationTokenSource cts)
    {
        //entity?.ChangeGameState(IngameState.JUDGE);
    }

    private async UniTask ExecuteJudge(GameEntity entity, CancellationTokenSource cts)
    {
        //await UniTask.Delay(500);
        entity?.TryMoveTurn(_fruitFactory.Create());
    }

    private IFruitUnit SpawnFruit(
        FruitEntity fruitEntity,
        GameEntity gameEntity,
        Vector3 position,
        Transform parent = null
        )
    {
        var fruit = _fruitSpawner.Spawn(fruitEntity);
        fruit.SetVisible(true);
        fruit.SetPosition(position);
        fruit.SetParent(parent);
        fruit.OnRemove().Subscribe(value =>
        {
            gameEntity?.HervestFruits(value);
        }).AddTo(_disposable);
        fruit.OnCollide().Subscribe(value =>
        {
            gameEntity?.TryJudge();
        }).AddTo(_disposable);

        return fruit;
    }

    public void Initialize()
    {
        _ingameView.SetActive(true);
    }

    public void Dispose()
    {
        if (!_commonCts.IsCancellationRequested) _commonCts.Cancel();
        _commonCts.Dispose();
        _disposable.Dispose();
    }
}
