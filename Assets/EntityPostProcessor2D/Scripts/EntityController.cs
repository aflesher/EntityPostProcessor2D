﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	/// <summary>
	/// Automatically creates a Camera -> RenderTexture setup for this entity so that
	/// shaders and post processing effects can be applied to the gameObject as a whole.
	/// </summary>
	public class EntityController : MonoBehaviour
	{
		public float pixelsPerUnit = 1;
		public int sortingLayer = 0;
		public int orderInLayer = 0;
		public int textureWidth = 128;
		public int textureHeight = 128;
		public float outputScale = 1;
		[SerializeField]
		EntityPostProcessor postProcessorRef;

		public EntityPostProcessor postProcessor { get; private set; }

		public EntityRenderSource renderSource { get; private set; }

		public Vector2 renderOuputLocalPosition;
		public bool showCaptureRect;
		public bool autoEnableDisablePostProcessor = true;

		public FilterMode filterMode = FilterMode.Trilinear;
		public int depth = 24;

		static float curX;
		static string layerName = "EntityPostProcessor";

		[HideInInspector]
		public EntityRenderOutput renderOutput { get; private set; }

		MeshRenderer renderOutputMeshRenderer;

		// Use this for initialization
		void Awake()
		{
			int layer = LayerMask.NameToLayer(layerName);
			renderSource = GetComponentInChildren<EntityRenderSource>();
			if (!Validate(layer)) {
				return;
			}

			renderSource.controller = this;

			// backwards compatibility
			renderOutput = GetComponentInChildren<EntityRenderOutput>();
			if (renderOutput == null) {
				GameObject renderOutputGO = GameObject.CreatePrimitive(PrimitiveType.Quad);
				renderOutputGO.AddComponent<EntityRenderOutput>();
				renderOutput = renderOutputGO.GetComponent<EntityRenderOutput>();
				renderOutput.transform.parent = transform;
				renderOutput.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Sprites/Default"));
				renderOutput.name = "EntityRenderOutput";
				GameObject.DestroyImmediate(renderOutput.GetComponent<MeshCollider>());
				renderOutput.gameObject.layer = renderSource.gameObject.layer;
			}
			renderOutput.controller = this;
			renderOutputMeshRenderer = renderOutput.GetComponent<MeshRenderer>();

			renderOutputMeshRenderer.sortingOrder = orderInLayer;
			renderOutputMeshRenderer.sortingLayerID = sortingLayer;
			renderOutput.transform.localPosition = renderSource.transform.localPosition - (Vector3)renderOuputLocalPosition;
			renderOutput.transform.localScale = new Vector3(
				textureWidth * outputScale * pixelsPerUnit,
				textureHeight * outputScale * pixelsPerUnit,
				1
			);

			postProcessor = Instantiate(
				postProcessorRef,
				new Vector3(curX + (textureWidth * .5f * pixelsPerUnit) + 1, textureHeight * .5f * pixelsPerUnit, -10),
				Quaternion.identity
			);

			curX += textureWidth * pixelsPerUnit;
			postProcessor.SetupTexture(new Vector2Int(textureWidth, textureHeight), depth, filterMode, gameObject.name, pixelsPerUnit);
			postProcessor.cullingMask = LayerMask.GetMask(layerName);
			postProcessor.name = string.Format("{0} PostProcessor", gameObject.name);

			renderSource.transform.parent = postProcessor.transform;
			renderSource.transform.localPosition = new Vector3(renderOuputLocalPosition.x, renderOuputLocalPosition.y, 10);
			renderSource.gameObject.layer = LayerMask.NameToLayer(layerName);
			Transform[] children = renderSource.GetComponentsInChildren<Transform>();
			foreach (Transform child in children) {
				child.gameObject.layer = renderSource.gameObject.layer;
			}

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

			if (renderSource == null) {
				Debug.LogError("EntityPostProcessor2D is missing RenderSource child element");
				return false;
			}

			return true;
		}

		void OnDrawGizmos()
		{
			EntityRenderSource _renderSource = renderSource;
			if (_renderSource == null) {
				_renderSource = GetComponentInChildren<EntityRenderSource>();
			}
			if (showCaptureRect && _renderSource != null) {
				Gizmos.color = Color.blue;
				Vector2 size = new Vector2(textureWidth * .5f * pixelsPerUnit, textureHeight * .5f * pixelsPerUnit);
				float xMin = -size.x - renderOuputLocalPosition.x + _renderSource.transform.position.x;
				float yMin = -size.y - renderOuputLocalPosition.y + _renderSource.transform.position.y;
				float xMax = size.x - renderOuputLocalPosition.x + _renderSource.transform.position.x;
				float yMax = size.y - renderOuputLocalPosition.y + _renderSource.transform.position.y;

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
		/// will be automatically called when the EntityController gameobject is enabled or disabled.
		/// </summary>
		/// <param name="enabled">If set to <c>true</c> enabled.</param>
		public void SetPostProcessorEnabled(bool enabled)
		{
			if (postProcessor != null) {
				postProcessor.gameObject.SetActive(enabled);
			}
		}

		/// <summary>
		/// Shows/Hides the render output. Use this to show/hide the entity from the main camera
		/// </summary>
		/// <param name="visible"></param>
		public void SetRenderOutputVisible(bool visible)
		{
			renderOutputMeshRenderer.enabled = visible;
		}

		/// <summary>
		/// Calculates the real world rect space of the render output box
		/// </summary>
		/// <returns></returns>
		public Rect RenderOutputRect()
		{
			Vector2 size = new Vector2(renderOutput.transform.localScale.x, renderOutput.transform.localScale.y);
			Vector2 position = renderOutput.transform.position;

			return new Rect(position.x - (size.x * .5f), position.y - (size.y * .5f), size.x, size.y);
		}

		void OnDestroy()
		{
			if (postProcessor != null) {
				Destroy(postProcessor.gameObject);
			}
		}
	}
}
