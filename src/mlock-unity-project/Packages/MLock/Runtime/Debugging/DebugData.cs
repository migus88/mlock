using System;
using System.Collections.Generic;
using System.Linq;
using Migs.MLock.Interfaces;
using UnityEngine;

namespace Migs.MLock.Debugging
{
    /// <summary>
    /// Static class that holds debug data for MLock system
    /// Follows the Single Responsibility principle by only handling debug data collection and storage
    /// </summary>
    public static class DebugData
    {
        // Dictionary to store all lock services by type
        private static readonly Dictionary<Type, ILockService> LockServices = new Dictionary<Type, ILockService>();
        
        // Lock data for the editor window
        private static readonly List<LockDebugInfo> ActiveLocks = new List<LockDebugInfo>();
        
        // Lock data time cache
        private static double _lastUpdateTime;
        
        // Is debug data collection enabled
        private static bool _isEnabled;
        
        /// <summary>
        /// Register a lock service for debugging
        /// </summary>
        /// <param name="service">The lock service to register</param>
        /// <typeparam name="TLockTags">The enum type used for lock tags</typeparam>
        public static void RegisterLockService<TLockTags>(ILockService<TLockTags> service) where TLockTags : Enum
        {
            if (service == null) return;
            
            var type = typeof(TLockTags);
            if (!LockServices.ContainsKey(type))
            {
                LockServices[type] = service;
                UnityEngine.Debug.Log($"[MLock Debug] Registered lock service for {type.Name}");
            }
        }
        
        /// <summary>
        /// Unregister a lock service
        /// </summary>
        /// <param name="service">The lock service to unregister</param>
        /// <typeparam name="TLockTags">The enum type used for lock tags</typeparam>
        public static void UnregisterLockService<TLockTags>(ILockService<TLockTags> service) where TLockTags : Enum
        {
            if (service == null) return;
            
            var type = typeof(TLockTags);
            if (LockServices.ContainsKey(type) && LockServices[type] == service)
            {
                LockServices.Remove(type);
                UnityEngine.Debug.Log($"[MLock Debug] Unregistered lock service for {type.Name}");
            }
        }
        
        /// <summary>
        /// Enable or disable debug data collection
        /// </summary>
        /// <param name="enabled">Whether to enable debug data collection</param>
        public static void SetEnabled(bool enabled)
        {
            _isEnabled = enabled;
        }
        
