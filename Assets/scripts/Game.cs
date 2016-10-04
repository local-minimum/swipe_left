using UnityEngine;
using System.Collections.Generic;

public enum GameStates {Intro, Swiping, Chat };

[CreateAssetMenu(fileName = "Game", menuName = "Swipe Left/Game", order = 1)]
public class Game : ScriptableObject {

    public GameStates gameState = GameStates.Chat;

    public Player player;

    public List<NPC> remainingNPCs = new List<NPC>();

    public NPC PopRandomNPC()
    {
        if (remainingNPCs.Count == 0)
        {
            Debug.Log("Game Over?");
            return null;
        }

        NPC npc = remainingNPCs[Random.Range(0, remainingNPCs.Count)];
        remainingNPCs.Remove(npc);
        return npc;
    }    
}
