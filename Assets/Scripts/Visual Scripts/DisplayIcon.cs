using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayIcon : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,   
    IDeselectHandler  
{

    [SerializeField] private int index; 

    private void ValueEnter() { GameManager.galleryManager.DisplayEntry(index); }

    private void ValueExit() { GameManager.galleryManager.DisplayEntry(-1); }

    public void OnPointerEnter(PointerEventData eventData) => ValueEnter();
    public void OnPointerExit(PointerEventData eventData) => ValueExit();

    public void OnSelect(BaseEventData eventData) => ValueEnter();
    public void OnDeselect(BaseEventData eventData) => ValueExit();
}