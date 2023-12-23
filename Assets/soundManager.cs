using UnityEngine;

public class soundManager : MonoBehaviour
{
    public static soundManager inst;

    AudioSource ac;

    [SerializeField]
    AudioClip moveSound;

    [SerializeField]
    AudioClip goalSound;

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
}
