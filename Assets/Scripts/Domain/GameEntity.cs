using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    private readonly ReactiveProperty<IngameState> _gameState = new();
    public IReadOnlyReactiveProperty<IngameState> CurrentGameState => _gameState;

    public IngameStateSolver IngameStateSolver {get; private set;}

    public GameEntity()
    {
        this.IngameStateSolver = new IngameStateSolver(this);
    }

    public void ChangeGameState()
    {
        _gameState.Value = this.IngameStateSolver.Solve();
    }
}