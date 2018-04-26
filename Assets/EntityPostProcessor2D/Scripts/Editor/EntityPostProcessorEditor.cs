using UnityEngine;
using UnityEditor;
using System.Linq;

namespace EntityPostProcessor
{
	[CustomEditor(typeof(EntityPostProcessor))]
	[CanEditMultipleObjects]
	public class EntityPostProcessorEditor : Editor
	{
		Texture headerTexture;

		void OnEnable()
		{
			headerTexture = EditorTextures.PostProcessorHeader;
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.Space();
			var headerRect = GUILayoutUtility.GetRect(0.0f, 5.0f);
			headerRect.width = headerTexture.width;
			headerRect.height = headerTexture.height;
			GUILayout.Space(headerRect.height);
			GUI.DrawTexture(headerRect, headerTexture);
		}
	}
}
