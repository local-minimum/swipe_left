using UnityEngine;
using System.Collections.Generic;

public class ChatBot : MonoBehaviour {

    List<ChatItem> history = new List<ChatItem>();

    [SerializeField]
    ChatItem Current;

    [SerializeField]
    Animator chatAnimator;

    string triggerShowOptions = "Show";
    string triggerHideOptions = "Hide";

    [SerializeField]
    PsychologyProfile playerProfile;

    [SerializeField]
    PsychologyProfile npcProfile;

    [SerializeField]
    UITextFit themUIPrefab;

    [SerializeField]
    UITextFit weUIPrefab;

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

    void Update()
    {
        if (nextItem && Random.value < nextPollP)
        {
            nextItem = false;
            if (Current.actor == Actor.NPC)
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
        UITextFit utf = Instantiate(themUIPrefab);
        float npcSocialValue = npcProfile.GetValue(Current.social);
        string txt = Current.GetOptionBasedOnSocialValue(npcSocialValue);
        if (Current.social != SocialDimension.Neutral)
        {
            npcProfile.UpdateValue(Current.social, Current.SelectedValue);
        }
        utf.SetText(txt);
        utf.transform.SetParent(chatRect);

        yield return new WaitForSeconds(Random.Range(minDelayNext, maxDelayNext));


        Current = Current.NextChatItem();

        nextItem = Current != null;
        npcProfile.AddToHistory(Current);

    }

    void weChat()
    {
        UITextFit utf = Instantiate(weUIPrefab);
        utf.transform.SetParent(chatRect);
        utf.SetText(Current.SelectedOption);
        if (Current.social != SocialDimension.Neutral)
        {
            playerProfile.UpdateValue(Current.social, Current.SelectedValue);
        }
    }

    void showOptions()
    {
        UITextFit utf;
        for (int i=0; i<Current.OptionList.Length; i++)
        {
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
            utf.SetText(i, Current.OptionList[i]);

        }

        for (int i=Current.OptionList.Length; i < optionsRect.childCount; i++)
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
        Current.SetIndex(btn.GetComponent<UITextFit>().Index);
        weChat();

        Current = Current.NextChatItem();
        npcProfile.AddToHistory(Current);

        if (Current != null)
        {
            StartCoroutine(DelayNext());
        }
    }

    IEnumerator<WaitForSeconds> DelayNext()
    {
        yield return new WaitForSeconds(Random.Range(minDelayNext, maxDelayNext));
        nextItem = true;
    }
}
