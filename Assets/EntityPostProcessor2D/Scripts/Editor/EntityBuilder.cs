#if UNITY_EDITOR

namespace EntityPostProcessor
{
	using UnityEngine;
	using UnityEditor;

	internal class EntityBuilder
	{

		[MenuItem("GameObject/EntityPostProcessor2D/Entity", false, 1)]
		static void CreateEntity()
		{
			GameObject gameObject = new GameObject("Entity");
			gameObject.AddComponent<EntityController>();

			GameObject sourceObject = new GameObject("RenderSource");
			sourceObject.transform.parent = gameObject.transform;
			sourceObject.AddComponent<EntityRenderSource>();
			gameObject.GetComponent<EntityController>().material = new Material(Shader.Find("Sprites/Default"));

			Selection.activeGameObject = gameObject;

		}
	}
}
#endif