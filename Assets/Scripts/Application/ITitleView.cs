using System;
using UniRx;

public interface ITitleView
{
    public IObservable<Unit> OnClickStartButton();
}
