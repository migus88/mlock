using System;
using Migs.Examples.Shared;
using Migs.MLock.Interfaces;
using UnityEngine;

namespace Migs.MLock.Examples.Shared
{
    public class CarController : MonoBehaviour, ILockable<CarLockTags>
    {
        // The lock tags associated with this car controller
        public CarLockTags LockTags => CarLockTags.PlayerInput;
        
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 100f;
        
        private bool _isLocked;

        private void Awake()
        {
            CarLockService.Instance.Subscribe(this);
        }

        private void OnDestroy()
        {
            CarLockService.Instance.Unsubscribe(this);
        }
        
        private void Update()
        {
            if (_isLocked)
                return;
            
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            
            transform.Translate(Vector3.forward * verticalInput * _moveSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up * horizontalInput * _rotationSpeed * Time.deltaTime);
        }

        // Called when the car is locked
        public void HandleLocking()
        {
            _isLocked = true;
        }

        // Called when the car is unlocked
        public void HandleUnlocking()
        {
            _isLocked = false;
        }
    }
} 