using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sandland.LockSystem.Demo.Shared;
using Sandland.LockSystem.Interfaces;
using Sandland.LockSystem.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sandland.LockSystem.Demo.Singleton
{
    public abstract class LockPanel : MonoBehaviour
    {
        protected abstract ILockService<InputLockTag> Service { get; }
        
        [SerializeField] private InputLockTag _tag;
        [SerializeField] private TextMeshProUGUI _counter;
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _removeButton;
        [SerializeField] private Button _fiveSecondsLockButton;
        [SerializeField] private Button _tenSecondsLockButton;
        

        private readonly Queue<ILock<InputLockTag>> _locks = new ();
        private int _tempLocksCount = 0;

        private void Awake()
        {
            _addButton.onClick.AddListener(OnAddClicked);
            _removeButton.onClick.AddListener(OnRemoveClicked);
            _fiveSecondsLockButton.onClick.AddListener(OnFiveSecondsLockClicked);
            _tenSecondsLockButton.onClick.AddListener(OnTenSecondsLockClicked);
        }

        private void Update()
        {
            UpdateCounter();
        }

        private async void OnTenSecondsLockClicked()
        {
            // This lock will live until the end of the method (note the 'using' keyword)
            using var @lock = Service.LockOnly(_tag);
            
            _tempLocksCount++;
            await Task.Delay(10000);
            _tempLocksCount--;
        }

        private async void OnFiveSecondsLockClicked()
        {
            // Lock will work only inside the using scope
            using (Service.LockOnly(_tag))
            {
                _tempLocksCount++;
                await Task.Delay(5000);
            }

            _tempLocksCount--;
        }

        private void OnRemoveClicked()
        {
            if (_locks.Count == 0)
            {
                return;
            }
            
            var @lock = _locks.Dequeue();
            @lock.Unlock();
        }

        private void OnAddClicked()
        {
            var @lock = Service.LockOnly(_tag);
            _locks.Enqueue(@lock);
        }

        private void UpdateCounter()
        {
            _counter.text = (_locks.Count + _tempLocksCount).ToString();
        }
    }
}