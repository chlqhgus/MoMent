using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleToggleButton
{
    /// <summary>
    ///  Sample script for the demo.
    ///  Feel free to delete this one.
    /// </summary>
    public class ImageColorManager : MonoBehaviour
    {
        [SerializeField]
        private ToggleButton redToggleButton = null;
        [SerializeField]
        private ToggleButton greenToggleButton = null;
        [SerializeField]
        private ToggleButton blueToggleButton = null;
        
        [SerializeField]
        private Image imageToColor = null;

        private void OnEnable()
        {
            redToggleButton.Clicked += OnButtonClicked;
            greenToggleButton.Clicked += OnButtonClicked;
            blueToggleButton.Clicked += OnButtonClicked;
        }

        private void OnDisable()
        {
            redToggleButton.Clicked -= OnButtonClicked;
            greenToggleButton.Clicked -= OnButtonClicked;
            blueToggleButton.Clicked -= OnButtonClicked;
        }

        private void OnButtonClicked(object sender, ToggleButtonClickedEventArgs e)
        {
            if (!e.IsOn)
            {
                if (!redToggleButton.IsOn &&
                    !greenToggleButton.IsOn &&
                    !blueToggleButton.IsOn)
                {
                    // resetting the color to original
                    imageToColor.color = Color.white;
                }

                return;
            }

            // disabling the other buttons
            if (e.Button != redToggleButton && redToggleButton.IsOn)
            {
                redToggleButton.Click();
            }
            if (e.Button != greenToggleButton && greenToggleButton.IsOn)
            {
                greenToggleButton.Click();
            }
            if (e.Button != blueToggleButton && blueToggleButton.IsOn)
            {
                blueToggleButton.Click();
            }

            imageToColor.color = e.Button.BackgroundColorOn;
        }
    }
}
