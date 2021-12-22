using UnityEngine;

namespace Noise.PerlinNoise
{
	// TODO DK: Make some functionality to renormalize all colors back to 0-1 space after done. But make it optional.
	// TODO DK: Make some functionality to shift all values by a certain amount
	// TODO DK: Maybe make some functionality to multiply certain values against a certain amount. Like a gradiant vs gradiant multiplication.


	public enum ValueDeterminationType
	{
		None,
		Random,
		VerticalStripes,
		HashedVerticalStripes,
		HashedVerticalStripes_Smoothed,
		HashedBothDirections,
		HashedBothDirections_Smoothed,
		Perlin_1D,
		Perlin_2D,
		Perlin_3D,
	};

	public class PerlinNoiseGenerator
	{
		#region CONSTANT VARIABLES
		private const int gradientsMask1D = 1;
		private float[] gradients1D =
		{
			1f, -1f,
		};

		private static float sqr2 = Mathf.Sqrt(2f);
		private const int gradientsMask2D = 7;
		private Vector2[] gradients2D =
			{
			new Vector2( 1f, 0f),
			new Vector2(-1f, 0f),
			new Vector2( 0f, 1f),
			new Vector2( 0f,-1f),
			new Vector2( 1f, 1f).normalized,
			new Vector2(-1f, 1f).normalized,
			new Vector2( 1f,-1f).normalized,
			new Vector2(-1f,-1f).normalized
		};

