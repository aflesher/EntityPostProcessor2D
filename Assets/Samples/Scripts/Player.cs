using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityPostProcessor;

public class Player : MonoBehaviour
{

	public bool showOutline;
	public bool showDisplacement;

	OutlineEffect outlineEffect;
	DisplacementEffect displacementEffect;
	Entity entity;

	// Use this for initialization
	void Start()
	{
		entity = GetComponent<Entity>();
		outlineEffect = entity.postProcessor.GetComponent<OutlineEffect>();
		displacementEffect = entity.postProcessor.GetComponent<DisplacementEffect>();
	}
	
	// Update is called once per frame
	void Update()
	{
		outlineEffect.enabled = showOutline;
		displacementEffect.enabled = showDisplacement;
	}
}
