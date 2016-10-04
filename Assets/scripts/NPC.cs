using UnityEngine;
using System.Collections.Generic;

public struct ChatHistoryItem
{
    public string txt;
    public Actor actor;

    public ChatHistoryItem(Actor actor, string txt)
    {
        this.actor = actor;
        this.txt = txt;
    }
}


[CreateAssetMenu(fileName = "NPC", menuName = "Swipe Left/NPC", order = 1)]
public class NPC : ScriptableObject {

    public PsychologyProfile mind;

    [SerializeField, HideInInspector]
    List<ChatHistoryItem> history = new List<ChatHistoryItem>();

    [SerializeField]
    ChatItem startMessage;

    public ChatItem abandonMessage;
    
    public ChatItem Current;

    public Sprite FullImage;

    [SerializeField]
    Sprite avatar;

    public Sprite Avatar
    {
        get
        {
            if (avatar == null)
            {
                return FullImage;
            } else
            {
                return avatar;
            }
        }
    }

    public string UserName;

    public void AddToHistory(ChatItem item)
    {
        if (item != null)
        {
            history.Add(new ChatHistoryItem(item.actor, item.SelectedOption));
        }
    }

    public bool ChatHasEnded
    {
        get
        {
            return Current == abandonMessage || Current == null;
        }
    }

    public void InitiateChat()
    {
        Current = startMessage;
    }
}
