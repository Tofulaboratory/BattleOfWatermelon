using System;
using UniRx;
using UnityEngine;

public interface IFruitUnit
{
    public IObservable<string> OnRemove();
    public IObservable<string> OnCollide();
    public int GetFruitLevel();
    public void Initialize(FruitEntity entity);
    public void Release();
    public void SetVisible(bool isVisible);
    public void SetPosition(Vector3 pos);
    public void SetParent(Transform parent);
}
