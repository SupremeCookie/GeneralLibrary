public class StaticSingleton<T> where T : class, new()
{
#pragma warning disable 0649
	protected static T pInstance;
#pragma warning restore 0649

	public static T Instance
	{
		get
		{
			return pInstance;
		}
	}

	public static bool HasInstance { get { return pInstance != null; } }

	public static System.Action OnInstanceInjected;
}