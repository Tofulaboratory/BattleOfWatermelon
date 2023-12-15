using System;
using Cysharp.Threading.Tasks;
using UniRx;

public interface IMatchingView
{
    public UniTask DirectMatchBeginAsync();
    public UniTask DirectMatchEndAsync();
    public void SetActive(bool isActivate);
}
