using System;
using System.Collections.Generic;
using Sandland.LockSystem.Interfaces;
using UnityEngine;

namespace Sandland.LockSystem.Lockables
{
    public abstract class ForegroundTintLockable<TLockTag> : MonoBehaviour, ILockable<TLockTag> where TLockTag : Enum
    {
        public List<ILock<TLockTag>> Locks { get; } = new();
        [field:SerializeField] public TLockTag LockTag { get; set; }
        
        [SerializeField] private GameObject _tint;
        
        public void Lock()
        {
            _tint.SetActive(true);
        }

        public void Unlock()
        {
            _tint.SetActive(false);
        }
    }
}