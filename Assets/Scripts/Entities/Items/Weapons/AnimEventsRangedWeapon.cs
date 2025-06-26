using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEventsRangedWeapon : MonoBehaviour
{
    public UnityEvent OnLeverClickEvent, OnShellEjectEvent, OnFullReloadEvent;

    void OnLeverClick() => OnLeverClickEvent?.Invoke();
    void OnShellEject() => OnShellEjectEvent?.Invoke();
    void OnFullReload() => OnFullReloadEvent?.Invoke();
}
