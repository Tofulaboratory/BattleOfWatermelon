using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIngameView
{
    public void ApplyNextFrame(FruitEntity entity);
    public void SetActive(bool isActivate);
}