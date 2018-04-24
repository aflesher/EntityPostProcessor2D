using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	public class Player : MonoBehaviour
	{

		EntityEffects entityEffects;
		PlayerRenderSource playerRenderSource;

		// Use this for initialization
		void Start()
		{
			entityEffects = GetComponent<Entity>().postProcessor.GetComponent<EntityEffects>();
			playerRenderSource = GetComponent<Entity>().renderSource.GetComponent<PlayerRenderSource>();
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void Dissolve()
		{
			entityEffects.StartDissolve();
		}

		public void ToggleAnimation()
		{
			playerRenderSource.animating = !playerRenderSource.animating;
		}


		public bool isAnimating {
			get {
				return playerRenderSource.animating;
			}
		}

		public bool isDissolving {
			get {
				return !entityEffects.dissolveReady;
			}
		}
	}

}
