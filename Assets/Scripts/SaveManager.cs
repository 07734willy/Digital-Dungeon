using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveManager : MonoBehaviour {

    public List<GameObject> prefabs;
    protected GameManager gameManager;

    public void Start() {
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void SaveData() {
        PlayerPrefs.DeleteAll();
        SaveInventory();
        // Probably save difficulty here as well
        PlayerPrefs.Save();
    }

    public void LoadData() {
        LoadInventory();
        // Also load difficulty
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
        }
    }

    public void SaveInventory() {
        Pickup[] inventory = this.gameManager.GetPlayer().inventory;
        List<int> indices = new List<int>();
        foreach (Pickup pickup in inventory) {
            int index = prefabs.IndexOf((GameObject)PrefabUtility.GetCorrespondingObjectFromSource(pickup.gameObject));
            Debug.Assert(index >= 0);
            if (index >= 0) {
                indices.Add(index);
            }
        }
        SaveIntList(indices, "inventory");
    }

    public void SaveIntList(List<int> list, string name) {
        int i;
        for (i = 0; i < list.Count; i++) {
            PlayerPrefs.SetInt("_ilist:" + name + i.ToString(), list[i]);
            Debug.Log("saving to _ilist:" + name + i.ToString());
            Debug.Log("Value: " + list[i].ToString());
        }
    }

    public List<int> LoadIntList(string name) {
        List<int> list = new List<int>();
        int i = 0;
        string key = "_ilist:" + name + i.ToString();
        while (PlayerPrefs.HasKey(key)) {
            list.Add(PlayerPrefs.GetInt(key));
            Debug.Log("loading from _ilist:" + name + i.ToString());
            Debug.Log("Value: " + list[i].ToString());
            i++;
            key = "_ilist:" + name + i.ToString();
        }
        return list;
    }
}
