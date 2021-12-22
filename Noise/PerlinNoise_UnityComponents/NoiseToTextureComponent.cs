using UnityEngine;

namespace Noise.PerlinNoise
{
	[ExecuteInEditMode]
	public class NoiseToTextureComponent : MonoBehaviour
	{
		private Texture2D _texture;

		public void InitTexture(int resolution, bool fullyReinitialise = false)
		{
			if (_texture == null || fullyReinitialise)
			{
				_texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, mipChain: true);
				_texture.name = "PerlinNoise_Map";
				_texture.wrapMode = TextureWrapMode.Clamp;
				_texture.filterMode = FilterMode.Point;
			}
			else
			{
				_texture.Resize(resolution, resolution);
			}
		}

		public void RegenerateTexture(float[,] noiseMap)
		{
			// Note DK: Turn into the actual texture.
			int resolution = _texture.width;
			float stepSize = 1f / resolution;

			for (int y = 0; y < _texture.height; ++y)
			{
				for (int x = 0; x < _texture.width; ++x)
				{
					_texture.SetPixel(x, y, Color.white * noiseMap[x, y]);    // Nice simple transition -> new Color((x + 0.5f) * stepSize, (y + 0.5f) * stepSize, 0f));
				}
			}

			_texture.Apply();
		}

		public Texture2D GetTexture()
		{
			if (_texture == null)
			{
				Debug.LogError("No texture to return, will return null");
			}

			return _texture;
		}
	}
}
