using System;
using System.Collections.Generic;
using Sandland.LockSystem.Interfaces;
using UnityEngine;

namespace Sandland.LockSystem.Lockables
{
    public abstract class VisibilityLockableBehaviour<TLockTags> : MonoBehaviour, ILockable<TLockTags> where TLockTags : Enum
    {
        public List<ILock<TLockTags>> Locks { get; } = new();
        [field:SerializeField] public TLockTags LockTags { get; set; }
        
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