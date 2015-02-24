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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MaterialUI
{
	public class EZAnim : MonoBehaviour
	{
		[HideInInspector()]
		public List<EZStruct> theStructs;

		private bool[] activeList;

		private EZStruct tempStruct;
		private EZStruct oldStruct;

		private float tempFloat;
		private Vector2 tempVector2;
		private Vector3 tempVector3;
		private Rect tempRect;
		private Color tempColor;
		private Material tempMaterial;

		void Start ()
		{
			activeList = new bool[theStructs.Count];

			for (int i = 0; i < theStructs.Count; i++)
			{
				tempStruct = theStructs[i];

				tempStruct.RealComponent = tempStruct.targetGameObject.GetComponent(tempStruct.targetComponent);

				if (tempStruct.valueType == ValType.Field)
					tempStruct.realField = tempStruct.targetGameObject.GetComponent(tempStruct.targetComponent).GetType().GetField(tempStruct.targetVariable);
				
				if (tempStruct.valueType == ValType.Property)
					tempStruct.realProperty = tempStruct.targetGameObject.GetComponent(tempStruct.targetComponent).GetType().GetProperty(tempStruct.targetVariable);

				

				if (tempStruct.methodOnEnd)
				{
					tempStruct.methodRealComponent = tempStruct.methodTargetGameObject.GetComponent(tempStruct.methodTargetComponent);
					tempStruct.methodRealMethod = tempStruct.methodTargetGameObject.GetComponent(tempStruct.methodTargetComponent).GetType().GetMethod(tempStruct.methodTargetMethod);
				}
					
				theStructs[i] = tempStruct;
			}
		}

		private void AnimStruct(int i)
		{
			activeList[i] = true;
			tempStruct = theStructs[i];

			tempStruct.animStartTime = Time.realtimeSinceStartup + tempStruct.delay;

			if (tempStruct.variableType == "System.Single")
			{
				if (tempStruct.valueType == ValType.Field)
					tempStruct.initialFloat = (float)tempStruct.realField.GetValue(tempStruct.RealComponent);
				else if (tempStruct.valueType == ValType.Property)
					tempStruct.initialFloat = (float)tempStruct.realProperty.GetValue(tempStruct.RealComponent, null);
			}
			else if (tempStruct.variableType == "System.Int32")
			{
				if (tempStruct.valueType == ValType.Field)
					tempStruct.initialInt = (int)tempStruct.realField.GetValue(tempStruct.RealComponent);
				else if (tempStruct.valueType == ValType.Property)
					tempStruct.initialInt = (int)tempStruct.realProperty.GetValue(tempStruct.RealComponent, null);
			}
			else if (tempStruct.variableType == "UnityEngine.Vector2")
			{
				if (tempStruct.valueType == ValType.Field)
					tempStruct.initialVector2 = (Vector2)tempStruct.realField.GetValue(tempStruct.RealComponent);
				else if (tempStruct.valueType == ValType.Property)
					tempStruct.initialVector2 = (Vector2)tempStruct.realProperty.GetValue(tempStruct.RealComponent, null);
			}
			else if (tempStruct.variableType == "UnityEngine.Vector3")
			{
				if (tempStruct.valueType == ValType.Field)
					tempStruct.initialVector3 = (Vector3)tempStruct.realField.GetValue(tempStruct.RealComponent);
				else if (tempStruct.valueType == ValType.Property)
					tempStruct.initialVector3 = (Vector3)tempStruct.realProperty.GetValue(tempStruct.RealComponent, null);
			}
			else if (tempStruct.variableType == "UnityEngine.Rect")
			{
				if (tempStruct.valueType == ValType.Field)
					tempStruct.initialRect = (Rect)tempStruct.realField.GetValue(tempStruct.RealComponent);
				else if (tempStruct.valueType == ValType.Property)
					tempStruct.initialRect = (Rect)tempStruct.realProperty.GetValue(tempStruct.RealComponent, null);
			}
			else if (tempStruct.variableType == "UnityEngine.Color")
			{
				if (tempStruct.valueType == ValType.Field)
					tempStruct.initialColor = (Color)tempStruct.realField.GetValue(tempStruct.RealComponent);
				else if (tempStruct.valueType == ValType.Property)
					tempStruct.initialColor = (Color)tempStruct.realProperty.GetValue(tempStruct.RealComponent, null);
			}
			else if (tempStruct.variableType == "UnityEngine.Material")
			{
				if (tempStruct.valueType == ValType.Field)
					tempStruct.initialMaterial = (Material)tempStruct.realField.GetValue(tempStruct.RealComponent);
				else if (tempStruct.valueType == ValType.Property)
					tempStruct.initialMaterial = (Material)tempStruct.realProperty.GetValue(tempStruct.RealComponent, null);
			}

			tempStruct.called = false;

			theStructs[i] = tempStruct;
		}

		public void Animate()
		{
			AnimStruct(0);
		}

		public void AnimateAll()
		{
			for (int i = 0; i < theStructs.Count; i++)
			{
				AnimStruct(i);
			}
		}

		public void AnimateByIndex(int index)
		{
			for (int i = 0; i < theStructs.Count; i++)
			{
				if (i == index)
					AnimStruct(i);
			}
		}

		public void AnimateByName(string name)
		{
			for (int i = 0; i < theStructs.Count; i++)
			{
				if (theStructs[i].animName == name)
					AnimStruct(i);
			}
		}

		public void AnimateByTag(string tag)
		{
			for (int i = 0; i < theStructs.Count; i++)
			{
				if (theStructs[i].animTag == tag)
					AnimStruct(i);
			}
		}

		void Update()
		{
			for (int i = 0; i < theStructs.Count; i++)
			{
				if (activeList[i] == true)
				{
					tempStruct = theStructs[i];

					tempStruct.animDeltaTime = Time.realtimeSinceStartup - tempStruct.animStartTime;

					if (tempStruct.animDeltaTime < tempStruct.animationDuration)
					{
						if (tempStruct.variableType == "System.Single")
						{
							tempFloat = UpdateAnimation(tempStruct.initialFloat, tempStruct.targetFloat, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, tempFloat);
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, tempFloat, null);
						}
						else if (tempStruct.variableType == "System.Int32")
						{
							tempFloat = UpdateAnimation(tempStruct.initialInt, tempStruct.targetInt, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, Mathf.RoundToInt(tempFloat));
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, Mathf.RoundToInt(tempFloat), null);
						}
						else if (tempStruct.variableType == "UnityEngine.Vector2")
						{
							tempVector2 = tempStruct.initialVector2;

							if (tempStruct.modifyParameter1)
								tempVector2.x = UpdateAnimation(tempStruct.initialVector2.x, tempStruct.targetVector2.x, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.modifyParameter2)
								tempVector2.y = UpdateAnimation(tempStruct.initialVector2.y, tempStruct.targetVector2.y, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, tempVector2);
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, tempVector2, null);
						}
						else if (tempStruct.variableType == "UnityEngine.Vector3")
						{
							tempVector3 = tempStruct.initialVector3;

							if (tempStruct.modifyParameter1)
								tempVector3.x = UpdateAnimation(tempStruct.initialVector3.x, tempStruct.targetVector3.x, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.modifyParameter2)
								tempVector3.y = UpdateAnimation(tempStruct.initialVector3.y, tempStruct.targetVector3.y, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.modifyParameter3)
								tempVector3.z = UpdateAnimation(tempStruct.initialVector3.z, tempStruct.targetVector3.z, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, tempVector3);
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, tempVector3, null);
						}
						else if (tempStruct.variableType == "UnityEngine.Rect")
						{
							tempRect = tempStruct.initialRect;

							if (tempStruct.modifyParameter1)
								tempRect.x = UpdateAnimation(tempStruct.initialRect.x, tempStruct.targetRect.x, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.modifyParameter2)
								tempRect.y = UpdateAnimation(tempStruct.initialRect.y, tempStruct.targetRect.y, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.modifyParameter3)
								tempRect.width = UpdateAnimation(tempStruct.initialRect.width, tempStruct.targetRect.width, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.modifyParameter4)
								tempRect.height = UpdateAnimation(tempStruct.initialRect.height, tempStruct.targetRect.height, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, tempRect);
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, tempRect, null);
						}
						else if (tempStruct.variableType == "UnityEngine.Color")
						{
							tempColor = tempStruct.initialColor;

							if (tempStruct.modifyParameter1)
								tempColor.r = UpdateAnimation(tempStruct.initialColor.r, tempStruct.targetColor.r, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.modifyParameter2)
								tempColor.g = UpdateAnimation(tempStruct.initialColor.g, tempStruct.targetColor.g, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.modifyParameter3)
								tempColor.b = UpdateAnimation(tempStruct.initialColor.b, tempStruct.targetColor.b, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.modifyParameter4)
								tempColor.a = UpdateAnimation(tempStruct.initialColor.a, tempStruct.targetColor.a, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, tempColor);
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, tempColor, null);
						}
						else if (tempStruct.variableType == "UnityEngine.Material")
						{
							tempMaterial = tempStruct.initialMaterial;
							tempColor = tempMaterial.color;

							if (tempStruct.modifyParameter1)
								tempColor.r = UpdateAnimation(tempStruct.initialColor.r, tempStruct.initialColor.r, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.modifyParameter2)
								tempColor.g = UpdateAnimation(tempStruct.initialColor.g, tempStruct.initialColor.g, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.modifyParameter3)
								tempColor.b = UpdateAnimation(tempStruct.initialColor.b, tempStruct.initialColor.b, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							if (tempStruct.modifyParameter4)
								tempColor.a = UpdateAnimation(tempStruct.initialColor.a, tempStruct.initialColor.a, tempStruct.animDeltaTime, tempStruct.animationDuration, tempStruct.animationType);

							tempMaterial.color = tempColor;

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, tempMaterial);
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, tempMaterial, null);
						}
					}
					else
					{
						if (tempStruct.variableType == "System.Single")
						{
							tempFloat = tempStruct.targetFloat;

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, tempFloat);
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, tempFloat, null);
						}
						else if (tempStruct.variableType == "System.Int32")
						{
							tempFloat = tempStruct.targetFloat;

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, Mathf.RoundToInt(tempFloat));
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, Mathf.RoundToInt(tempFloat), null);
						}
						else if (tempStruct.variableType == "UnityEngine.Vector2")
						{
							tempVector2 = tempStruct.initialVector2;

							if (tempStruct.modifyParameter1)
								tempVector2.x = tempStruct.targetVector2.x;

							if (tempStruct.modifyParameter2)
								tempVector2.y = tempStruct.targetVector2.y;

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, tempVector2);
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, tempVector2, null);
						}
						else if (tempStruct.variableType == "UnityEngine.Vector3")
						{
							tempVector3 = tempStruct.initialVector3;

							if (tempStruct.modifyParameter1)
								tempVector3.x = tempStruct.targetVector3.x;

							if (tempStruct.modifyParameter2)
								tempVector3.y = tempStruct.targetVector3.y;

							if (tempStruct.modifyParameter3)
								tempVector3.z = tempStruct.targetVector3.z;

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, tempVector3);
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, tempVector3, null);
						}
						else if (tempStruct.variableType == "UnityEngine.Rect")
						{
							tempRect = tempStruct.initialRect;

							if (tempStruct.modifyParameter1)
								tempRect.x = tempStruct.targetRect.x;

							if (tempStruct.modifyParameter2)
								tempRect.y = tempStruct.targetRect.y;

							if (tempStruct.modifyParameter3)
								tempRect.width = tempStruct.targetRect.width;

							if (tempStruct.modifyParameter4)
								tempRect.height = tempStruct.targetRect.height;

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, tempRect);
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, tempRect, null);
						}
						else if (tempStruct.variableType == "UnityEngine.Color")
						{
							tempColor = tempStruct.initialColor;

							if (tempStruct.modifyParameter1)
								tempColor.r = tempStruct.targetColor.r;

							if (tempStruct.modifyParameter2)
								tempColor.g = tempStruct.targetColor.g;

							if (tempStruct.modifyParameter3)
								tempColor.b = tempStruct.targetColor.b;

							if (tempStruct.modifyParameter4)
								tempColor.a = tempStruct.targetColor.a;

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, tempColor);
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, tempColor, null);
						}
						else if (tempStruct.variableType == "UnityEngine.Material")
						{
							tempMaterial = tempStruct.initialMaterial;
							tempColor = tempMaterial.color;

							if (tempStruct.modifyParameter1)
								tempColor.r = tempStruct.initialColor.r;

							if (tempStruct.modifyParameter2)
								tempColor.g = tempStruct.initialColor.g;

							if (tempStruct.modifyParameter3)
								tempColor.b = tempStruct.initialColor.b;

							if (tempStruct.modifyParameter4)
								tempColor.a = tempStruct.initialColor.a;

							tempMaterial.color = tempColor;

							if (tempStruct.valueType == ValType.Field)
								tempStruct.realField.SetValue(tempStruct.RealComponent, tempMaterial);
							else if (tempStruct.valueType == ValType.Property)
								tempStruct.realProperty.SetValue(tempStruct.RealComponent, tempMaterial, null);
						}
						activeList[i] = false;

						if (tempStruct.methodOnEnd && !tempStruct.called)
						{
							StartCoroutine(delayedCall());
							oldStruct = tempStruct;
							tempStruct.called = true;
						}
					}
					
					theStructs[i] = tempStruct;
				}
			}
		}

		IEnumerator delayedCall()
		{
			yield return new WaitForEndOfFrame();

			object[] parametersArray = new object[] { oldStruct.methodParam };

			oldStruct.methodRealMethod.Invoke(oldStruct.methodRealComponent, parametersArray);
		}

		float UpdateAnimation(float initialValue, float targetValue, float time, float duration, AnimType animationType)
		{
			switch (animationType)
			{
				case AnimType.Linear:
					return Anim.Linear(initialValue, targetValue, time, duration);
				case AnimType.Overshoot:
					return Anim.Overshoot(initialValue, targetValue, time, duration);
				case AnimType.Bounce:
					return Anim.Bounce(initialValue, targetValue, time, duration);
				case AnimType.EaseOutCubed:
					return Anim.Cube.Out(initialValue, targetValue, time, duration);
				case AnimType.EaseOutQuint:
					return Anim.Quint.Out(initialValue, targetValue, time, duration);
				case AnimType.EaseOutSept:
					return Anim.Sept.Out(initialValue, targetValue, time, duration);
				case AnimType.EaseInCubed:
					return Anim.Cube.In(initialValue, targetValue, time, duration);
				case AnimType.EaseInQuint:
					return Anim.Quint.In(initialValue, targetValue, time, duration);
				case AnimType.EaseInSept:
					return Anim.Sept.In(initialValue, targetValue, time, duration);
				case AnimType.EaseInOutCubed:
					return Anim.Cube.InOut(initialValue, targetValue, time, duration);
				case AnimType.EaseInOutQuint:
					return Anim.Quint.InOut(initialValue, targetValue, time, duration);
				case AnimType.EaseInOutSept:
					return Anim.Sept.InOut(initialValue, targetValue, time, duration);
				case AnimType.SoftEaseOutCubed:
					return Anim.Cube.SoftOut(initialValue, targetValue, time, duration);
				case AnimType.SoftEaseOutQuint:
					return Anim.Quint.SoftOut(initialValue, targetValue, time, duration);
				case AnimType.SoftEaseOutSept:
					return Anim.Sept.SoftOut(initialValue, targetValue, time, duration);
			}
			return 0f;
		}
	}
}
