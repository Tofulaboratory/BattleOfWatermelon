using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameInitializer : IStartable
{
    private readonly GameUsecase _gameUsecase;

    [Inject]
    public GameInitializer(GameUsecase gameUsecase)
    {
        _gameUsecase = gameUsecase;
    }

    public void Start()
    {
        _gameUsecase.ChangeOutgameState(OutgameState.TITLE);
    }
}
