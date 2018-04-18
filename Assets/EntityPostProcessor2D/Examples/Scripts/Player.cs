using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	public class Player : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{
			
		}
		
		// Update is called once per frame
		void Update()
		{
			
		}

		public void Dissolve()
		{
			GetComponent<Entity>().postProcessor.GetComponent<EntityEffects>().StartDissolve();
		}
	}

}
