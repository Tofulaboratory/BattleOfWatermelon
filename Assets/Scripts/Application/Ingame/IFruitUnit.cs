using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFruitUnit
{
    public void Initialize(FruitEntity entity);
    public void SetHold(bool value);
    public void SetVisible(bool isVisible);
    public void SetPosition(Vector3 pos);
    public void SetParent(Transform parent);
}
