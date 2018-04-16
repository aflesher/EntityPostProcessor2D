using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace EntityPostProcessor
{
	[CustomPropertyDrawer(typeof(TextureSizeAttribute))]
	public class TextureSizePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			int[] values = { 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 };
			string[] names = values.Select(p => p.ToString()).ToArray();
			EditorGUI.BeginProperty(position, label, property);

			// Look up the layer name using the current layer ID
			int oldIndex = Array.IndexOf(values, property.intValue);

			// Show the popup for the names
			int newIndex = EditorGUI.Popup(position, label.text, oldIndex, names);

			// If the index changes, look up the ID for the new index to store as the new ID
			if (oldIndex != newIndex) {
				property.intValue = values[newIndex];
			}

			EditorGUI.EndProperty();
		}
	}
}