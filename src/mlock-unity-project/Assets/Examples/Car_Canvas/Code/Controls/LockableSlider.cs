using System;
using Migs.Examples.Shared;
using Migs.MLock.Examples.Shared;
using Migs.MLock.Interfaces;
using UnityEngine.UI;

namespace Migs.MLock.Examples.Car_Canvas.Code.Controls
{
    public class LockableSlider : Slider, ILockable<CarLockTags>
    {
        public CarLockTags LockTags { get; set; }

        protected override void Awake()
        {
            base.Awake();
            CarLockService.Instance.Subscribe(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            CarLockService.Instance.Unsubscribe(this);
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