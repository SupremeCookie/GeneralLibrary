public class StaticSingleton<T> where T : class
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

	public static bool HasInstance { get { return _instance != null; } }
}