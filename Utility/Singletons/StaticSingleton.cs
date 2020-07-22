public class StaticSingleton<T> where T : class
{
	private static T _instance;
	public static T Instance
	{
		get
		{
			return _instance;
		}
	}
}