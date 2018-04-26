#if UNITY_EDITOR

namespace EntityPostProcessor
{
	using UnityEngine;
	using UnityEditor;

	internal class EntityControllerBuilder
	{

		[MenuItem("GameObject/EntityPostProcessor2D/Controller", false, 1)]
		static void CreateEntity()
		{
			GameObject gameObject = new GameObject("EntityController");
			gameObject.AddComponent<EntityController>();

			GameObject sourceObject = new GameObject("EntityRenderSource");
			sourceObject.transform.parent = gameObject.transform;
			sourceObject.AddComponent<EntityRenderSource>();
			gameObject.GetComponent<EntityController>().material = new Material(Shader.Find("Sprites/Default"));

			Selection.activeGameObject = gameObject;

		}
	}
}
#endif