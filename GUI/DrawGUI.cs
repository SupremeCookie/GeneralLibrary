﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public enum OffsetType
{
	Left,
	Right,
};


#if DEBUG
//Note: If needed, make GUISkin to change style of assets.
public class DrawGUI : SingletonMonoBehaviour<DrawGUI>
{
	public event System.Action OnDrawGUI;

#if UNITY_EDITOR && false
	private bool _drawGui = true;
#else
	private bool _drawGui = false;
#endif


	public bool IsVisible { get { return _drawGui; } }


	private Dictionary<string, DrawGUIGroup> groups;

	protected override void Awake()
	{
		base.Awake();

		groups = new Dictionary<string, DrawGUIGroup>();
	}

	public void OnGUI()
	{
		if (!_drawGui)
		{
			return;
		}

		var guiMatrix = GUI.matrix;
		GUI.matrix = DrawGUI.GetScreenScaledMatrix();

		_elementYOffsetLeft = new Dictionary<int, float>();
		_elementYOffsetRight = new Dictionary<int, float>();

#if GLOBALDATASCRIPTABLE_AVAILABLE
		_activeGroup = IniControl.GlobalData.DebugWindow_LastActiveWindowName;
#endif

		groups.Clear();
		//Debug.Log("Pre Draw GUI, group count: " + groups?.Count);

		//Todo experiment with the order of these calls.
		{
			OnDrawMainWindow();

			OnDrawGUI?.Invoke();

			OnDrawGroups();

			OnDrawActiveScreen();
		}

		//Debug.Log("Post Draw GUI, group count: " + groups?.Count);

		GUI.matrix = guiMatrix;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			_drawGui = !_drawGui;
		}
	}

	//TODO: store the size of the window in some file somewhere, similar to the Tweakables system in monopoly.
	//But for that I first need a system like that ;)

	private const int indent = 4;

#pragma warning disable 0649
	private Rect _mainWindow;
	private Rect _groupSection;
	private Rect _contentSection;
