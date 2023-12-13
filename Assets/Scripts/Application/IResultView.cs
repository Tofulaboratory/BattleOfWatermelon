using System;
using UniRx;

public interface IResultView
{
    public IObservable<Unit> OnClickTitleButton();
    public void SetActive(bool isActivate);
}
