using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using VContainer;

public class TitlePresenter : IDisposable
{
    private readonly CompositeDisposable _disposable = new();

    private readonly ITitleView _titleView;

    public TitlePresenter(ITitleView titleView, GameFactory gameFactory, GameRegistry gameRegistry, Action onTransitionGame)
    {
        _titleView = titleView;
        _titleView.OnClickStartButton().Subscribe(_=>{
            var entity = gameFactory.Create(IngameType.SINGLE);
            gameRegistry.Save(entity);

            onTransitionGame.Invoke();
            _titleView.SetActive(false);

            AudioManager.Instance.PlaySE("button01");
        }).AddTo(_disposable);

         _titleView.OnClickMultiStartButton().Subscribe(_=>{
            var entity = gameFactory.Create(IngameType.MULTI);
            gameRegistry.Save(entity);

            onTransitionGame.Invoke();
            _titleView.SetActive(false);

            AudioManager.Instance.PlaySE("button01");
        }).AddTo(_disposable);
    }

    public void Initialize()
    {
        _titleView?.SetActive(true);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
