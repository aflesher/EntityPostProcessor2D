using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityPostProcessor;

public class Player : MonoBehaviour
{

	public bool showOutline;
	public bool showDisplacement;
	public bool showDissolve;

	OutlineEffect outlineEffect;
	DisplacementEffect displacementEffect;
	DissolveEffect dissolveEffect;
	Entity entity;

	// Use this for initialization
	void Start()
	{
		entity = GetComponent<Entity>();
		outlineEffect = entity.postProcessor.GetComponent<OutlineEffect>();
		displacementEffect = entity.postProcessor.GetComponent<DisplacementEffect>();
		dissolveEffect = entity.postProcessor.GetComponent<DissolveEffect>();

		dissolveEffect.Completed += DissolveComplete;
	}
	
	// Update is called once per frame
	void Update()
	{
		outlineEffect.enabled = showOutline;
		displacementEffect.enabled = showDisplacement;

		if (!dissolveEffect.enabled && showDissolve) {
			dissolveEffect.enabled = true;
		}
	}

	void DissolveComplete()
	{
		StartCoroutine(DissolveCoroutine());
	}

	IEnumerator DissolveCoroutine()
	{
		yield return new WaitForSeconds(.5f);
		dissolveEffect.enabled = false;
		showDissolve = false;
	}
}
