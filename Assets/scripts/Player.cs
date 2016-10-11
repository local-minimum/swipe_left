using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Player", menuName = "Swipe Left/Player", order = 1)]
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
            friend.ResetChat();
            return true;
        }

    }

    public bool HasFriends
    {
        get
        {
            return friends.Count > 0;
        }
    }
    public PsychologyProfile mind;

}
