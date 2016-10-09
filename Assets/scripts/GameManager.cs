using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager instance
    {
        get 
        {
            return FindObjectOfType<GameManager>();
        }
    }

    public event System.Action OnNewNPCSet;

    [SerializeField]
    Game _game;

    [SerializeField]
    Player _player;

    [SerializeField]
    SwipeStage swipeMode;

    [SerializeField]
    Canvas chatMode;

    [SerializeField]
    DatesManager _datesManager;

    [SerializeField]
    int setSize = 4;

    public int SetSize
    {
        get { return setSize; }
    }
    [SerializeField]
    float timeBetweenSets = 4f * 60f;

    [SerializeField]
    float timeBeweensVar = 60f;

    public SwipeStage swipeBrowseManager
    {
        get
        {
            return swipeMode;
        }
    }

    public Game game
    {
        get { return _game; }
    }

    public Player player
    {
        get { return _player; }
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

    GameStates _previousState = GameStates.Dates;

    public void ReturnToPreviousState()
    {
        Debug.Log("Mode revert to " + _previousState);
        SetGameState(_previousState);
        _previousState = GameStates.Dates;   
    }

    public void SetGameState(GameStates state) {
        _previousState = _game.gameState;
        _game.gameState = state;
        SetGameState();
    }

    public void SetGameState()
    {
        if (_game.gameState == GameStates.Chat)
        {
            chatMode.gameObject.SetActive(true);
            swipeMode.gameObject.SetActive(false);
            _datesManager.gameObject.SetActive(false);
        } else if (_game.gameState == GameStates.Swiping)
        {
            chatMode.gameObject.SetActive(false);
            swipeMode.gameObject.SetActive(true);
            _datesManager.gameObject.SetActive(false);

            swipeMode.TestIfNext();

        } else if (_game.gameState == GameStates.Intro)
        {
            chatMode.gameObject.SetActive(false);
            swipeMode.gameObject.SetActive(false);
            _datesManager.gameObject.SetActive(false);
        } else if (_game.gameState == GameStates.Dates)
        {
            chatMode.gameObject.SetActive(false);
            swipeMode.gameObject.SetActive(false);
            _datesManager.gameObject.SetActive(true);
        }
    }

    IEnumerator<WaitForSeconds> SetCreator()
    {

        while (true)
        {
            int c = _game.remainingNPCs.Count;
            if (c == 0)
            {
                Debug.Log("No more NPCs to make set from");
                break;
            }
            if (!swipeMode.hasSet)
            {
                Debug.Log("New set");
                if (c > 2 * setSize) {
                    swipeMode.remainingInSet = setSize;
                } else if (c > setSize)
                {
                    swipeMode.remainingInSet = Mathf.RoundToInt(c / 2);
                } else
                {
                    swipeMode.remainingInSet = c;
                    Debug.Log("Last NPC set");
                }
                if (OnNewNPCSet != null)
                {
                    OnNewNPCSet();
                }
                
            } else
            {
                Debug.Log("Player has yet to look at current set");
            }
            yield return new WaitForSeconds(Random.Range(timeBetweenSets, timeBetweenSets + timeBeweensVar));
        }
    }
    
}
