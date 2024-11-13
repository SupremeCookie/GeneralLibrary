using UnityEngine;
using UnityEngine.UIElements;

public class EditorComment : MonoBehaviour
{
#if UNITY_EDITOR
	[SerializeField][TextArea(minLines: 7, maxLines: 15)] private string comment;
#endif
}
