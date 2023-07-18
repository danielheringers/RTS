using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanvasManager
{
    void ShowCanvas();
    void HideCanvas();
    bool IsCanvasVisible { get; }
    GameObject GetCanvasGameObject();
}

public interface IInteractable
{
    void Interact();
    void HideCanvas();
}