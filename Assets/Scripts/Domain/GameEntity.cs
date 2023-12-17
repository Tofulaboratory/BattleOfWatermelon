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

    public bool IsMulti() => IngameType == IngameType.MULTI;

    private readonly ReactiveProperty<IngameState> _ingameState = new();
    public IReadOnlyReactiveProperty<IngameState> CurrentGameState => _ingameState;

    public IngameStateSolver IngameStateSolver { get; private set; }

    public GameEntity(IngameType ingameType, GameBoardEntity gameBoardEntity)
    {
        this.IngameType = ingameType;
        this.GameBoardEntity = gameBoardEntity;

        this.IngameStateSolver = new IngameStateSolver(this);
    }

    public void GameReady(FruitEntity hold, FruitEntity next)
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

    public void ReleaseFruit()
    {
        GameBoardEntity.ReleaseFruit();
        ChangeGameState(IngameState.WAIT_FRUITS);
    }

    public void TryChangeJudge()
    {
        if (_ingameState.Value == IngameState.WAIT_FRUITS)
        {
            ChangeGameState(IngameState.JUDGE);
        }
    }

    public void Judge()
    {
        if (GameBoardEntity.IsExistUnsafeFruit())
        {
            ChangeGameState(IngameState.RESULT);
            return;
        }

        ChangeGameState(IngameState.CHANGE_PLAYER);
    }

    public void TryMoveTurn(FruitEntity fruitEntity)
    {
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

    public void EndGame()
    {
        //TODO
    }

    public string GetResultText()
    {
        switch (IngameType)
        {
            case IngameType.SINGLE:
                return $"スコア\n{GameBoardEntity.PlayerEntities[0].Score.Value}";
            case IngameType.MULTI:
            var winner = "";
            if(GameBoardEntity.PlayerEntities[0].Score.Value>GameBoardEntity.PlayerEntities[1].Score.Value)
            {
                winner = GameBoardEntity.PlayerEntities[0].Name;
            }
            else if(GameBoardEntity.PlayerEntities[0].Score.Value<GameBoardEntity.PlayerEntities[1].Score.Value)
            {
                winner = GameBoardEntity.PlayerEntities[1].Name;
            }
            else
            {
                return "引き分け！";
            }
            return $"{winner}の勝利！";
        }

        return "";
    }

    // public void SolveGameState()
    // {
    //     _ingameState.Value = this.IngameStateSolver.Solve();
    // }
}