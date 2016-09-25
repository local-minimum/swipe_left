using UnityEngine;
using System.Collections;

public enum Actor {Player, NPC };
public enum SocialDimension {Neutral, Maturity};
public enum IndexDirection {Next, Previos};

[CreateAssetMenu(fileName = "ChatItem", menuName = "Chat Item", order = 1)]
public class ChatItem : ScriptableObject {
    public Actor actor;

    public string[] OptionList;

    public SocialDimension social;

    public float[] values;

    public ChatItem[] nextChatItem;

    int index = 0;

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

    public bool MoveIndex(IndexDirection direction)
    {
        if (direction == IndexDirection.Next)
        {
            if (index < OptionList.Length - 2)
            {
                index++;
                return true;
            } else
            {
                return false;
            }
        } else
        {
            if (index > 0)
            {
                index--;
                return true;
            } else
            {
                return false;
            }
        }
    }

    int GetOptionBasedOnSocialValueIndex(float value)
    {
        int lowerBound = -1;
        int upperBound = -1;
        for (int i = 0; i < values.Length; i++)
        {
            if (upperBound == -1 && value < values[i])
            {
                upperBound = i;
            }
            if (lowerBound == -1 && values[i] > value)
            {
                lowerBound = i - 1;
            }
        }

        if (lowerBound < 0 && values[values.Length - 1] < value)
        {
            lowerBound = values.Length - 1;
        }

        if (upperBound < 0)
        {
            return lowerBound;
        }
        else if (lowerBound < 0)
        {
            return upperBound;
        } else
        {
            return Random.Range(values[lowerBound], values[upperBound]) > value ? upperBound : lowerBound;
        }
    }

    public ChatItem NextChatItem()
    {
        if (nextChatItem.Length > index)
        {
            return nextChatItem[index];
        }

        return null;
    }

}
