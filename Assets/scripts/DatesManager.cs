using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class DatesManager : MonoBehaviour {

    [SerializeField]
    GameManager gm;

    [SerializeField]
    Button browse;
	
	void Update () {
        //TODO: Wasteful
        browse.interactable = gm.swipeBrowseManager.hasSet;	
	}

    public void ClickBrowse()
    {
        gm.SetGameState(GameStates.Swiping);
    }
}
