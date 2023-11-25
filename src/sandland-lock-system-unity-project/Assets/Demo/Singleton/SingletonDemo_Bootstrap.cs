using System;
using System.Threading.Tasks;
using Sandland.LockSystem.Demo.Shared;
using Sandland.LockSystem.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Sandland.LockSystem.Demo.Singleton
{
    public class SingletonDemo_Bootstrap : MonoBehaviour
    {
        [SerializeField] private Button _tintButton;
        [SerializeField] private Button _loadingButton;
        [SerializeField] private Button _transparentButton;
        
        private void Awake()
        {
            _tintButton.onClick.AddListener(OnTintPressed);
            _loadingButton.onClick.AddListener(OnLoadingPressed);
            _transparentButton.onClick.AddListener(OnTransparentPressed);
        }

        private async void OnTransparentPressed() => await ShowTint(OverlayTag.Transparent);

        private async void OnLoadingPressed() => await ShowTint(OverlayTag.Loading);

        private async void OnTintPressed() => await ShowTint(OverlayTag.Tint);

        private async Task ShowTint(OverlayTag overlayTag)
        {
            Debug.Log("This printed before the overlay is shown");
            await Task.Delay(2000);

            using (SingletonDemo_UiOverlayService.Instance.LockOnly(overlayTag))
            {
                Debug.Log("The overlay is displayed now and will be until the end of this scope");

                await Task.Delay(3000);
            }
            
            Debug.Log("The overlay is hidden at this point");
            await Task.Delay(500);

            using var @lock = SingletonDemo_UiOverlayService.Instance.LockOnly(overlayTag);
            
            Debug.Log("Now the overlay is shown again, but this time it's till the end of the method");
            await Task.Delay(2000);
            
            Debug.Log("The overlay is still shown, but it will be hidden after this will be printed");
        }
    }
}