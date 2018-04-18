using UnityEngine;
using UnityEditor;
using System.Linq;

namespace EntityPostProcessor
{
	[CustomEditor(typeof(EntityEffects))]
	[CanEditMultipleObjects]
	public class EntityEffectsEditor : Editor
	{

		bool showOutline = false;
		bool showDissolve = false;
		MonoScript script;

		void OnEnable()
		{
			script = MonoScript.FromMonoBehaviour((EntityEffects)target);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EntityEffects entityEffects = (EntityEffects)target;

			EditorGUI.BeginDisabledGroup(true);
			script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			showOutline = EditorGUILayout.Foldout(showOutline, "Outline");
			entityEffects.enableOutline = EditorGUILayout.Toggle(entityEffects.enableOutline);
			EditorGUILayout.EndHorizontal();
			if (showOutline) {
				EditorGUI.BeginDisabledGroup(!entityEffects.enableOutline);
				entityEffects.outlineSize = EditorGUILayout.Slider("Size", entityEffects.outlineSize, 0, 5);
				entityEffects.outlineColor = EditorGUILayout.ColorField("Color", entityEffects.outlineColor);
				EditorGUI.EndDisabledGroup();
			}

			showDissolve = EditorGUILayout.Foldout(showDissolve, "Dissolve");
			if (showDissolve) {
				entityEffects.dissolveSpeed = EditorGUILayout.FloatField("Speed", entityEffects.dissolveSpeed);
				entityEffects.dissolveEdgeColor = EditorGUILayout.ColorField("Edge Color", entityEffects.dissolveEdgeColor);
				entityEffects.dissolveEdgeSize = EditorGUILayout.FloatField("Edge Size", entityEffects.dissolveEdgeSize);
			}

			serializedObject.ApplyModifiedProperties();

			EditorUtility.SetDirty(target);
		}
	}
}
