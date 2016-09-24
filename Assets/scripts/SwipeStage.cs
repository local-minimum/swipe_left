using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SwipeStage : MonoBehaviour {

    [SerializeField]
    Text CurrentUserName;

    [SerializeField]
    Image CurrentPhoto;

    [SerializeField]
    Sprite[] UserPhotos;

    [SerializeField]
    string[] UserNames;

    int showingIndex = -1;

    Animator anim;

    [SerializeField, Range(0, 1)]
    float swapDelay = 0.3f;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(DisplayNextInQueue());
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
        //TODO: Do something with the liking
        StartCoroutine(DisplayNextInQueue());
    }

    IEnumerator<WaitForSeconds> DisplayNextInQueue()
    {
        anim.SetTrigger("Intermission");
        showingIndex++;

        yield return new WaitForSeconds(swapDelay);
        //TODO: This should not be in the future
        showingIndex %= UserNames.Length;

        CurrentUserName.text = UserNames[showingIndex];
        CurrentPhoto.sprite = UserPhotos[showingIndex];
    }
}
