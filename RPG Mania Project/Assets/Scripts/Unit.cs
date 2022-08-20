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
    private int currentRnd;
    public GameObject battleController;
    private BattleSystem battleSystem;

    private new string tag;
    
    private GameObject[] players;
    private GameObject[] enemys;
    private GameObject target;
    private GameObject targetSelectable;
    private Unit targetUnit;
    private bool isSelectingTarget = false;

    private Camera mainCam {get {return Camera.main.GetComponent<Camera>();}}
    private Ray ray;
    private RaycastHit2D hover;
    private GameObject targetHover;

    public UnityEvent specialAction;
    private bool specialFinished;
    
    public IEnumerator Action()
    {
        battleController = GameObject.Find("BattleController");
        battleSystem = battleController.GetComponent<BattleSystem>();
        tag = gameObject.tag;
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        players = GameObject.FindGameObjectsWithTag("Player");

        if(tag == "Enemy"){

            currentRnd = random.Next(100);
            if(currentRnd < chanceToSpecial){
                specialAction.Invoke();
                battleSystem.turnOver = true;
                battleSystem.UpdateBattleState();
                yield return true;
            }
            else{
                target = randomSelectTarget(true);
                targetUnit = target.GetComponent<Unit>();
                targetSelectable = target.transform.Find("selectableIndicator").gameObject;
                targetSelectable.SetActive(true);
                yield return new WaitForSeconds(2f);
                Attack();
                Debug.Log("inimigo atacando");
                targetSelectable.SetActive(false);
                battleSystem.turnOver = true;
                battleSystem.UpdateBattleState();
                yield return true;
            }
        }
        else{

            switch(chanceToSpecial){
                case 0:
                    yield return StartCoroutine(playerSelectTargetRoutine(false));
                    yield return new WaitForSeconds(2);
                    Attack();
                break;

                case 1:
                    specialAction.Invoke();
                    yield return new WaitUntil(() => specialFinished);
                break;
            }

            targetSelectable.transform.parent.gameObject.GetComponent<MouseExitDeactivate>().alwaysActive = false;
            battleSystem.turnOver = true;
            battleSystem.UpdateBattleState();
            yield return true; 
        } 
    }


    public void Attack()
    {    
        targetUnit = target.GetComponent<Unit>();
        currentRnd = random.Next(100);
        if(currentRnd < chanceToHit){
            targetUnit.currentHP -= damage;
            targetUnit.checkForDeath();
        }
        else{
            Debug.Log("ERROU");
        }
    }

    public void checkForDeath(){
        if(currentHP <= 0){
            gameObject.SetActive(false);
        } 
    }

    public void Cleanup()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys){
            GameObject enemySelectable = enemy.transform.Find("selectableIndicator").gameObject;
            GameObject enemyHover = enemy.transform.Find("hoverIndicator").gameObject;
            enemySelectable.SetActive(false);
            enemyHover.SetActive(false);
        }
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players){
            GameObject playerSelectable = player.transform.Find("selectableIndicator").gameObject;
            GameObject playerHover = player.transform.Find("hoverIndicator").gameObject;
            playerSelectable.SetActive(false);
            playerHover.SetActive(false);
        }
        isSelectingTarget = false;
        specialFinished = false;
    }

    public GameObject randomSelectTarget(bool selectPlayer)
    {
        GameObject cur_target;

        if(selectPlayer){
            currentRnd = random.Next(players.Length);
            cur_target = players[currentRnd];
        }
        else{
            currentRnd = random.Next(enemys.Length);
            cur_target = enemys[currentRnd];
        }

        return cur_target;
    }

    public GameObject manualSelectTarget(bool selectPlayer)
    {
        GameObject cur_target;

        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        players = GameObject.FindGameObjectsWithTag("Player");

        ray = mainCam.ScreenPointToRay(Input.mousePosition);
        hover = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        
        if(hover){
            if(selectPlayer){
                if(hover.collider.gameObject.tag == "Player"){
                    targetHover = hover.transform.Find("hoverIndicator").gameObject;
                    targetHover.SetActive(true);
                    if(Input.GetMouseButtonDown(0)){
                        cur_target = hover.collider.gameObject;
                        foreach (GameObject player in players){
                            GameObject playerSelectable = player.transform.Find("selectableIndicator").gameObject;
                            playerSelectable.SetActive(false);
                        }
                        targetSelectable = cur_target.transform.Find("selectableIndicator").gameObject;
                        targetSelectable.SetActive(true);
                        targetSelectable.transform.parent.gameObject.GetComponent<MouseExitDeactivate>().alwaysActive = true;
                        isSelectingTarget = false;
                        return cur_target;
                    }
                }
            }
            else{
                if(hover.collider.gameObject.tag == "Enemy"){
                    targetHover = hover.transform.Find("hoverIndicator").gameObject;
                    targetHover.SetActive(true);
                    if(Input.GetMouseButtonDown(0)){
                        cur_target = hover.collider.gameObject;
                        foreach (GameObject enemy in enemys){
                            GameObject enemySelectable = enemy.transform.Find("selectableIndicator").gameObject;
                            enemySelectable.SetActive(false);
                        }
                        targetSelectable = cur_target.transform.Find("selectableIndicator").gameObject;
                        targetSelectable.SetActive(true);
                        targetSelectable.transform.parent.gameObject.GetComponent<MouseExitDeactivate>().alwaysActive = true;
                        isSelectingTarget = false;
                        return cur_target;
                    }
                }

            }
        }
        return null;
    }

    IEnumerator playerSelectTargetRoutine(bool selectPlayer){
        if(isSelectingTarget == false){isSelectingTarget = true;}
        switch(selectPlayer){
            case true:
                foreach (GameObject player in players){
                    GameObject playerSelectable = player.transform.Find("selectableIndicator").gameObject;
                    playerSelectable.SetActive(true);
                }
            break;
            case false:
                foreach (GameObject enemy in enemys){
                    GameObject enemySelectable = enemy.transform.Find("selectableIndicator").gameObject;
                    enemySelectable.SetActive(true);
                }
            break;
        }
        while(isSelectingTarget){
            target = manualSelectTarget(selectPlayer);

            yield return null;
        }
        Debug.Log("finalizou rotina");
        yield return true;
    }

    public void hello()
    {
        Debug.Log("hello");
        specialFinished = true;
    }

    IEnumerator playerHealRoutine(){
        yield return StartCoroutine(playerSelectTargetRoutine(true));
        targetUnit = target.GetComponent<Unit>();
        yield return new WaitForSeconds(2);
        targetUnit.currentHP += 1;
        if(targetUnit.currentHP > targetUnit.maxHP){
            targetUnit.currentHP = targetUnit.maxHP;
        }
        specialFinished = true;

    }

    public void playerHeal(){
        StartCoroutine(playerHealRoutine());
    }

    
}
