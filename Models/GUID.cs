public class GUID
{
	private System.Guid _guid;
	public System.Guid Guid
	{
		get
		{
			if (_guid.Equals(default(System.Guid)))
			{
				_guid = System.Guid.NewGuid();
			}

			return _guid;
		}
	}

	public GUID()
	{
		_guid = System.Guid.NewGuid();
	}

	public GUID(System.Guid guid)
	{
		_guid = guid;
	}

	public override string ToString()
	{
		return _guid.ToString().Substring(0, 8);
	}
}
