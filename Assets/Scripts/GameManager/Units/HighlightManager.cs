using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightManager : MonoBehaviour
{
    private Transform highlightedObj;
    private Transform selectedObj;
    public LayerMask selectableLayer;

    private Outline highLightOutline;
    private RaycastHit hit;

    void Update()
    {
        HoverHighLight();
    }
    public void HoverHighLight()
    {
        if(highlightedObj != null)
        {
            highLightOutline.enabled = false;
            highlightedObj  = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, selectableLayer))
        {
            highlightedObj = hit.transform;

            if(highlightedObj.CompareTag("Enemy") && highlightedObj != selectedObj)
            {
                highLightOutline = highlightedObj.GetComponent<Outline>();
                highLightOutline.enabled = true;
            }
            else
            {
                highlightedObj = null;
            }
        }
    }

    public void SelectedHighlight()
    {
        if (highlightedObj.CompareTag("Enemy"))
        {
            if(selectedObj != null)
            {
                selectedObj.GetComponent<Outline>().enabled = false;
            }

            selectedObj = hit.transform;
            selectedObj.GetComponent<Outline>().enabled = true;

            highLightOutline.enabled = true;
            highlightedObj = null;
        }
    }

    public void DeselectHighLight()
    {
        selectedObj.GetComponent<Outline>().enabled = false;
        selectedObj = null;
    }
}
