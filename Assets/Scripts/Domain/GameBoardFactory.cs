using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class GameBoardFactory
{
    private readonly PlayerFactory _playerFactory;

    [Inject]
    public GameBoardFactory(PlayerFactory playerFactory)
    {
        _playerFactory = playerFactory;
    }

    internal GameBoardEntity CreateEntity(PlayerEntity playerEntity) => new(playerEntity);

    public GameBoardEntity Create(IngameType type)
    {
        return CreateEntity(_playerFactory.Create());
    }
}
