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
        PlayerPrefs.SetInt("health", player.maxHealth);
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
        LoadEquipped();

        Player player = GameObject.Find("Player").GetComponent<Player>();
        player.health = PlayerPrefs.GetInt("health", player.maxHealth);
        player.maxHealth = PlayerPrefs.GetInt("health", player.maxHealth);
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
        List<string> prefab_strings = LoadStringList("inventory");
        foreach (string prefab_string in prefab_strings) {
            //GameObject go = Instantiate<GameObject>(prefabs[index]) as GameObject;
            //go = PrefabUtility.ConnectGameObjectToPrefab(go, prefabs[index]);
			Debug.Log(prefab_string);
			Debug.Log(Resources.Load(prefab_string, typeof(GameObject)));
			GameObject go = Instantiate(Resources.Load(prefab_string, typeof(GameObject))) as GameObject;
			Debug.Log(go);
			go = null;
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
			Debug.Log(go);
        }
    }

    public void SaveInventory() {
        Pickup[] inventory = this.gameManager.GetPlayer().inventory;
        List<string> prefab_strings = new List<string>();
        foreach (Pickup pickup in inventory) {
            //int index = prefabs.IndexOf((GameObject)PrefabUtility.GetCorrespondingObjectFromSource(pickup.gameObject));''
			string prefab_string = pickup.tag;
            Debug.Assert(prefab_string != "" || pickup.name == "invenDummy");
            if (prefab_string != "Pickup" && prefab_string != "Pickup") {
                prefab_strings.Add(prefab_string);
            }
        }
        SaveStringList(prefab_strings, "inventory");
    }

    public void LoadEquipped() {
        Player player = gameManager.GetPlayer();
        List<string> prefab_strings = LoadStringList("equipped");
        //List<int> indices = LoadIntList("equipped");
        foreach (string prefab_string in prefab_strings) {
            //GameObject go = Instantiate<GameObject>(prefabs[index]) as GameObject;
            //go = PrefabUtility.ConnectGameObjectToPrefab(go, prefabs[index]);
			GameObject go = Instantiate(Resources.Load(prefab_string, typeof(GameObject))) as GameObject;
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
        List<string> prefab_strings = new List<string>();
        foreach (Pickup pickup in equipped) {
            //int index = prefabs.IndexOf((GameObject)PrefabUtility.GetCorrespondingObjectFromSource(pickup.gameObject));
			string prefab_string = pickup.tag;
            /*Debug.Assert(index >= 0);
            if (index >= 0) {
                indices.Add(index);
            }*/
			Debug.Assert(prefab_string != "" || pickup.name == "invenDummy");
            if (prefab_string != "Pickup" && prefab_string != "Pickup") {
                prefab_strings.Add(prefab_string);
            }
        }
        SaveStringList(prefab_strings, "equipped");
    }

    public void SaveStringList(List<string> list, string name) {
        int i;
        for (i = 0; i < list.Count; i++) {
            PlayerPrefs.SetString("_ilist:" + name + i.ToString(), list[i]);
            //Debug.Log("saving to _ilist:" + name + i.ToString());
            //Debug.Log("Value: " + list[i].ToString());
        }
    }

    public List<string> LoadStringList(string name) {
        List<string> list = new List<string>();
        int i = 0;
        string key = "_ilist:" + name + i.ToString();
        while (PlayerPrefs.HasKey(key)) {
            list.Add(PlayerPrefs.GetString(key));
            PlayerPrefs.DeleteKey(key);
            //Debug.Log("loading from _ilist:" + name + i.ToString());
            //Debug.Log("Value: " + list[i].ToString());
            i++;
            key = "_ilist:" + name + i.ToString();
        }
        return list;
    }
}
