using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue System/Dialogue Data")]
// public class Dialogue : ScriptableObject
// {
//     public List<DialogueNode> dialogueNodes = new List<DialogueNode>();
// }

// [System.Serializable]
// public class DialogueNode {
//     public string characterName;
//     [TextArea(3, 10)] public string sentence;
// }

[System.Serializable]
public class Dialogue {

	public string name;

	[TextArea(3, 10)]
	public string[] sentences;

}
