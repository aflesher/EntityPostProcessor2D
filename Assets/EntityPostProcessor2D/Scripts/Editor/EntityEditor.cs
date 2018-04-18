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
			EditorGUILayout.LabelField("Render Output", EditorStyles.boldLabel);
			entity.pixelsPerUnit = EditorGUILayout.FloatField("Pixels Per Unit", entity.pixelsPerUnit);
			string[] sortingLayerNames = SortingLayer.layers.Select(l => l.name).ToArray();
			int[] sortingLayerValues = sortingLayerNames.Select(l => SortingLayer.NameToID(l)).ToArray();
			entity.sortingLayer = EditorGUILayout.IntPopup("Sorting Layer", entity.sortingLayer, sortingLayerNames, sortingLayerValues);


			entity.orderInLayer = EditorGUILayout.IntField("Order In Layer", entity.orderInLayer);
			entity.material = (Material)EditorGUILayout.ObjectField("Material", entity.material, typeof(Material), true);

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Render Capture", EditorStyles.boldLabel);
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
				entity.renderOuputLocalPosition = EditorGUILayout.Vector2Field(
					new GUIContent(
						"Source Capture Offset",
						"Changes the position of the source object within the capture camera"
					),
					entity.renderOuputLocalPosition
				);
				entity.showCaptureRect = EditorGUILayout.Toggle("Show Capture Rect", entity.showCaptureRect);
				entity.renderSource = (GameObject)EditorGUILayout.ObjectField("Render Source", entity.renderSource, typeof(GameObject), true);
				entity.autoEnableDisablePostProcessor = EditorGUILayout.Toggle(
					new GUIContent(
						"Auto Enable/Disable PP",
						"When set to true the post processing camera and source render object will be enabled/disabled when the entity is enabled/disabled"
					),
					entity.autoEnableDisablePostProcessor
				);
				entity.filterMode = (FilterMode)EditorGUILayout.EnumPopup("Output Filter Mode", entity.filterMode);

				int[] depthValues = { 0, 16, 24 };
				string[] depthNames = depthValues.Select(p => p.ToString()).ToArray();

				entity.depth = EditorGUILayout.IntPopup("Output Depth", entity.depth, depthNames, depthValues);
			}

			serializedObject.ApplyModifiedProperties();

			EditorUtility.SetDirty(target);
		}
	}
}
