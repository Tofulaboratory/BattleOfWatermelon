using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class FruitEntity
{
    public string ID { get; private set; }

    private readonly ReactiveProperty<int> _level = new();
    public IReadOnlyReactiveProperty<int> Level => _level;

    private readonly ReactiveProperty<FruitState> _state = new();
    public IReadOnlyReactiveProperty<FruitState> State => _state;

    public Vector2 Position { get; private set; }

    public FruitLevelSolver fruitLevelSolver { get; private set; }

    public FruitEntity(int level, FruitState fruitState = FruitState.HOLD)
    {
        ID = Guid.NewGuid().ToString();

        fruitLevelSolver = new FruitLevelSolver();
        _level.Value = level >= 0 ? level : fruitLevelSolver.Solve();
        _state.Value = fruitState;
    }

    public void Release() => _state.Value = FruitState.FALL;

    public void StandBy(Vector2 position)
    {
        Position = position;
        _state.Value = FruitState.STANDBY;
    }

    public void Harvest(Vector2 position)
    {
        Position = position;
        _state.Value = FruitState.HARVEST;
    }

    public bool IsSafe()
    {
        if (_state.Value != FruitState.STANDBY) return true;
        if (ValueDefines.SAFE_ZONE_MIN_X < Position.x && Position.x < ValueDefines.SAFE_ZONE_MAX_X)
        {
            if (ValueDefines.SAFE_ZONE_MIN_Y < Position.y && Position.y < ValueDefines.SAFE_ZONE_MAX_Y)
            {
                return true;
            }
        }

        return false;
    }
}
