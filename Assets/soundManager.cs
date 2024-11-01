using UnityEngine;

public class soundManager : MonoBehaviour
{
    public static soundManager inst;

    AudioSource ac;

    [SerializeField]
    AudioClip moveSound;

    [SerializeField]
    AudioClip goalSound;

    [SerializeField]
    AudioClip getSound;

    [SerializeField]
    AudioClip levelSound;

    void Awake()
    {
        inst = this;
        ac = GetComponent<AudioSource>();
    }

    public void ad_move()
    {
        ac.PlayOneShot(moveSound);
    }

    public void ad_goal()
    {
        ac.PlayOneShot(goalSound);
    }
    public void ad_getItem()
    {
        ac.PlayOneShot(getSound);
    }
    public void ad_levelUp()
    {
        ac.PlayOneShot(levelSound);
    }
}
