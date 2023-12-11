using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class FruitLevelSolver
{
    public FruitLevelSolver()
    {
    }

    internal int Solve()
    {
        return Random.Range(0,ValueDefines.FRUIT_INIT_LEVEL_MAX);
    }
}
