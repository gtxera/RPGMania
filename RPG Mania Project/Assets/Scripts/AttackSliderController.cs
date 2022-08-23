using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackSliderController : MonoBehaviour
{
    [SerializeField] GameObject hitArea;
    private RectTransform hitAreaTransform;
    [SerializeField] GameObject hitCritArea;
    private RectTransform hitCritAreaTransform;
    private RectTransform sliderTransform;
    private Slider slider;
    GameObject handle;
    Image handleImg;

    public int dificulty;
    private float increment = 1f;
    private bool goingUp;
    public bool moving;
    private float value;

    private float hitDificulty;
    private float hitCritDificulty;

    public int dmgToReturn;

    void Awake()
    {
        handle = gameObject.transform.Find("Handle").gameObject;
        handleImg = handle.GetComponent<Image>();
        sliderTransform = gameObject.GetComponent<RectTransform>();
        hitAreaTransform = hitArea.GetComponent<RectTransform>();
        hitCritAreaTransform = hitCritArea.GetComponent<RectTransform>();
        slider = gameObject.GetComponent<Slider>();
    }
    void OnEnable()
    {   
        handleImg.color = new Color(255, 255, 255, 255);
        
        slider.value = 0;

        float sliderWidth = sliderTransform.rect.width;

        hitDificulty = sliderWidth/10 * (2f + 0.4f * dificulty);
        hitCritDificulty = sliderWidth/10 * (4.5f + 0.05f * dificulty);

        hitAreaTransform.offsetMin = new Vector2(hitDificulty, 0f);
        hitAreaTransform.offsetMax = new Vector2(-hitDificulty, 0f);

        hitCritAreaTransform.offsetMin = new Vector2(hitCritDificulty, 0f);
        hitCritAreaTransform.offsetMax = new Vector2(-hitCritDificulty, 0f);

        dmgToReturn = 0;
        moving = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(moving){
            value = slider.value;

            if(goingUp){
                value += (increment + 0.3f * dificulty) *  Time.deltaTime;
            }
            else{
                value -= (increment + 0.3f * dificulty) *  Time.deltaTime;
            }

            if(value >= 1){
                goingUp = false;
            }
            if(value <= 0){
                goingUp = true;
            }

        slider.value = value;
        }

        if(Input.GetKeyDown("space")){
            moving = false;

            if(value >= 0.45f + 0.005f * dificulty && value <= 0.55f - 0.005f * dificulty){
                dmgToReturn = 2;
                handleImg.color = new Color(255, 245, 0, 255);
            }
            else if(value >= 0.2f + 0.04f * dificulty && value <= 0.8f - 0.04f * dificulty){
                dmgToReturn = 1;
                handleImg.color = new Color32(36, 58, 130, 255);
            }
            else{
                handleImg.color = new Color32(144, 50, 50, 255);
            }
        }
    }
}
