using Migs.MLock.Examples.Car_Canvas.Code.Controls;
using UnityEditor;
using UnityEditor.UI;

namespace Migs.MLock.Examples.Car_Canvas.Code.Editor
{
    [CustomEditor(typeof(LockableToggle), true)]
    public class LockableToggleEditor : ToggleEditor
    {
        private SerializedProperty _lockTagsProp;

        protected override void OnEnable()
        {
            base.OnEnable();
            _lockTagsProp = serializedObject.FindProperty("<LockTags>k__BackingField");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            // Display the default inspector for the remainder of the component
            base.OnInspectorGUI();
        
            // Display the LockTags serialized property first
            EditorGUILayout.PropertyField(_lockTagsProp);
        
            serializedObject.ApplyModifiedProperties();
        }
    }
}