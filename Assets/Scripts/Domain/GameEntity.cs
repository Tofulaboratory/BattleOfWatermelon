using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class GameEntity
{
    public IngameType IngameType { get; private set; }
    public GameBoardEntity GameBoardEntity { get; private set; }

    private readonly ReactiveProperty<IngameState> _ingameState = new();
    public IReadOnlyReactiveProperty<IngameState> CurrentGameState => _ingameState;

    public IngameStateSolver IngameStateSolver { get; private set; }

    public GameEntity(IngameType ingameType, GameBoardEntity gameBoardEntity)
    {
        this.IngameType = ingameType;
        this.GameBoardEntity = gameBoardEntity;

        this.IngameStateSolver = new IngameStateSolver(this);
    }

    public void Initialize(FruitEntity hold, FruitEntity next)
    {
        this.GameBoardEntity.Initialize(
            hold,
            next
        );

        ChangeGameState(IngameState.BEGIN);
    }

    public void ChangeGameState(IngameState state)
    {
        _ingameState.Value = state;
    }

    public void TryJudge()
    {
        if (_ingameState.Value == IngameState.WAIT_FRUITS)
        {
            ChangeGameState(IngameState.JUDGE);
        }
    }

    public void TryMoveTurn(FruitEntity fruitEntity)
    {
        //TODO 判定テスト
        if (GameBoardEntity.IsExistUnsafeFruit())
        {
            ChangeGameState(IngameState.RESULT);
            return;
        }

        GameBoardEntity.MoveTurn(fruitEntity);
        ChangeGameState(IngameState.PROGRESS);
    }

    public void HervestFruits(string id)
    {
        GameBoardEntity.HervestFruits(id);
    }

    public (int, Vector2) TryMergeFruits()
    {
        return GameBoardEntity.MergeFruit();
    }

    // public void SolveGameState()
    // {
    //     _ingameState.Value = this.IngameStateSolver.Solve();
    // }
}