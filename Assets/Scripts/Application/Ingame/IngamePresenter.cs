using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class IngamePresenter : IDisposable
{
    private readonly CompositeDisposable _disposable = new();

    public IngamePresenter()
    {
        
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
