using System.Collections.Generic;
using Migs.MLock.Debugging;
using UnityEditor;
using UnityEngine;

namespace Migs.MLock.Editor.DebugWindow
{
    /// <summary>
    /// Editor window for debugging MLock system
    /// Follows Single Responsibility principle by only handling UI and user interaction
    /// </summary>
    public class MLockDebugWindow : EditorWindow
    {
        // UI state
        private Vector2 _scrollPosition;
        private bool _isAutoRefresh = true;
        private readonly Dictionary<int, bool> _foldoutStates = new Dictionary<int, bool>();
        
        // Styles
        private GUIStyle _headerStyle;
        private GUIStyle _subheaderStyle;
        private GUIStyle _lockItemStyle;
        private GUIStyle _affectedItemStyle;
        
        // Menu item to open the window
        [MenuItem("Window/MLock/Locks Debug")]
        public static void ShowWindow()
        {
            var window = GetWindow<MLockDebugWindow>();
            window.titleContent = new GUIContent("MLock Debug");
            window.Show();
        }
        
        private void OnEnable()
        {
            // Enable debug data collection
            DebugData.SetEnabled(true);
            EditorApplication.update += OnEditorUpdate;
        }
        
        private void OnDisable()
        {
            // Disable debug data collection to avoid unnecessary processing
            DebugData.SetEnabled(false);
            EditorApplication.update -= OnEditorUpdate;
        }
        
        private void InitializeStyles()
        {
            if (_headerStyle != null) return;
            
            _headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14,
                margin = new RectOffset(4, 4, 8, 8)
            };
            
            _subheaderStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 12,
                margin = new RectOffset(4, 4, 4, 4)
            };
            
            _lockItemStyle = new GUIStyle(EditorStyles.helpBox)
            {
                margin = new RectOffset(4, 4, 2, 2),
                padding = new RectOffset(8, 8, 8, 8)
            };
            
            _affectedItemStyle = new GUIStyle(EditorStyles.label)
            {
                margin = new RectOffset(20, 4, 1, 1)
            };
        }
        
        private void OnEditorUpdate()
        {
            if (_isAutoRefresh)
            {
                // Update data and repaint window
                DebugData.UpdateData();
                Repaint();
            }
        }
        
        private void OnGUI()
        {
            InitializeStyles();
            
            DrawToolbar();
            
            EditorGUILayout.Space();
            
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            DrawLockServices();
            DrawActiveLocks();
            
            EditorGUILayout.EndScrollView();
        }
        
        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            
            // Refresh button
            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
            {
                DebugData.UpdateData();
            }
            
            // Auto refresh toggle
            _isAutoRefresh = EditorGUILayout.ToggleLeft("Auto Refresh", _isAutoRefresh, GUILayout.Width(100));
            
            GUILayout.FlexibleSpace();
            
            // Unlock all button
            if (GUILayout.Button("Unlock All", EditorStyles.toolbarButton))
            {
                if (EditorUtility.DisplayDialog("Unlock All", 
                        "Are you sure you want to unlock all active locks?", 
                        "Yes", "Cancel"))
                {
                    DebugData.UnlockAll();
                    DebugData.UpdateData();
                }
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawLockServices()
        {
            EditorGUILayout.LabelField("Lock Services", _headerStyle);
            
            var services = DebugData.GetLockServices();
            if (services.Count == 0)
            {
                EditorGUILayout.HelpBox("No lock services registered. Lock services must be registered with MLockDebugData.RegisterLockService().", MessageType.Info);
                return;
            }
            
            foreach (var pair in services)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField($"Type: {pair.Key.Name}", _subheaderStyle);
                EditorGUILayout.LabelField($"Service: {pair.Value.GetType().Name}");
                EditorGUILayout.EndVertical();
            }
        }
        
        private void DrawActiveLocks()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Active Locks", _headerStyle);
            
            var locks = DebugData.GetActiveLocks();
            if (locks.Count == 0)
            {
                EditorGUILayout.HelpBox("No active locks.", MessageType.Info);
                return;
            }
            
            foreach (var lockInfo in locks)
            {
                EditorGUILayout.BeginVertical(_lockItemStyle);
                
                // Foldout for lock details
                if (!_foldoutStates.ContainsKey(lockInfo.Id))
                {
                    _foldoutStates[lockInfo.Id] = true;
                }
                
                EditorGUILayout.BeginHorizontal();
                
                _foldoutStates[lockInfo.Id] = EditorGUILayout.Foldout(_foldoutStates[lockInfo.Id], 
                    $"Lock ID: {lockInfo.Id} ({lockInfo.LockType})");
                
                GUILayout.FlexibleSpace();
                
                if (GUILayout.Button("Unlock", GUILayout.Width(60)))
                {
                    if (DebugData.UnlockById(lockInfo.Id))
                    {
                        DebugData.UpdateData();
                    }
                }
                
                EditorGUILayout.EndHorizontal();
                
                if (_foldoutStates[lockInfo.Id])
                {
                    EditorGUI.indentLevel++;
                    
                    DrawLockDetails(lockInfo);
                    
                    EditorGUI.indentLevel--;
                }
                
                EditorGUILayout.EndVertical();
            }
        }
        
        private void DrawLockDetails(LockDebugInfo lockInfo)
        {
            EditorGUILayout.LabelField($"Include Tags: {lockInfo.IncludeTags}");
            EditorGUILayout.LabelField($"Exclude Tags: {lockInfo.ExcludeTags}");
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Affected Lockables:", _subheaderStyle);
            
            if (lockInfo.AffectedLockables.Count == 0)
            {
                EditorGUILayout.LabelField("No affected lockables", EditorStyles.label);
                return;
            }
            
            foreach (var lockable in lockInfo.AffectedLockables)
            {
                EditorGUILayout.LabelField(lockable, _affectedItemStyle);
            }
        }
    }
} 