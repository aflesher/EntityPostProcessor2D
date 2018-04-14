using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	public class Entity : MonoBehaviour
	{

		[Header("Render Ouput")]
		[Tooltip("The size of the texture in pixels")]
		public Vector2Int textureSize;
		public float pixelsPerUnit = 1;
		[SortingLayer]
		public int sortingLayer = 0;
		public int orderInLayer = 0;
		public MeshRenderer renderOutput;

		[Header("Render Source")]
		public GameObject renderSource;

		[Header("Post Processor")]
		public EntityPostProcessor postProcessor;

		static float curX;
		static string layerName = "EntityPostProcessor";


		// Use this for initialization
		void Awake()
		{
			int layer = LayerMask.NameToLayer(layerName);
			if (layer == -1) {
				Debug.LogError(string.Format("WholeEffects2D requires layer [{0}]", layerName));
				return;
			}

			renderOutput.sortingOrder = orderInLayer;
			renderOutput.sortingLayerID = sortingLayer;

			postProcessor = Instantiate(
				postProcessor,
				new Vector3(curX + (textureSize.x * .5f) + 1, textureSize.y * .5f, -10),
				Quaternion.identity
			);

			curX += textureSize.x;
			postProcessor.SetupTexture(textureSize);
			postProcessor.cullingMask = LayerMask.GetMask(layerName);
			postProcessor.name = string.Format("{0}PostProcessor", gameObject.name);

			renderSource.transform.parent = postProcessor.transform;
			renderSource.transform.localPosition = new Vector3(0, 0, 10);
			renderSource.layer = LayerMask.NameToLayer(layerName);

			renderOutput.material.SetTexture("_MainTex", postProcessor.renderTexture);
			renderOutput.transform.localScale = new Vector3(textureSize.x * pixelsPerUnit, textureSize.y * pixelsPerUnit, 1);
		}
	}
}
