using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class TitlePresenter : IDisposable
{
    private readonly CompositeDisposable _disposable = new();

    private readonly ITitleView _titleView;

    public TitlePresenter(ITitleView titleView, GameEntity gameEntity)
    {
        _titleView = titleView;
        titleView.OnClickStartButton().Subscribe(_=>{
            gameEntity.ChangeGameState(IngameState.BEGIN);
        }).AddTo(_disposable);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
