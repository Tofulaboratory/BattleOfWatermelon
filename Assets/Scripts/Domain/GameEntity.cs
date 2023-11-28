using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameEntity
{
    public IngameType IngameType {get; private set;}
    public GameBoardEntity GameBoardEntity {get; private set;}
    public PlayerEntity PlayerEntity {get; private set;}

    private readonly ReactiveProperty<IngameState> _ingameState = new();
    public IReadOnlyReactiveProperty<IngameState> CurrentGameState => _ingameState;

    public IngameStateSolver IngameStateSolver {get; private set;}

    public GameEntity(IngameType ingameType,GameBoardEntity gameBoardEntity,PlayerEntity playerEntity)
    {
        this.IngameType = ingameType;
        this.GameBoardEntity = gameBoardEntity;
        this.PlayerEntity = playerEntity;

        this.IngameStateSolver = new IngameStateSolver(this);
    }

    public void ChangeGameState(IngameState state)
    {
        _ingameState.Value = state;
    }

    public void SolveGameState()
    {
        _ingameState.Value = this.IngameStateSolver.Solve();
    }
}