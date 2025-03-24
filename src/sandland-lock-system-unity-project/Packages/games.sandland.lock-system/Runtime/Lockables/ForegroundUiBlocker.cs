using System;
using System.Collections.Generic;
using Sandland.LockSystem.Interfaces;
using UnityEngine;

namespace Sandland.LockSystem.Lockables
{
    public abstract class ForegroundUiBlocker<TLockTags> : MonoBehaviour, ILockable<TLockTags> where TLockTags : Enum
    {
        public List<ILock<TLockTags>> Locks { get; } = new();
        [field:SerializeField] public TLockTags LockTags { get; set; }
        
        [SerializeField] private GameObject _tint;

        protected virtual void Awake()
        {
            _tint.SetActive(false);
        }

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