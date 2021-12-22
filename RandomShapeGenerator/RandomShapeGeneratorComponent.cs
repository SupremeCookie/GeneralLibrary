using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomShapeGenerator
{
	public class RandomShapeGeneratorComponent : MonoBehaviour
	{
		[System.Serializable]
		public enum JitterModeType
		{
			None,
			FlatFloat,
			BetweenTwoFloats,
		};

		[SerializeField] [HideInInspector] private JitterModeType _jitterMode;
		public JitterModeType JitterMode
		{
			get { return _jitterMode; }
		}

		[SerializeField] [HideInInspector] private JitterModeType _jitterModeCenterPos;
		public JitterModeType JitterModeCenterPos
		{
			get { return _jitterModeCenterPos; }
		}

		[SerializeField] [HideInInspector] private Vector2 _jitterRange;
		private Vector2 _positionJitterFromCenter;


		[SerializeField] [HideInInspector] private int _randomShapeCount;
		public int RandomShapeCount
		{
			get { return _randomShapeCount; }
			set
			{
				bool DidRandomShapeChange = _randomShapeCount != value;
				_randomShapeCount = value;

				if (DidRandomShapeChange)
				{
					OnRandomShapeCountGotChanged();
				}
			}
		}

#pragma warning disable 0649
		[SerializeField] private int _pointsForRandomShapeCount;
#pragma warning restore 0649


		[SerializeField] [HideInInspector] private List<NamedBezierSpline> _splines;
		[SerializeField] [HideInInspector] private List<SplineDisplayerComponent> _splineDisplayers;

		public List<NamedBezierSpline> Splines { get { return _splines; } }
		public List<SplineDisplayerComponent> SplineDisplayers { get { return _splineDisplayers; } }


		[SerializeField] [HideInInspector] private List<RandomShapeComponent> _randomShapes = new List<RandomShapeComponent>();



		public void InjectData(List<NamedBezierSpline> splines)
		{
			_splines = splines;
			CreateSplineDisplayers();

			SetSplineDisplayerData();

			InitSplines();
		}

		public void ClearAllDisplayers()
		{
			TryCleanNullComponents();
			RemoveSplineDisplayers(_splineDisplayers.Count);
			RemoveLeftOverChildren();
		}



		private void CreateSplineDisplayers()
		{
			if (_splineDisplayers != null)
			{
				TryCleanNullComponents();
				Debug.Assert(!_splineDisplayers.Any(s => s == null), "Some of the spline displayers are null, please fix.");

				var currentCount = _splineDisplayers.Count;
				var splineCount = _splines.Count;

				var delta = splineCount - currentCount;
				if (delta > 0)
				{
					AddSplineDisplayers(delta);
				}
				if (delta < 0)
				{
					RemoveSplineDisplayers(UnityEngine.Mathf.Abs(delta));
				}
			}
			else
			{
				_splineDisplayers = new List<SplineDisplayerComponent>(_splines.Count);
				AddSplineDisplayers(_splines.Count);
			}
		}

		private void AddSplineDisplayers(int count)
		{
			float yOffset = -5.0f;
			for (int i = 0; i < count; ++i)
			{
				var newSplineDisplayerGO = new GameObject("SplineDisplayer");
				newSplineDisplayerGO.transform.SetParent(transform);
				newSplineDisplayerGO.transform.localPosition = new Vector3(0, yOffset * i, 0.0f);

				var splineDisplayer = newSplineDisplayerGO.AddComponent<SplineDisplayerComponent>();
				_splineDisplayers.Add(splineDisplayer);
			}
		}

		private void RemoveSplineDisplayers(int count)
		{
			var countCopy = count;
			for (int i = _splineDisplayers.Count - 1; i >= 0; --i)
			{
				if (countCopy <= 0)
				{
					continue;
				}


				if (Application.isPlaying)
				{
					Destroy(_splineDisplayers[i].gameObject);
				}
				else
				{
					DestroyImmediate(_splineDisplayers[i].gameObject);
				}


				--countCopy;
			}
		}

		private void SetSplineDisplayerData()
		{
			Debug.Assert(!_splines.IsNullOrEmpty(), "No splines to display");
			Debug.Assert(!_splineDisplayers.IsNullOrEmpty(), "No splineDisplayers to display splines on");

			for (int i = 0; i < _splines.Count; ++i)
			{
				var splineDisp = _splineDisplayers[i];
				splineDisp.InjectData(_splines[i]);
			}
		}

		private void InitSplines()
		{
			for (int i = 0; i < _splineDisplayers.Count; ++i)
			{
				_splineDisplayers[i].Spline.TryInitPointObjects();
			}
		}

		private void TryCleanNullComponents()
		{
			for (int i = _splineDisplayers.Count - 1; i >= 0; --i)
			{
				if (_splineDisplayers[i] == null)
				{
					_splineDisplayers.RemoveAt(i);
				}
			}
		}

		private void RemoveLeftOverChildren()
		{
			var splineDisplayers = GetSplineDisplayers();
			if (!splineDisplayers.IsNullOrEmpty())
			{
				for (int i = splineDisplayers.Count - 1; i >= 0; --i)
				{
					if (Application.isPlaying)
					{
						Destroy(splineDisplayers[i].gameObject);
					}
					else
					{
						DestroyImmediate(splineDisplayers[i].gameObject);
					}
				}
			}
		}

		private List<SplineDisplayerComponent> GetSplineDisplayers()
		{
			return GetComponentsInChildren<SplineDisplayerComponent>().ToList();
		}



		private void OnRandomShapeCountGotChanged()
		{
			// Check if we are more or less
			// If less, remove leftovers
			// If more, add more

			if (_splineDisplayers.IsNullOrEmpty())
			{
				_splineDisplayers = GetSplineDisplayers();
			}

			TryCleanNullRandomShapes();

			Debug.Assert(!_splineDisplayers.IsNullOrEmpty(), "No spline displayers to make random shapes for, maybe make those first?, or figure out why they aren't registered!");

			// This delta logic is ffed, go clean it up, the _randomShapeCount is not correct.

			int structuresCount = _splineDisplayers.Count;
			int desiredCount = structuresCount * _randomShapeCount;

			//Debug.Log(structuresCount + " :  " + desiredCount + " :  " + _randomShapeCount);

			int currentcount = _randomShapes.Count;
			int delta = desiredCount - currentcount;

			//Debug.Log("Delta!: " + delta);

			if (delta < 0)
			{
				int removeCount = Mathf.Abs(delta);
				RemoveRandomShapes(removeCount);
			}
			else if (delta > 0)
			{
				AddRandomShapes(delta);
			}


			GenerateNewRandomShapes();
		}

		public void GenerateNewRandomShapes()
		{
			Debug.Assert(!_splines.IsNullOrEmpty(), "Splines are null or empty, try pushing the spline data to the RandomShapeGenerator");

			int structuresCount = _splineDisplayers.Count;
			int desiredCount = structuresCount * _randomShapeCount;

			List<RandomShape> newRandomShapes = new List<RandomShape>(desiredCount);

			Vector2 jitterRange = Vector2.zero;
			switch (_jitterMode)
			{
				case JitterModeType.FlatFloat:
				{
					jitterRange = Vector2.one * _jitterRange.y;
					break;
				}

				case JitterModeType.BetweenTwoFloats:
				{
					jitterRange = _jitterRange;
					break;
				}
			}

			for (int i = 0; i < desiredCount; ++i)
			{
				int splineIndex = i % structuresCount;
				var shapeStruct = new ShapeStructure() { ShapeSpline = _splines[splineIndex].Spline, };

				Debug.Assert(_pointsForRandomShapeCount > 2, "Points For Random Shape Count needs to be above 2.");
				var randomShape = RandomShapeGenerator.Generate(shapeStruct, _pointsForRandomShapeCount, jitterRange, _positionJitterFromCenter);

				_randomShapes[i].Injectdata(randomShape);
			}
		}

		public void RemoveAllRandomShapeComponents()
		{
			TryCleanNullRandomShapes();
			RemoveRandomShapeComponents();
			RemoveLeftOverRandomShapes();

			_randomShapeCount = 0;
		}

		private void TryCleanNullRandomShapes()
		{
			for (int i = _randomShapes.Count - 1; i >= 0; --i)
			{
				if (_randomShapes[i] == null)
				{
					_randomShapes.RemoveAt(i);
				}
			}
		}

		public void RegenerateAllRandomShapeComponents()
		{
			OnRandomShapeCountGotChanged();
		}


		private void RemoveLeftOverRandomShapes()
		{
			var randomShapeComps = GetComponentsInChildren<RandomShapeComponent>();
			if (randomShapeComps.Length > 0)
			{
				for (int i = randomShapeComps.Length - 1; i >= 0; --i)
				{
					if (Application.isPlaying)
					{
						Destroy(randomShapeComps[i].gameObject);
					}
					{
						DestroyImmediate(randomShapeComps[i].gameObject);
					}
				}
			}
		}


		private void AddRandomShapes(int addCount)
		{
			Debug.Log($"Add {addCount} amount of shapes");

			bool addCountIsCorrect = addCount % _splineDisplayers.Count == 0;
			Debug.Assert(addCountIsCorrect, "We can't add an odd amount of room shapes, each structure needs an equal amount of roomshapes");

			if (!addCountIsCorrect)
			{
				return;
			}


			float baseXPos = 10f;


			for (int i = 0; i < addCount; ++i)
			{
				float xOffset = baseXPos * Mathf.FloorToInt((float)_randomShapes.Count / _splineDisplayers.Count);
				float yOffset = (i % _splineDisplayers.Count) * -5f; // Whilst technically incorrect, this does get us the right number.

				var newRandomShapeGO = new GameObject("RandomShape");
				newRandomShapeGO.transform.SetParent(transform, false);
				newRandomShapeGO.transform.localPosition = new Vector3(baseXPos + xOffset, yOffset, 0);

				var randomShapeComp = newRandomShapeGO.AddComponent<RandomShapeComponent>();
				_randomShapes.Add(randomShapeComp);
			}
		}

		private void RemoveRandomShapeComponents()
		{
			if (_randomShapes.Count > 0)
			{
				RemoveRandomShapes(_randomShapes.Count);
			}
		}

		private void RemoveRandomShapes(int removeCount)
		{
			Debug.Log($"Remove {removeCount} amount of shapes");

			int copyRemoveCount = removeCount;
			for (int i = _randomShapes.Count - 1; i >= 0; --i)
			{
				if (copyRemoveCount <= 0)
				{
					break;
				}

				if (Application.isPlaying)
				{
					Destroy(_randomShapes[i].gameObject);
				}
				else
				{
					DestroyImmediate(_randomShapes[i].gameObject);
				}

				copyRemoveCount--;
			}
		}

#if UNITY_EDITOR
		[UnityEditor.CustomEditor(typeof(RandomShapeGeneratorComponent))]
		public class RandomShapeGeneratorComponentEditor : UnityEditor.Editor
		{
			public override void OnInspectorGUI()
			{
				var casted = target as RandomShapeGeneratorComponent;

				int randomShapeCount = UnityEditor.EditorGUILayout.IntSlider(casted.RandomShapeCount, 0, 5);
				casted.RandomShapeCount = randomShapeCount;

				DrawJitterMode(casted);

				DrawJitterCenterPosMode(casted);

				GUILayout.Space(10);



				if (GUILayout.Button("Generate new random shapes"))
				{
					casted.GenerateNewRandomShapes();
					UnityEditor.SceneView.RepaintAll();
				}

				if (GUILayout.Button("Remove Random Shapes Components"))
				{
					casted.RemoveAllRandomShapeComponents();
					UnityEditor.SceneView.RepaintAll();
				}

				if (GUILayout.Button("Re-Generate Random Shape Components"))
				{
					casted.RegenerateAllRandomShapeComponents();
					UnityEditor.SceneView.RepaintAll();
				}


#if RogueLike
				GUILayout.Space(10);
				if (GUILayout.Button("Load Settings From Level Generation"))
				{
					var levelData = IniControl.LevelGenData;
					casted._pointsForRandomShapeCount = levelData.LevelGen_PointsForRandomShapes;
					casted._jitterMode = levelData.LevelGen_JitterMode;
					casted._jitterRange = levelData.LevelGen_JitterRange;
					casted._jitterModeCenterPos = levelData.LevelGen_JitterModeCenterPos;
					casted._positionJitterFromCenter = levelData.LevelGen_JitterCenterPosRange;
				}

				if (GUILayout.Button("Save Settings For Level Generation"))
				{
					var levelData = IniControl.LevelGenData;
					levelData.LevelGen_PointsForRandomShapes = casted._pointsForRandomShapeCount;
					levelData.LevelGen_JitterMode = casted._jitterMode;
					levelData.LevelGen_JitterRange = casted._jitterRange;
					levelData.LevelGen_JitterModeCenterPos = casted._jitterModeCenterPos;
					levelData.LevelGen_JitterCenterPosRange = casted._positionJitterFromCenter;

					UnityEditor.EditorUtility.SetDirty(levelData);
					UnityEditor.AssetDatabase.SaveAssets();
				}
#endif


				base.OnInspectorGUI();
			}

			private void DrawJitterMode(RandomShapeGeneratorComponent casted)
			{
				var jitterMode = UnityEditor.EditorGUILayout.EnumPopup(casted.JitterMode);
				casted._jitterMode = (JitterModeType)jitterMode;


				switch (casted.JitterMode)
				{
					case JitterModeType.None:
					{
						GUILayout.Space(10);
						casted._jitterRange = Vector2.zero;
						break;
					}

					case JitterModeType.FlatFloat:
					{
						UnityEditor.EditorGUILayout.BeginHorizontal();

						UnityEditor.EditorGUILayout.LabelField("Jitter Value");
						var maxFloat = UnityEditor.EditorGUILayout.FloatField(casted._jitterRange.y);
						maxFloat = Mathf.Min(maxFloat, 1.0f);
						casted._jitterRange.x = maxFloat;
						casted._jitterRange.y = maxFloat;

						UnityEditor.EditorGUILayout.EndHorizontal();

						GUILayout.Space(10);
						break;
					}

					case JitterModeType.BetweenTwoFloats:
					{
						var range = UnityEditor.EditorGUILayout.Vector2Field("Jitter Range", casted._jitterRange);

						range.x = Mathf.Max(range.x, 0.0f);
						range.y = Mathf.Min(range.y, 1.0f);

						if (range.x > range.y) { float currentX = range.x; range.x = range.y; range.y = currentX; }

						casted._jitterRange = range;

						GUILayout.Space(10);
						break;
					}
				}
			}

			private void DrawJitterCenterPosMode(RandomShapeGeneratorComponent casted)
			{
				var jitterMode = UnityEditor.EditorGUILayout.EnumPopup(casted.JitterModeCenterPos);
				casted._jitterModeCenterPos = (JitterModeType)jitterMode;


				switch (casted.JitterModeCenterPos)
				{
					case JitterModeType.None:
					{
						GUILayout.Space(10);
						casted._positionJitterFromCenter = Vector2.zero;
						break;
					}

					case JitterModeType.FlatFloat:
					{
						UnityEditor.EditorGUILayout.BeginHorizontal();

						UnityEditor.EditorGUILayout.LabelField("Jitter Center Pos Value");
						var maxFloat = UnityEditor.EditorGUILayout.FloatField(casted._positionJitterFromCenter.y);
						maxFloat = Mathf.Min(maxFloat, 0.9f);
						maxFloat = Mathf.Max(maxFloat, -0.9f);
						casted._positionJitterFromCenter.x = maxFloat;
						casted._positionJitterFromCenter.y = maxFloat;

						UnityEditor.EditorGUILayout.EndHorizontal();

						GUILayout.Space(10);
						break;
					}

					case JitterModeType.BetweenTwoFloats:
					{
						var range = UnityEditor.EditorGUILayout.Vector2Field("Jitter Center Pos Range", casted._positionJitterFromCenter);

						range.x = Mathf.Max(range.x, -0.9f);
						range.y = Mathf.Min(range.y, 0.9f);

						if (range.x > range.y) { float currentX = range.x; range.x = range.y; range.y = currentX; }

						casted._positionJitterFromCenter = range;

						GUILayout.Space(10);
						break;
					}
				}
			}
		}
#endif
	}
}