using UnityEngine;
using System.Collections;

public delegate void ClickAction(UIButtonish btn);

public class UIButtonish : MonoBehaviour {
    enum ButtonStages {Disabled, Passive, Hover, Pressed};

    public event ClickAction OnClickAction;

    ButtonStages stage = ButtonStages.Passive;
    Animator anim;
    string EnterTrigger = "Enter";
    string ExitTrigger = "Exit";
    string PressTrigger = "Pressed";
    bool pointerIsOver = false;
    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    public void OnMouseOver()
    {
        if (stage == ButtonStages.Passive)
        {
            anim.SetTrigger(EnterTrigger);
            stage = ButtonStages.Hover;
        }
        pointerIsOver = true;
    }

    public void OnMouseExit()
    {
        if (stage == ButtonStages.Hover)
        {
            anim.SetTrigger(ExitTrigger);
            stage = ButtonStages.Passive;
        }
        pointerIsOver = false;
    }

    public void OnPress()
    {
        if (stage == ButtonStages.Hover)
        {
            stage = ButtonStages.Pressed;
            anim.SetTrigger(PressTrigger);
        }
    }

    public void OnRelease()
    {
        if (stage == ButtonStages.Pressed && pointerIsOver)
        {

            stage = ButtonStages.Passive;
            anim.SetTrigger(ExitTrigger);
            if (OnClickAction != null)
            {
                OnClickAction(this);
            }
        } else
        {
            anim.SetTrigger(ExitTrigger);
            stage = ButtonStages.Passive;
        }
    }

}
