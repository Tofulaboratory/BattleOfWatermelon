using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameStateSolver
{
    private readonly GameEntity gameEntity;

    public IngameStateSolver(GameEntity gameEntity)
    {
        this.gameEntity = gameEntity;
    }

    internal IngameState Solve()
    {
        var ret = IngameState.READY;
        return ret;
    }
}
