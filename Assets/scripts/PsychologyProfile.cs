using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PychProfile", menuName = "Psychology Profile", order = 1)]
public class PsychologyProfile : ScriptableObject {

    [SerializeField, HideInInspector]
    float[] dimensionValues;

    [SerializeField, Range(0, 1)]
    float maleability = 0.3f;

    public float GetValue(SocialDimension dimension)
    {
        int index = GetDimensionIndex(dimension);
        if (index < 0 || index >= dimensionValues.Length)
        {
            return 0.5f;
        } else
        {
            return dimensionValues[index];
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
}
