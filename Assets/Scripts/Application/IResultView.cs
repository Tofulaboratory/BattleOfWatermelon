using System;
using UniRx;

public interface IResultView
{
    public IObservable<Unit> OnClickTitleButton();
    public void ApplyResultText(string text);
    public void SetActive(bool isActivate);
}
