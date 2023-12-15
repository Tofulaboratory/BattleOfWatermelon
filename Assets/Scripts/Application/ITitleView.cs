using System;
using UniRx;

public interface ITitleView
{
    public IObservable<Unit> OnClickStartButton();
    public IObservable<Unit> OnClickMultiStartButton();
    public void SetActive(bool isActivate);
}
