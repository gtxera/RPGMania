using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState{ START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public GameObject player3Prefab;

    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;
    public GameObject enemy4Prefab;
    public GameObject enemy5Prefab;

    public Transform player1BattleStation;
    public Transform player2BattleStation;
    public Transform player3BattleStation;

    public Transform enemy1BattleStation;
    public Transform enemy2BattleStation;
    public Transform enemy3BattleStation;
    public Transform enemy4BattleStation;
    public Transform enemy5BattleStation;

    public GameObject BattleHUD;
    public GameObject BattleHUDObj;
    public BattleHUDSetup BattleHUDSetup;

    public GameObject attackButtonGO;
    public Button attackButton;
    public bool attackButtonClicked = false;
    private GameObject attackToggleGO;
    private Toggle attackToggle;
    public GameObject specialButtonGO;
    public Button specialButton;
    public bool specialButtonClicked = false;

    public int roundSize;
    public int currentTurn = 0;
    public bool turnOver;
    public GameObject currentGO;
    public GameObject currentIndicator;
    public Unit currentUnit;
    public BattleState state;
    

    Unit player1Unit;
    Unit player2Unit;
    Unit player3Unit;

    Unit enemy1Unit;
    Unit enemy2Unit;
    Unit enemy3Unit;
    Unit enemy4Unit;
    Unit enemy5Unit;

    public List<GameObject> unitsList = new List<GameObject>();
    public List<GameObject> unitsOrder = new List<GameObject>();
    public List<BattleState> turnsList = new List<BattleState>();
    public BattleState[] turnsOrder;  

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
        UpdateBattleState();

    }

    void Update()
    {
    }

    void SetupBattle()
    {
        
        BattleHUDObj = Instantiate(BattleHUD);
        BattleHUDSetup = BattleHUDObj.GetComponent<BattleHUDSetup>();
        attackButton = attackButtonGO.GetComponent<Button>();
        attackButton.onClick.AddListener(PlayerAttack);
        specialButton = specialButtonGO.GetComponent<Button>();
        specialButton.onClick.AddListener(PlayerSpecial);

        GameObject combatCarryover = GameObject.Find("CombatCarryover(Clone)");
        GoToCombat combatCarryoverPrefabs = combatCarryover.GetComponent<GoToCombat>();

        player1Prefab = combatCarryoverPrefabs.player1;
        player2Prefab = combatCarryoverPrefabs.player2;
        player3Prefab = combatCarryoverPrefabs.player3;
        
        enemy1Prefab = combatCarryoverPrefabs.enemy1;
        enemy2Prefab = combatCarryoverPrefabs.enemy2;
        enemy3Prefab = combatCarryoverPrefabs.enemy3;
        enemy4Prefab = combatCarryoverPrefabs.enemy4;
        enemy5Prefab = combatCarryoverPrefabs.enemy5;

        Destroy(combatCarryover);

        if(player1Prefab){
            GameObject player1GO = Instantiate(player1Prefab, player1BattleStation);
            unitsList.Add(player1GO);
            turnsList.Add(BattleState.PLAYERTURN);
            player1Unit = player1GO.GetComponent<Unit>();
            BattleHUDSetup.player1GO = player1GO;
        }
        if(player2Prefab){
            GameObject player2GO = Instantiate(player2Prefab, player2BattleStation);
            unitsList.Add(player2GO);
            turnsList.Add(BattleState.PLAYERTURN);
            player2Unit = player2GO.GetComponent<Unit>();
            BattleHUDSetup.player2GO = player2GO;
        }
        if(player3Prefab){
            GameObject player3GO = Instantiate(player3Prefab, player3BattleStation);
            unitsList.Add(player3GO);
            turnsList.Add(BattleState.PLAYERTURN);
            player3Unit = player3GO.GetComponent<Unit>();
            BattleHUDSetup.player3GO = player3GO;
        }

        if(enemy1Prefab){
            GameObject enemy1GO = Instantiate(enemy1Prefab, enemy1BattleStation);
            unitsList.Add(enemy1GO);
            turnsList.Add(BattleState.ENEMYTURN);
            enemy1Unit = enemy1GO.GetComponent<Unit>();
            BattleHUDSetup.enemy1GO = enemy1GO;
        }
        if(enemy2Prefab){
            GameObject enemy2GO = Instantiate(enemy2Prefab, enemy2BattleStation);
            unitsList.Add(enemy2GO);
            turnsList.Add(BattleState.ENEMYTURN);
            enemy2Unit = enemy2GO.GetComponent<Unit>();
            BattleHUDSetup.enemy2GO = enemy2GO;
        }
        if(enemy3Prefab){
            GameObject enemy3GO = Instantiate(enemy3Prefab, enemy3BattleStation);
            unitsList.Add(enemy3GO);
            turnsList.Add(BattleState.ENEMYTURN);
            enemy3Unit = enemy3GO.GetComponent<Unit>();
            BattleHUDSetup.enemy3GO = enemy3GO;
        }
        if(enemy4Prefab){
            GameObject enemy4GO = Instantiate(enemy4Prefab, enemy4BattleStation);
            unitsList.Add(enemy4GO);
            turnsList.Add(BattleState.ENEMYTURN);
            enemy4Unit = enemy4GO.GetComponent<Unit>();
            BattleHUDSetup.enemy4GO = enemy4GO;
        }
        if(enemy5Prefab){
            GameObject enemy5GO = Instantiate(enemy5Prefab, enemy5BattleStation);
            unitsList.Add(enemy5GO);
            turnsList.Add(BattleState.ENEMYTURN);
            enemy5Unit = enemy5GO.GetComponent<Unit>();
            BattleHUDSetup.enemy5GO = enemy5GO;
        }

        roundSize = unitsList.Count();
        turnsOrder = new BattleState[roundSize];

        for(int i = 0; i < roundSize; i++){
            unitsOrder.Add(unitsList[i]);
        }

        unitsOrder = unitsOrder.OrderBy(x=> x.GetComponent<Unit>().initiative).ToList();
        unitsOrder.Reverse();

        for(int i = 0; i < roundSize; i++){
            int newPosition = unitsOrder.IndexOf(unitsList[i]);
            turnsOrder[newPosition] = turnsList[i];
        }
        
        state = turnsOrder[currentTurn];
        currentGO = unitsOrder[currentTurn];
        currentUnit = currentGO.GetComponent<Unit>();
        currentIndicator = currentGO.transform.Find("turnIndicator").gameObject;
        currentTurn = 7;
    }

    public void UpdateBattleState()
    {
        currentUnit.Cleanup();
        currentIndicator.SetActive(false);
        attackButtonClicked = false;
        specialButtonClicked = false;
        turnOver = false;
        currentTurn += 1;
        if(currentTurn > roundSize -1 ){currentTurn = 0;}
        state = turnsOrder[currentTurn];
        currentGO = unitsOrder[currentTurn];
        currentUnit = currentGO.GetComponent<Unit>();
        currentIndicator = currentGO.transform.Find("turnIndicator").gameObject;
        currentIndicator.SetActive(true);
        if(currentGO.activeInHierarchy){
            switch(state){
                case BattleState.ENEMYTURN:
                    attackButtonClicked = false;
                    specialButtonClicked = false;
                    attackButton.enabled = false;
                    specialButton.enabled = false;
                    StartCoroutine(currentUnit.Action());
                    break;
                case BattleState.PLAYERTURN:
                    attackButton.enabled = true;
                    specialButton.enabled = true;
                    break;
            }
        }
        else{
            UpdateBattleState();
        }
    }

    void PlayerAttack(){
        currentUnit.Cleanup();
        if(!attackButtonClicked){
            if(specialButtonClicked){
                StopCoroutine(currentUnit.Action());
                specialButtonClicked = false;
            }
            currentUnit.chanceToSpecial = 0;
            StartCoroutine(currentUnit.Action());
            attackButtonClicked = true;
        }
        else{
            StopCoroutine(currentUnit.Action());
            attackButtonClicked = false;
        }
    }
    void PlayerSpecial(){
        currentUnit.Cleanup();
        if(!specialButtonClicked){
            if(attackButtonClicked){
                StopCoroutine(currentUnit.Action());
                attackButtonClicked = false;
            }
            currentUnit.chanceToSpecial = 1;
            StartCoroutine(currentUnit.Action());
            specialButtonClicked = true;
        }
        else{
            StopCoroutine(currentUnit.Action());
            specialButtonClicked = false;
        }
    }
}
