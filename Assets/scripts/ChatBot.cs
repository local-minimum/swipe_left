﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChatBot : MonoBehaviour {    

    [SerializeField]
    Animator chatAnimator;

    string triggerShowOptions = "Show";
    string triggerHideOptions = "Hide";

    [SerializeField]
    GameManager gm;

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
        if (npc)
        {
            ReplayHistory();
            nameField.text = npc.UserName;
            avatarImage.sprite = npc.Avatar;
        }
    }

    void ReplayHistory()
    {
        int l = npc.HistoryLength;
        Debug.Log(string.Format("{0} items in chat history", l));
        for (int i = 0; i < l; i++) {
            ChatHistoryItem item = npc.GetHistoryItem(i);
            if (!item.Empty)
            {
                if (item.actor == Actor.NPC)
                {
                    theyChatItem(item.txt);
                }
                else if (item.actor == Actor.Player)
                {
                    weChatItem(item.txt);
                }
            }
        }

        if (npc.ChatHasEnded)
        {
            theyLeft();
        }
    }

    void Update()
    {
        if (nextItem && npc != null && Random.value < nextPollP)
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
        npc.AddToHistory(npc.Current);
        npc.Current = npc.Current.NextChatItem();

        yield return new WaitForSeconds(Random.Range(minDelayNext, maxDelayNext));

        nextItem = !npc.ChatHasEnded;
        
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
        nextItem = false;
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

    void weChatItem(string txt)
    {
        UITextFit utf = Instantiate(weUIPrefab);
        utf.transform.SetParent(chatRect);
        utf.SetText(txt);
    }

    void weChat()
    {
        weChatItem(npc.Current.SelectedOption);

        if (npc.Current.social != SocialDimension.Neutral)
        {
            gm.player.mind.UpdateValue(npc.Current.social, npc.Current.SelectedValue);
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
            npc.AddToHistory(npc.Current);
            npc.Current = npc.Current.NextChatItem();            
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
