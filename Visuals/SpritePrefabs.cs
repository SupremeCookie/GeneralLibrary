using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaskedSpritePrefab
{
	public byte mask;
	public GameObject spritePrefab;
}

public class SpritePrefabs : MonoBehaviour
{
	[SerializeField] private GameObject spritePrefab;
	[SerializeField] private List<MaskedSpritePrefab> prefabs;

	[SerializeField] private List<byte> masks = new List<byte>();
	[SerializeField] private List<byte> missingMasks = new List<byte>();

	public GameObject GetSpritePrefab(byte mask)
	{
		if (!masks.Contains(mask)) { masks.Add(mask); }

		for (int i = 0; i < prefabs.Count; ++i)
		{
			if (prefabs[i].mask == mask)
			{
				return prefabs[i].spritePrefab;
			}
		}

		if (!missingMasks.Contains(mask)) { missingMasks.Add(mask); }

		return spritePrefab;
	}
}