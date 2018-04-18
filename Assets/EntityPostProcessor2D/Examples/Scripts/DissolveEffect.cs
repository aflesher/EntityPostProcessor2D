using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	public class DissolveEffect : EntityEffect
	{
		public delegate void OnComplete();

		public OnComplete Completed;

		float progress = 0;
		public float speed;

		void OnEnable()
		{
			progress = 0;
			material.SetFloat("_Progress", progress);
		}

		void Update()
		{
			progress = Mathf.Clamp01(progress + (Time.deltaTime * speed));
			material.SetFloat("_Progress", progress);

			if (Mathf.Approximately(progress, 1) && Completed != null) {
				Completed();
			}
		}
	}
}
