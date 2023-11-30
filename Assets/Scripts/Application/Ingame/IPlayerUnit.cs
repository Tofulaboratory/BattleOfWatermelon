using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerUnit
{
    public void Initialize(PlayerEntity entity);
    public void HoldFruit(IFruitUnit fruitUnit);
    public Vector3 GetPosition();
}
