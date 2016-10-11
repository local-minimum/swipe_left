using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class DatesManager : MonoBehaviour {

    [SerializeField]
    GameManager gm;

    [SerializeField]
    Button browse;

    [SerializeField]
    Text noDate;

    void Update() {
        //TODO: Wasteful
        browse.interactable = gm.swipeBrowseManager.hasSet;
    }

    void Start()
    {
        SetStatus();
    }

    public void ClickBrowse()
    {
        gm.SetGameState(GameStates.Swiping);
    }

    void OnEnable() {
        gm.OnGameLoaded += SetStatus;
    }

    void OnDisable()
    {
        gm.OnGameLoaded -= SetStatus;
    }

    void SetStatus()
    {
        noDate.enabled = !gm.player.HasFriends;
    }
}
