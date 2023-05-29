using TMPro;
using UnityEngine;

namespace Assets.Scripts.Media
{
    /// <summary>
    /// A class used to display information about the current slide in the presentation using a TextMeshProUGUI component.
    /// </summary>
    /// <remarks>
    /// The class uses a PresentationManager component to obtain information about the current slide and displays it using a TextMeshProUGUI component.
    /// The component subscribes to PresentationManager events to update the displayed information when necessary.
    /// If no PresentationManager is found, the component displays an error message in the text informer.
    /// </remarks>
    [RequireComponent(typeof(PresentationManager))]
    public class SlideTextInformer : MonoBehaviour
    {
        private TextMeshProUGUI _textInformer;
        private PresentationManager _presentation;

        private int _totalCount = 0;
        
        private void Start()
        {
            _textInformer = GetComponentInChildren<TextMeshProUGUI>();
            _presentation = GetComponent<PresentationManager>();
            
            if (_presentation == null)
            {
                SetInformerText("<color=red>Component <PresentationManager> does not found!</color>");
                return;
            }
            
            // Set the total count of images in the presentation.
            _totalCount = _presentation.images.Count;
            
            Debug.Log($"Object ({gameObject.name}) initialized with {_totalCount} photos.");
            
            // Set the initial text of the text informer to show the current slide index and total count.
            SetInformerText($"{_presentation.currentImageIndex}/{_totalCount}");

            // Subscribe to PresentationManager events to update text informer when the slide changes or an error occurs.
            _presentation.OnImageChanged += HandleImageChanged;
            _presentation.OnErrorReceived += HandleErrorReceived;
        }

        private void OnDestroy()
        {
            // Unsubscribe from PresentationManager events when the component is destroyed.
            _presentation.OnImageChanged -= HandleImageChanged;
            _presentation.OnErrorReceived -= HandleErrorReceived;
        }

        /// <summary>
        /// Sets the text of the text informer.
        /// </summary>
        /// <param name="text">The text to display.</param>
        private void SetInformerText(string text)
        {
            _textInformer.SetText(text);
        }

        #region Action handlers
        /// <summary>
        /// Called when an error occurs in the PresentationManager.
        /// Displays an error message in the text informer with the given error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        private void HandleErrorReceived(string message)
        {
            Debug.LogError($"[{gameObject.name}] {message}");
            SetInformerText($"<color=red>{message}</color>");
        }
        
        /// <summary>
        /// Called when the slide changes in the PresentationManager.
        /// Updates the text informer to show the current slide index and total count.
        /// </summary>
        /// <param name="currentImageIndex">The index of the current slide.</param>
        private void HandleImageChanged(int currentImageIndex)
        {
            SetInformerText($"{currentImageIndex}/{_totalCount}");
        }
        #endregion
    }
}
