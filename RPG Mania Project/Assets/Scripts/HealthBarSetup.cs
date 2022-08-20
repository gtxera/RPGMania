using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSetup : MonoBehaviour
{
    public GameObject targetGO;
    private Vector3 targetPosition;
    private Unit targetUnit;
    private Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = targetGO.transform.position;
        gameObject.transform.position = targetPosition;
        gameObject.transform.position -= new Vector3(0, 0.7f, 0);
        gameObject.transform.localScale = new Vector3(0.02f, 0.02f, 0);

        healthSlider = gameObject.GetComponent<Slider>();
        targetUnit = targetGO.GetComponent<Unit>();
        healthSlider.maxValue = targetUnit.maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = targetUnit.currentHP;
    }
}
