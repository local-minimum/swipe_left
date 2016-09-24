using UnityEngine;
using System.Collections;

public delegate void SwipeVote(bool liked);

public class swiper : MonoBehaviour {

    public event SwipeVote OnSwipeVote;
       
    Animator anim;

    string triggerLiking = "Liking";
    string triggerLikeLevel = "Like";

    float likeLevel = 0.5f;
    bool swiping = false;
    bool hovering = false;

    [SerializeField, Range(0.2f, 0.5f)]
    float swipeRange = 0.3f;
    [SerializeField, Range(0f, 1f)]
    float acceptThreshold = 0.5f;
    [SerializeField, Range(0.9f, 2f)]
    float acceptWithoutRelease = 1.1f;

    void Start()
    {
        Input.simulateMouseWithTouches = true;
        anim = GetComponentInParent<Animator>();
    }

    void Update()
    {
        if (hovering && !swiping)
        {
            swiping = Input.GetMouseButtonDown(0);
            if (swiping)
            {
                Cursor.visible = false;
            }
        } else if (swiping && Input.GetMouseButtonUp(0))
        {
            float scaledLikeLevel = GetScaledLikeLevel();
            if (Mathf.Abs(scaledLikeLevel) > acceptThreshold)
            {
                TriggerLiking(scaledLikeLevel > 0);
            }
            EndSwipe();
        } else if (swiping)
        {
            float scaledLikeLevel = GetScaledLikeLevel();
            if (Mathf.Abs(scaledLikeLevel) > acceptWithoutRelease)
            {
                TriggerLiking(scaledLikeLevel > 0);
                EndSwipe();
            }
        }
    }

    float GetScaledLikeLevel ()
    {
        return (likeLevel - 0.5f) * 2f;
    }

    void EndSwipe()
    {
        hovering = false;
        anim.SetBool(triggerLiking, false);        
        likeLevel = 0.5f;
        anim.SetFloat(triggerLikeLevel, likeLevel);
        swiping = false;
        Cursor.visible = true;
        SetDragMarker(0.5f);
    }

    void LateUpdate()
    {
        if (swiping)
        {
            
            float mouseFraction = Input.mousePosition.x / Screen.width;
            float anchorX = Mathf.Clamp(mouseFraction, 0.5f - swipeRange, 0.5f + swipeRange);
            
            likeLevel = (mouseFraction - (0.5f - swipeRange)) / (2f * swipeRange);
            anim.SetFloat(triggerLikeLevel, likeLevel);
            SetDragMarker(anchorX);
        }
    }

    void SetDragMarker(float anchorX)
    {
        RectTransform rt = transform as RectTransform;
        Vector3 position = Vector3.zero;
        position.x = Screen.width * (anchorX - 0.5f);
        rt.anchoredPosition = position;
    }

    void TriggerLiking(bool liked)
    {
        if (OnSwipeVote != null)
        {
            OnSwipeVote(liked);
        }
    }

    public void MouseOver()
    {
        if (!swiping)
        {
            likeLevel = 0.5f;
            anim.SetFloat(triggerLikeLevel, likeLevel);
            anim.SetBool(triggerLiking, true);
            hovering = true;            
        }
    }

    public void MouseExit()
    {
        if (!swiping)
        {
            anim.SetBool(triggerLiking, false);
            hovering = false;
        }
    }

}
