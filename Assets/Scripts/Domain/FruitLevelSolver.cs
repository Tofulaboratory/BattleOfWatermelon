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
        //TODO 初期レベル算出
        return Random.Range(1,3);
    }
}
