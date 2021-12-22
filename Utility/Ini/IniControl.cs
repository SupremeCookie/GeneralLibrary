#if GLOBALDATASCRIPTABLE_AVAILABLE
using RogueLike;
using RogueLike.LevelGenerator;
#endif

public class IniControl
{
#if GLOBALDATASCRIPTABLE_AVAILABLE
	public static GlobalDataScriptable GlobalData { get { return GlobalDataScriptable.Instance; } }

	public static LevelGeneratorDataScriptable LevelGenData { get { return LevelGeneratorDataScriptable.Instance; } }
#endif
}