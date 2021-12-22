public class Singleton<T> where T : class, new()
{
	private static T _instance;
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new T();
			}

			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	public static bool HasInstance { get { return _instance != null; } }
}
