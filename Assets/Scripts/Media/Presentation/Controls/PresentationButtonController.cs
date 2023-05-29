using Assets.Scripts.Media;
using UnityEngine;
using UnityEngine.UI;

public class PresentationButtonController : MonoBehaviour
{
    [SerializeField] private PresentationManager _presentationManager;

    private Button _buttonAutoSlide;
    private Button _buttonNextSlide;
    private Button _buttonPreviousSlide;

    private ColorBlock _buttonAutoSlideColorBlock;
    
    private void Start()
    {
        _buttonAutoSlide = BindButton("ButtonAutoSlide");
        _buttonNextSlide = BindButton("ButtonNextSlide");
        _buttonPreviousSlide = BindButton("ButtonPreviousSlide");

        _buttonAutoSlideColorBlock = _buttonAutoSlide.colors;
        
        UpdateAutoSlideButton();
    }

    private Button BindButton(string buttonName)
    {
        var button = GameObject.Find(buttonName)?.GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError($"{this.name} failed to bind the button {buttonName}!");
        }
        else
        {
            switch (buttonName)
            {
                case "ButtonAutoSlide":
                    button.onClick.AddListener(ToggleSlideShow);
                    break;
                case "ButtonNextSlide":
                    button.onClick.AddListener(NextSlide);
                    break;
                case "ButtonPreviousSlide":
                    button.onClick.AddListener(PreviousSlide);
                    break;
            }
        }
        return button;
    }

    private void UpdateAutoSlideButton()
    {
        if (_buttonAutoSlide != null)
        {
            var buttonColors = _buttonAutoSlideColorBlock;
            
            if (_presentationManager.SlideShow)
            {
                buttonColors.highlightedColor = Color.white;
                buttonColors.normalColor = Color.white;
            }
            else
            {
                buttonColors.highlightedColor = Color.gray;
                buttonColors.normalColor = Color.gray;
            }

            _buttonAutoSlide.colors = buttonColors;
        }
    }
    
    private void ToggleSlideShow()
    {
        _presentationManager.ToggleSlideShow();
        UpdateAutoSlideButton();
    }

    private void NextSlide()
    {
        _presentationManager.StopSlideShow();
        _presentationManager.ShowNextImage();
        UpdateAutoSlideButton();
    }

    private void PreviousSlide()
    {
        _presentationManager.StopSlideShow();
        _presentationManager.ShowPreviousImage();
        UpdateAutoSlideButton();
    }
}
