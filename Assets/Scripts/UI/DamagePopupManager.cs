using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupManager : MonoBehaviour
{
    public GameObject prefab;
    public float popupHeight;

    public void ShowDamage(Unit unitHit, int damage)
    {
        Vector3 position = new Vector3(unitHit.transform.position.x, popupHeight, unitHit.transform.position.z);
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = damage.ToString();

        Destroy(popup, 1f);
    }
}
