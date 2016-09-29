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

    bool nextItem = true;

    void Update()
    {
        if (nextItem && Random.value < 0.1f)
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
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        UITextFit utf = Instantiate(themUIPrefab);
        utf.SetText(Current.SelectedOption);
        utf.transform.SetParent(chatRect);

        yield return new WaitForSeconds(Random.Range(1f, 4f));
        Current = Current.NextChatItem();
        nextItem = Current != null;
    }

    void weChat()
    {
        UITextFit utf = Instantiate(weUIPrefab);
        utf.SetText(Current.SelectedOption);
        utf.transform.SetParent(chatRect);

    }

    void showOptions()
    {
        UITextFit utf;
        for (int i=0; i<Current.OptionList.Length; i++)
        {
            if (i < optionsRect.childCount)
            {
                GameObject child = optionsRect.GetChild(i).gameObject;
                child.SetActive(true);
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
        btn.OnClickAction -= ChatBot_OnClickAction;

        chatAnimator.SetTrigger(triggerHideOptions);
        Current.SetIndex(btn.GetComponent<UITextFit>().Index);
        weChat();

        Current = Current.NextChatItem();
        if (Current != null)
        {
            StartCoroutine(DelayNext());
        }
    }

    IEnumerator<WaitForSeconds> DelayNext()
    {
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        nextItem = true;
    }
}
