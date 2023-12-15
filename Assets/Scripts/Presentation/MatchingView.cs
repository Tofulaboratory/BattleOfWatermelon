using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class MatchingView : ViewBase, IMatchingView
{
    //全員たかしなので、プレイヤー2のみ変更
    [SerializeField] private Image player2Image;
    [SerializeField] private TMP_Text player2Name;

    [SerializeField] private Sprite playerSprite;
    private readonly string playerName = "たかし";
    private readonly int directMatchEndDuration = 500;

    /// <summary>
    /// フレームがinする演出
    /// </summary>
    private async UniTask DirectSlideInFrameAsync()
    {
        //TODO
        await UniTask.Delay(500);
    }

    /// <summary>
    /// フレームがoutする演出
    /// </summary>
    private async UniTask DirectSlideOutFrameAsync()
    {
        //TODO
        await UniTask.Delay(500);
    }

    private void ApplyPlayerInfo()
    {
        player2Image.sprite = playerSprite;
        player2Name.text = playerName;
    }

    /// <summary>
    /// マッチング開始演出
    /// </summary>
    public async UniTask DirectMatchBeginAsync()
    {
        await DirectSlideInFrameAsync();
    }

    /// <summary>
    /// マッチング終了演出
    /// </summary>
    public async UniTask DirectMatchEndAsync()
    {
        ApplyPlayerInfo();
        await UniTask.Delay(directMatchEndDuration);
        await DirectSlideOutFrameAsync();
        SetActive(false);
    }

    public void SetActive(bool isActivate)
    {
        base.SetActiveCanvasGroup(isActivate);
    }
}