        /// <summary>
        /// Update debug data by scanning all registered lock services
        /// </summary>
        public static void UpdateData()
        {
            if (!_isEnabled) return;
            
            // Only update at reasonable intervals
            if (Time.realtimeSinceStartup - _lastUpdateTime < 0.5f) return;
            _lastUpdateTime = Time.realtimeSinceStartup;
            
            ActiveLocks.Clear();
            
            foreach (var pair in LockServices)
            {
                var lockTagsType = pair.Key;
                var service = pair.Value;
                
                // Use reflection to access private fields
                // This is only for debugging purposes and only in editor mode
                var serviceType = service.GetType();
                var activeLocks = serviceType.GetField("_activeLocks", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(service);
                
                var lockableToDataMap = serviceType.GetField("_lockableToDataMap", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(service);
                
                if (activeLocks == null || lockableToDataMap == null) continue;
                
                // Get lock info via reflection
                var lockInfoMethod = typeof(DebugData).GetMethod("GetLockInfo", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                
                var genericMethod = lockInfoMethod?.MakeGenericMethod(lockTagsType);
                genericMethod?.Invoke(null, new[] { activeLocks, lockableToDataMap, ActiveLocks });
            }
        }
        
        /// <summary>
        /// Get all active locks
        /// </summary>
        /// <returns>A list of active locks</returns>
        public static List<LockDebugInfo> GetActiveLocks()
        {
            return ActiveLocks;
        }
        
        /// <summary>
        /// Get all registered lock services
        /// </summary>
        /// <returns>A dictionary of lock services by type</returns>
        public static Dictionary<Type, ILockService> GetLockServices()
        {
            return LockServices;
        }
        
        /// <summary>
        /// Unlock a specific lock
        /// </summary>
        /// <param name="lockId">The ID of the lock to unlock</param>
        /// <returns>True if the lock was found and unlocked, false otherwise</returns>
        public static bool UnlockById(int lockId)
        {
            foreach (var pair in LockServices)
            {
                var lockTagsType = pair.Key;
                var service = pair.Value;
                
                // Use reflection to unlock
                var unlockMethod = typeof(DebugData).GetMethod("TryUnlockById", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                
                var genericMethod = unlockMethod?.MakeGenericMethod(lockTagsType);
                var result = (bool)(genericMethod?.Invoke(null, new object[] { service, lockId }) ?? false);
                
                if (result) return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Unlock all locks
        /// </summary>
        public static void UnlockAll()
        {
            foreach (var pair in LockServices)
            {
                var lockTagsType = pair.Key;
                var service = pair.Value;
                
                // Use reflection to unlock all
                var unlockAllMethod = typeof(DebugData).GetMethod("TryUnlockAll", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                
                var genericMethod = unlockAllMethod?.MakeGenericMethod(lockTagsType);
                genericMethod?.Invoke(null, new object[] { service });
            }
        }
        
        // Generic helper method for getting lock info - will be called through reflection
        private static void GetLockInfo<TLockTags>(
            HashSet<ILock<TLockTags>> activeLocks,
            Dictionary<ILockable<TLockTags>, ILockableData<TLockTags>> lockableToDataMap,
            List<LockDebugInfo> result) where TLockTags : Enum
        {
            foreach (var @lock in activeLocks)
            {
                var lockInfo = new LockDebugInfo
                {
                    Id = @lock.Id,
                    LockType = typeof(TLockTags).Name,
                    IncludeTags = @lock.IncludeTags?.ToString() ?? "None",
                    ExcludeTags = @lock.ExcludeTags?.ToString() ?? "None",
                    AffectedLockables = new List<string>()
                };
                
                // Find affected lockables
                foreach (var pair in lockableToDataMap)
                {
                    var lockable = pair.Key;
                    var data = pair.Value;
                    
                    if (data.Locks.Contains(@lock))
                    {
                        lockInfo.AffectedLockables.Add(lockable.ToString());
                    }
                }
                
                result.Add(lockInfo);
            }
        }
        
        // Generic helper method for unlocking by ID - will be called through reflection
        private static bool TryUnlockById<TLockTags>(ILockService service, int lockId) where TLockTags : Enum
        {
            var lockService = (ILockService<TLockTags>)service;
            var activeLocks = typeof(BaseLockService<TLockTags>)
                .GetField("_activeLocks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(lockService) as HashSet<ILock<TLockTags>>;
            
            if (activeLocks == null) return false;
            
            var lockToRemove = activeLocks.FirstOrDefault(l => l.Id == lockId);
            if (lockToRemove != null)
            {
                lockToRemove.Dispose();
                return true;
            }
            
            return false;
        }
        
        // Generic helper method for unlocking all - will be called through reflection
        private static void TryUnlockAll<TLockTags>(ILockService service) where TLockTags : Enum
        {
            var lockService = (ILockService<TLockTags>)service;
            var activeLocks = typeof(BaseLockService<TLockTags>)
                .GetField("_activeLocks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(lockService) as HashSet<ILock<TLockTags>>;
            
            if (activeLocks == null) return;
            
            // Create a copy of the collection to avoid modification during enumeration
            var locksToRemove = activeLocks.ToList();
            foreach (var @lock in locksToRemove)
            {
                @lock.Dispose();
            }
        }
    }
    
    /// <summary>
    /// Debug information for a lock
    /// </summary>
    public class LockDebugInfo
    {
        public int Id { get; set; }
        public string LockType { get; set; }
        public string IncludeTags { get; set; }
        public string ExcludeTags { get; set; }
        public List<string> AffectedLockables { get; set; }
    }
} 