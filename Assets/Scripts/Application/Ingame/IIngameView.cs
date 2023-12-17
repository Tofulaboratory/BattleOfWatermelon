using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IIngameView
{
    public void ApplyNextFrame(FruitEntity entity);
    public void ApplyScoreText(int index,int score);
    public void ApplyTurnIndicator(string name);
    public void SetActiveScore2(bool isActivate);
    public void SetActive(bool isActivate);
    public UniTask ShowBeltAsync(string name);
}