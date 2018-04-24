using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	public class EntityEffects : MonoBehaviour
	{
		public static readonly int ShaderIdDissolveProgress = Shader.PropertyToID("_Dissolve_Progress");
		public static readonly int ShaderIdDissolveEdgeSize = Shader.PropertyToID("_Dissolve_EdgeSize");
		public static readonly int ShaderIdDissolveEdgeColor = Shader.PropertyToID("_Dissolve_EdgeColor");
		public static readonly int ShaderIdOutlineColor = Shader.PropertyToID("_Outline_OutlineColor");
		public static readonly int ShaderIdOutlineSize = Shader.PropertyToID("_Outline_OutlineSize");
		public static readonly int ShaderIdColorColor = Shader.PropertyToID("_Color_Color");

		public static readonly string ShaderKeywordDissolve = "DISSOLVE";
		public static readonly string ShaderKeywordOutline = "OUTLINE";
		public static readonly string ShaderKeywordColor = "COLORED";

		Material material;

		#region outline color

		[SerializeField]
		Color _outlineColor;

		public Color outlineColor {
			get { return _outlineColor; }
			set {
				_outlineColor = value;
				if (material != null) {
					material.SetColor(ShaderIdOutlineColor, _outlineColor);
				}
			}
		}

		#endregion

		#region outline size

		[SerializeField]
		float _outlineSize;

		public float outlineSize {
			get { return _outlineSize; }
			set {
				_outlineSize = value;
				if (material != null) {
					material.SetFloat(ShaderIdOutlineSize, _outlineSize);
				}
			}
		}

		#endregion

		#region enable outline

		[SerializeField]
		bool _enableOutline;

		public bool enableOutline {
			get { return _enableOutline; }
			set {
				_enableOutline = value;
				if (material != null) {
					if (_enableOutline) {
						material.EnableKeyword(ShaderKeywordOutline);
					} else {
						material.DisableKeyword(ShaderKeywordOutline);
					}
				}
			}
		}

		#endregion

		#region dissolve edge color

		[SerializeField]
		Color _dissolveEdgeColor;

		public Color dissolveEdgeColor {
			get { return _dissolveEdgeColor; }
			set {
				_dissolveEdgeColor = value;
				if (material != null) {
					material.SetColor(ShaderIdDissolveEdgeColor, _dissolveEdgeColor);
				}
			}
		}

		#endregion

		#region dissolve edge size

		[SerializeField]
		float _dissolveEdgeSize;

		public float dissolveEdgeSize {
			get { return _dissolveEdgeSize; }
			set {
				_dissolveEdgeSize = value;
				if (material != null) {
					material.SetFloat(ShaderIdDissolveEdgeSize, _dissolveEdgeSize);
				}
			}
		}

		#endregion

		#region color enable

		[SerializeField]
		bool _colorEnable;

		public bool colorEnable {
			get { return _colorEnable; }
			set {
				_colorEnable = value;
				if (material != null) {
					if (_colorEnable) {
						material.EnableKeyword(ShaderKeywordColor);
					} else {
						material.DisableKeyword(ShaderKeywordColor);
					}
				}
			}
		}

		#endregion

		#region color color

		[SerializeField]
		Color _colorColor;

		public Color colorColor {
			get { return _colorColor; }
			set {
				_colorColor = value;
				if (material != null) {
					material.SetColor(ShaderIdColorColor, _colorColor);
				}
			}
		}

		#endregion

		public float dissolveSpeed;

		bool dissolving = false;
		[HideInInspector]
		public bool dissolveReady = true;
		float dissolveProgress = 0;

		void Awake()
		{
			material = new Material(Shader.Find("EntityPostProcessor2D/Entity"));
			material.DisableKeyword(ShaderKeywordDissolve);

			colorEnable = colorEnable;
			colorColor = colorColor;
			enableOutline = enableOutline;
			outlineColor = outlineColor;
			outlineSize = outlineSize;
			dissolveEdgeColor = dissolveEdgeColor;
			dissolveEdgeSize = dissolveEdgeSize;
		}

		void Update()
		{
			if (dissolving) {
				dissolveProgress = Mathf.Clamp01(dissolveProgress + (Time.deltaTime * dissolveSpeed));
				material.SetFloat(ShaderIdDissolveProgress, dissolveProgress);

				if (Mathf.Approximately(dissolveProgress, 1)) {
					dissolving = false;
					StartCoroutine(DisableDissolveCoroutine());
				}
			}
		}

		IEnumerator DisableDissolveCoroutine()
		{
			yield return new WaitForSeconds(1);
			material.DisableKeyword(ShaderKeywordDissolve);
			dissolveReady = true;
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, material);
		}

		public void StartDissolve()
		{
			dissolving = true;
			dissolveReady = false;
			dissolveProgress = 0;
			material.SetFloat(ShaderIdDissolveProgress, 0);
			material.EnableKeyword(ShaderKeywordDissolve);
		}
	}
}
