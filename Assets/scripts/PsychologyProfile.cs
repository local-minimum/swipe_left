using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PychProfile", menuName = "Psychology Profile", order = 1)]
public class PsychologyProfile : ScriptableObject {

    [SerializeField, HideInInspector]
    float[] dimensionValues;

    [SerializeField, Range(0, 1)]
    float maleability = 0.3f;

    [SerializeField, Range(0, 1)]
    float leaveChatThrehold = 0.1f;

    [Range(0, 1)]
    public float interest = 0.5f;

    List<ChatItem> history = new List<ChatItem>();
    
    public ChatItem abandonMessage;

    public bool isSocial
    {
        get
        {
            return dimensionValues.Length > 0;
        }
    }

    public bool hasAllDimensions
    {
        get
        {
            return System.Enum.GetValues(typeof(SocialDimension)).Length - 1 == dimensionValues.Length;
        }
    }

    public void ExpandToAllDimension()
    {
        int l = System.Enum.GetValues(typeof(SocialDimension)).Length - 1;
        float[] newDimValues = new float[l];
        for (int i=0; i< l; i++)
        {
            newDimValues[i] = 0.5f;
        }

        System.Array.Copy(dimensionValues, newDimValues, Mathf.Min(newDimValues.Length, dimensionValues.Length));
        dimensionValues = newDimValues;

    }

    public void AddToHistory(ChatItem item)
    {
        history.Add(item);
    }

    public int nDimensions
    {
        get
        {
            return dimensionValues.Length;
        }
       
    }

    public float GetValue(SocialDimension dimension)
    {
        int index = GetDimensionIndex(dimension);        
        if (index < 0 || index >= dimensionValues.Length)
        {
            if (index > 0)
            {
                Debug.LogWarning(name + " has not set social value for " + dimension);
            }
            return 0.5f;
        } else
        {
            return dimensionValues[index];
        }
    }

    public void SetValue(SocialDimension dimension, float value)
    {
        int index = GetDimensionIndex(dimension);
        if (index < 0)
        {
            Debug.LogWarning("Can't set value for neutral dimension " + dimension);
            return;
        } else if (index >= dimensionValues.Length)
        {
            Debug.LogWarning("Can't set value for dimension because not included " + dimension);
            return;
        } else
        {
            dimensionValues[index] = value;
        }

    }

    public void UpdateValue(SocialDimension dimension, float value)
    {
        int index = GetDimensionIndex(dimension);
        if (index < 0)
        {
            Debug.LogWarning("Can't set value for neutral dimension " + dimension);
            return;
        } 

        if (index >= dimensionValues.Length)
        {
            float[] newDimensionValues = new float[index + 1];
            System.Array.Copy(dimensionValues, newDimensionValues, dimensionValues.Length);
            for (int i=dimensionValues.Length; i< newDimensionValues.Length; i++)
            {
                newDimensionValues[i] = 0.5f;
            }
            dimensionValues = newDimensionValues;
        }

        dimensionValues[index] = Mathf.Lerp(dimensionValues[index], value, maleability);
    }

    int GetDimensionIndex(SocialDimension dimension)
    {
        return (int)dimension - 1;
    }

    public bool UpdateInterestAndGetStayInChat(SocialDimension dimension, float othersValue)
    {
        float deltaInterest = GetInterestDelta(dimension, othersValue);
        UpdateInterest(deltaInterest);
        return interest > leaveChatThrehold;
    }

    float GetInterestDelta(SocialDimension dimension, float othersValue)
    {
        float delta = Mathf.Min(1, Mathf.Abs(GetValue(dimension) - othersValue) * 1.2f);
        Debug.Log(string.Format("{0} : {1} -> {2}", GetValue(dimension), othersValue, Mathf.Pow(delta, 2f/3f)));
        if (Random.value > Mathf.Pow(delta, 2f/3f))
        {
            Debug.Log("Interest increase");
            return Mathf.Clamp01(interest * (1.05f + Random.value / 3f)); 
        } else
        {
            Debug.Log("Interest decrease");
            return interest / 1.5f * Random.value;
        }
    }

    void UpdateInterest(float value)
    {
        interest = (interest + value) / 2f;
    }
}
