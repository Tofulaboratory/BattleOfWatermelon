using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class FruitEntity
{
    public string ID { get; private set; }

    private readonly ReactiveProperty<int> _level = new();
    public IReadOnlyReactiveProperty<int> Level => _level;

    private readonly ReactiveProperty<FruitState> _state = new();
    public IReadOnlyReactiveProperty<FruitState> State => _state;

    public Vector2 Position { get; private set; }

    public FruitLevelSolver fruitLevelSolver { get; private set; }

    public FruitEntity(int level)
    {
        ID = Guid.NewGuid().ToString();

        fruitLevelSolver = new FruitLevelSolver();
        _level.Value = level >= 0 ? level : fruitLevelSolver.Solve();
    }

    public bool IsMaxLevel() => _level.Value >= ValueDefines.MAX_FRUIT_LEVEL;

    public void IncrementLevel() => _level.Value++;

    public void SetHold(bool value) => _state.Value = value ? FruitState.HOLD : FruitState.FALL;

    public void StandBy() => _state.Value = FruitState.STANDBY;
    public void Harvest(Vector2 position)
    {
        Position = position;
        _state.Value = FruitState.HARVEST;
    }
}
