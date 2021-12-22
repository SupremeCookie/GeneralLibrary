using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
	   AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ReadonlyAttribute : PropertyAttribute
{
	public ReadonlyAttribute() { }
}