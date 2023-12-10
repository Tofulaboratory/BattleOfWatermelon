using System;
using UniRx;
using UnityEngine;

public interface IFruitUnit
{
    public IObservable<string> OnRemove();
    public int GetFruitLevel();
    public void Initialize(FruitEntity entity);
    public void SetHold(bool value);
    public void SetVisible(bool isVisible);
    public void SetPosition(Vector3 pos);
    public void SetParent(Transform parent);
}
