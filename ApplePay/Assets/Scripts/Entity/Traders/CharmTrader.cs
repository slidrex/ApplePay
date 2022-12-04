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
        obj = Instantiate(cards, FindObjectOfType<Pay.UI.UIHolder>().HUDCanvas.transform.position, Quaternion.identity, FindObjectOfType<Pay.UI.UIHolder>().HUDCanvas.transform);
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
                obj.SetActive(true);
                GetComponent<Animator>().SetBool("isOpen", true);
            }
            if(Input.GetKeyDown(KeyCode.Escape) && traded == true)
            {
                traded = false;
                obj.SetActive(false);
                GetComponent<Animator>().SetBool("isOpen", false);
                
            }
        }
        else
        {
            traded = false;
            if (obj != null) obj.SetActive(false);
            button.SetBool("isActive", false);
            GetComponent<Animator>().SetBool("isOpen", false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, tradeDistance);
    }
}
