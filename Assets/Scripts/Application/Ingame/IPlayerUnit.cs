using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerUnit
{
    public void Initialize(PlayerEntity entity);
    public void HoldFruit(IFruitUnit fruitUnit);
    public Vector3 GetPosition();
    public void MovePosition(float direction);
    public Transform GetTransform();
    public void ReleaseFruit();
    public GameObject GetObj();
    public void ChangeColor(int index);
}
