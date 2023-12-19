using System;
using UniRx;

public interface ITitleView
{
    public IObservable<Unit> OnClickStartButton();
    public IObservable<Unit> OnClickMultiStartButton();
    public IObservable<Unit> OnClickSettingButton();
    public void SetActive(bool isActivate);
}
