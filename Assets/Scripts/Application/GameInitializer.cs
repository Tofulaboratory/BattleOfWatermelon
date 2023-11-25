using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameInitializer : IStartable
{
    private readonly GameController _gameController;

    [Inject]
    public GameInitializer(GameController gameController)
    {
        _gameController = gameController;
    }

    public void Start()
    {
        _gameController.Execute();
    }
}
