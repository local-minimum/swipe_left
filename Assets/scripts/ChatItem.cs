using UnityEngine;
using System.Collections;

public enum Actor {Player, NPC };
public enum SocialDimension {Neutral, Maturity, Eagerness, Irony, Aggressive};
public enum IndexDirection {Next, Previos};

[CreateAssetMenu(fileName = "ChatItem", menuName = "Chat Item", order = 1)]
public class ChatItem : ScriptableObject {
    public Actor actor;

    public string[] OptionList;

    public SocialDimension social;

    public float[] values;

    public ChatItem[] nextChatItem;

    int index = 0;

    public bool singleNextChat = true;

    public bool HasOptions()
    {
        return OptionList.Length > 1;
    }

    public string NextOption
    {
        get
        {
            if (OptionList.Length > index + 1)
            {
                return OptionList[index + 1];
            }
            return null;
        }
    }

    public string PreviousOption
    {
        get
        {
            if (index > 0)
            {
                return OptionList[index - 1];
            }
            return null;
        }
    }

    public string SelectedOption
    {
        get
        {            
            return OptionList[index];
        }
    }

    public string GetOptionBasedOnSocialValue(float value)
    {
        index = GetOptionBasedOnSocialValueIndex(value);
        if (index >= 0)
        {
            return OptionList[index];
        } else
        {
            return null;
        }
    }

    int GetOptionBasedOnSocialValueIndex(float value)
    {
        int lowerBound = -1;
        int upperBound = -1;
        for (int i = 0; i < values.Length; i++)
        {
            if (value < values[i] && (upperBound == -1 || values[upperBound] > values[i]))
            {
                upperBound = i;
            }
            else if (value > values[i] && (lowerBound == -1 || values[lowerBound] < values[i]))
            {
                lowerBound = i;
            }
        }

        if (upperBound < 0)
        {
            return lowerBound;
        }
        else if (lowerBound < 0)
        {
            return upperBound;
        }
        else
        {
            return Random.Range(values[lowerBound], values[upperBound]) > value ? upperBound : lowerBound;
        }
    }

    public bool MoveIndex(IndexDirection direction)
    {
        if (direction == IndexDirection.Next)
        {
            if (index < OptionList.Length - 2)
            {
                index++;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (index > 0)
            {
                index--;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void SetIndex(int idx)
    {
        index = Mathf.Clamp(idx, 0, OptionList.Length - 1);        
    }

    public float SelectedValue
    {
        get
        {
            return values[index];
        }
    }

    public ChatItem NextChatItem()
    {
        if (singleNextChat)
        {
            if (nextChatItem.Length > 0)
            {
                return nextChatItem[0];
            }
        }
        else if (nextChatItem.Length > index)
        {
            return nextChatItem[index];
        }

        return null;
    }

}
