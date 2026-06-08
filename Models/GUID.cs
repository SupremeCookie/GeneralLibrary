using System;
using UnityEngine;

[System.Serializable]
public class GUID : IEquatable<GUID>
{
	[SerializeField, Readonly] private string _guid;

	public System.Guid Guid
	{
		get
		{
			//if (_guid.Equals(default(System.Guid)))
			if (string.IsNullOrEmpty(_guid))
			{
				_guid = System.Guid.NewGuid().ToString();
			}

			return new System.Guid(_guid);
		}
	}

	public GUID()
	{
		_guid = System.Guid.NewGuid().ToString();
	}

	public GUID(string guid)
	{
		_guid = guid;
	}

	public GUID(System.Guid guid)
	{
		_guid = guid.ToString();
	}

	public override string ToString()
	{
		return _guid;
	}

	public string ToShortString()
	{
		return _guid.Substring(0, 8);
	}


	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj))
		{
			//Debug.Log("[EQUALSING GUID:  target is null]");
			return false;
		}

		if (ReferenceEquals(this, obj))
		{
			//Debug.Log("[EQUALSING GUID:  target is same as obj]");
			return true;
		}

		if (obj.GetType() != this.GetType())
		{
			//Debug.Log("[EQUALSING GUID:  types are not the same]");
			return false;
		}

		return this.Equals(obj);
	}

	public bool Equals(GUID other)
	{
		if (ReferenceEquals(null, other))
		{
			//Debug.Log("[EQUALSING GUID:  target is null]");
			return false;
		}

		if (ReferenceEquals(this, other))
		{
			//Debug.Log("[EQUALSING GUID:  target is same as obj]");
			return true;
		}

		//Debug.Log($"[EQUALSING GUID:  checking strings]:   {other.ToString()},  {_guid},   {string.Equals(other.ToString(), _guid)}");
		return string.Equals(other.ToString(), _guid);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(_guid);
	}
}
