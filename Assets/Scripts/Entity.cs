﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	public class Entity : MonoBehaviour
	{

		[Header("Render Ouput")]
		[Tooltip("The size of the texture in pixels")]
		public float pixelsPerUnit = 1;
		[SortingLayer]
		public int sortingLayer = 0;
		public int orderInLayer = 0;
		public Material material;
		[TextureSize]
		public int textureWidth = 128;
		[TextureSize]
		public int textureHeight = 128;


		[Header("Post Processor")]
		[SerializeField]
		EntityPostProcessor postProcessorRef;

		public EntityPostProcessor postProcessor { get; private set; }

		[Header("Advanced")]
		[Tooltip("Local position of the render output texture")]
		public Vector2 renderOuputLocalPosition;
		[Space(10)]
		public GameObject renderSource;
		public bool showCaptureRect;
		public bool autoEnableDisablePostProcessor = true;

		static float curX;
		static string layerName = "EntityPostProcessor";


		// Use this for initialization
		void Awake()
		{
			int layer = LayerMask.NameToLayer(layerName);
			if (!Validate(layer)) {
				return;
			}

			GameObject renderOutput = GameObject.CreatePrimitive(PrimitiveType.Quad);
			MeshRenderer renderOutputMeshRenderer = renderOutput.GetComponent<MeshRenderer>();
			renderOutput.name = "RenderOutput";
			renderOutput.GetComponent<MeshRenderer>().material = material;
			renderOutput.transform.parent = gameObject.transform;

			GameObject.DestroyImmediate(renderOutput.GetComponent<MeshCollider>());

			renderOutputMeshRenderer.sortingOrder = orderInLayer;
			renderOutputMeshRenderer.sortingLayerID = sortingLayer;
			renderOutput.transform.localPosition = renderSource.transform.localPosition - (Vector3)renderOuputLocalPosition;
			renderOutput.transform.localScale = new Vector3(textureWidth * pixelsPerUnit, textureHeight * pixelsPerUnit, 1);

			postProcessor = Instantiate(
				postProcessorRef,
				new Vector3(curX + (textureWidth * .5f) + 1, textureHeight * .5f, -10),
				Quaternion.identity
			);

			curX += textureWidth;
			postProcessor.SetupTexture(new Vector2Int(textureWidth, textureHeight));
			postProcessor.cullingMask = LayerMask.GetMask(layerName);
			postProcessor.name = string.Format("{0}PostProcessor", gameObject.name);

			renderSource.transform.parent = postProcessor.transform;
			renderSource.transform.localPosition = new Vector3(renderOuputLocalPosition.x, renderOuputLocalPosition.y, 10);
			renderSource.layer = LayerMask.NameToLayer(layerName);

			renderOutputMeshRenderer.material.SetTexture("_MainTex", postProcessor.renderTexture);
		}

		bool Validate(int layer)
		{
			if (layer == -1) {
				Debug.LogError(string.Format("EntityPostProcessor2D requires layer [{0}]", layerName));
				return false;
			}

			if (postProcessorRef == null) {
				Debug.LogError("EntityPostProcessor2D requires a post processor");
				return false;
			}

			return true;
		}

		void OnDrawGizmos()
		{
			if (showCaptureRect) {
				Gizmos.color = Color.blue;
				Vector2 size = new Vector2((textureWidth * pixelsPerUnit * .5f), (textureHeight * pixelsPerUnit * .5f));
				float xMin = -size.x + renderOuputLocalPosition.x;
				float yMin = -size.y + renderOuputLocalPosition.y;
				float xMax = size.x + renderOuputLocalPosition.x;
				float yMax = size.y + renderOuputLocalPosition.y;

				Gizmos.DrawLine(new Vector3(xMin, yMin), new Vector3(xMin, yMax));
				Gizmos.DrawLine(new Vector3(xMin, yMax), new Vector3(xMax, yMax));
				Gizmos.DrawLine(new Vector3(xMax, yMax), new Vector3(xMax, yMin));
				Gizmos.DrawLine(new Vector3(xMax, yMin), new Vector3(xMin, yMin));

			}
		}

		void OnEnable()
		{
			if (autoEnableDisablePostProcessor) {
				SetPostProcessorEnabled(true);
			}
		}

		void OnDisable()
		{
			if (autoEnableDisablePostProcessor) {
				SetPostProcessorEnabled(false);
			}
		}

		/// <summary>
		/// Activates/Deactivates the Post Processor GameObject. If autoEnableDisablePostProcessor is set to true this
		/// will be automatically called when the entity gameobject is enabled or disabled.
		/// </summary>
		/// <param name="enabled">If set to <c>true</c> enabled.</param>
		public void SetPostProcessorEnabled(bool enabled)
		{
			if (postProcessor != null) {
				postProcessor.gameObject.SetActive(enabled);
			}
		}

		void OnDestroy()
		{
			if (postProcessor != null) {
				Destroy(postProcessor.gameObject);
			}
		}
	}
}
