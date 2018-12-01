using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {

    public List<GameObject> prefabs;
    protected GameManager gameManager;

    public void Start() {
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void SaveData() {
        //PlayerPrefs.DeleteAll();
        SaveInventory();
        SaveEquipped();
        // Probably save difficulty here as well
        PlayerPrefs.SetString("difficulty", this.gameManager.difficulty.ToString().ToLower());

        PlayerPrefs.SetString("levelname", SceneManager.GetActiveScene().name);

        Player player = GameObject.Find("Player").GetComponent<Player>();
        PlayerPrefs.SetInt("health", player.health);
        PlayerPrefs.SetFloat("evasion", player.evasion);
        PlayerPrefs.SetInt("armor", player.armor);
        PlayerPrefs.SetInt("level", player.level);
        PlayerPrefs.SetInt("gold", player.gold);
        PlayerPrefs.SetInt("arrows", player.arrows);
        PlayerPrefs.SetInt("totalExperience", player.totalExperience);
        PlayerPrefs.SetFloat("expMilestone", (float)player.expMilestone);

        PlayerPrefs.Save();
    }

    public void LoadData() {
        LoadInventory();
        LoadEqipped();

        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.health = PlayerPrefs.GetInt("health", player.health);
        player.evasion = PlayerPrefs.GetFloat("evasion", player.evasion);
        player.armor = PlayerPrefs.GetInt("armor", player.armor);
        player.level = PlayerPrefs.GetInt("level", player.level);
        player.gold = PlayerPrefs.GetInt("gold", player.gold);
        player.arrows = PlayerPrefs.GetInt("arrows", player.arrows);
        player.totalExperience = PlayerPrefs.GetInt("totalExperience", player.totalExperience);
        player.expMilestone = PlayerPrefs.GetFloat("expMilestone", (float)player.expMilestone);
        // Also load difficulty
        this.gameManager.setDifficulty(PlayerPrefs.GetString("difficulty", "normal"));
    }

    public void LoadInventory() {
        Player player = gameManager.GetPlayer();
        List<int> indices = LoadIntList("inventory");
        foreach (int index in indices) {
            GameObject go = Instantiate<GameObject>(prefabs[index]) as GameObject;
            go = PrefabUtility.ConnectGameObjectToPrefab(go, prefabs[index]);
            Pickup pickup = go.GetComponent<Pickup>();
         
            pickup.transform.parent = GameObject.Find("InventoryInven").transform;
            pickup.GetComponent<SpriteRenderer>().enabled = false;
            pickup.SetCharacter(player);

            Transform inv = GameObject.Find("InventoryInven").transform;
            int num_children = inv.childCount;
            int i;
            for (i = 0; i < num_children; i++) {
                if (inv.GetChild(i).name == "invenDummy") {
                    DestroyImmediate(inv.GetChild(i).gameObject);
                    pickup.transform.SetSiblingIndex(i);
                    break;
                }
            }
        }
    }

    public void SaveInventory() {
        Pickup[] inventory = this.gameManager.GetPlayer().inventory;
        List<int> indices = new List<int>();
        foreach (Pickup pickup in inventory) {
            int index = prefabs.IndexOf((GameObject)PrefabUtility.GetCorrespondingObjectFromSource(pickup.gameObject));
            Debug.Assert(index >= 0 || pickup.name == "invenDummy");
            if (index >= 0) {
                indices.Add(index);
            }
        }
        SaveIntList(indices, "inventory");
    }

    public void LoadEqipped() {
        Player player = gameManager.GetPlayer();
        List<int> indices = LoadIntList("equipped");
        foreach (int index in indices) {
            GameObject go = Instantiate<GameObject>(prefabs[index]) as GameObject;
            go = PrefabUtility.ConnectGameObjectToPrefab(go, prefabs[index]);
            Pickup pickup = go.GetComponent<Pickup>();

            pickup.transform.parent = GameObject.Find("EquippedInven").transform;
            pickup.GetComponent<SpriteRenderer>().enabled = false;
            pickup.SetCharacter(player);
            Weapon weapon = pickup.gameObject.GetComponent<Weapon>();
            if (weapon.isRanged) {
                player.SetRangedWeapon(weapon);
            } else {
                player.SetMeleeWeapon(weapon);
            }
        }
    }

    public void SaveEquipped() {
        Pickup[] equipped = this.gameManager.GetPlayer().equipped;
        List<int> indices = new List<int>();
        foreach (Pickup pickup in equipped) {
            int index = prefabs.IndexOf((GameObject)PrefabUtility.GetCorrespondingObjectFromSource(pickup.gameObject));
            Debug.Assert(index >= 0);
            if (index >= 0) {
                indices.Add(index);
            }
        }
        SaveIntList(indices, "equipped");
    }

    public void SaveIntList(List<int> list, string name) {
        int i;
        for (i = 0; i < list.Count; i++) {
            PlayerPrefs.SetInt("_ilist:" + name + i.ToString(), list[i]);
            //Debug.Log("saving to _ilist:" + name + i.ToString());
            //Debug.Log("Value: " + list[i].ToString());
        }
    }

    public List<int> LoadIntList(string name) {
        List<int> list = new List<int>();
        int i = 0;
        string key = "_ilist:" + name + i.ToString();
        while (PlayerPrefs.HasKey(key)) {
            list.Add(PlayerPrefs.GetInt(key));
            PlayerPrefs.DeleteKey(key);
            //Debug.Log("loading from _ilist:" + name + i.ToString());
            //Debug.Log("Value: " + list[i].ToString());
            i++;
            key = "_ilist:" + name + i.ToString();
        }
        return list;
    }
}
