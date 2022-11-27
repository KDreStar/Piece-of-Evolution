using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterDatabase
{
    private Monster[] monsters;

    public MonsterDatabase() {
        monsters = Resources.LoadAll<Monster>("Monsters");
    }

    //실제 게임에서 쓰이는 CharacterData로 변경
    public List<CharacterData> GetFieldMonsterList(string field) {
        return GetFieldMonsterList(new string[] {field});
    }

    //실제 게임에서 쓰이는 CharacterData로 변경
    public List<CharacterData> GetFieldMonsterList(string[] field) {
        List<Monster> temp = new List<Monster>();

        for (int i=0; i<monsters.Length; i++) {
            foreach (string f1 in field) {
                if (Array.Find(monsters[i].Fields, x => x.Equals(f1)) != null) {
                    temp.Add(monsters[i]);
                    break;
                }
            }
        }

        Debug.Log("OK?");
        List<CharacterData> result = new List<CharacterData>();
        for (int i=0; i<temp.Count; i++) {
            Debug.Log("OK");
            CharacterData characterData = new CharacterData();

            characterData.SetData(temp[i]);
            result.Add(characterData);
        }

        return result;
    }

    public List<CharacterData> GetFieldMonsterList() {
        List<CharacterData> result = new List<CharacterData>();

        for (int i=0; i<monsters.Length; i++) {
            CharacterData characterData = new CharacterData();

            characterData.SetData(monsters[i]);
            result.Add(characterData);
        }

        return result;
    }
}