#pragma warning restore 0649

	private string _activeGroup;

	private void OnDrawMainWindow()
	{
		// TODO
		// Draw resize control
		// Enable drag and drop of window.

#if GLOBALDATASCRIPTABLE_AVAILABLE
		var data = IniControl.GlobalData;

		// MainWindow
		var xPos = data.DebugWindow_XPos;
		if (xPos <= 0)
		{
			xPos = 15;
			data.DebugWindow_XPos = xPos;
		}

		var yPos = data.DebugWindow_YPos;
		if (yPos <= 0)
		{
			yPos = 15;
			data.DebugWindow_YPos = yPos;
		}

		var width = data.DebugWindow_Width;
		if (width <= 0)
		{
			width = 425;
			data.DebugWindow_Width = width;
		}

		var height = data.DebugWindow_Height;
		if (height <= 0)
		{
			height = 400;
			data.DebugWindow_Height = height;
		}

		// Groups
		var groupLabelsHeight = 45;
		groupLabelsHeight = Mathf.Min(groupLabelsHeight, height);

		// Content
		var contentYPos = yPos + Mathf.Max(0, groupLabelsHeight + (indent * 3));
		var contentHeight = Mathf.Max(0, height - (groupLabelsHeight + indent));

		// Rectangles
		_mainWindow = new Rect(xPos, yPos, width, height);
		_groupSection = new Rect(xPos + indent, yPos + indent, width - (indent * 2), groupLabelsHeight);
		_contentSection = new Rect(xPos + indent, contentYPos - 15, width - (indent * 2), contentHeight);

		// BG
		GUI.Box(_mainWindow, "");

		// Separator
		GUI.Box(new Rect(xPos, Mathf.Max(contentYPos - indent, 0), width, indent * 0.5f), "");
#endif
	}

	private void OnDrawGroups()
	{
		// Style
		var normalStyle = GUI.skin.GetStyle("button");
		normalStyle.alignment = TextAnchor.MiddleLeft;

		var activeStyle = new GUIStyle(GUI.skin.GetStyle("button"));
		activeStyle.alignment = TextAnchor.MiddleLeft;
		activeStyle.normal = activeStyle.active;

		// Positioning
		var spacing = 6;
		var elementHeight = _groupSection.height - (indent * 5);

		// Pre calculate width of group headers.
		var totalWidhtOfGroups = 0f;
		foreach (var group in groups)
		{
			var style = IsGroupActive(group.Key) ? activeStyle : normalStyle;
			var size = style.CalcSize(new GUIContent(group.Key));

			totalWidhtOfGroups += size.x + spacing;
		}

		var scrollViewRect = new Rect(_groupSection);
		scrollViewRect.height = elementHeight;
		scrollViewRect.width = totalWidhtOfGroups;

#if GLOBALDATASCRIPTABLE_AVAILABLE
		float xOffset = 0;

		// Place group buttons, inside a scrollview
		IniControl.GlobalData.DebugWindow_ScrollPos = GUI.BeginScrollView(_groupSection, IniControl.GlobalData.DebugWindow_ScrollPos, scrollViewRect, alwaysShowHorizontal: true, alwaysShowVertical: false);
		foreach (var group in groups)
		{
			var style = IsGroupActive(group.Key) ? activeStyle : normalStyle;
			var size = style.CalcSize(new GUIContent(group.Key));

			float xPos = _groupSection.x + xOffset + spacing;
			float yPos = _groupSection.y + indent;

			var rect = new Rect(xPos, yPos, size.x, elementHeight);

			if (GUI.Button(rect, group.Key, style))
			{
				ActivateGroup(group.Key);
			}

			xOffset += size.x;
		}
#endif
		GUI.EndScrollView();
	}

	private void OnDrawActiveScreen()
	{
		foreach (var group in groups)
		{
			if (group.Key.Equals(_activeGroup))
			{
				group.Value.Render();
			}
		}
	}



	public static bool CreateGroup(string groupName, out DrawGUIGroup group)
	{
		return Instance.InternalCreateGroup(groupName, out group);
	}

	private bool InternalCreateGroup(string groupName, out DrawGUIGroup group)
	{
		if (groups.ContainsKey(groupName))
		{
			group = groups[groupName];
		}
		else
		{
			group = new DrawGUIGroup(_contentSection);
			groups.Add(groupName, group);
		}

		return IsGroupActive(groupName);
	}

	private void ActivateGroup(string groupName)
	{
		_activeGroup = groupName;
#if GLOBALDATASCRIPTABLE_AVAILABLE
		IniControl.GlobalData.DebugWindow_LastActiveWindowName = _activeGroup;
#endif
		//Debug.Log(_activeGroup + " is Now Active");
	}

	private bool IsGroupActive(string groupName)
	{
		bool result = string.Equals(groupName, _activeGroup);

		//Debug.Log("activeGroup:  " + _activeGroup + "   testGroup: " + groupName + "    Is the same?: " + (result));
		return result;
	}



	#region MANUAL
	private const int rectWidth = 200;
	private const int rectHeight = 25;

	private float elementRightSideX = Screen.width - rectWidth;
	private Dictionary<int, float> _elementYOffsetLeft = new Dictionary<int, float>();
	private Dictionary<int, float> _elementYOffsetRight = new Dictionary<int, float>();

	#region OFFSETS
	private float elementYOffsetLeft(int index)
	{
		if (_elementYOffsetLeft.ContainsKey(index))
		{
			return _elementYOffsetLeft[index];
		}
		else
		{
			_elementYOffsetLeft.Add(index, 0f);
		}

		return 0f;
	}

	private void elementYOffsetLeft(int index, float value)
	{
		if (_elementYOffsetLeft.ContainsKey(index))
		{
			_elementYOffsetLeft[index] = value;
		}
		else
		{
			_elementYOffsetLeft.Add(index, value);
		}
	}

	private float elementYOffsetRight(int index)
	{
		if (_elementYOffsetRight.ContainsKey(index))
		{
			return _elementYOffsetRight[index];
		}
		else
		{
			_elementYOffsetRight.Add(index, 0);
		}

		return 0f;
	}

	private void elementYOffsetRight(int index, float value)
	{
		if (_elementYOffsetRight.ContainsKey(index))
		{
			_elementYOffsetRight[index] = value;
		}
		else
		{
			_elementYOffsetRight.Add(index, value);
		}
	}
	#endregion

	public static void Manual_IncreaseElementYOffset(OffsetType offset, int column)
	{
		if (offset == OffsetType.Left)
		{
			Instance.elementYOffsetLeft(column, Instance.elementYOffsetLeft(column) + 25f);
		}
		else
		{
			Instance.elementYOffsetRight(column, Instance.elementYOffsetRight(column) + 25f);
		}
	}

	public static Rect Manual_CreateRect(bool isLeftSide = true, int columnOffset = 0, int rowOffset = 0, int yOffsetCount = 1, float xOffset = 0f, float heightOffsetScalar = 0f, float widthScalar = 1f, float minusWidth = 0f, bool offsetIsMinusWidth = false)
	{
		float horizontalOffset = columnOffset * rectWidth;
		float verticalOffset = rowOffset * rectHeight;

		var xCoord = isLeftSide ? 0f + horizontalOffset : Instance.elementRightSideX - horizontalOffset;
		var yOffset = isLeftSide ? Instance.elementYOffsetLeft(columnOffset) : Instance.elementYOffsetRight(columnOffset);
		yOffset += verticalOffset;

		if (offsetIsMinusWidth)
			minusWidth = xOffset;

		var rect = new Rect(xCoord + xOffset, yOffset + (rectHeight * heightOffsetScalar), (rectWidth * widthScalar) - minusWidth, rectHeight);

		if (isLeftSide)
			Manual_AddYOffset(OffsetType.Left, yOffsetCount, columnOffset);
		else
			Manual_AddYOffset(OffsetType.Right, yOffsetCount, columnOffset);

		return rect;
	}

	public static Rect Manual_CreateFreeRect(Vector2 anchors, Vector2 size)
	{
		var guiScale = ScaledGUIScale();
		anchors = (anchors - (size * 0.5f)).DivideByVec2(guiScale);
		size = size.DivideByVec2(guiScale);

		Rect newRect = new Rect(anchors, size);
		return newRect;
	}

	public static Rect Manual_CreateBox(Vector2 anchors, Vector2 size)
	{
		Rect newRect = Manual_CreateFreeRect(anchors, size);
		GUI.Box(newRect, GUIContent.none);
		return newRect;
	}

	public static Rect Manual_CreateBox(Rect rect)
	{
		GUI.Box(rect, GUIContent.none);
		return rect;
	}

	public static Rect Manual_CreateButton(Vector2 anchors, Vector2 size, string content, System.Action onButtonPress)
	{
		var box = Manual_CreateBox(anchors, size);

		GUIContent contentObj = new GUIContent();
		contentObj.text = content;

		if (GUI.Button(box, contentObj))
		{
			onButtonPress?.Invoke();
		}

		return box;
	}

	public static Rect Manual_CreateButton(Rect rect, string content, System.Action onButtonPress)
	{
		GUIContent contentObj = new GUIContent();
		contentObj.text = content;

		if (GUI.Button(rect, contentObj))
		{
			onButtonPress?.Invoke();
		}

		return rect;
	}

	public static int Manual_CreateIntField(Rect rect, string label, int value)
	{
		var rectLabel = new Rect(rect);
		rectLabel.x += 5;
		rectLabel.width *= 0.25f;

		var rectResult = new Rect(rect);
		rectResult.x += rectLabel.width;
		rectResult.width *= 0.75f;

		GUI.Label(rectLabel, label);

		var result = GUI.TextField(rectResult, value.ToString());
		result = Regex.Replace(result, "[^0-9.,]", "");

		if (result.Length == 0)
		{
			result += "0";
		}

		var parsedValue = float.Parse(result);
		return Mathf.RoundToInt(parsedValue);
	}

	public static void Manual_AddYOffset(OffsetType offset, int count = 1, int column = 0)
	{
		for (int i = 0; i < count; ++i)
		{
			Manual_IncreaseElementYOffset(offset, column);
		}
	}
	#endregion



	#region STATIC UTILITY
	private const float DESIGN_WIDTH = 1400;
	private const float DESIGN_HEIGHT = 786;

	public static Vector3 ScaledGUIScale()
	{
		var guiScale = Vector3.zero;
		guiScale.x = Screen.width / DESIGN_WIDTH;
		guiScale.y = Screen.height / DESIGN_HEIGHT;
		guiScale.z = 1;
		return guiScale;
	}

	public static Matrix4x4 GetScreenScaledMatrix()
	{
		var guiScale = ScaledGUIScale();

		return Matrix4x4.TRS(Vector3.zero, Quaternion.identity, guiScale);
	}
	#endregion
}
#endif

