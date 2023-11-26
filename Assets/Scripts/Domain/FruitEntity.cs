using System;
using Cysharp.Threading.Tasks;
using UniRx;

public class FruitEntity
{
    private readonly ReactiveProperty<int> _level = new();
    public IReadOnlyReactiveProperty<int> Level => _level;

    public FruitLevelSolver fruitLevelSolver {get; private set;}

    public FruitEntity(){
        fruitLevelSolver = new FruitLevelSolver();
        _level.Value = fruitLevelSolver.Solve();
    }

    public bool IsMaxLevel() => _level.Value>=ValueDefines.MAX_FRUIT_LEVEL;

    public void IncrementLevel()
    {
        _level.Value ++;
    }
}
