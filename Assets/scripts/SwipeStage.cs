using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SwipeStage : MonoBehaviour {

    [SerializeField]
    Text CurrentUserName;

    [SerializeField]
    Image CurrentPhoto;

    [SerializeField]
    GameManager gameManager;

    //TODO: This needs to serialize always
    public int remainingInSet = 4;

    string triggerVoteLike = "VoteLove";
    string triggerVoteHate = "VoteHate";
    string triggerVoting = "Voting";
    string triggerAbortVoting = "AbortVote";

    Animator anim;

    [SerializeField, Range(0, 3)]
    float swapDelay = 0.3f;

    [SerializeField, Range(0, 1)]
    float swapDelay2 = 0.3f;

    public bool hasSet
    {
        get
        {
            return remainingInSet > 0;
        }
    }

    NPC npc;

    void Awake()
    {
        anim = GetComponent<Animator>();
        remainingInSet = Mathf.Clamp(PlayerPrefs.GetInt("SwipeStage.Set.Remaining", 0), 0, gameManager.SetSize);

    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("SwipeStage.Set.Remaining", remainingInSet);
    }

    
    void OnEnable()
    {
        GetComponentInChildren<swiper>().OnSwipeVote += SwipeStage_OnSwipeVote;        
    }

    void OnDisable()
    {
        GetComponentInChildren<swiper>().OnSwipeVote -= SwipeStage_OnSwipeVote;
    }

    private void SwipeStage_OnSwipeVote(bool liked)
    {
        anim.SetTrigger(liked ? triggerVoteLike : triggerVoteHate);
        gameManager.game.ClearSwipeNPC();
        TestIfNext();
    }

    public void TestIfNext()
    {

        if (remainingInSet > 0)
        {
            npc = gameManager.game.SwipeNPC;
            if (npc == null)
            {
                Debug.Log("Getting new NPC to vote on");
                npc = gameManager.game.PopRandomNPC();
                remainingInSet--;
            } else
            {
                Debug.Log("Had not voted for " + npc);
            }
            if (npc != null)
            {
                StartCoroutine(DisplayNextInQueue());
            } else
            {
                remainingInSet = 0;
                gameManager.ReturnToPreviousState();
            }
        } else
        {
            Debug.Log("End of set");
            gameManager.ReturnToPreviousState();
        }
    }

    IEnumerator<WaitForSeconds> DisplayNextInQueue()
    {

        
        anim.ResetTrigger(triggerVoting);
        yield return new WaitForSeconds(swapDelay);


        CurrentUserName.text = npc.UserName;
        CurrentPhoto.sprite = npc.FullImage;

        yield return new WaitForSeconds(swapDelay2);

        anim.ResetTrigger(triggerAbortVoting);
        anim.ResetTrigger(triggerVoteHate);
        anim.ResetTrigger(triggerVoteLike);
        anim.SetTrigger(triggerVoting);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            gameManager.ReturnToPreviousState();
        }
    }

    public NPC NonSwipedNPC
    {
        get
        {
            return npc;
        }
    }
}