public class DrawGUIGroup
{
	private Rect _contentRect;
	private Vector2 _scrollViewPos;

	public DrawGUIGroup(Rect contentRect)
	{
		_contentRect = contentRect;
	}

	// TODO, make some scrollview implementation, this means we need to defer the rendering of all the elements, because they need to be rendered inside the scrollview section.
	internal void Render()
	{
		//IniControl.GlobalData.DebugWindow_ScrollPos = GUI.BeginScrollView(_groupSection, IniControl.GlobalData.DebugWindow_ScrollPos, scrollViewRect, alwaysShowHorizontal: true, alwaysShowVertical: false);
	}


	public void DrawLabel(string label = "emptyLabel")
	{
		var rect = GetNewRect();

		GUI.Label(rect, label);
	}

	public int DrawSlider(int value, int min, int max, string label = "emptyLabel")
	{
		var result = Mathf.Max(value, min);
		var rect = GetNewRect();

		bool hasLabel = !label.Equals("emptyLabel");
		var labelRect = rect;
		var sliderRect = rect;
		sliderRect.y += elementSize * 0.25f;

		if (hasLabel)
		{
			labelRect.width *= 0.3f;
			labelRect.width -= 5;
			sliderRect.width *= 0.7f;
			sliderRect.x += labelRect.width + 5;

			GUI.Label(labelRect, label);
		}

		result = (int)GUI.HorizontalSlider(sliderRect, result, min, max);

		return result;
	}

