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

    void Start()
    {
        if (Current.actor == Actor.NPC)
        {
            StartCoroutine(theyChat());
        } else
        {
            showOptions();
        }
    }

    IEnumerator<WaitForSeconds> theyChat()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        UITextFit utf = Instantiate(themUIPrefab);
        utf.SetText(Current.SelectedOption);
        utf.transform.SetParent(chatRect);
    }

    void weChat()
    {
        UITextFit utf = Instantiate(weUIPrefab);
        utf.SetText(Current.SelectedOption);
        utf.transform.SetParent(chatRect);

    }

    void showOptions()
    {
        for (int i=0; i<Current.OptionList.Length; i++)
        {
            UITextFit utf = Instantiate(weOptionPrefab);
            utf.SetText(i, Current.OptionList[i]);
            utf.transform.SetParent(optionsRect);
            utf.GetComponent<UIButtonish>().OnClickAction += ChatBot_OnClickAction;
        }
        chatAnimator.SetTrigger(triggerShowOptions);
    }

    private void ChatBot_OnClickAction(UIButtonish btn)
    {
        chatAnimator.SetTrigger(triggerHideOptions);
        Current.SetIndex(btn.GetComponent<UITextFit>().Index);
        weChat();
    }
}
