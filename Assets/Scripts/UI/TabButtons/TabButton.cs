using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public abstract class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private int _id;

    protected TabManager tabManagerRef;

    [SerializeField] protected Sprite spriteIdle;

    [Header("UnityEvents")]
    [SerializeField] protected UnityEvent selectionEvent;
    [SerializeField] protected UnityEvent deselectionEvent;

    //[Header("DarkMaskSettings")]
    //[Tooltip("The ratio of how dense darkMask is: 0.0 is the darkest mask, 1.0 is the lightest")]
    protected float darkeningRatio;
    protected Color darkMaskColor;
    protected Image imageRef;

    public int Id { get => _id; set => _id = value; }
    public UnityEvent SelectionEvent => selectionEvent;
    public UnityEvent DeselectionEvent => deselectionEvent;

    private void Awake()
    {
        tabManagerRef = transform.GetComponentInParent<TabManager>();
        imageRef = GetComponent<Image>();
        imageRef.sprite = spriteIdle;
        darkeningRatio = 0.8f;
        darkMaskColor = new Color(1.0f * darkeningRatio, 1.0f * darkeningRatio, 1.0f * darkeningRatio);
    }

    private void OnEnable()
    {
        tabManagerRef.SelectButton += OnSelect;
        tabManagerRef.DeselectButton += OnDeSelect;
    }

    private void OnDisable()
    {
        tabManagerRef.SelectButton -= OnSelect;
        tabManagerRef.DeselectButton -= OnDeSelect;
    }

    private void OnDestroy()
    {
        tabManagerRef.SelectButton -= OnSelect;
        tabManagerRef.DeselectButton -= OnDeSelect;
    }

    protected void DarkenImage()
    {
        imageRef.color = darkMaskColor;
    }

    protected void BrightenImage()
    {
        imageRef.color = Color.white;
    }

    protected abstract void OnSelect(TabButton tabButton_in);
    protected abstract void OnDeSelect(TabButton tabButton_in);
    public abstract void OnPointerClick(PointerEventData eventData);
    public abstract void OnPointerEnter(PointerEventData eventData);
    public abstract void OnPointerExit(PointerEventData eventData);
}
