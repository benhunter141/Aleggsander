using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve extraHeight; //linear ish upwards
    public TextMeshProUGUI text;
    float timer = 0;
    Vector3 origin;

    private void Awake()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
    }

    private void Update()
    {
        //Debug.Log($"alpha:{opacityCurve.Evaluate(timer)}, time:{timer}");
        text.color = new Color(1, 1, 1, opacityCurve.Evaluate(timer));
        var pos = new Vector3(origin.x, origin.y + extraHeight.Evaluate(timer), origin.z);
        transform.position = pos;
        timer += Time.deltaTime;
    }
}
