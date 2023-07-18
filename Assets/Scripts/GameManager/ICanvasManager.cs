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
