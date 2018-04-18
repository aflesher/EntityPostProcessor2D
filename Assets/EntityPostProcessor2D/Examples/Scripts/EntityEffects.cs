using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	public class EntityEffects : MonoBehaviour
	{

		public Color outlineColor;
		public float outlineSize;
		public bool enableOutline;

		public float dissolveSpeed;
		public float dissolveEdgeSize;
		public Color dissolveEdgeColor;

		Material material;

		bool dissolving = true;
		float dissolveProgress = 0;

		void Awake()
		{
			material = new Material(Shader.Find("EntityPostProcessor2D/Entity"));
			material.DisableKeyword("DISSOLVE");
			SetMaterialProperties();
		}

		void Update()
		{
			if (dissolving) {
				dissolveProgress = Mathf.Clamp01(dissolveProgress + (Time.deltaTime * dissolveSpeed));
				material.SetFloat("_Dissolve_Progress", dissolveProgress);

				if (Mathf.Approximately(dissolveProgress, 1)) {
					dissolving = false;
					StartCoroutine(DisableDissolveCoroutine());
				}
			}
		}

		IEnumerator DisableDissolveCoroutine()
		{
			yield return new WaitForSeconds(1);
			material.DisableKeyword("DISSOLVE");
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, material);
		}

		public void SetMaterialProperties()
		{
			if (material == null) {
				return;
			}

			material.SetFloat("_Outline_OutlineSize", outlineSize);
			material.SetColor("_Outline_OutlineColor", outlineColor);
			if (enableOutline) {
				material.EnableKeyword("OUTLINE");
			} else {
				material.DisableKeyword("OUTLINE");
			}

			material.SetFloat("_Dissolve_EdgeSize", dissolveEdgeSize);
			material.SetColor("_Dissolve_EdgeColor", dissolveEdgeColor);
		}

		public void StartDissolve()
		{
			dissolving = true;
			dissolveProgress = 0;
			material.SetFloat("_Dissolve_Progress", 0);
			material.EnableKeyword("DISSOLVE");
		}
	}
}
