using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    [SerializeField]
    Game game;

    [SerializeField]
    Canvas swipeMode;

    [SerializeField]
    Canvas chatMode;

    void Start()
    {
        if (!game.loaded)
        {
            game.LoadNewGame();
        }

        if (game.gameState == GameStates.Chat)
        {
            chatMode.gameObject.SetActive(true);
            swipeMode.gameObject.SetActive(false);
        } else if (game.gameState == GameStates.Swiping)
        {
            chatMode.gameObject.SetActive(false);
            swipeMode.gameObject.SetActive(true);
        }
    }

}
