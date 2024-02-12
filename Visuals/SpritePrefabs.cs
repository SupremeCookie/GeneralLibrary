using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SpritePrefabs : MonoBehaviour
{
	[SerializeField] private GameObject spritePrefab;

	[SerializeField] private List<byte> masks = new List<byte>();

	public GameObject GetSpritePrefab(byte mask)
	{
		if (!masks.Contains(mask)) { masks.Add(mask); }

		return spritePrefab;
	}
}