using System.Threading.Tasks;
using Sandland.LockSystem.Interfaces;
using Sandland.LockSystem.Lockables;
using UnityEngine;

namespace Sandland.LockSystem.Demo
{
    public class LockDemo : MonoBehaviour
    {
        [SerializeField] private ForegroundTintLockable _tint;
        
        private ILockService _lockService;
        
        private void Awake()
        {
            _lockService = new BasicLockService();
            _lockService.AddLockable(_tint);
        }

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
            using (_lockService.TintLock())
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
            using var tintLock = _lockService.TintLock();
            
            Debug.Log("Showing Tint");
            await Task.Delay(5000);
            Debug.Log("The lock will be lifted here, because it's the end of the scope");
        }
    }
}