	public float DrawSlider(float value, float min, float max, string label = "emptyLabel")
	{
		var result = Mathf.Max(value, min);
		var rect = GetNewRect();

		bool hasLabel = !label.Equals("emptyLabel");
		var labelRect = rect;
		var sliderRect = rect;
		sliderRect.y += elementSize * 0.25f;

		if (hasLabel)
		{
			labelRect.width *= 0.3f;
			labelRect.width -= 5;
			sliderRect.width *= 0.7f;
			sliderRect.x += labelRect.width + 5;

			GUI.Label(labelRect, label);
		}

		result = GUI.HorizontalSlider(sliderRect, result, min, max);


		return result;
	}

	public int DrawIntField(int defaultValue, string label = "emptyLabel")
	{
		var result = defaultValue;
		var rect = GetNewRect();

		bool hasLabel = !label.Equals("emptyLabel");
		var labelRect = rect;
		var textFieldRect = rect;

		if (hasLabel)
		{
			labelRect.width *= 0.3f;
			labelRect.width -= 5;
			textFieldRect.width *= 0.7f;
			textFieldRect.x += labelRect.width + 5;

			GUI.Label(labelRect, label);
		}

		var textFieldValue = GUI.TextField(textFieldRect, result.ToString(), maxLength: 15);
		var canConvert = int.TryParse(textFieldValue, out result);

		if (canConvert)
		{
			return result;
		}

		return defaultValue;
	}

	public float DrawFloatField(float defaultValue, string label = "emptyLabel")
	{
		var result = defaultValue;
		var rect = GetNewRect();

		bool hasLabel = !label.Equals("emptyLabel");
		var labelRect = rect;
		var textFieldRect = rect;

		if (hasLabel)
		{
			labelRect.width *= 0.3f;
			labelRect.width -= 5;
			textFieldRect.width *= 0.7f;
			textFieldRect.x += labelRect.width + 5;

			GUI.Label(labelRect, label);
		}

		var textFieldValue = GUI.TextField(textFieldRect, result.ToString(), maxLength: 15);
		var canConvert = float.TryParse(textFieldValue, out result);

		if (canConvert)
		{
			return result;
		}

		return defaultValue;
	}

	public Vector2 DrawVectorField(Vector2 defaultValue, string label = "emptyLabel")
	{
		var result = defaultValue;
		var rect = GetNewRect();

		bool hasLabel = !label.Equals("emptyLabel");
		var labelRect = rect;
		var textFieldRect = rect;

		if (hasLabel)
		{
			labelRect.width *= 0.3f;
			labelRect.width -= 5;
			textFieldRect.width *= 0.7f;
			textFieldRect.x += labelRect.width + 5;

			GUI.Label(labelRect, label);
		}

		var floatOneRect = new Rect(textFieldRect);
		floatOneRect.width *= 0.49f;
		var floatTwoRect = new Rect(textFieldRect);
		floatTwoRect.width *= 0.49f;
		floatTwoRect.x = floatOneRect.x + floatOneRect.width + (textFieldRect.width * 0.02f);

		var floatFieldOne = GUI.TextField(floatOneRect, result.x.ToString(), maxLength: 15);
		var canConvertOne = float.TryParse(floatFieldOne, out result.x);

		var floatFieldTwo = GUI.TextField(floatTwoRect, result.y.ToString(), maxLength: 15);
		var canConvertTwo = float.TryParse(floatFieldTwo, out result.y);

		if (canConvertOne && canConvertTwo)
		{
			return result;
		}

		return default;
	}

	public bool DrawCheckBox(bool defaultValue, string label = "emptyLabel")
	{
		bool result = defaultValue;
		var rect = GetNewRect();

		result = GUI.Toggle(rect, result, label);

		return result;
	}

	public void DrawButton(string label = "emptyLabel", System.Action callback = null)
	{
		var rect = GetNewRect();

		if (GUI.Button(rect, label))
		{
			callback?.Invoke();
		}
	}

	public void DrawDisableableButton(string label = "emptyLabel", bool condition = false, System.Action callback = null)
	{
		if (condition)
		{
			DrawButton(label, callback);
		}
		else
		{
			DrawLabel(label);
		}
	}

	public void DrawSpace(float size)
	{
		GetNewRect(size: size);
	}

#if false  // Under construction, not usable yet.
	public int DrawDropdown(int currentIndex, string[] options)
	{
		var result = 0;

		var rect = GetNewRect();

		return result;
	}
#endif


	private const int elementSize = 20;
	private float totalElementOffset = 0;
	private Rect GetNewRect(bool autoIncrementElementOffset = true, float size = elementSize)
	{
		var rect = new Rect(_contentRect);
		rect.y += totalElementOffset + (size * 1.15f);
		rect.height = elementSize;

		if (autoIncrementElementOffset)
			totalElementOffset += size * 1.15f;

		return rect;
	}
}