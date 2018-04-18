#if UNITY_EDITOR

namespace EntityPostProcessor
{
	using UnityEngine;
	using UnityEditor;

	internal class EntityPostProcessorBuilder
	{

		[MenuItem("GameObject/EntityPostProcessor2D/PostProcesssor", false, 1)]
		static void CreatePostProcessor()
		{
			GameObject gameObject = new GameObject("EntityPostProcessor");
			gameObject.AddComponent<EntityPostProcessor>();
			gameObject.AddComponent<Camera>();
			gameObject.GetComponent<Camera>().orthographic = true;

			Selection.activeGameObject = gameObject;

		}
	}
}
#endif