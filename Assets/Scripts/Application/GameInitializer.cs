using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameInitializer : IStartable
{
    private readonly TitleController _titleController;

    [Inject]
    public GameInitializer(TitleController titleController)
    {
        _titleController = titleController;
    }

    public void Start()
    {
        _titleController.Execute();
    }
}
