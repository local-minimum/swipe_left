using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [SerializeField]
    Game _game;

    [SerializeField]
    SwipeStage swipeMode;

    [SerializeField]
    Canvas chatMode;

    [SerializeField]
    int setSize = 4;

    [SerializeField]
    float timeBetweenSets = 4f * 60f;

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
        StartCoroutine(SetCreator());
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

            swipeMode.TestIfNext();

        } else if (_game.gameState == GameStates.Intro)
        {
            chatMode.gameObject.SetActive(false);
            swipeMode.gameObject.SetActive(false);
        }
    }

    IEnumerator<WaitForSeconds> SetCreator()
    {

        while (true)
        {
            int c = _game.remainingNPCs.Count;
            if (c == 0)
            {
                //TODO: Download more?
                break;
            }
            if (!swipeMode.hasSet)
            {
                if (c > 2 * setSize) {
                    swipeMode.remainingInSet = setSize;
                } else if (c > setSize)
                {
                    swipeMode.remainingInSet = Mathf.RoundToInt(c / 2);
                } else
                {
                    swipeMode.remainingInSet = c;
                }
                //TODO: Signal new set created
            }
            yield return new WaitForSeconds(Random.Range(timeBetweenSets, timeBetweenSets + 60f));
        }
    }

}
