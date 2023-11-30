using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class GameFactory
{
    private readonly GameBoardFactory _gameBoardFactory;

    [Inject]
    public GameFactory(GameBoardFactory gameBoardFactory)
    {
        _gameBoardFactory = gameBoardFactory;
    }

    internal GameEntity CreateEntity(IngameType type,GameBoardEntity gameBoardEntity) => new(type,gameBoardEntity);

    public GameEntity Create(IngameType type)
    {
        return CreateEntity(
            type,
            _gameBoardFactory.Create(type)
        );
    }
}
