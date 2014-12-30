//  Copyright 2014 Invex Games http://invexgames.com
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.

//	Credit to the 'EventTrigger' script at http://www.cratesmith.com/archives/221, which helped immensely

//	May the gods bestow the mightiest of luck upon any who find
//	themselves staring into the abyss that is this wretched code
//	(I'll be cleaning this up and improving on it in the near future)

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MaterialUI
{
	public class AnimTrigger : MonoBehaviour
	{
		public GameObject target;

		public string targetComponent;
		public string targetVariable;
		public enum VariableType{Field, Property}
		public VariableType variableType;
		public string valueType;

		public enum AnimationType{Linear, EaseOutCubed, EaseOutQuint, EaseOutSept, EaseInCubed, EaseInQuint, EaseInSept, EaseInOutCubed, EaseInOutQuint, EaseInOutSept, SoftEaseOutCubed, SoftEaseOutQuint, SoftEaseOutSept};
		public AnimationType animationType;

		public float duration = 1f;

	//	Float
		float initialFloat;
		public float targetFloat;
		float tempFloat;

	//	Int
		int initialInt;
		public int targetInt;
		int tempInt;
		
	//	Vector2
		public bool modifyVec2X;
		public bool modifyVec2Y;
		public bool modifyVec2Z;
		Vector3 initialVec2;
		public Vector2 targetVec2;
		public Vector2 tempVec2;

	//	Vector3
		public bool modifyVec3X;
		public bool modifyVec3Y;
		public bool modifyVec3Z;
		Vector3 initialVec3;
		public Vector3 targetVec3;
		Vector3 tempVec3;

	//	Rect
		public bool modifyRectWidth;
		public bool modifyRectHeight;
		public bool modifyRectX;
		public bool modifyRectY;
		Rect initialRect;
		public Rect targetRect;
		Rect tempRect;

	//	Color
		public bool modifyColorR;
		public bool modifyColorG;
		public bool modifyColorB;
		public bool modifyColorA;
		Color initialColor;
		public Color targetColor;
		Color tempColor;

	//	Material
		public bool modifyMatColorR;
		public bool modifyMatColorG;
		public bool modifyMatColorB;
		public bool modifyMatColorA;
		Color initialMatColor;
		Material initialMaterial;
		public Color targetMatColor;
		Color tempMatColor;
		Material tempMaterial;

		float animStartTime;
		float animDeltaTime;
		int state = 0;
		bool variableIsValid;
		
		Component realComponent;
		public PropertyInfo realProperty;
		public FieldInfo realField;

		void Start ()
		{
			variableIsValid = false;

			if (target && (targetComponent != null))
			{
				if (target.GetComponent(targetComponent))
				{
					realComponent = target.GetComponent(targetComponent);

					if (variableType == VariableType.Field)
					{
						realField = target.GetComponent (targetComponent).GetType ().GetField (targetVariable);
						variableIsValid = true;
					}
					else if (variableType == VariableType.Property)
					{
						realProperty = target.GetComponent (targetComponent).GetType ().GetProperty (targetVariable);
						variableIsValid = true;
					}
				}
			}
		}

		public void Animate ()
		{
			if (variableIsValid)
			{
				if (valueType == "System.Single")
				{
					if (variableType == VariableType.Field)
					{
						initialFloat = (float) realField.GetValue(realComponent);
					}
					else if (variableType == VariableType.Property)
					{
						initialFloat = (float) realProperty.GetValue(realComponent, null);
					}
				}
				else if (valueType == "System.Int32")
				{
					if (variableType == VariableType.Field)
					{
						initialInt = (int) realField.GetValue(realComponent);
					}
					else if (variableType == VariableType.Property)
					{
						initialInt = (int) realProperty.GetValue(realComponent, null);
					}
				}
				else if (valueType == "UnityEngine.Vector2")
				{
					if (variableType == VariableType.Field)
					{
						initialVec2 = (Vector2) realField.GetValue(realComponent);
						tempVec2 = initialVec2;
					}
					else if (variableType == VariableType.Property)
					{
						initialVec2 = (Vector2) realProperty.GetValue(realComponent, null);
						tempVec2 = initialVec2;
					}
				}
				else if (valueType == "UnityEngine.Vector3")
				{
					if (variableType == VariableType.Field)
					{
						initialVec3 = (Vector3) realField.GetValue(realComponent);
						tempVec3 = initialVec3;
					}
					else if (variableType == VariableType.Property)
					{
						initialVec3 = (Vector3) realProperty.GetValue(realComponent, null);
						tempVec3 = initialVec3;
					}
				}
				else if (valueType == "UnityEngine.Rect")
				{
					if (variableType == VariableType.Field)
					{
						initialRect = (Rect) realField.GetValue(realComponent);
						tempRect = initialRect;
					}
					else if (variableType == VariableType.Property)
					{
						initialRect = (Rect) realProperty.GetValue(realComponent, null);
						tempRect = initialRect;
					}
				}
				else if (valueType == "UnityEngine.Color")
				{
					if (variableType == VariableType.Field)
					{
						initialColor = (Color) realField.GetValue(realComponent);
						tempColor = initialColor;
					}
					else if (variableType == VariableType.Property)
					{
						initialColor = (Color) realProperty.GetValue(realComponent, null);
						tempColor = initialColor;
					}
				}
				else if (valueType == "UnityEngine.Material")
				{
					if (variableType == VariableType.Field)
					{
						initialMaterial = (Material) realField.GetValue(realComponent);
						initialMatColor = initialMaterial.color;

						tempMaterial = initialMaterial;
						tempMatColor = initialMatColor;
					}
					else if (variableType == VariableType.Property)
					{
						initialMaterial = (Material) realProperty.GetValue(realComponent, null);
						initialMatColor = initialMaterial.color;
						
						tempMaterial = initialMaterial;
						tempMatColor = initialMatColor;
					}
				}
			}
			else
			{
				Debug.Log("Target variable isn't valid!");
			}

			animStartTime = Time.realtimeSinceStartup;
			state = 1;
		}

		void Update ()
		{
			if (state == 1)
			{
				animDeltaTime = Time.realtimeSinceStartup - animStartTime;

				if (animDeltaTime < duration)
				{
					if (valueType == "System.Single")
					{
						tempFloat = UpdateAnimation(initialFloat, targetFloat, animDeltaTime, duration);
						
						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, tempFloat);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, tempFloat, null);
					}
					else if (valueType == "System.Int32")
					{
						tempInt = Mathf.RoundToInt(UpdateAnimation(initialInt, targetInt, animDeltaTime, duration));
						
						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, tempInt);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, tempInt, null);
					}
					else if (valueType == "UnityEngine.Vector2")
					{
						if (modifyVec2X)
							tempVec2.x = UpdateAnimation(initialVec2.x, targetVec2.x, animDeltaTime, duration);
						
						if (modifyVec2Y)
							tempVec2.y = UpdateAnimation(initialVec2.y, targetVec2.y, animDeltaTime, duration);
						
						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, tempVec2);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, tempVec2, null);
					}
					else if (valueType == "UnityEngine.Vector3")
					{
						if (modifyVec3X)
							tempVec3.x = UpdateAnimation(initialVec3.x, targetVec3.x, animDeltaTime, duration);

						if (modifyVec3Y)
							tempVec3.y = UpdateAnimation(initialVec3.y, targetVec3.y, animDeltaTime, duration);

						if (modifyVec3Z)
							tempVec3.z = UpdateAnimation(initialVec3.z, targetVec3.z, animDeltaTime, duration);

						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, tempVec3);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, tempVec3, null);
					}
					else if (valueType == "UnityEngine.Rect")
					{
						if (modifyRectWidth)
							tempRect.width = UpdateAnimation(initialRect.width, targetRect.width, animDeltaTime, duration);

						if (modifyRectHeight)
							tempRect.height = UpdateAnimation(initialRect.height, targetRect.height, animDeltaTime, duration);

						if (modifyRectX)
							tempRect.x = UpdateAnimation(initialRect.x, targetRect.x, animDeltaTime, duration);

						if (modifyRectY)
							tempRect.y = UpdateAnimation(initialRect.y, targetRect.y, animDeltaTime, duration);
						
						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, tempRect);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, tempRect, null);
					}
					else if (valueType == "UnityEngine.Color")
					{
						if (modifyColorR)
							tempColor.r = UpdateAnimation(initialColor.r, targetColor.r, animDeltaTime, duration);

						if (modifyColorG)
							tempColor.g = UpdateAnimation(initialColor.g, targetColor.g, animDeltaTime, duration);
						
						if (modifyColorB)
							tempColor.b = UpdateAnimation(initialColor.b, targetColor.b, animDeltaTime, duration);
						
						if (modifyColorA)
							tempColor.a = UpdateAnimation(initialColor.a, targetColor.a, animDeltaTime, duration);
						
						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, tempColor);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, tempColor, null);
					}
					else if (valueType == "UnityEngine.Material")
					{
						if (modifyMatColorR)
							tempMatColor.r = UpdateAnimation(initialMatColor.r, targetMatColor.r, animDeltaTime, duration);
						
						if (modifyMatColorG)
							tempMatColor.g = UpdateAnimation(initialMatColor.g, targetMatColor.g, animDeltaTime, duration);
						
						if (modifyMatColorB)
							tempMatColor.b = UpdateAnimation(initialMatColor.b, targetMatColor.b, animDeltaTime, duration);
						
						if (modifyMatColorA)
							tempMatColor.a = UpdateAnimation(initialMatColor.a, targetMatColor.a, animDeltaTime, duration);

						tempMaterial.color = tempMatColor;

						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, tempMaterial);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, tempMaterial, null);
					}
				}
				else
				{
					if (valueType == "System.Single")
					{
						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, targetFloat);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, targetFloat, null);
					}
					else if (valueType == "System.Int32")
					{
						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, targetInt);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, targetInt, null);
					}
					else if (valueType == "UnityEngine.Vector2")
					{
						if (modifyVec2X)
							tempVec2.x = targetVec2.x;
						
						if (modifyVec2Y)
							tempVec2.y = targetVec2.y;
						
						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, tempVec2);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, tempVec2, null);
					}
					else if (valueType == "UnityEngine.Vector3")
					{
						if (modifyVec3X)
							tempVec3.x = targetVec3.x;
						
						if (modifyVec3Y)
							tempVec3.y = targetVec3.y;
						
						if (modifyVec3Z)
							tempVec3.z = targetVec3.z;
						
						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, tempVec3);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, tempVec3, null);
					}
					else if (valueType == "UnityEngine.Rect")
					{
						if (modifyRectWidth)
							tempRect.width = targetRect.width;
						
						if (modifyRectHeight)
							tempRect.height = targetRect.height;
						
						if (modifyRectX)
							tempRect.x = targetRect.x;
						
						if (modifyRectY)
							tempRect.y = targetRect.y;
						
						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, tempRect);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, tempRect, null);
					}
					else if (valueType == "UnityEngine.Color")
					{
						if (modifyColorR)
							tempColor.r = targetColor.r;
						
						if (modifyColorG)
							tempColor.g = targetColor.g;
						
						if (modifyColorB)
							tempColor.b = targetColor.b;
						
						if (modifyColorA)
							tempColor.a = targetColor.a;
						
						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, tempColor);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, tempColor, null);
					}
					else if (valueType == "UnityEngine.Material")
					{
						if (modifyMatColorR)
							tempMatColor.r = targetMatColor.r;
						
						if (modifyMatColorG)
							tempMatColor.g = targetMatColor.g;
						
						if (modifyMatColorB)
							tempMatColor.b = targetMatColor.b;
						
						if (modifyMatColorA)
							tempMatColor.a = targetMatColor.a;
						
						tempMaterial.color = tempMatColor;
						
						if (variableType == VariableType.Field)
							realField.SetValue(realComponent, tempMaterial);
						else if (variableType == VariableType.Property)
							realProperty.SetValue(realComponent, tempMaterial, null);
					}

					state = 0;
				}
			}
		}

		float UpdateAnimation (float initialValue, float targetValue, float time, float duration)
		{
			switch (animationType)
			{
				case AnimationType.Linear:
					return Anim.Linear(initialValue, targetValue, time, duration);
				case AnimationType.EaseOutCubed:
					return Anim.Cube.Out(initialValue, targetValue, time, duration);
				case AnimationType.EaseOutQuint:
					return Anim.Quint.Out(initialValue, targetValue, time, duration);
				case AnimationType.EaseOutSept:
					return Anim.Sept.Out(initialValue, targetValue, time, duration);
				case AnimationType.EaseInCubed:
					return Anim.Cube.In(initialValue, targetValue, time, duration);
				case AnimationType.EaseInQuint:
					return Anim.Quint.In(initialValue, targetValue, time, duration);
				case AnimationType.EaseInSept:
					return Anim.Sept.In(initialValue, targetValue, time, duration);
				case AnimationType.EaseInOutCubed:
					return Anim.Cube.InOut(initialValue, targetValue, time, duration);
				case AnimationType.EaseInOutQuint:
					return Anim.Quint.InOut(initialValue, targetValue, time, duration);
				case AnimationType.EaseInOutSept:
					return Anim.Sept.InOut(initialValue, targetValue, time, duration);
				case AnimationType.SoftEaseOutCubed:
					return Anim.Cube.SoftOut(initialValue, targetValue, time, duration);
				case AnimationType.SoftEaseOutQuint:
					return Anim.Quint.SoftOut(initialValue, targetValue, time, duration);
				case AnimationType.SoftEaseOutSept:
					return Anim.Sept.SoftOut(initialValue, targetValue, time, duration);
			}

			return 0f;
		}

	}
}
