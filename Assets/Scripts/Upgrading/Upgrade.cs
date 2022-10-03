using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour
{
    [SerializeField]
    Sprite upgradeSlotFull;
    [SerializeField]
    GameObject upgradeSlot;
    [SerializeField]
    GameObject upgradePanel;
    [SerializeField]
    GameObject shopPanel;


    [SerializeField]
    GameObject rangeCircle;
    GameObject rC;

    TowerShop shop;

    TMP_Text text;

    UpgradePanelData currentData;
    private void Start()
    {
        upgradePanel.SetActive(false);
        shop = GameObject.Find("ShopController").GetComponent<TowerShop>();
        text = upgradePanel.transform.Find("PriceCounter").GetComponent<TMP_Text>();
    }

    public void SetPanel(UpgradePanelData data)
    {
        Destroy(GameObject.Find("Range(Clone)"));
        if (shop.canBuild)
        {
            currentData = data;

            rC = Instantiate(rangeCircle);
            rC.transform.position = data.transform.position;
            TowerBehaviour tB = data.transform.GetChild(0).gameObject.GetComponent<TowerBehaviour>();
            rC.transform.localScale = new Vector2(tB.range * 2, tB.range * 2);

            for (int i = 0; i < upgradePanel.transform.Find("UpgradeAmount").childCount; i++)
            {
                Destroy(upgradePanel.transform.Find("UpgradeAmount").GetChild(i).gameObject);
            }
            for (int i = 0; i < data.upgradeAmount; i++)
            {
                GameObject slot = Instantiate(upgradeSlot, upgradePanel.transform.Find("UpgradeAmount"));
                if (i <= data.Level - 1)
                {
                    slot.GetComponent<Image>().sprite = upgradeSlotFull;
                }
            }
            upgradePanel.SetActive(true);
            shopPanel.SetActive(false);
            upgradePanel.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = data.icon;

            if (data.Upgrade == null)
            {
                upgradePanel.transform.Find("Upgrade").gameObject.SetActive(false); 
                text.gameObject.SetActive(false);
            }
            else
            {
                upgradePanel.transform.Find("Upgrade").gameObject.SetActive(true);
                text.gameObject.SetActive(true);
            }
            text.text = "Price: " + data.nextCost.ToString();
        }
    }

    public void ExitMenu()
    {
        Destroy(GameObject.Find("Range(Clone)"));
    }

    public void UpgradeCurrent()
    {
        if (shop.counter.CheckRemovedResources(currentData.nextCost))
        {
            GameObject upgrade = Instantiate(currentData.Upgrade, currentData.transform.position, currentData.transform.rotation);
            Instantiate(currentData.transform.GetChild(0).gameObject, upgrade.transform);
            shop.counter.Removeresources(currentData.nextCost);
            Destroy(currentData.gameObject);
            SetPanel(upgrade.GetComponent<UpgradePanelData>());
        }
    }

}
