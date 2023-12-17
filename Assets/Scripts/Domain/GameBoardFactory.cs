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

    internal GameBoardEntity CreateEntity(PlayerEntity[] playerEntities) => new(playerEntities);

    public GameBoardEntity Create(IngameType type)
    {
        GameBoardEntity ret = null;
        switch (type)
        {
            case IngameType.SINGLE:
            ret = CreateEntity(_playerFactory.CreateSingle());
                break;
            case IngameType.MULTI:
            ret = CreateEntity(_playerFactory.CreateMulti());
                break;
            default:
                break;
        }
        return ret;
    }
}
