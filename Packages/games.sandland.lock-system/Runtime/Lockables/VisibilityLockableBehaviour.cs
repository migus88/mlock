using System.Collections.Generic;
using Sandland.LockSystem.Interfaces;
using UnityEngine;

namespace Sandland.LockSystem.Lockables
{
    public class VisibilityLockableBehaviour : MonoBehaviour, ILockable
    {
        [field:SerializeField] public string Category { get; set; }
        public List<ILock> Locks { get; } = new();
        
        public void Lock()
        {
            gameObject.SetActive(false);
        }

        public void Unlock()
        {
            gameObject.SetActive(true);
        }
    }
}