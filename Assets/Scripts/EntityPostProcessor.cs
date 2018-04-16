using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	public class EntityPostProcessor : MonoBehaviour
	{

		Camera entityCamera;

		public RenderTexture renderTexture { get; private set; }

		void Awake()
		{
			entityCamera = GetComponent<Camera>();
		}

		public LayerMask cullingMask {
			get {
				return entityCamera.cullingMask;
			}
			set {
				entityCamera.cullingMask = value;
			}
		}

		public void SetupTexture(Vector2Int size, int depth, FilterMode filterMode)
		{
			renderTexture = new RenderTexture(size.x, size.y, depth) {
				name = "Target Texture",
				filterMode = filterMode,
				antiAliasing = QualitySettings.antiAliasing > 0 ? QualitySettings.antiAliasing : 1
			};

			entityCamera.targetTexture = renderTexture;
			entityCamera.orthographicSize = size.y / 2;
		}
	}
}
