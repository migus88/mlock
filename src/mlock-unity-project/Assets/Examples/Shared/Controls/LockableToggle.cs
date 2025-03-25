using Migs.MLock.Examples.Shared.Enums;
using Migs.MLock.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Migs.MLock.Examples.Shared.Controls
{
    public class LockableToggle : Toggle, ILockable<LockTags>
    {
        public LockTags LockTags => _lockTags;
        
        [SerializeField] private LockTags _lockTags;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            LockService.Instance.Subscribe(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LockService.Instance.Unsubscribe(this);
        }

        public void HandleLocking()
        {
            interactable = false;
        }

        public void HandleUnlocking()
        {
            interactable = true;
        }
    }
} 