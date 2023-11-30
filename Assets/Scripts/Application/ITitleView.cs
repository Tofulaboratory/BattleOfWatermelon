using System;
using UniRx;

public interface ITitleView
{
    public IObservable<Unit> OnClickStartButton();
    public void SetActive(bool isActivate);
}
