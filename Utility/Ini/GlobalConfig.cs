/// <summary>
/// This is the entry class for any global data that we might want to address.
/// It is not purposed to be editable data, it is only to be used as configurable data.
/// Editing the data is however allowed in debug or development context. But prohibited by runtime player influenced actions.
/// </summary>
public class GlobalConfig
{
	// TODO DK: We need some kind of mechanism in play where we can display a whole bunch of data in the editor
	// It has headers/categories/maybe tags, underneath data can be hosted
	// That way it is possible to say: search for the "Player" category, and see it and all the attributes related to it.

	// This data needs to be serialized somehow, likely in the resources or streamingassets folder
	// Preferably the data is editable outside the editor, how this will work together with the editor idk yet. Maybe a load from disk, or store to disk method? 
	// And before we exit the editor, if we detect any outstanding changes, we ask the dev what to do with these?

	// Importance is to ensuring this data is visible in editor, easy to use and configure, and we need an equivalent for unity object/scene data, prefabs, etc.
	// Research into this topic is wholly valid.

	public static GeneralScriptableData GeneralScriptableData { get { return GeneralScriptableData.Instance; } }
}