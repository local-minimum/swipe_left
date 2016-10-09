using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    [SerializeField]
    Game _game;

    [SerializeField]
    Canvas swipeMode;

    [SerializeField]
    Canvas chatMode;

    public Game game
    {
        get { return _game; }
    }

    void Start()
    {
        if (!_game.loaded)
        {
            _game.LoadNewGame();
        }
        SetGameState();
    }
    
    public void SetGameState(GameStates state) {
        _game.gameState = state;
        SetGameState();
    }

    public void SetGameState()
    {
        if (_game.gameState == GameStates.Chat)
        {
            chatMode.gameObject.SetActive(true);
            swipeMode.gameObject.SetActive(false);
        } else if (_game.gameState == GameStates.Swiping)
        {
            chatMode.gameObject.SetActive(false);
            swipeMode.gameObject.SetActive(true);

            swipeMode.GetComponent<SwipeStage>().TestIfNext();

        } else if (_game.gameState == GameStates.Intro)
        {
            chatMode.gameObject.SetActive(false);
            swipeMode.gameObject.SetActive(false);
        }
    }

}
