public class StaticSingleton<T> where T : class, new()
{
#pragma warning disable 0649
	private static T _instance;
#pragma warning restore 0649

	public static T Instance
	{
		get
		{
			return _instance;
		}
	}

	public bool HasInstance { get { return _instance != null; } }

	public StaticSingleton()
	{
		if (!HasInstance)
		{
			_instance = new T();
		}
	}
}