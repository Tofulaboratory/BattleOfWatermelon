using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIngameView
{
    public void ApplyNextFrame(FruitEntity entity);
    public void ApplyScoreText(int index,int score);
    public void ApplyTurnIndicator(string name);
    public void SetActive(bool isActivate);
}