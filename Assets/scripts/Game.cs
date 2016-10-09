using UnityEngine;
using System.Collections.Generic;

public enum GameStates {Intro, Swiping, Chat, Dates};

[CreateAssetMenu(fileName = "Game", menuName = "Swipe Left/Game", order = 1)]
public class Game : ScriptableObject {

    public GameStates gameState = GameStates.Chat;

    public Player player;

    [HideInInspector]
    public List<NPC> remainingNPCs = new List<NPC>();

    [SerializeField]
    bool _loaded = false;

    [SerializeField]
    NPC _swipeNPC = null;

    public NPC SwipeNPC
    {
        get
        {
            return _swipeNPC;
        }
    }

    public void ClearSwipeNPC()
    {
        _swipeNPC = null;
    }

    public NPC PopRandomNPC()
    {
        if (remainingNPCs.Count == 0)
        {
            Debug.Log("Out of NPCs");
            _swipeNPC = null;
            return null;
        }

        NPC npc = remainingNPCs[Random.Range(0, remainingNPCs.Count)];
        remainingNPCs.Remove(npc);
        _swipeNPC = npc;
        return npc;
    }

    public bool loaded
    {
        get
        {
            return _loaded;
        }
    }

    public void LoadNewGame()
    {
        Debug.Log("Setting up initial game");
        remainingNPCs.Clear();
        remainingNPCs.AddRange(Resources.LoadAll<NPC>("characters"));
        _swipeNPC = null;
        foreach (NPC npc in remainingNPCs)
        {
            npc.ResetChat();
        }

        gameState = GameStates.Intro;
        _loaded = true;
        Debug.Log(string.Format("{0} NPCs loaded", remainingNPCs.Count));
    }    
}
