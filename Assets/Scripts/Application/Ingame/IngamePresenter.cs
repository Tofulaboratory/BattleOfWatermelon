using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class IngamePresenter : IDisposable
{
    private readonly CompositeDisposable _disposable = new();
    private CancellationTokenSource _commonCts = new();

    private readonly IIngameView _ingameView;
    private readonly IResultView _resultView;
    private readonly FruitFactory _fruitFactory;
    private readonly FruitSpawner _fruitSpawner;
    private readonly PlayerSpawner _playerSpawner;
    private readonly GameRegistry _gameRegistry;

    private readonly List<IPlayerUnit> _playerUnitList = new();
    private readonly List<IFruitUnit> _fruitUnitList = new();

    public IngamePresenter(
        IIngameView ingameView,
        IResultView resultView,
        FruitFactory fruitFactory,
        FruitSpawner fruitSpawner,
        PlayerSpawner playerSpawner,
        GameRegistry gameRegistry,
        Action onTransitionTitle
        )
    {
        _ingameView = ingameView;
        _resultView = resultView;
        _fruitSpawner = fruitSpawner;
        _playerSpawner = playerSpawner;
        _gameRegistry = gameRegistry;
        _fruitFactory = fruitFactory;

        Bind(onTransitionTitle);
    }

    private void Bind(Action onTransitionTitle)
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
                case IngameState.INIT:
                    break;
                case IngameState.READY:
                    await ExecuteReady(gameEntity, _commonCts);
                    break;
                case IngameState.BEGIN:
                    await ExecuteBeginAsync(gameEntity, _commonCts);
                    break;
                case IngameState.PROGRESS:
                    break;
                case IngameState.WAIT_FRUITS:
                    ExecuteWaitFruits(gameEntity, _commonCts);
                    break;
                case IngameState.JUDGE:
                    await ExecuteJudgeAsync(gameEntity, _commonCts);
                    break;
                case IngameState.CHANGE_PLAYER:
                    break;
                case IngameState.RESULT:
                    await ExecuteResultAsync(gameEntity, _commonCts);
                    break;
                case IngameState.END:
                    await ExecuteEndAsync(gameEntity, _commonCts);
                    onTransitionTitle.Invoke();
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

        _resultView.OnClickTitleButton().Subscribe(_ =>
        {
            gameEntity?.ChangeGameState(IngameState.END);
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

        entity?.GameReady(
            _fruitFactory.Create(),
            _fruitFactory.Create()
        );
    }

    private async UniTask ExecuteBeginAsync(GameEntity entity, CancellationTokenSource cts)
    {
        //TODO 表示待ち
        await UniTask.Delay(500);

        entity?.ChangeGameState(IngameState.PROGRESS);
    }

    private void ExecuteWaitFruits(GameEntity entity, CancellationTokenSource cts)
    {
        //entity?.ChangeGameState(IngameState.JUDGE);
    }

    private async UniTask ExecuteJudgeAsync(GameEntity entity, CancellationTokenSource cts)
    {
        //await UniTask.Delay(500);
        entity?.TryMoveTurn(_fruitFactory.Create());
    }

    private async UniTask ExecuteResultAsync(GameEntity entity, CancellationTokenSource cts)
    {
        _resultView.SetActive(true);
    }

    private async UniTask ExecuteEndAsync(GameEntity entity, CancellationTokenSource cts)
    {
        _ingameView.SetActive(false);
        _resultView.SetActive(false);
        for (var i = _fruitUnitList.Count - 1; i >= 0; i--)
        {
            if (_fruitUnitList[i].IsUnityNull()) continue;
            _fruitUnitList[i].Remove();
        }
        _fruitUnitList.Clear();
        _gameRegistry.Delete();
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

            //TODO 処理の見直し
            for (var i = _fruitUnitList.Count - 1; i >= 0; i--)
            {
                var entity = _fruitUnitList[i];
                if (entity.GetFruitID() == value)
                {
                    entity.Remove();
                    break;
                }
            }
        }).AddTo(_disposable);
        fruit.OnCollide().Subscribe(value =>
        {
            gameEntity?.TryJudge();
        }).AddTo(_disposable);
        _fruitUnitList.Add(fruit);

        return fruit;
    }

    public void Initialize()
    {
        _ingameView.SetActive(true);
        _resultView.SetActive(false);
        _gameRegistry.CurrentGameEntity?.Value?.ChangeGameState(IngameState.READY);
    }

    public void Dispose()
    {
        if (!_commonCts.IsCancellationRequested) _commonCts.Cancel();
        _commonCts.Dispose();
        _disposable.Dispose();
    }
}
