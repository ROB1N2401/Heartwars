using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using TMPro;

public class AnimationControl : MonoBehaviour
{
    public static AnimationControl Instance { get; private set; }

    [SerializeField] private AudioSource source;
    [SerializeField] private string outTriggerName;
    [SerializeField] private string inTriggerName;
    [SerializeField] [Min(0)] private float animationDelay;
    [SerializeField] [Min(0)] private float soundDelay;


    private TextMeshProUGUI _tmProRef; //Text Box containing announcement, should be the first child in the hierarchy
    private Animator _animatorRef;
    private bool _firstTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _tmProRef = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _animatorRef = GetComponent<Animator>();
        _animatorRef.enabled = false;
        _firstTime = true;
    }

    public void RollAnnouncementOut(Player player_in)
    {
        if (_firstTime)
        {
            _animatorRef.enabled = true;
            source.Play();
        }

        StartCoroutine(StartAnimation(player_in));
    }

    private IEnumerator StartAnimation(Player player_in)
    {
        _tmProRef.text = $"{player_in.Side} player has been eliminated!";

        if(!_firstTime)
        {
            ActivateClip(outTriggerName);
            source.Play();
        }
        else
            _firstTime = false;

        yield return new WaitForSeconds(animationDelay);

        ActivateClip(inTriggerName);

        yield return new WaitForSeconds(soundDelay);

        source.Play();
    }

    private void ActivateClip(string triggerName_in)
    {
        _animatorRef.SetTrigger(triggerName_in);
    }

}
