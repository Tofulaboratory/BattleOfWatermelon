using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class GameBoardFactory
{
    public GameBoardFactory()
    {
    }

    internal GameBoardEntity CreateEntity() => new();

    public GameBoardEntity Create()
    {
        return CreateEntity();
    }
}
