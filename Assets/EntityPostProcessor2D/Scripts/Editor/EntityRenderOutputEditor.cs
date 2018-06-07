using UnityEngine;
using UnityEditor;
using System.Linq;

namespace EntityPostProcessor
{
	[CustomEditor(typeof(EntityRenderOutput))]
	[CanEditMultipleObjects]
	public class EntityRenderOutputEditor : Editor
	{
		Texture headerTexture;

		void OnEnable()
		{
			headerTexture = EditorTextures.RenderOutputHeader;
		}

		public override void OnInspectorGUI()
		{
			EntityRenderOutput entity = (EntityRenderOutput)target;

			EditorGUILayout.Space();
			var headerRect = GUILayoutUtility.GetRect(0.0f, 5.0f);
			headerRect.width = headerTexture.width;
			headerRect.height = headerTexture.height;
			GUILayout.Space(headerRect.height);
			GUI.DrawTexture(headerRect, headerTexture);
		}
	}
}
