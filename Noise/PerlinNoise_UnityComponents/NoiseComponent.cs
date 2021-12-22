using UnityEngine;

namespace Noise.PerlinNoise
{
	[ExecuteInEditMode]
	public class NoiseComponent : MonoBehaviour
	{
		[Range(0, 512)] [Delayed] public int NoiseMapResolution = 256;
		[Range(0.1f, 64f)] public float Frequency = 1.0f;
		[Range(1, 8)] public int Octaves = 1;   // Note DK: This is how many times we 'repeat' the algorithm in order to noisify/grainify the result again. Its basically multisampling.
		[Range(0.01f, 10f)] public float Strength = 1;
		public bool UseSeededRandom;
		public ValueDeterminationType DeterminationType = ValueDeterminationType.None;

		public bool AllowEditing = true;
		public float QuadSize { get; set; } = 5.0f;


		private NoiseToTextureComponent _noiseToTexture;
		private TexturedQuadComponent _texturedQuad;

		private PerlinNoiseGenerator _perlinNoiseGen;

		private bool _hasInitialised;

		public float[,] ResultingNoiseMap { get { return _perlinNoiseGen.NoiseMap; } }


		public void OnEnable()
		{
			if (!_hasInitialised)
			{
				InitComponent();
			}
		}

		public void OnDisable()
		{
			_hasInitialised = false;
		}

		public void InitComponent()
		{
			_hasInitialised = true;

			if (DeterminationType == ValueDeterminationType.None)
			{
				DeterminationType = PerlinNoiseGenerator.CURRENT_DETERM_TYPE;
			}

			_noiseToTexture = gameObject.GetOrAddComponent<NoiseToTextureComponent>();
			_perlinNoiseGen = new PerlinNoiseGenerator();
			_texturedQuad = gameObject.GetOrAddComponent<TexturedQuadComponent>();


			_texturedQuad.Init(QuadSize);
			ChangeSeededRandomMode();

			InitTexture();
		}


		public void InitTexture(bool alsoRegenerate = false, bool fullyReinitialise = false)
		{
			_noiseToTexture.InitTexture(NoiseMapResolution, fullyReinitialise);

			if (alsoRegenerate)
			{
				GenerateNoiseMap();
			}
		}

		public void ApplyVariables()
		{
			_noiseToTexture.InitTexture(NoiseMapResolution);
			ChangeSeededRandomMode();
			ChangeDeterminationType();
		}

		public void ApplySettingsModel(NoiseSetting setting)
		{
			NoiseMapResolution = setting.NoiseMapResolution;
			Frequency = setting.Frequency;
			Octaves = setting.Octaves;
			Strength = setting.Strength;
			UseSeededRandom = setting.UseSeededRandom;
			DeterminationType = setting.DeterminationType;

			ApplyVariables();
		}

		public void ChangeSeededRandomMode()
		{
			var random = NoiseUtility.GetRandom(UseSeededRandom);
			_perlinNoiseGen.Random = random;
		}

		public void ChangeDeterminationType()
		{
			_perlinNoiseGen.CurrentDetermType = DeterminationType;
		}

		public void GenerateNoiseMap()
		{
			ChangeSeededRandomMode();

			_perlinNoiseGen.GenerateNoiseMap(NoiseMapResolution, Octaves, Frequency, Strength);
			_noiseToTexture.RegenerateTexture(_perlinNoiseGen.NoiseMap);
			_texturedQuad.Draw(_noiseToTexture.GetTexture());
		}

	}

#if UNITY_EDITOR
	[ExecuteInEditMode]
	[UnityEditor.CustomEditor(typeof(NoiseComponent))]
	public class NoiseComponentEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			var castedTarget = (target as NoiseComponent);

			bool allowEditing = castedTarget.AllowEditing;
			UnityEditor.EditorGUI.BeginDisabledGroup(!allowEditing);

			int currentResolution = castedTarget.NoiseMapResolution;
			int currentOctaves = castedTarget.Octaves;
			float currentFrequency = castedTarget.Frequency;
			float currentStrength = castedTarget.Strength;
			bool currentUseSeededRandom = castedTarget.UseSeededRandom;
			var currentDeterminationType = castedTarget.DeterminationType;

			UnityEditor.EditorGUI.BeginChangeCheck();

			if (GUILayout.Button("Re-initialize Texture"))
			{
				castedTarget.InitTexture(fullyReinitialise: true);
			}

			if (GUILayout.Button("Generate Noise Map"))
			{
				castedTarget.GenerateNoiseMap();
			}

			GUILayout.Space(10);

			base.OnInspectorGUI();

			if (UnityEditor.EditorGUI.EndChangeCheck())
			{
				if (currentResolution != castedTarget.NoiseMapResolution)
				{
					Debug.Log("Resolution changed, reinitialising texture");
					castedTarget.InitTexture(true);
				}

				if (currentFrequency != castedTarget.Frequency)
				{
					Debug.Log("Frequency changed");
					castedTarget.GenerateNoiseMap();
				}

				if (currentUseSeededRandom != castedTarget.UseSeededRandom)
				{
					Debug.Log("Change SeededRandom mode");
					castedTarget.ChangeSeededRandomMode();
				}

				if (currentDeterminationType != castedTarget.DeterminationType)
				{
					Debug.Log("Change Value Determination Type");
					castedTarget.ChangeDeterminationType();
					castedTarget.GenerateNoiseMap();
				}

				if (currentOctaves != castedTarget.Octaves)
				{
					Debug.Log("Change Octaves Value");
					castedTarget.GenerateNoiseMap();
				}

				if (currentStrength != castedTarget.Strength)
				{
					Debug.Log("Change Strength Value");
					castedTarget.GenerateNoiseMap();
				}
			}

			UnityEditor.EditorGUI.EndDisabledGroup();
		}
	}
#endif
}