using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	public class EntityPostProcessor : MonoBehaviour
	{

		Camera entityCamera;

		public RenderTexture renderTexture { get; private set; }

		public Color outlineColor;
		public float outlineSize;
		public bool enableOutline;

		Material material;

		void Awake()
		{
			entityCamera = GetComponent<Camera>();
			material = new Material(Shader.Find("EntityPostProcessor2D/Entity"));

			material.SetFloat("_Outline_OutlineSize", outlineSize);
			material.SetColor("_Outline_OutlineColor", outlineColor);
			if (enableOutline) {
				material.EnableKeyword("OUTLINE");
			} else {
				material.DisableKeyword("OUTLINE");
			}
		}

		public LayerMask cullingMask
		{
			get
			{
				return entityCamera.cullingMask;
			}
			set
			{
				entityCamera.cullingMask = value;
			}
		}

		public void SetupTexture(Vector2Int size, int depth, FilterMode filterMode)
		{
			renderTexture = new RenderTexture(size.x, size.y, depth)
			{
				name = "Target Texture",
				filterMode = filterMode,
				antiAliasing = QualitySettings.antiAliasing > 0 ? QualitySettings.antiAliasing : 1
			};

			entityCamera.targetTexture = renderTexture;
			entityCamera.orthographicSize = size.y / 2;
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, material);
		}
	}
}
