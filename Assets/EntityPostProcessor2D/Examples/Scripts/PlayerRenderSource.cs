using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	public class PlayerRenderSource : MonoBehaviour
	{
		bool _animating = true;
		Animator[] animators;
		float animationSpeed = 1;

		// Use this for initialization
		void Start()
		{
			animators = GetComponentsInChildren<Animator>();
		}

		// Update is called once per frame
		void Update()
		{

		}

		public bool animating
		{
			get
			{
				return _animating;
			}
			set
			{
				_animating = value;
				foreach (Animator animator in animators) {
					animator.speed = _animating ? animationSpeed : 0;
				}
			}
		}
	}
}
