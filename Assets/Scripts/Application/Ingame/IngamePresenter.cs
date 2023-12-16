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
    private readonly SpawnObjectControllerSpawner _spawnObjectControllerSpawner;
    private readonly GameRegistry _gameRegistry;

    private readonly Dictionary<string, IPlayerUnit> _playerUnitDic = new();
    private SpawnObjectController _spawnObjectController;

    public IngamePresenter(
        IIngameView ingameView,
        IResultView resultView,
        FruitFactory fruitFactory,
        FruitSpawner fruitSpawner,
        PlayerSpawner playerSpawner,
        SpawnObjectControllerSpawner spawnObjectControllerSpawner,
        GameRegistry gameRegistry,
        Action onTransitionTitle
        )
    {
        _ingameView = ingameView;
        _resultView = resultView;
        _fruitSpawner = fruitSpawner;
        _playerSpawner = playerSpawner;
        _spawnObjectControllerSpawner = spawnObjectControllerSpawner;
        _gameRegistry = gameRegistry;
        _fruitFactory = fruitFactory;

        _spawnObjectController = _spawnObjectControllerSpawner.Spawn();

        Bind(onTransitionTitle);
    }

    private void Bind(Action onTransitionTitle)
    {
        var gameEntity = _gameRegistry.CurrentGameEntity?.Value;
        var gameBoardEntity = gameEntity.GameBoardEntity;

        for (int i = 0; i < gameBoardEntity.PlayerEntities.Length; i++)
        {
            var playerEntity = gameBoardEntity.PlayerEntities[i];
            var playerUnit = _playerSpawner.Spawn(playerEntity);

            _playerUnitDic.Add(playerEntity.ID, playerUnit);
            _spawnObjectController.RegisterObj(playerUnit.GetObj());

            int index = i;
            playerEntity.Score.Subscribe(score => _ingameView.ApplyScoreText(index,score)).AddTo(_disposable);

            playerEntity.HeldFruit.Where(item => item != null).Subscribe(item =>
            {
                var player = _playerUnitDic[playerEntity.ID];
                var fruitUnit = SpawnFruit(item, gameEntity, player.GetPosition(), player.GetTransform());
                player.HoldFruit(fruitUnit);
            }).AddTo(_disposable);

            playerEntity.IsMyTurn.Where(item => item == true).Subscribe(_=>
            {
                _ingameView.ApplyTurnIndicator(playerEntity.Name);
            }).AddTo(_disposable);
        }

        gameBoardEntity.InNextFruitEntity.Where(item => item != null).Subscribe(item =>
        {
            _ingameView.ApplyNextFrame(item);
        }).AddTo(_disposable);

        gameEntity.CurrentGameState.Subscribe(async state =>
        {
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

        gameBoardEntity.HervestFruitEntities.ObserveAdd().Subscribe(value =>
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
                _playerUnitDic[gameBoardEntity.GetCurrentTurnPlayerID()].MovePosition(value);
            }).AddTo(_disposable);

        InputEventProvider.Instance.GetKeyDownSpaceObservable.Where(_ =>
            gameEntity?.CurrentGameState.Value == IngameState.PROGRESS
            ).Subscribe(value =>
            {
                _playerUnitDic[gameBoardEntity.GetCurrentTurnPlayerID()].ReleaseFruit();
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
        _spawnObjectController.ClearRegisteredObj();
        _spawnObjectController.Destroy();
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
        fruit.RegisterOnFall(() => _spawnObjectController.RegisterObj(fruit.GetObj()));
        fruit.SetVisible(true);
        fruit.SetPosition(position);
        fruit.OnRemove().Subscribe(value =>
        {
            gameEntity?.HervestFruits(value);
        }).AddTo(_disposable);
        fruit.OnCollide().Subscribe(value =>
        {
            gameEntity?.TryJudge();
        }).AddTo(_disposable);

        if (parent == null)
        {
            _spawnObjectController.RegisterObj(fruit.GetObj());
        }
        else
        {
            fruit.SetParent(parent);
        }

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
