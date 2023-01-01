using UnityEngine;

public class InteractHint : MonoBehaviour
{
    public Canvas canvas;
    public UnityEngine.UI.Image image;
    public UnityEngine.UI.Text text;
    public void DisappearInteractHint()
    {
        GetComponent<Animator>().SetTrigger("Disappear");
    }
    private void Destroy() => Destroy(gameObject);
}
