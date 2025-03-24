using System;
using System.Collections.Generic;
using Sandland.LockSystem.Interfaces;
using UnityEngine;

namespace Sandland.LockSystem.Demo.Shared
{
    public abstract class PlayerController : MonoBehaviour, ILockable<InputLockTags>
    {
        [field:SerializeField] public InputLockTags LockTags { get; set; }
        public List<ILock<InputLockTags>> Locks { get; } = new();
        protected abstract ILockService<InputLockTags> Service { get; }

        [SerializeField] private float _maxSpeed = 50f;
        [SerializeField] private float _velocity = 5f;
        [SerializeField] private float _turningSpeed = 50f;
        [SerializeField] private Rigidbody _rigidbody;
        
        private float _horizontalAxis;
        private float _verticalAxis;
        private bool _isLocked;
    
        private void Update()
        {
            _verticalAxis = Input.GetAxis("Vertical");
            _horizontalAxis = Input.GetAxis("Horizontal");
        }
        
        private void OnEnable()
        {
            // Registering this lockable in the service
            Service.AddLockable(this);
        }

        private void OnDisable()
        {
            // Removing the registration of this class from the service
            Service.RemoveLockable(this, true);
        }

        private void FixedUpdate()
        {
            if (_isLocked)
            {
                return;
            }
            
            // Accelerate or decelerate the car
            var desiredForwardMovement = transform.forward * (_verticalAxis * _velocity);
            var newVelocity = _rigidbody.linearVelocity + desiredForwardMovement;
        
            // Limit the speed
            newVelocity = Vector3.ClampMagnitude(newVelocity, _maxSpeed);
        
            // Apply the speed after clamping
            _rigidbody.linearVelocity = new Vector3(newVelocity.x, _rigidbody.linearVelocity.y, newVelocity.z);

            // Turn the car left or right
            var turn = _horizontalAxis * _turningSpeed * Time.deltaTime;
            var turnRotation = Quaternion.Euler(0f, turn, 0f);
            _rigidbody.MoveRotation(_rigidbody.rotation * turnRotation);
        }
        public void Lock()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _isLocked = true;
        }

        public void Unlock()
        {
            _isLocked = false;
        }
    }
}