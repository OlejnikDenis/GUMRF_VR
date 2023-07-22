using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject canvas;

    public GameObject infoIcon;

    private bool isVisible = true;

    private void Start()
    {
        ShowCanvas();
    }
    
    public void OnButtonClick()
    {
        if (isVisible)
        {
            HideCanvas();
        }
        else
        {
            ShowCanvas();
        }
    }
    
    private void ShowCanvas()
    {
        canvas.SetActive(true);
        infoIcon.SetActive(false);
        isVisible = true;
    }

    private void HideCanvas()
    {
        canvas.SetActive(false);
        infoIcon.SetActive(true);
        isVisible = false;
    }
}
