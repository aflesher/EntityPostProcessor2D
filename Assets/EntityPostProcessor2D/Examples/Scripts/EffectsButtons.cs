using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EntityPostProcessor
{

	public class EffectsButtons : MonoBehaviour
	{

		public Button dissolveButton;
		public Button toggleAnimationsButton;
		public Button outlineButton;
		public Button colorButton;

		public Player player;

		EntityEffects entityEffects;
		Text colorButtonText;
		Text toggleAnimationsButtonText;
		Text outlineButtonText;

		// Use this for initialization
		void Start()
		{
			entityEffects = player.GetComponent<EntityController>().postProcessor.GetComponent<EntityEffects>();

			colorButtonText = colorButton.GetComponentInChildren<Text>();
			toggleAnimationsButtonText = toggleAnimationsButton.GetComponentInChildren<Text>();
			outlineButtonText = outlineButton.GetComponentInChildren<Text>();
		}

		// Update is called once per frame
		void Update()
		{
			toggleAnimationsButtonText.text = player.isAnimating ? "ANIMATION ON" : "ANIMATION OFF";
			colorButtonText.text = entityEffects.colorEnable ? "COLOR ON" : "COLOR OFF";
			outlineButtonText.text = entityEffects.enableOutline ? "OUTLINE ON" : "OUTLINE OFF";
			dissolveButton.interactable = !player.isDissolving;
		}

		public void ToggleColor()
		{
			entityEffects.colorEnable = !entityEffects.colorEnable;
		}

		public void ToggleOutline()
		{
			entityEffects.enableOutline = !entityEffects.enableOutline;
		}
	}
}
