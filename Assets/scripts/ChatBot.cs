using UnityEngine;
using System.Collections.Generic;

public class ChatBot : MonoBehaviour {

    List<ChatItem> history = new List<ChatItem>();

    [SerializeField]
    ChatItem Current;

    [SerializeField]
    PsychologyProfile playerProfile;

    [SerializeField]
    PsychologyProfile npcProfile;
}
