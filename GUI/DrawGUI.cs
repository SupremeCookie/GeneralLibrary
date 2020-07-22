using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public enum OffsetType
{
	Left,
	Right,
};

public class DrawGUI : SingletonMonoBehaviour<DrawGUI>
{
	public event System.Action OnDrawGUI;

	private const int rectWidth = 200;
	private const int rectHeight = 25;

#if UNITY_EDITOR
	private bool _drawGui = true;
#else
	private bool _drawGui = false;
#endif

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

		OnDrawGUI?.Invoke();

		GUI.matrix = guiMatrix;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			_drawGui = !_drawGui;
		}
	}

	public static void IncreaseElementYOffset(OffsetType offset, int column)
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

	public static Rect CreateRect(bool isLeftSide = true, int columnOffset = 0, int rowOffset = 0, int yOffsetCount = 1, float xOffset = 0f, float heightOffsetScalar = 0f, float widthScalar = 1f, float minusWidth = 0f, bool offsetIsMinusWidth = false)
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
			AddYOffset(OffsetType.Left, yOffsetCount, columnOffset);
		else
			AddYOffset(OffsetType.Right, yOffsetCount, columnOffset);

		return rect;
	}

	public static Rect CreateFreeRect(Vector2 anchors, Vector2 size)
	{
		var guiScale = ScaledGUIScale();
		anchors = (anchors - (size * 0.5f)).DivideByVec2(guiScale);
		size = size.DivideByVec2(guiScale);

		Rect newRect = new Rect(anchors, size);
		return newRect;
	}

	public static Rect CreateBox(Vector2 anchors, Vector2 size)
	{
		Rect newRect = CreateFreeRect(anchors, size);
		GUI.Box(newRect, GUIContent.none);
		return newRect;
	}

	public static Rect CreateBox(Rect rect)
	{
		GUI.Box(rect, GUIContent.none);
		return rect;
	}

	public static Rect CreateButton(Vector2 anchors, Vector2 size, string content, System.Action onButtonPress)
	{
		var box = CreateBox(anchors, size);

		GUIContent contentObj = new GUIContent();
		contentObj.text = content;

		if (GUI.Button(box, contentObj))
		{
			onButtonPress?.Invoke();
		}

		return box;
	}

	public static Rect CreateButton(Rect rect, string content, System.Action onButtonPress)
	{
		GUIContent contentObj = new GUIContent();
		contentObj.text = content;

		if (GUI.Button(rect, contentObj))
		{
			onButtonPress?.Invoke();
		}

		return rect;
	}

	public static int CreatIntField(Rect rect, string label, int value)
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

	public static void AddYOffset(OffsetType offset, int count = 1, int column = 0)
	{
		for (int i = 0; i < count; ++i)
		{
			IncreaseElementYOffset(offset, column);
		}
	}

	#region STATIC UTILITY
	private const float DESIGN_WIDTH = 960;
	private const float DESIGN_HEIGHT = 540;

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
