using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script allows smoothly move between scenes using the screen fade
/// </summary>

public class SmoothSceneSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject sceneSwitcherCanvas;
    [SerializeField, Range(0, 5f)] private float transitionTime = 2.5f;

    private CanvasGroup _canvasGroup;
    private Canvas _canvas;

    public void ExitGameWithFade()
    {
        StartCoroutine(FadeCanvasGroup(true, null));
        StartCoroutine(ExitApplication());
    }
    
    /// <summary>
    /// Initiates a smooth scene transition by fading the elements of the CanvasGroup in or out, depending on the target scene.
    /// </summary>
    /// <param name="sceneName">The name of the target scene to switch to.</param>
    public void FadeCanvasGroupWrapper(string sceneName)
    {
        // Is there a scene with that name?
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("[SmoothSceneSwitcher] Not specified scene name for transition!", gameObject);
            return;
        } 
        else
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName))
            {
                StartCoroutine(FadeCanvasGroup(true, sceneName));
            }
            else
            {
                Debug.LogError($"[SmoothSceneSwitcher] The specified scene name ({sceneName}) does not exist!", gameObject);
                return;
            }
        }
    }

    private void Awake()
    {
        _canvasGroup = sceneSwitcherCanvas.GetComponent<CanvasGroup>();
        _canvas = sceneSwitcherCanvas.GetComponent<Canvas>();

        if (_canvas.worldCamera == null)
        {
            Debug.LogWarning("[SmoothSceneSwitcher] Camera not selected! Trying to find with tag 'MainCamera'", gameObject);
            if (Camera.main != null)
            {
                _canvas.worldCamera = Camera.main;
            }
            else
            {
                Debug.LogError("[SmoothSceneSwitcher] Camera with tag 'MainCamera' not found!", gameObject);
                return;
            }
        }

        // Enable transition effect
        StartCoroutine(FadeCanvasGroup(false));
    }


    /// <summary>
    /// The method controls the fading during transitions between scenes.
    /// </summary>
    /// <param name="fade">if true -> black, else transparent</param>
    /// <param name="sceneName">Scene name from build settings</param>
    /// <returns>null</returns>
    private IEnumerator FadeCanvasGroup(bool fade, string sceneName = null)
    {
        float startTransition = Time.time;
        float endTransition = startTransition + transitionTime;
       
        sceneSwitcherCanvas.SetActive(true);

        // Small "planeDistance" allows to hide objects nearby camera
        _canvas.planeDistance = 0.1f; 

        if (fade)
        {
            // Transition from transparent to black
            while (Time.time <= endTransition)
            {
                float transitionAlpha = Mathf.InverseLerp(startTransition, endTransition, Time.time);
                _canvasGroup.alpha = transitionAlpha;
                yield return null;
            }
            // Sometimes, the value is very close, but not equal to 1. This is a fix
            _canvasGroup.alpha = 1;
            
            if (!string.IsNullOrEmpty(sceneName))
                SceneManager.LoadSceneAsync(sceneName);
        }
        else
        {
            // Transition from black to transparent
            while (Time.time <= endTransition)
            {
                float transitionAlpha = Mathf.InverseLerp(endTransition, startTransition, Time.time);
                _canvasGroup.alpha = transitionAlpha;
                yield return null;
            }
            // Fix 0.02.. -> 0.0
            _canvasGroup.alpha = 0;
            sceneSwitcherCanvas.SetActive(false);
        }
    }

    private IEnumerator ExitApplication()
    {
        yield return new WaitForSeconds(transitionTime);
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
