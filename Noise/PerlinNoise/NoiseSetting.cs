using System.Collections.Generic;
using UnityEngine;

namespace Noise.PerlinNoise
{
	[System.Serializable]
	public class NoiseSetting
	{
		[SerializeField] [Range(0, 512)] private int _noiseMapResolution = 256;
		private void SetNoiseMapResolution(int resolution) { _noiseMapResolution = Mathf.Min(512, resolution); _noiseMapResolution = Mathf.Max(0, resolution); }    // Note DK: Limit range to 0-512
		public int NoiseMapResolution { get { return _noiseMapResolution; } set { SetNoiseMapResolution(value); } }

		[SerializeField] [Range(0, 64)] private float _frequency = 1.0f;
		private void SetFrequency(float frequency) { _frequency = Mathf.Min(64f, frequency); Mathf.Max(0.1f, frequency); }
		public float Frequency { get { return _frequency; } set { SetFrequency(value); } }

		// Note DK: This is how many times we 'repeat' the algorithm in order to noisify/grainify the result again. It's in a way, multisampling.
		[SerializeField] [Range(1, 8)] private int _octaves = 1;
		private void SetOctaves(int octave) { _octaves = Mathf.Min(8, octave); _octaves = Mathf.Max(0, octave); }
		public int Octaves { get { return _octaves; } set { SetOctaves(value); } }

		[SerializeField] [Range(0, 10)] private float _strength = 1.0f;
		private void SetStrength(float strength) { _strength = Mathf.Min(10f, strength); _strength = Mathf.Max(0, strength); }
		public float Strength { get { return _strength; } set { SetStrength(value); } }

		[SerializeField] private bool _useSeededRandom = false;
		public bool UseSeededRandom { get { return _useSeededRandom; } set { _useSeededRandom = value; } }     // Note DK: Turning this true will result in deterministic results
		public CustomRandom Random { get { return NoiseUtility.GetRandom(_useSeededRandom); } }

		public ValueDeterminationType DeterminationType = ValueDeterminationType.None;



		public static List<NoiseSetting> CopySettings(List<NoiseSetting> input)
		{
			var result = new List<NoiseSetting>(input.Count);

			for (int i = 0; i < input.Count; ++i)
			{
				result.Add(input[i]);//.Copy());
			}

			return result;
		}
	}
}