		private const int gradientsMask3D = 15;
		private static Vector3[] gradients3D =
			{
			new Vector3( 1f, 1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3( 1f,-1f, 0f),
			new Vector3(-1f,-1f, 0f),
			new Vector3( 1f, 0f, 1f),
			new Vector3(-1f, 0f, 1f),
			new Vector3( 1f, 0f,-1f),
			new Vector3(-1f, 0f,-1f),
			new Vector3( 0f, 1f, 1f),
			new Vector3( 0f,-1f, 1f),
			new Vector3( 0f, 1f,-1f),
			new Vector3( 0f,-1f,-1f),

			new Vector3( 1f, 1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3( 0f,-1f, 1f),
			new Vector3( 0f,-1f,-1f)
		};
		#endregion




		public const ValueDeterminationType CURRENT_DETERM_TYPE = ValueDeterminationType.Random;
		public ValueDeterminationType CurrentDetermType = CURRENT_DETERM_TYPE;

		public const int GeneralResolution = 256;   // Note DK: a default resolution with enough definition to create satifying noise maps.
		public const int GeneralOctave = 1; // Note DK: Octaves are how many times we half the amplitude and double the frequency, its how we 'noisify' the result, so a simple result will become more detailed and grainy.
		public const float GeneralFrequency = 1.0f;
		public const float GeneralStrength = 1.0f;

		private float[,] _noiseMap;
		public float[,] NoiseMap { get { return _noiseMap; } }

		public CustomRandom Random { get; set; }

		/// <summary>
		/// Generates a perlin-based noise map, with an intended size of (resolution*resolution)
		/// </summary>
		public void GenerateNoiseMap(int resolution = GeneralResolution, int octaves = GeneralOctave, float frequency = GeneralFrequency, float strength = GeneralStrength)
		{
			TryInitHashIntegers(byPassCheck: true);

			_noiseMap = new float[resolution, resolution];

			// Note DK: The strength is inversed, as we are playing in the -1 to 1 space, doing for example 0.5^2 will result in a smaller value. 0.5^0.5 on the other hand increases the result.
			float calculationStrength = 1.0f / strength;

			for (int y = 0; y < resolution; ++y)
			{
				for (int x = 0; x < resolution; ++x)
				{
					_noiseMap[x, y] = SumNoise(x, y, resolution, octaves, frequency, calculationStrength);
				}
			}
		}

		private float SumNoise(int x, int y, float resolution, int octaves, float frequency, float strength)
		{
			float value = DeterminePointValue(x, y, resolution, frequency, strength);
			float amplitude = 1f;
			float totalSumSize = 1f;

			for (int i = 1; i < octaves; ++i)
			{
				frequency *= 2f;
				amplitude *= 0.5f;
				totalSumSize += amplitude;
				value += DeterminePointValue(x, y, resolution, frequency, strength) * amplitude;
			}


			float result = (value / totalSumSize);

			bool isPerlinNoise = ((int)CurrentDetermType) >= ((int)ValueDeterminationType.Perlin_1D);
			if (isPerlinNoise)
			{
				result = Mathf.Pow(Mathf.Abs(result), strength) * Mathf.Sign(result);

				float remapped = result * 0.5f + 0.5f;

				return remapped; // Note DK: Perlin noise exists in -1 to 1 space. Since our textures only care about 0-1 space, we remap the result.
			}

			return result;
		}

		private float DeterminePointValue(int x, int y, float resolution, float frequency, float strength)
		{
			var rand = Random;

			float rx = x / resolution;          // Note DK: This will always turn back the X/Y variable to a 0-1 spaec.
			float ry = y / resolution;          // Note DK: This will always turn back the X/Y variable to a 0-1 spaec.

			float freqX = rx * frequency;
			float freqY = ry * frequency;

			int calcX = UnityEngine.Mathf.FloorToInt(freqX);    // This will always return 0 -.-'
			int calcY = UnityEngine.Mathf.FloorToInt(freqY);    // This will always return 0 -.-'

			switch (CurrentDetermType)
			{
				case ValueDeterminationType.Random:
				{
					return rand.NextFloat();
				}

				case ValueDeterminationType.VerticalStripes:
				{
					return (Mathf.FloorToInt(x * frequency) / 10) % 2;    // So we either return 0 or 1, and we swap every 10 units. So first 10 are 0, second 10 are 1, etc.
				}

				case ValueDeterminationType.HashedVerticalStripes:
				{
					calcX = Mathf.FloorToInt(x * frequency);

					int hashedValue = GetHashedValue(calcX);
					return hashedValue * (1.0f / (_hashedIntegers.Length - 1));
				}

				case ValueDeterminationType.HashedVerticalStripes_Smoothed:
				{
					float progressFraction = (freqX % 1) * _hashedIntegers.Length;
					int hashIndex = UnityEngine.Mathf.FloorToInt(progressFraction);

					int i0 = hashIndex;
					int i1 = hashIndex + 1;

					float t = progressFraction - hashIndex;
					t = NoiseUtility.Smooth(t);

					int hashOne = GetHashedValue(i0);
					int hashTwo = GetHashedValue(i1);

					return Mathf.Lerp(hashOne, hashTwo, t) * (1f / (_hashedIntegers.Length - 1));
				}



				case ValueDeterminationType.HashedBothDirections:
				{
					float xProgressFraction = (freqX % 1) * _hashedIntegers.Length;
					int xHashIndex = UnityEngine.Mathf.FloorToInt(xProgressFraction);

					float yProgressFraction = (freqY % 1) * _hashedIntegers.Length;
					int yHashIndex = UnityEngine.Mathf.FloorToInt(yProgressFraction);


					int hashedValue = GetHashedValue(xHashIndex);
					hashedValue = GetHashedValue(hashedValue + yHashIndex);

					return hashedValue * (1.0f / (_hashedIntegers.Length - 1));
				}

				case ValueDeterminationType.HashedBothDirections_Smoothed:
				{
					float xProgressFraction = (freqX % 1);      // Note DK: This makes sure we know the 0-1 progress on the current frequency interval. Times by the hash indices, so we know the fracitonal index.
					int xHashIndex = UnityEngine.Mathf.FloorToInt(xProgressFraction);     // Note DK: In case we are in between two fractions, we floor to get the actual index.

					float yProgressFraction = (freqY % 1) * _hashedIntegers.Length;
					int yHashIndex = UnityEngine.Mathf.FloorToInt(yProgressFraction);

					int ix0 = xHashIndex;
					int ix1 = ix0 + 1;

					int iy0 = yHashIndex;
					int iy1 = iy0 + 1;


					int h0 = GetHashedValue(ix0);
					int h1 = GetHashedValue(ix1);
					int h00 = GetHashedValue(h0 + iy0);
					int h10 = GetHashedValue(h1 + iy0);
					int h01 = GetHashedValue(h0 + iy1);
					int h11 = GetHashedValue(h1 + iy1);


					float tx = xProgressFraction - xHashIndex;
					tx = NoiseUtility.Smooth(tx);

					float ty = yProgressFraction - yHashIndex;
					ty = NoiseUtility.Smooth(ty);



					return Mathf.Lerp(
							Mathf.Lerp(h00, h10, tx),
							Mathf.Lerp(h01, h11, tx),
							ty) * (1f / (_hashedIntegers.Length - 1));
				}



				case ValueDeterminationType.Perlin_1D:
				{
					int hashIndex = Mathf.FloorToInt(freqX);

					int i0 = hashIndex;

					float t0 = freqX - hashIndex;
					float t1 = t0 - 1f;

					i0 &= 255;
					int i1 = hashIndex + 1;

					float g0 = gradients1D[GetHashedValue(i0) & gradientsMask1D];
					float g1 = gradients1D[GetHashedValue(i1) & gradientsMask1D];

					float v0 = g0 * t0;
					float v1 = g1 * t1;


					float t = NoiseUtility.Smooth(t0);
					float result = (Mathf.Lerp(v0, v1, t) * 2f);

					result = Mathf.Pow(Mathf.Abs(result), strength) * Mathf.Sign(result);

					return result;
				}

				case ValueDeterminationType.Perlin_2D:
				{
					int ix0 = Mathf.FloorToInt(freqX);
					int iy0 = Mathf.FloorToInt(freqY);

					float tx0 = freqX - ix0;
					float ty0 = freqY - iy0;
					float tx1 = tx0 - 1f;
					float ty1 = ty0 - 1f;

					ix0 &= 255;
					iy0 &= 255;
					int ix1 = ix0 + 1;
					int iy1 = iy0 + 1;

					int h0 = GetHashedValue(ix0);
					int h1 = GetHashedValue(ix1);

					Vector2 g00 = gradients2D[GetHashedValue(h0 + iy0) & gradientsMask2D];
					Vector2 g10 = gradients2D[GetHashedValue(h1 + iy0) & gradientsMask2D];
					Vector2 g01 = gradients2D[GetHashedValue(h0 + iy1) & gradientsMask2D];
					Vector2 g11 = gradients2D[GetHashedValue(h1 + iy1) & gradientsMask2D];

					float v00 = Dot(g00, tx0, ty0);
					float v10 = Dot(g10, tx1, ty0);
					float v01 = Dot(g01, tx0, ty1);
					float v11 = Dot(g11, tx1, ty1);

					float tx = NoiseUtility.Smooth(tx0);
					float ty = NoiseUtility.Smooth(ty0);

					float result = Mathf.Lerp(
						Mathf.Lerp(v00, v10, tx),
						Mathf.Lerp(v01, v11, tx),
						ty) * sqr2;

					result = Mathf.Pow(Mathf.Abs(result), strength) * Mathf.Sign(result);

					return result;
				}

				case ValueDeterminationType.Perlin_3D:
				{
					int ix0 = Mathf.FloorToInt(freqX);
					int iy0 = Mathf.FloorToInt(freqY);
					int iz0 = Mathf.FloorToInt(0);  // Note DK: We work in 2 dimensions, so z is always 0

					float tx0 = freqX - ix0;
					float ty0 = freqY - iy0;
					float tz0 = 0 - iz0;
					float tx1 = tx0 - 1f;
					float ty1 = ty0 - 1f;
					float tz1 = tz0 - 1f;

					ix0 &= 255;
					iy0 &= 255;
					iz0 &= 255;
					int ix1 = ix0 + 1;
					int iy1 = iy0 + 1;
					int iz1 = iz0 + 1;

					int h0 = GetHashedValue(ix0);
					int h1 = GetHashedValue(ix1);
					int h00 = GetHashedValue(h0 + iy0);
					int h10 = GetHashedValue(h1 + iy0);
					int h01 = GetHashedValue(h0 + iy1);
					int h11 = GetHashedValue(h1 + iy1);

					Vector3 g000 = gradients3D[GetHashedValue(h00 + iz0) & gradientsMask3D];
					Vector3 g100 = gradients3D[GetHashedValue(h10 + iz0) & gradientsMask3D];
					Vector3 g010 = gradients3D[GetHashedValue(h01 + iz0) & gradientsMask3D];
					Vector3 g110 = gradients3D[GetHashedValue(h11 + iz0) & gradientsMask3D];
					Vector3 g001 = gradients3D[GetHashedValue(h00 + iz1) & gradientsMask3D];
					Vector3 g101 = gradients3D[GetHashedValue(h10 + iz1) & gradientsMask3D];
					Vector3 g011 = gradients3D[GetHashedValue(h01 + iz1) & gradientsMask3D];
					Vector3 g111 = gradients3D[GetHashedValue(h11 + iz1) & gradientsMask3D];

					float v000 = Dot(g000, tx0, ty0, tz0);
					float v100 = Dot(g100, tx1, ty0, tz0);
					float v010 = Dot(g010, tx0, ty1, tz0);
					float v110 = Dot(g110, tx1, ty1, tz0);
					float v001 = Dot(g001, tx0, ty0, tz1);
					float v101 = Dot(g101, tx1, ty0, tz1);
					float v011 = Dot(g011, tx0, ty1, tz1);
					float v111 = Dot(g111, tx1, ty1, tz1);

					float tx = NoiseUtility.Smooth(tx0);
					float ty = NoiseUtility.Smooth(ty0);
					float tz = NoiseUtility.Smooth(tz0);

					float result = Mathf.Lerp(
						Mathf.Lerp(Mathf.Lerp(v000, v100, tx), Mathf.Lerp(v010, v110, tx), ty),
						Mathf.Lerp(Mathf.Lerp(v001, v101, tx), Mathf.Lerp(v011, v111, tx), ty),
						tz);

					result = Mathf.Pow(Mathf.Abs(result), strength) * Mathf.Sign(result);

					return result;
				}
			}

			return 0f;
		}

		private int[] _hashedIntegers;
		private int GetHashedValue(int hashIndex)
		{
			TryInitHashIntegers();

			return _hashedIntegers[hashIndex % _hashedIntegers.Length];
		}

		private void TryInitHashIntegers(bool byPassCheck = false)
		{
			if (_hashedIntegers == null || byPassCheck)
			{
				_hashedIntegers = new int[256];
				for (int i = 0; i < _hashedIntegers.Length; ++i)
				{
					_hashedIntegers[i] = i;
				}

				_hashedIntegers.Shuffle(Random);
			}
		}


		private static float Dot(Vector2 g, float x, float y)
		{
			return g.x * x + g.y * y;
		}

		private static float Dot(Vector3 g, float x, float y, float z)
		{
			return g.x * x + g.y * y + g.z * z;
		}
	}
}
