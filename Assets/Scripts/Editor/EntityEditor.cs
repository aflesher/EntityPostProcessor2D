using UnityEngine;
using UnityEditor;
using System.Linq;

namespace EntityPostProcessor
{
	[CustomEditor(typeof(Entity))]
	[CanEditMultipleObjects]
	public class EntityEditor : Editor
	{

		bool showAdvanced = false;
		SerializedProperty postProcessor;

		void OnEnable()
		{
			postProcessor = serializedObject.FindProperty("postProcessorRef");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			Entity entity = (Entity)target;

			EditorGUILayout.Space();
			entity.pixelsPerUnit = EditorGUILayout.FloatField("Pixels Per Unit", entity.pixelsPerUnit);

			string[] sortingLayerNames = SortingLayer.layers.Select(l => l.name).ToArray();
			int[] sortingLayerValues = sortingLayerNames.Select(l => SortingLayer.NameToID(l)).ToArray();
			entity.sortingLayer = EditorGUILayout.IntPopup("Sorting Layer", entity.sortingLayer, sortingLayerNames, sortingLayerValues);

			// entity.sortingLayer = EditorGUILayout.
			entity.orderInLayer = EditorGUILayout.IntField("Order In Layer", entity.orderInLayer);
			entity.material = (Material)EditorGUILayout.ObjectField("Material", entity.material, typeof(Material), true);

			int[] textureSizeValues = { 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 };
			string[] textureSizeNames = textureSizeValues.Select(p => p.ToString()).ToArray();
			EditorGUILayout.LabelField("Render Texture Size");
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Width", GUILayout.Width(50));
			entity.textureWidth = EditorGUILayout.IntPopup(entity.textureWidth, textureSizeNames, textureSizeValues);
			EditorGUILayout.LabelField("Height", GUILayout.Width(50));
			entity.textureHeight = EditorGUILayout.IntPopup(entity.textureHeight, textureSizeNames, textureSizeValues);
			EditorGUILayout.EndHorizontal();

			postProcessor.objectReferenceValue = (EntityPostProcessor)EditorGUILayout.ObjectField(
				"Post Processor",
				postProcessor.objectReferenceValue,
				typeof(EntityPostProcessor),
				true
			);

			EditorGUILayout.Space();
			showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Advanced");
			if (showAdvanced) {
				entity.renderOuputLocalPosition = EditorGUILayout.Vector2Field("Render Output Local Position", entity.renderOuputLocalPosition);
				entity.showCaptureRect = EditorGUILayout.Toggle("Show Capture Rect", entity.showCaptureRect);
				entity.autoEnableDisablePostProcessor = EditorGUILayout.Toggle("Auto Enable/Disable PP", entity.autoEnableDisablePostProcessor);
				entity.filterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", entity.filterMode);

				int[] depthValues = { 0, 16, 24 };
				string[] depthNames = depthValues.Select(p => p.ToString()).ToArray();

				entity.depth = EditorGUILayout.IntPopup("Depth", entity.depth, depthNames, depthValues);
			}

			serializedObject.ApplyModifiedProperties();

			EditorUtility.SetDirty(target);
		}
	}
}
