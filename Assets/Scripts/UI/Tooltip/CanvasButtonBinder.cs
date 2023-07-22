using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasButtonBinder : MonoBehaviour
{
    public CanvasSwitcher canvasSwitcher;

    public Button switchCanvasButton;
    
    void Start()
    {
        switchCanvasButton.onClick.AddListener(canvasSwitcher.OnButtonClick);
    }
}
