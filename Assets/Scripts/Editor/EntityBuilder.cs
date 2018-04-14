#if UNITY_EDITOR

namespace EntityPostProcessor
{
	using UnityEngine;
	using UnityEditor;

	internal class EntityBuilder
	{
		
		[MenuItem("GameObject/EntityPostProcessor2D/EntitySprite", false, 1)]
		static void CreateEntity()
		{
			GameObject gameObject = new GameObject("Entity");
			gameObject.AddComponent<Entity>();

			GameObject outputObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
			outputObject.name = "RenderOutput";
			outputObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Sprites/Default"));
			outputObject.transform.parent = gameObject.transform;

			GameObject.DestroyImmediate(outputObject.GetComponent<MeshCollider>());

			gameObject.GetComponent<Entity>().renderOutput = outputObject.GetComponent<MeshRenderer>();

			GameObject sourceObject = new GameObject("RenderSource");
			sourceObject.AddComponent<SpriteRenderer>();
			sourceObject.transform.parent = gameObject.transform;
			gameObject.GetComponent<Entity>().renderSource = sourceObject;

			Selection.activeGameObject = gameObject;

		}
	}
}
#endif