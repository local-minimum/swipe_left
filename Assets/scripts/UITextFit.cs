using UnityEngine;
using UnityEngine.UI;

public class UITextFit : MonoBehaviour {

    [SerializeField]
    float overshoot = 10;

    int index = -1;

    void Reset()
    {
        UpdateSize();
    }

    void Start() {
        UpdateSize();
    }

    void Update()
    {
        UpdateSize();
    }

    void UpdateSize() { 
        Text text = GetComponentInChildren<Text>();
        LayoutElement lElem = GetComponent<LayoutElement>();

        float innerHeight = LayoutUtility.GetPreferredHeight(text.rectTransform);
        float padding = text.rectTransform.offsetMin.y + text.rectTransform.offsetMax.y;
        float anchorHeight = text.rectTransform.anchorMax.y - text.rectTransform.anchorMin.y;
        lElem.minHeight = overshoot + padding +  innerHeight / anchorHeight;
        //Debug.Log(overshoot + padding + innerHeight / anchorHeight);
	}
	
    public void SetText(int idx, string txt)
    {
        index = idx;
        SetText(txt);
    }

    public void SetText(string txt)
    {
        Text text = GetComponentInChildren<Text>();
        text.text = txt;        
        UpdateSize();
    }

    public int Index
    {
        get
        {
            return index;
        }
    }
}
