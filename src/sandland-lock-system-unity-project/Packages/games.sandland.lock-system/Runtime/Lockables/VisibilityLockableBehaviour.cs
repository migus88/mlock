using System;
using System.Collections.Generic;
using Sandland.LockSystem.Interfaces;
using UnityEngine;

namespace Sandland.LockSystem.Lockables
{
    public abstract class VisibilityLockableBehaviour<TLockTag> : MonoBehaviour, ILockable<TLockTag> where TLockTag : Enum
    {
        public List<ILock<TLockTag>> Locks { get; } = new();
        [field:SerializeField] public TLockTag LockTag { get; set; }
        
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