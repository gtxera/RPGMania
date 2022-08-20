using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHUDSetup : MonoBehaviour
{
    private GameObject battleSystem;
    [SerializeField] private GameObject HealthBar;

    public GameObject player1GO;
    public GameObject player2GO;
    public GameObject player3GO;
    private GameObject player1HealthBar;
    private GameObject player2HealthBar;
    private GameObject player3HealthBar;
    private HealthBarSetup player1HealthBarSetup;
    private HealthBarSetup player2HealthBarSetup;
    private HealthBarSetup player3HealthBarSetup;

    public GameObject enemy1GO;
    public GameObject enemy2GO;
    public GameObject enemy3GO;
    public GameObject enemy4GO;
    public GameObject enemy5GO;
    private GameObject enemy1HealthBar;
    private GameObject enemy2HealthBar;
    private GameObject enemy3HealthBar;
    private GameObject enemy4HealthBar;
    private GameObject enemy5HealthBar;
    private HealthBarSetup enemy1HealthBarSetup;
    private HealthBarSetup enemy2HealthBarSetup;
    private HealthBarSetup enemy3HealthBarSetup;
    private HealthBarSetup enemy4HealthBarSetup;
    private HealthBarSetup enemy5HealthBarSetup;

    private RaycastHit currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        battleSystem = GameObject.Find("BattleController");
        HealthBarsSetup();
    }

    void Update()
    {
        
    }

    void HealthBarsSetup()
    {
        if(player1GO){
            player1HealthBar = Instantiate(HealthBar);
            player1HealthBarSetup = player1HealthBar.GetComponent<HealthBarSetup>();
            player1HealthBarSetup.targetGO = player1GO;
            player1HealthBar.transform.SetParent(gameObject.transform);
        }
        if(player1GO){
            player2HealthBar = Instantiate(HealthBar);
            player2HealthBarSetup = player2HealthBar.GetComponent<HealthBarSetup>();
            player2HealthBarSetup.targetGO = player2GO;
            player2HealthBar.transform.SetParent(gameObject.transform);
        }
        if(player1GO){
            player3HealthBar = Instantiate(HealthBar);
            player3HealthBarSetup = player3HealthBar.GetComponent<HealthBarSetup>();
            player3HealthBarSetup.targetGO = player3GO;
            player3HealthBar.transform.SetParent(gameObject.transform);
        }
        
        if(enemy1GO){    
            enemy1HealthBar = Instantiate(HealthBar);
            enemy1HealthBarSetup = enemy1HealthBar.GetComponent<HealthBarSetup>();
            enemy1HealthBarSetup.targetGO = enemy1GO;
            enemy1HealthBar.transform.SetParent(gameObject.transform);
        }
        if(enemy2GO){ 
            enemy2HealthBar = Instantiate(HealthBar);
            enemy2HealthBarSetup = enemy2HealthBar.GetComponent<HealthBarSetup>();
            enemy2HealthBarSetup.targetGO = enemy2GO;
            enemy2HealthBar.transform.SetParent(gameObject.transform);
        }
        if(enemy3GO){ 
            enemy3HealthBar = Instantiate(HealthBar);
            enemy3HealthBarSetup = enemy3HealthBar.GetComponent<HealthBarSetup>();
            enemy3HealthBarSetup.targetGO = enemy3GO;
            enemy3HealthBar.transform.SetParent(gameObject.transform);
        }
        if(enemy4GO){ 
            enemy4HealthBar = Instantiate(HealthBar);
            enemy4HealthBarSetup = enemy4HealthBar.GetComponent<HealthBarSetup>();
            enemy4HealthBarSetup.targetGO = enemy4GO;
            enemy4HealthBar.transform.SetParent(gameObject.transform);
        }
        if(enemy5GO){ 
            enemy5HealthBar = Instantiate(HealthBar);
            enemy5HealthBarSetup = enemy5HealthBar.GetComponent<HealthBarSetup>();
            enemy5HealthBarSetup.targetGO = enemy5GO;
            enemy5HealthBar.transform.SetParent(gameObject.transform);
        }
    }

    void hoverIndicator()
    {

    }
}
