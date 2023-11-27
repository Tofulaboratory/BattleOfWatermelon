using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFactory
{
    private readonly GameBoardFactory _gameBoardFactory;
    private readonly PlayerFactory _playerFactory;

    public GameFactory(GameBoardFactory gameBoardFactory,PlayerFactory playerFactory)
    {
        _gameBoardFactory = gameBoardFactory;
        _playerFactory = playerFactory;
    }

    internal GameEntity CreateEntity(IngameType type,GameBoardEntity gameBoardEntity,PlayerEntity playerEntity) => new(type,gameBoardEntity,playerEntity);

    public GameEntity Create(IngameType type)
    {
        return CreateEntity(
            type,
            _gameBoardFactory.Create(),
            _playerFactory.Create()
        );
    }
}
