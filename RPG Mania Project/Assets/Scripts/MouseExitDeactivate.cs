using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseExitDeactivate : MonoBehaviour
{
    public GameObject hoverIndicator;
    public bool alwaysActive = false;
    void OnMouseExit()
    {
        if(!alwaysActive){
            hoverIndicator.SetActive(false);
        }
    }
}
