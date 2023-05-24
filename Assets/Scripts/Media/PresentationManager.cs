using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Assets.Scripts.Media
{
    /// <summary>
    /// A class used for controlling the presentation of images in a specified folder and its subfolders using a game object's renderer to display the images.
    /// </summary>
    /// <remarks>
    /// The class uses the `ImageSearcher` component to find images in the specified folder and its subfolders, and the game object's renderer to display the images.
    /// The class allows for manual control of the presentation using the `ShowNextImage()` and `ShowPreviousImage()` methods or automatic showing of images using the `AutoShowNext` property.
    /// </remarks>
    [RequireComponent(typeof(ImageSearcher), typeof(SlideTextInformer))]
    public class PresentationManager : MonoBehaviour
    {
        [SerializeField] public bool AutoShowNext = true;
        [SerializeField] [Range(1f, 20f)] public float AutoShowNextInterval = 5f;

        public int currentImageIndex { get; private set; }
        public List<string> images { get; private set; }
        
        private Renderer _renderer;
        private ImageSearcher _imageSearcher;
        private Coroutine _autoShowNextCoroutine;
        
        public Action<int> OnImageChanged;
        public Action<string> OnErrorReceived;
        
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _imageSearcher = GetComponent<ImageSearcher>();
            images = _imageSearcher.imagePaths;

            ToggleAutoShowNext();
        }
        
        #region Presentation slide controller
        /// <summary>
        /// Loads an image from a specified file path and sets it as the main texture of the game object's renderer.
        /// </summary>
        /// <param name="pathToImage">The file path to the image.</param>
        private void ShowImage(string pathToImage)
        {
            // Check if the file exists
            if (!File.Exists(pathToImage))
            {
                var errorMessage = $"File does not exist ({pathToImage})";
                
                Debug.LogError(errorMessage);
                OnErrorReceived?.Invoke(errorMessage);
                
                return;
            }
        
            // Load the image into a texture
            var texture = new Texture2D(2, 2);
            var imageData = File.ReadAllBytes(pathToImage);
            texture.LoadImage(imageData);
        
            // Apply the texture to the renderer's material
            _renderer.material.mainTexture = texture;

            OnImageChanged?.Invoke(currentImageIndex + 1);
        }

        /// <summary>
        /// Shows the next image in the list of images. Increments the current image index.
        /// </summary>
        public void ShowNextImage()
        {
            if (currentImageIndex >= images.Count)
            {
                return;
            }

            ShowImage(images[currentImageIndex]);
            currentImageIndex++;
        }

        /// <summary>
        /// Shows the previous image in the list of images. Decrements the current image index.
        /// </summary>
        public void ShowPreviousImage()
        {
            if (currentImageIndex <= 0)
            {
                return;
            }

            currentImageIndex--;
            ShowImage(images[currentImageIndex]);
        }

        /// <summary>
        /// Automatically shows the next image in the list of images every `switchIntervalSeconds`  while `AutoShowNext` is true.
        /// </summary>
        /// <param name="switchIntervalSeconds">The number of seconds between each automatic image switch.</param>
        /// <returns>An IEnumerator used for starting a coroutine.</returns>
        private IEnumerator ShowNextImageAutomatically(float switchIntervalSeconds)
        {
            while(AutoShowNext)
            {
                ShowNextImage(); 

                yield return new WaitForSeconds(switchIntervalSeconds);
            }
        }
        
        /// <summary>
        /// Enables or disables the automatic switching of images.
        /// </summary>
        public void ToggleAutoShowNext()
        {
            // Set the AutoShowNext variable to the given value
            AutoShowNext = !AutoShowNext;

            // Start or stop the coroutine based on the value of AutoShowNext
            if (AutoShowNext && _autoShowNextCoroutine == null)
            {
                _autoShowNextCoroutine = StartCoroutine(ShowNextImageAutomatically(AutoShowNextInterval));
            }
            else if (!AutoShowNext && _autoShowNextCoroutine != null)
            {
                StopCoroutine(_autoShowNextCoroutine);
                _autoShowNextCoroutine = null;
            }
        }
        
        #endregion
    }
}
