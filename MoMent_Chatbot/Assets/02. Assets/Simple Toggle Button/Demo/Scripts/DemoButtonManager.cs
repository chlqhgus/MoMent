using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SimpleToggleButton
{
    public class DemoButtonManager : MonoBehaviour
    {
        [SerializeField]
        private Button demoSceneButton = null;

        [SerializeField]
        private string targetScene = string.Empty;

        private void Start()
        {
            demoSceneButton.onClick.AddListener(LoadScene);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(targetScene);
        }

        private void OnValidate()
        {
            if (demoSceneButton == null)
            {
                return;
            }

            demoSceneButton.gameObject.SetActive(Application
                .CanStreamedLevelBeLoaded(targetScene));
        }
    }
}
