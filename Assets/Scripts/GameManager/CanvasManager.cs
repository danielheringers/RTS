using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour, IInteractable
{
    public GameObject canvasGameObject;

    public void ShowCanvas()
    {
        canvasGameObject.gameObject.SetActive(true);
    }

    public void HideCanvas()
    {
        canvasGameObject.gameObject.SetActive(false);
    }

    public bool IsCanvasVisible => canvasGameObject.gameObject.activeSelf;

    public GameObject GetCanvasGameObject()
    {
        return canvasGameObject;
    }

    public void Interact()
    {

    }
}