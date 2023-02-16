public class ThreadChecker
{
	private int mainThreadID;

	public void SetMainThreadID()
	{
		mainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
	}

	public bool IsCurrentThreadMainThread()
	{
		return mainThreadID == System.Threading.Thread.CurrentThread.ManagedThreadId;
	}
}