//  Copyright 2014 Invex Games http://invexgames.com
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.

//	Credit to the 'EventTriggerEditor' script at http://www.cratesmith.com/archives/221, which helped immensely

//	May the gods bestow the mightiest of luck upon any who find
//	themselves staring into the abyss that is this wretched code
//	(I'll be cleaning this up and improving on it in the near future)

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(AnimTrigger))]
public class AnimTriggerEditor : Editor
{
	private bool showDoc = false;
	private bool showAdv = false;
	public int variableType;
	public string valueType;

//	public enum AnimationType{Linear, EaseOutCubed, EaseOutQuint, EaseOutSept, EaseInCubed, EaseInQuint, EaseInSept, EaseInOutCubed, EaseInOutQuint, EaseInOutSept, SoftEaseOutCubed, SoftEaseOutQuint, SoftEaseOutSept};
	public int animationType;
	
	string[] exclusions;
	string[] replacements;
	string[] replacementNames;
	string[] inclusions;
	string[] inclusionNames;
	
	private string[] FindComponentStrings(GameObject targetGameObject)
	{
		List<string> componentStrings = new List<string>();
		Component[] components = targetGameObject.GetComponents<Component>();
		bool isExcluded;
		foreach(Component component in components)
		{
			isExcluded = false;

			for(int i = 0; i < exclusions.Length; i++)
			{
				if (component.GetType().ToString() == exclusions[i])
				{
					isExcluded = true;
				}
			}

			for(int i = 0; i < replacements.Length; i++)
			{
				if (component.GetType().ToString() == replacements[i])
				{
					isExcluded = true;
					componentStrings.Add(replacementNames[i]);
				}
			}

			if (!isExcluded)
				componentStrings.Add(component.GetType().ToString());
		}

		componentStrings.Add("--NONE--");
		return componentStrings.ToArray();
	}

	private string[] FindVariableStrings(GameObject targetGameObject, string targetComponent)
	{
		List<string> variableStrings = new List<string>();

		if (targetGameObject)
		{
			if (targetGameObject.GetComponent(targetComponent))
			{
				Component realComponent = targetGameObject.GetComponent(targetComponent);
				System.Type behaviorType = realComponent.GetType ();

				if (variableType == 0)
				{
					FieldInfo[] fields = behaviorType.GetFields();
					foreach(FieldInfo field in fields)
					{
						string fieldType = field.FieldType.ToString();
						foreach (string inclusion in inclusions)
						{
							if (fieldType == inclusion)
							{
								variableStrings.Add(field.Name);
							}
						}
					}
				}
				else if (variableType == 1)
				{
					PropertyInfo[] properties = behaviorType.GetProperties();
					foreach(PropertyInfo property in properties)
					{
						string propertyType = property.PropertyType.ToString();

						foreach (string inclusion in inclusions)
						{
							if (propertyType == inclusion && property.CanWrite)
							{
								variableStrings.Add(property.Name);
							}
						}
					}
				}
			}
		}

		variableStrings.Add("--NONE--");
		return variableStrings.ToArray();
	}

	public override void OnInspectorGUI()
	{
		OnInspectorGUI_Settings();
		OnInspectorGUI_Advanced ();
	}
	
