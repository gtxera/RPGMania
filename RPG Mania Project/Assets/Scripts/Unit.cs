using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int initiative;

    public int maxHP;
    public int currentHP;

    public int damage;
    public int chanceToHit;    
    public int chanceToSpecial;

    static System.Random random = new System.Random();
    public GameObject battleController;
    private BattleSystem battleSystem;

    private int currentRnd;
    private new string tag;
    
    private GameObject[] players;
    private GameObject[] enemys;
    private GameObject target;
    private GameObject targetSelectable;
    private Unit targetUnit;
    private bool isSelectingTarget = false;
    private bool hasSelectedTarget;

    private Camera mainCam {get {return Camera.main.GetComponent<Camera>();}}
    private Ray ray;
    private RaycastHit2D hover;
    private GameObject targetHover;

    public UnityEvent specialAction; 
    
    public IEnumerator Action()
    {
        battleController = GameObject.Find("BattleController");
        battleSystem = battleController.GetComponent<BattleSystem>();
        tag = gameObject.tag;

        if(tag == "Enemy"){
            players = GameObject.FindGameObjectsWithTag("Player");

            currentRnd = random.Next(players.Length);
            target = players[currentRnd];
            targetSelectable = target.transform.Find("selectableIndicator").gameObject;
            targetSelectable.SetActive(true);
            yield return new WaitForSeconds(2f);
            targetUnit = target.GetComponent<Unit>();
            currentRnd = random.Next(100);
            if(currentRnd < chanceToSpecial){
                specialAction.Invoke();
                battleSystem.turnOver = true;
                battleSystem.UpdateBattleState();
                yield return true;
            }
            else{
                Attack();
                Debug.Log("inimigo atacando");
                targetSelectable.SetActive(false);
                battleSystem.turnOver = true;
                battleSystem.UpdateBattleState();
                yield return true;
            }
        }
        else{
            enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemys){
            GameObject enemySelectable = enemy.transform.Find("selectableIndicator").gameObject;
                enemySelectable.SetActive(true);
            }
            if(isSelectingTarget == false){isSelectingTarget = true;}
            yield return new WaitUntil(() => hasSelectedTarget);
            yield return new WaitForSeconds(2);

            switch(chanceToSpecial){
                case 0:
                    Attack();
                break;

                case 1:
                    specialAction.Invoke();
                break;
            }

            targetSelectable.SetActive(false);
            targetHover.SetActive(false);
            targetSelectable.transform.parent.gameObject.GetComponent<MouseExitDeactivate>().alwaysActive = false;
            battleSystem.turnOver = true;
            battleSystem.UpdateBattleState();
            hasSelectedTarget = false;
            yield return true; 
        } 
    }


    public void Attack()
    {
        currentRnd = random.Next(100);
        if(currentRnd < chanceToHit){
            targetUnit.currentHP -= damage;
        }
        else{
            Debug.Log("ERROU");
        }
    }

    void Update()
    {
        if(isSelectingTarget){
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            hover = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if(hover){
                if(hover.collider.gameObject.tag == "Enemy"){
                    targetHover = hover.transform.Find("hoverIndicator").gameObject;
                    targetHover.SetActive(true);
                    if(Input.GetMouseButtonDown(0)){
                        Debug.Log("atacou");
                        target = hover.collider.gameObject;
                        targetUnit = target.GetComponent<Unit>();
                        foreach (GameObject enemy in enemys){
                            GameObject enemySelectable = enemy.transform.Find("selectableIndicator").gameObject;
                            enemySelectable.SetActive(false);
                        }
                        targetSelectable = target.transform.Find("selectableIndicator").gameObject;
                        targetHover = target.transform.Find("hoverIndicator").gameObject;
                        targetSelectable.SetActive(true);
                        targetSelectable.transform.parent.gameObject.GetComponent<MouseExitDeactivate>().alwaysActive = true;
                        isSelectingTarget = false;
                        hasSelectedTarget = true;
                    }
                }
            }
        }
    }

    public void Cleanup()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys){
            GameObject enemySelectable = enemy.transform.Find("selectableIndicator").gameObject;
            enemySelectable.SetActive(false);
        }
        isSelectingTarget = false;
    }

    public void hello()
    {
        Debug.Log("hello");
    }

    
}
