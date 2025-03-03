using UnityEngine;

public class DroppedItemContentPanel : ContentPanel
{
    [SerializeField] private UnityEngine.UI.Text header;
    [SerializeField] private UnityEngine.UI.Text description;
    [SerializeField] private Animator animator;
    public Animator GetAnimator() => animator;
    public void SetHeader(string text) => header.text = text;
    public void SetDescription(string text) => description.text = text;
    public void DestroyImage() => Destroy(gameObject);
}
