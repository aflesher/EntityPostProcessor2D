using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	/// <summary>
	/// Uses a camera to render child objects to a texture
	/// </summary>
	public class EntityPostProcessor : MonoBehaviour
	{

		Camera entityCamera;

		public RenderTexture renderTexture { get; private set; }

		void Awake()
		{
			entityCamera = GetComponent<Camera>();
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

		/// <summary>
		/// Creates a render texture and sets the camera to target it
		/// </summary>
		/// <param name="size">The width and height of the texture in pixels.</param>
		/// <param name="depth">The precision of the render texture's depth buffer in bits (0, 16, 24/32 are supported).</param>
		/// <param name="filterMode">Filtering mode of the texture.</param>
		/// <param name="name">The name of the object.</param>
		public void SetupTexture(Vector2Int size, int depth, FilterMode filterMode, string name)
		{
			renderTexture = new RenderTexture(size.x, size.y, depth)
			{
				name = string.Format("{0} Target Texture", name),
				filterMode = filterMode,
				antiAliasing = QualitySettings.antiAliasing > 0 ? QualitySettings.antiAliasing : 1
			};

			entityCamera.targetTexture = renderTexture;
			entityCamera.orthographicSize = size.y / 2;
		}
	}
}
