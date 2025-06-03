using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinReaction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject CoinInfoPanel_ref;
    CoinInfo CoinInfo_ref;
    GameObject InfoCoinPanel_ref;
    TMP_Text CoinName_ref;
    TMP_Text CoinDesciption_ref;
    private void Start()
    {
        InfoCoinPanel_ref = CoinInfoPanel_ref.transform.Find("InfoCoin").gameObject;
        CoinName_ref = InfoCoinPanel_ref.transform.Find("CoinName").GetComponent<TMP_Text>();
        CoinDesciption_ref = InfoCoinPanel_ref.transform.Find("CoinDescription").GetComponent<TMP_Text>();
        CoinInfo_ref = gameObject.GetComponent<CoinInfo>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        CoinInfoPanel_ref.SetActive(true);
        CoinName_ref.text = CoinInfo_ref.CoinSlot_ref.CoinName;
        CoinDesciption_ref.text = CoinInfo_ref.CoinSlot_ref.CoinDescription;
        CoinInfoPanel_ref.transform.localPosition = CoinInfo_ref.CoinSlot_ref.InfoPanelPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CoinInfoPanel_ref.SetActive(false);
    }

}
