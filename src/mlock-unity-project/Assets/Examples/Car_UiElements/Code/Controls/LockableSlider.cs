using System;
using Migs.Examples.Shared;
using Migs.MLock.Examples.Shared;
using Migs.MLock.Interfaces;
using UnityEngine.UIElements;

namespace Migs.MLock.Examples.Car.UiElements.Code.Controls
{
    public class LockableSlider : Slider, ILockable<CarLockTags>, IDisposable
    {
        public CarLockTags LockTags { get; }

        public LockableSlider(CarLockTags lockTags, float start, float end) : base(start, end)
        {
            LockTags = lockTags;
            CarLockService.Instance.Subscribe(this);
        }

        public void HandleLocking()
        {
            SetEnabled(false);
        }

        public void HandleUnlocking()
        {
            SetEnabled(true);
        }

        public void Dispose()
        {
            CarLockService.Instance.Unsubscribe(this);
        }
    }
}