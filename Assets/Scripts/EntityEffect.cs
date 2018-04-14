using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEffect : MonoBehaviour
{
	public Material material;

	protected virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}
}
