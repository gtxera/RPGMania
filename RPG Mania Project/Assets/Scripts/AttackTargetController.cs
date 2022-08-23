using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTargetController : MonoBehaviour
{
    private Transform targetTransform;

    private Ray ray;
    private RaycastHit2D hover;
    private Camera mainCam {get {return Camera.main.GetComponent<Camera>();}}

    private float increment = 3f;
    public bool moving;
    public int dificulty;
    private float verticalDirection = 0;
    private float horizontalDirection = 0;

    public int dmgToReturn;

    void Awake()
    {
        targetTransform = gameObject.GetComponent<Transform>();
    }


    void OnEnable()
    {
        moving = true;
        targetTransform.position = new Vector3(Random.Range(-6, 6), Random.Range(-4, 4), 3);
        
        verticalDirection = Mathf.Round(Random.Range(-1f, 1f));
        horizontalDirection = Mathf.Round(Random.Range(-1f, 1f));
        while(verticalDirection == 0){
            verticalDirection = Mathf.Round(Random.Range(-1f, 1f));
        }
        while(horizontalDirection == 0){
            horizontalDirection = Mathf.Round(Random.Range(-1f, 1f));
        }
    }


    void Update()
    {
        if(moving){targetTransform.position += new Vector3 (increment * horizontalDirection * Time.deltaTime, increment * verticalDirection * Time.deltaTime, 0);}

        if(Input.GetMouseButtonDown(0)){
            moving = false;
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            hover = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if(hover){

                if(hover.collider.gameObject.name == "HitCritTarget"){
                    dmgToReturn = 2;
                    Debug.Log(dmgToReturn);
                }
                else if(hover.collider.gameObject.name == "HitTarget"){
                    dmgToReturn = 1;
                    Debug.Log(dmgToReturn);
                }
                else{
                    dmgToReturn = 0;
                    Debug.Log(dmgToReturn);
                }
            }
            else{
                dmgToReturn = 0;
                Debug.Log(dmgToReturn);
            }

        }

    }

    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("bateu");
        switch(col.gameObject.tag){
            case "horizontalCollider":
                horizontalDirection = horizontalDirection * - 1;
            break;
            case "verticalCollider":
                verticalDirection = verticalDirection * - 1;
            break;
        }
        
    }

}
