using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToCombat : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject enemy5;

    [SerializeField] GameObject combatCarryoverGO;
    private GameObject combatCarryover;
    private GoToCombat combatCarryoverPrefabs;

    public void CombatSetup()
    {
        SceneManager.LoadScene("SampleScene");
        combatCarryover = Instantiate(combatCarryoverGO);
        combatCarryoverPrefabs = combatCarryover.GetComponent<GoToCombat>();

        combatCarryoverPrefabs.player1 = player1;
        combatCarryoverPrefabs.player2 = player2;
        combatCarryoverPrefabs.player3 = player3;

        combatCarryoverPrefabs.enemy1 = enemy1;
        combatCarryoverPrefabs.enemy2 = enemy2;
        combatCarryoverPrefabs.enemy3 = enemy3;
        combatCarryoverPrefabs.enemy4 = enemy4;
        combatCarryoverPrefabs.enemy5 = enemy5;
    }
}
