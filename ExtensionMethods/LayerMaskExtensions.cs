using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class LayerMaskExtensions
{
	public static bool Contains(this LayerMask layers, int layer)
	{
		if ((layers & 1 << layer) == 1 << layer)
		{
			return true;
		}

		return false;
	}
}