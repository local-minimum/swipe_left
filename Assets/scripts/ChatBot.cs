using UnityEngine;
using System.Collections.Generic;

public class ChatBot : MonoBehaviour {

    List<ChatItem> history = new List<ChatItem>();

    [SerializeField]
    ChatItem Current;

    [SerializeField]
    PsychologyProfile playerProfile;

    [SerializeField]
    PsychologyProfile npcProfile;

    [SerializeField]
    UITextFit themUIPrefab;

    [SerializeField]
    UITextFit weUIPrefab;

    [SerializeField]
    RectTransform scrollRect;

    void Start()
    {
        if (Current.actor == Actor.NPC)
        {
            StartCoroutine(theyChat());
        } else
        {
            weChat();
        }
    }

    IEnumerator<WaitForSeconds> theyChat()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        UITextFit utf = Instantiate(themUIPrefab);
        utf.SetText(Current.SelectedOption);
        utf.transform.SetParent(scrollRect);
    }

    void weChat()
    {
        UITextFit utf = Instantiate(weUIPrefab);
        utf.SetText(Current.SelectedOption);
        utf.transform.SetParent(scrollRect);

    }
}
