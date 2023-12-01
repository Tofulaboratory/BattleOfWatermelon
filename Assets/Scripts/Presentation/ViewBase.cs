using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ViewBase : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public void SetActiveCanvasGroup(bool isActivate)
    {
        canvasGroup.alpha = isActivate ? 1 : 0;
        canvasGroup.blocksRaycasts = isActivate;
        canvasGroup.interactable = isActivate;
    }
}
