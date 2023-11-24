using System.Threading.Tasks;
using Sandland.LockSystem.Demo.Shared;
using Sandland.LockSystem.Utils;
using UnityEngine;

namespace Sandland.LockSystem.Demo.Singleton
{
    public class SingletonLockDemo : MonoBehaviour
    {
        private async void Start()
        {
            Debug.Log("Waiting 2 seconds");
            await Task.Delay(2000);

            await WithBraces();
            
            Debug.Log("Waiting 2 seconds");
            await Task.Delay(2000);

            await NoBraces();
        }

        private async Task WithBraces()
        {
            using (SingleLockService.Instance.LockOnly(LockTag.Tint))
            {
                Debug.Log("Showing Tint");
                await Task.Delay(5000);
                Debug.Log("Hiding Tint");
            }
            
            Debug.Log("This will be printed after the tint disappeared.");
            await Task.Delay(2000);
        }

        private async Task NoBraces()
        {
            using var tintLock = SingleLockService.Instance.LockOnly(LockTag.Tint);
            
            Debug.Log("Showing Tint");
            await Task.Delay(5000);
            Debug.Log("The lock will be lifted here, because it's the end of the scope");
        }
    }
}