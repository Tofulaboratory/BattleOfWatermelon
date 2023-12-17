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
    //全員ただしなので、プレイヤー2のみ変更
    [SerializeField] private Image player2Image;
    [SerializeField] private TMP_Text player2Name;

    [SerializeField] private Sprite playerSprite;
    private readonly string playerName = "ただし";
    [SerializeField] private Sprite unknownSprite;
    private readonly string unknownName = "?";
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

        player2Image.SetNativeSize();
    }

    private void ApplyUnknownInfo()
    {
        player2Image.sprite = unknownSprite;
        player2Name.text = unknownName;

        player2Image.SetNativeSize();
    }

    /// <summary>
    /// マッチング開始演出
    /// </summary>
    public async UniTask DirectMatchBeginAsync()
    {
        ApplyUnknownInfo();
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
