using UnityEngine;

public class smartPhoneInput : MonoBehaviour
{
    private Vector2 fingerDown;
    private Vector2 fingerUp;

    static bool resetButtonDown;
    public static bool ResetButtonDown
    {
        get
        {
            bool val = resetButtonDown;
            resetButtonDown = false;
            return val;
        }
    }

    static float horizontalVal;
    public static float Horizontal
    {
        get
        {
            float val = horizontalVal;
            horizontalVal = 0;
            return val;
        }
    }

    static float verticalVal;
    public static float Vertical
    {
        get
        {
            float val = verticalVal;
            verticalVal = 0;
            return val;
        }
    }

    // 指を離したときだけ処理を行う
    public bool detectSwipeOnlyAfterRelease = false;

    // しきい値以上スワイプするとスワイプとして検知する
    public float SWIPE_THRESHOLD = 20f;

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            // スワイプをし始めた位置を記録する
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }
        }

        if (Input.GetButtonDown("Horizontal"))
        {
            horizontalVal = Input.GetAxisRaw("Horizontal");
        }

        if (Input.GetButtonDown("Vertical"))
        {
            verticalVal = Input.GetAxisRaw("Vertical");
        }
    }

    void checkSwipe()
    {
        // しきい値以上縦方向にスワイプしたかどうか判定する
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            if (fingerDown.y - fingerUp.y > 0)
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }
        // しきい値以上横方向にスワイプしたかどうか判定する
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            if (fingerDown.x - fingerUp.x > 0)
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    void OnSwipeUp()
    {
        verticalVal = 1;
    }

    void OnSwipeDown()
    {
        verticalVal = -1;
    }

    void OnSwipeLeft()
    {
        horizontalVal = -1;
    }

    void OnSwipeRight()
    {
        horizontalVal = 1;
    }

    public void ResetButton()
    {
        resetButtonDown = true;
    }
}
