using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityPostProcessor
{
	/// <summary>
	/// Used to designate a child object of an Entity as the render source.
	/// </summary>
	public class EntityRenderOutput : MonoBehaviour
	{
		[HideInInspector]
		public EntityController controller;
	}
}
