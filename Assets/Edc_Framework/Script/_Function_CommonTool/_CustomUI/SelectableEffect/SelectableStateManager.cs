using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CustomSelectableState
{
    Normal = 0,
    Highlighted,
    Pressed,
    Selected,
    Disabled,
}

public class SelectableStateManager : MonoBehaviour, 
IPointerEnterHandler, IPointerExitHandler,
IPointerDownHandler, IPointerUpHandler,
ISelectHandler, IDeselectHandler,
IPointerClickHandler,
IEventSystemHandler
{
    public CustomSelectableState currentState;
    public bool isGamepad;
    [LabelText("点击后是否保持选中状态")]
    public bool isKeepSelected;
    public Selectable selectable;
    private IBaseSelectableEffect[] ibaseSelectableEffects;
    private CustomSelectableState lastState;

    private bool isPointerDown;
    private bool isPointerInside;
    private bool hasSelection;

    [Button("修改按钮至当前状态")]
    public void SetState()
    {
        ibaseSelectableEffects = GetComponentsInChildren<IBaseSelectableEffect>();
        foreach (var item in ibaseSelectableEffects)
        {
           item.SetState(currentState.ToString());
        }
    }

    private void Reset()
    {
        selectable = GetComponent<Selectable>();
    }
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        SetState();
    }

    private void Init()
    {
        currentState = GetCustomSelectableState();
        lastState = currentState;
        ibaseSelectableEffects = GetComponentsInChildren<IBaseSelectableEffect>();
    }

    private void UpdateState()
    {
        currentState = GetCustomSelectableState();
        if (lastState != currentState)
        {
            foreach (var item in ibaseSelectableEffects)
            {
                item.ChangeState(GetDuration(), currentState.ToString());
            }
            lastState = currentState;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
        UpdateState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
        UpdateState();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        UpdateState();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        UpdateState();
    }

    public void OnSelect(BaseEventData eventData)
    {
        hasSelection = true;
        UpdateState();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        hasSelection = false;
        UpdateState();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!(isGamepad || isKeepSelected) && EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    /// <summary>
    /// 设置组件是否可交互
    /// </summary>
    public void SetInteractable(bool isInteractable)
    {
        selectable.interactable = isInteractable;
        UpdateState();
    }

    private CustomSelectableState GetCustomSelectableState()
    {
        if (!selectable.interactable)
        {
            return CustomSelectableState.Disabled;
        }

        if (isPointerDown)
        {
            return CustomSelectableState.Pressed;
        }

        if (hasSelection)
        {
            return CustomSelectableState.Selected;
        }

        if (isPointerInside)
        {
            return CustomSelectableState.Highlighted;
        }
        return CustomSelectableState.Normal;
    }

    private float GetDuration()
    {
        switch (currentState)
        {
            case CustomSelectableState.Normal:
                return 0.25f;
            case CustomSelectableState.Highlighted:
                return 0.15f;
            case CustomSelectableState.Pressed:
                return 0.05f;
            case CustomSelectableState.Selected:
                return 0.15f;
            default:
                return 0.15f;
        }
    }
}
