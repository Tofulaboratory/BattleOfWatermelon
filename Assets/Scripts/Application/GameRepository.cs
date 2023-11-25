using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameRepository
{
    public GameEntity CurrentGameEntity { get; private set; }
    public void Save(GameEntity entity) => CurrentGameEntity = entity;
}