	public void OnInspectorGUI_Settings()
	{
		AnimTrigger  trigger = (AnimTrigger)target;
		GameObject    targetObject = trigger.target;

		exclusions = new string[] {"UnityEngine.MeshFilter", "UnityEngine.BoxCollider"};
		replacements = new string[] {"UnityEngine.Transform", "UnityEngine.RectTransform", "UnityEngine.UI.Image", "UnityEngine.CharacterController", "UnityEngine.Animation", "UnityEngine.MeshRenderer"};
		replacementNames = new string[] {"Transform", "RectTransform", "Image", "CharacterController", "Animation", "Renderer"};
		
		inclusions = new string[] {"System.Single", "System.Int32", "UnityEngine.Vector2", "UnityEngine.Vector3", "UnityEngine.Rect", "UnityEngine.Color", "UnityEngine.Material"};
		
		inclusionNames = new string[] {"Float", "Int", "Vector2", "Vector3", "Rect", "Color", "Material"};

		trigger.target = (GameObject)EditorGUILayout.ObjectField("Target Object", (Object)trigger.target, typeof(GameObject));

		string[] compStrings = targetObject!=null ? FindComponentStrings(targetObject):new string[] {"No object selected"};

		SerializedObject m_object = new SerializedObject(target);
		SerializedProperty prop;

		m_object.Update();

		trigger.targetComponent = StringPopup("Component", compStrings, trigger.targetComponent);

		prop = m_object.FindProperty ("variableType");

		EditorGUILayout.PropertyField (prop, true);
		variableType = prop.enumValueIndex;

		string[] strings = targetObject!=null ? FindVariableStrings(targetObject, trigger.targetComponent):new string[] {"No object selected"};

		trigger.targetVariable = StringPopup("Variable", strings, trigger.targetVariable);

		EditorGUILayout.Space ();

		if (variableType == 0)
		{
			if (trigger.target != null && trigger.targetComponent != null)
			{
				if (trigger.target.GetComponent(trigger.targetComponent))
				{
					if (trigger.target.GetComponent (trigger.targetComponent).GetType ().GetField (trigger.targetVariable) != null)
					{
						FieldInfo realField = trigger.target.GetComponent (trigger.targetComponent).GetType ().GetField (trigger.targetVariable);

						for (int i = 0; i < inclusions.Length; i++)
						{
//							Debug.Log(realField.FieldType.ToString ());
							if (realField.FieldType.ToString() == inclusions[i])
							{
								EditorGUILayout.LabelField ("Variable is of type " + inclusionNames[i]);
								trigger.valueType = inclusions[i];
							}
						}
					}
					else
						trigger.valueType = "";
				}
				else
					trigger.valueType = "";
			}
			else
				trigger.valueType = "";
		}
		else if (variableType == 1)
		{
			if (trigger.target != null && trigger.targetComponent != null)
			{
				if (trigger.target.GetComponent(trigger.targetComponent))
			    {
					if (trigger.target.GetComponent (trigger.targetComponent).GetType ().GetProperty (trigger.targetVariable) != null)
					{
						PropertyInfo realField = trigger.target.GetComponent (trigger.targetComponent).GetType ().GetProperty (trigger.targetVariable);
						
						for (int i = 0; i < inclusions.Length; i++)
						{
							if (realField.PropertyType.ToString() == inclusions[i])
							{
								EditorGUILayout.LabelField ("Variable is of type " + inclusionNames[i]);
								trigger.valueType = inclusions[i];
							}
						}
					}
					else
						trigger.valueType = "";
				}
				else
					trigger.valueType = "";
			}
			else
				trigger.valueType = "";
		}

		EditorGUILayout.Space ();

		valueType = trigger.valueType;

		if (valueType == "System.Single")
		{
			prop = m_object.FindProperty ("targetFloat");
			EditorGUILayout.PropertyField (prop, true);
		}
		else if (valueType == "System.Int32")
		{
			prop = m_object.FindProperty ("targetInt");
			EditorGUILayout.PropertyField (prop, true);
		}
		else if (valueType == "UnityEngine.Vector2")
		{
			prop = m_object.FindProperty ("modifyVec2X");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify x value"), true);

			prop = m_object.FindProperty ("modifyVec2Y");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify y value"), true);

			prop = m_object.FindProperty ("targetVec2");
			EditorGUILayout.PropertyField (prop, true);
		}
		else if (valueType == "UnityEngine.Vector3")
		{
			prop = m_object.FindProperty ("modifyVec3X");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify x value"), true);
			
			prop = m_object.FindProperty ("modifyVec3Y");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify y value"), true);
			
			prop = m_object.FindProperty ("modifyVec3Z");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify z value"), true);
			
			prop = m_object.FindProperty ("targetVec3");
			EditorGUILayout.PropertyField (prop, true);
		}
		else if (valueType == "UnityEngine.Rect")
		{
			prop = m_object.FindProperty ("modifyRectWidth");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify width value"), true);

			prop = m_object.FindProperty ("modifyRectHeight");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify height value"), true);

			prop = m_object.FindProperty ("modifyRectX");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify x value"), true);

			prop = m_object.FindProperty ("modifyRectY");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify y value"), true);

			prop = m_object.FindProperty ("targetRect");
			EditorGUILayout.PropertyField (prop, true);
		}
		else if (valueType == "UnityEngine.Color")
		{
			prop = m_object.FindProperty ("modifyColorR");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify red value"), true);
			
			prop = m_object.FindProperty ("modifyColorG");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify green value"), true);
			
			prop = m_object.FindProperty ("modifyColorB");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify blue value"), true);
			
			prop = m_object.FindProperty ("modifyColorA");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify alpha value"), true);
			
			prop = m_object.FindProperty ("targetColor");
			EditorGUILayout.PropertyField (prop, true);
		}
		else if (valueType == "UnityEngine.Material")
		{
			prop = m_object.FindProperty ("modifyMatColorR");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify red value"), true);
			
			prop = m_object.FindProperty ("modifyMatColorG");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify green value"), true);
			
			prop = m_object.FindProperty ("modifyMatColorB");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify blue value"), true);
			
			prop = m_object.FindProperty ("modifyMatColorA");
			EditorGUILayout.PropertyField (prop, new GUIContent("Modify alpha value"), true);
			
			prop = m_object.FindProperty ("targetMatColor");
			EditorGUILayout.PropertyField (prop, new GUIContent("Target Material Color"), true);
		}

		EditorGUILayout.Space ();

		prop = m_object.FindProperty ("animationType");
		EditorGUILayout.PropertyField (prop, true);
		variableType = prop.enumValueIndex;

		prop = m_object.FindProperty ("duration");
		EditorGUILayout.PropertyField (prop, true);

		EditorGUILayout.Space ();

		m_object.ApplyModifiedProperties();
	}
	
	public void OnInspectorGUI_Advanced ()
	{
		showAdv = EditorGUILayout.Foldout(showAdv, "Advanced");
		if(showAdv)
			DrawDefaultInspector();
	}
	
	private static string StringPopup (string label, string[] strings, string str)
	{
		int strId = System.Array.IndexOf(strings,str);
		
		if(strId==-1)
			strId = strings.Length-1;
		
		strId = EditorGUILayout.Popup(label, strId, strings, "popup");
		
		if(strId==-1 || strId==strings.Length-1)
			return "";
		
		return strings[strId];
	}
}
