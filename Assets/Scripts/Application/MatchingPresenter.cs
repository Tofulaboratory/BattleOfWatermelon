using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using VContainer;

public class MatchingPresenter : IDisposable
{
    private readonly CompositeDisposable _disposable = new();

    private readonly GameRegistry _gameRegistry;

    private readonly IMatchingView _matchingView;

    private Action _onTransitionGame;

    public MatchingPresenter(IMatchingView matchingView, GameRegistry gameRegistry, Action onTransitionGame)
    {
        _matchingView = matchingView;
        _gameRegistry = gameRegistry;
        _onTransitionGame = onTransitionGame;
    }

    public void Initialize()
    {
        if (_gameRegistry.CurrentGameEntity.Value.IngameType == IngameType.SINGLE) return;
        _matchingView.SetActive(true);
    }

    public async void Match()
    {
        if (_gameRegistry.CurrentGameEntity.Value.IngameType == IngameType.MULTI)
        {
            await _matchingView.DirectMatchBeginAsync();
            await _matchingView.DirectMatchEndAsync();
        }

        _onTransitionGame.Invoke();
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
