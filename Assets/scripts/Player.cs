using UnityEngine;
using System.Collections.Generic;

public class Player : ScriptableObject {

    [SerializeField]
    List<NPC> friends = new List<NPC>();

    public NPC[] Friends
    {
        get
        {
            return friends.ToArray();
        }
    }

    public bool AddFriend(NPC friend)
    {
        if (friends.Contains(friend))
        {
            return false;
        } else
        {
            friends.Add(friend);
            friend.InitiateChat();
            return true;
        }

    }
    public PsychologyProfile mind;

}
