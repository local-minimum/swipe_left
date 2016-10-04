using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChatBot : MonoBehaviour {    

    [SerializeField]
    Animator chatAnimator;

    string triggerShowOptions = "Show";
    string triggerHideOptions = "Hide";

    [SerializeField]
    Player player;

    [SerializeField]
    NPC npc;

    [SerializeField]
    UITextFit themUIPrefab;

    [SerializeField]
    UITextFit weUIPrefab;

    [SerializeField]
    UITextFit statusPrefab;

    [SerializeField]
    RectTransform chatRect;

    [SerializeField]
    UITextFit weOptionPrefab;

    [SerializeField]
    RectTransform optionsRect;

    [SerializeField, Range(0, 10)]
    float minDelayNext = 1f;

    [SerializeField,Range(0, 10)]
    float maxDelayNext = 2f;

    [SerializeField, Range(0, 1)]
    float nextPollP = 0.1f;

    bool nextItem = true;

    [SerializeField]
    Text nameField;

    [SerializeField]
    Image avatarImage;

    void Start()
    {
        npc.InitiateChat();
        nameField.text = npc.UserName;
        avatarImage.sprite = npc.Avatar;
    }

    void Update()
    {
        if (nextItem && Random.value < nextPollP)
        {
            nextItem = false;
            if (npc.Current.actor == Actor.NPC)
            {
                StartCoroutine(theyChat());
            }
            else
            {
                showOptions();
            }
        }
    }

    IEnumerator<WaitForSeconds> theyChat()
    {
        yield return new WaitForSeconds(Random.Range(minDelayNext, maxDelayNext));
        float npcSocialValue = npc.mind.GetValue(npc.Current.social);
        string txt = npc.Current.GetOptionBasedOnSocialValue(npcSocialValue);
        if (npc.Current.social != SocialDimension.Neutral)
        {
            npc.mind.UpdateValue(npc.Current.social, npc.Current.SelectedValue);
        }
        theyChatItem(txt);
        yield return new WaitForSeconds(Random.Range(minDelayNext, maxDelayNext));


        npc.Current = npc.Current.NextChatItem();

        nextItem = !npc.ChatHasEnded;
        npc.AddToHistory(npc.Current);
        if (npc.ChatHasEnded)
        {
            yield return new WaitForSeconds(1f);
            theyLeft();        
        }
    }

    void theyLeft()
    {
        UITextFit utf = Instantiate(statusPrefab);
        utf.FormatText(npc.UserName);
        utf.transform.SetParent(chatRect);
    }

    IEnumerator<WaitForSeconds> delayTheyLeft()
    {
        yield return new WaitForSeconds(1f);
        theyLeft();
    }

    void theyChatItem(string txt)
    {
        UITextFit utf = Instantiate(themUIPrefab);
        utf.SetText(txt);
        utf.transform.SetParent(chatRect);
    }

    void weChat()
    {
        UITextFit utf = Instantiate(weUIPrefab);
        utf.transform.SetParent(chatRect);
        utf.SetText(npc.Current.SelectedOption);
        if (npc.Current.social != SocialDimension.Neutral)
        {
            player.mind.UpdateValue(npc.Current.social, npc.Current.SelectedValue);
            if (!npc.mind.UpdateInterestAndGetStayInChat(npc.Current.social, npc.Current.SelectedValue))
            {
                string txt = npc.abandonMessage.GetOptionBasedOnSocialValue(npc.mind.GetValue(npc.abandonMessage.social));
                StartCoroutine(delayAbandon(txt));
                npc.Current = npc.abandonMessage;
            }
        }
    }

    IEnumerator<WaitForSeconds> delayAbandon(string txt)
    {
        yield return new WaitForSeconds(1f);
        theyChatItem(txt);
        Debug.Log("NPC left conversation");
        yield return new WaitForSeconds(1f);
        theyLeft();
    }

    void showOptions()
    {
        //TODO: Test what options to show based on own psy profile

        for (int i=0; i<npc.Current.OptionList.Length; i++)
        {
            UITextFit utf;
            if (i < optionsRect.childCount)
            {
                GameObject child = optionsRect.GetChild(i).gameObject;
                if (!child.activeSelf)
                {
                    child.SetActive(true);
                    child.GetComponent<UIButtonish>().OnClickAction += ChatBot_OnClickAction;
                }
                utf = child.GetComponent<UITextFit>();
            }
            else {
                utf = Instantiate(weOptionPrefab);
                utf.name = "We Talk Option " + (optionsRect.childCount + 1);
                utf.transform.SetParent(optionsRect);
                utf.GetComponent<UIButtonish>().OnClickAction += ChatBot_OnClickAction;
            }
            utf.SetText(i, npc.Current.OptionList[i]);

        }

        for (int i=npc.Current.OptionList.Length; i < optionsRect.childCount; i++)
        {
            GameObject child = optionsRect.GetChild(i).gameObject;
            child.GetComponent<UIButtonish>().OnClickAction -= ChatBot_OnClickAction;
            child.SetActive(false);

        }
        chatAnimator.SetTrigger(triggerShowOptions);
    }

    private void ChatBot_OnClickAction(UIButtonish btn)
    {

        chatAnimator.SetTrigger(triggerHideOptions);
        npc.Current.SetIndex(btn.GetComponent<UITextFit>().Index);
        weChat();

        if (npc.mind)
        {
            npc.Current = npc.Current.NextChatItem();
            npc.AddToHistory(npc.Current);
        }

        if (!npc.ChatHasEnded)
        {
            StartCoroutine(DelayNext());
        } else
        {
            StartCoroutine(delayTheyLeft());
        }
    }

    IEnumerator<WaitForSeconds> DelayNext()
    {
        yield return new WaitForSeconds(Random.Range(minDelayNext, maxDelayNext));
        nextItem = true;
    }
}
