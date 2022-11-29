using UnityEngine;

public class CharmTrader : MonoBehaviour
{
    [SerializeField] private GameObject cards;
    [SerializeField] private Animator button;
    [SerializeField] private float tradeDistance;
    private PlayerEntity player;
    private GameObject obj;
    private bool traded = false;
    private void Start()
    {
        player = FindObjectOfType<PlayerEntity>();
    }
    private void Update()
    {
        Trade();
    }
    private void Trade()
    {
        if(Vector2.Distance(player.transform.position, transform.position) <= tradeDistance)
        {
            button.SetBool("isActive", true);
            if(Input.GetKeyDown(KeyCode.C) && traded == false)
            {
                traded = true;
                GetComponent<Animator>().SetBool("isOpen", true);
                obj = Instantiate(cards, FindObjectOfType<Pay.UI.UIHolder>().HUDCanvas.transform.position, Quaternion.identity, FindObjectOfType<Pay.UI.UIHolder>().HUDCanvas.transform);
            }
            if(Input.GetKeyDown(KeyCode.Escape) && traded == true)
            {
                GetComponent<Animator>().SetBool("isOpen", false);
                traded = false;
                Destroy(obj);
            }
        }
        else
        {
            if (obj != null) Destroy(obj);
            traded = false;
            button.SetBool("isActive", false);
            GetComponent<Animator>().SetBool("isOpen", false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, tradeDistance);
    }
}
