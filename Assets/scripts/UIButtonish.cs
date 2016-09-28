using UnityEngine;
using System.Collections;

public class UIButtonish : MonoBehaviour {

    Animator anim;
    string EnterTrigger = "Enter";
    string ExitTrigger = "Exit";

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    public void OnMouseOver()
    {
        anim.SetTrigger(EnterTrigger);        
    }

    public void OnMouseExit()
    {
        anim.SetTrigger(ExitTrigger);
    }
}
