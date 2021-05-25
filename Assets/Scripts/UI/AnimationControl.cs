using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using TMPro;

public class AnimationControl : MonoBehaviour
{
    public static AnimationControl Instance { get; private set; }

    [Tooltip("The name of an animator boolean parameter. Case sensitive")]
    [SerializeField] private string outTriggerName;
    [SerializeField] private string inTriggerName;
    [SerializeField] [Min(0)] private float delayTime;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            AnimationControl.Instance.RollAnnouncementOut(PlayerManager.Instance.CurrentPlayer);
        }
    }

    public void RollAnnouncementOut(Player player_in)
    {
        if (_firstTime)
            _animatorRef.enabled = true;

        StartCoroutine(StartAnimation(player_in));
    }

    private IEnumerator StartAnimation(Player player_in)
    {
        _tmProRef.text = $"{player_in.Side} player has been eliminated!";

        if(!_firstTime)
        {
            ActivateClip(outTriggerName);
        }
        else
            _firstTime = false;

        yield return new WaitForSeconds(delayTime);

        ActivateClip(inTriggerName);
    }

    private void ActivateClip(string triggerName_in)
    {
        _animatorRef.SetTrigger(triggerName_in);
    }

}
