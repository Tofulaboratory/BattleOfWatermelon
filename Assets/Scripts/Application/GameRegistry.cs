using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class GameRegistry
{
    private readonly ReactiveProperty<GameEntity> _currentGameEntity = new();
    public IReadOnlyReactiveProperty<GameEntity> CurrentGameEntity => _currentGameEntity;
    public void Save(GameEntity entity) => _currentGameEntity.Value = entity;
}