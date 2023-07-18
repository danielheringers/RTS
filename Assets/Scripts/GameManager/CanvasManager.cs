using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour, ICanvasManager
{
    public GameObject canvasGameObject;

    public void ShowCanvas()
    {
        canvasGameObject.SetActive(true);
    }

    public void HideCanvas()
    {
        canvasGameObject.SetActive(false);
    }

    public bool IsCanvasVisible => canvasGameObject.activeSelf;

    public GameObject GetCanvasGameObject()
    {
        return canvasGameObject;
    }
}