using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterDatabase
{
    private Monster[] monsterList;

    public MonsterDatabase() {
        monsterList = Resources.LoadAll<Monster>("Monsters");
    }

    //실제 게임에서 쓰이는 CharacterData로 변경
    public void LoadFieldMonsterList(List<CharacterData> datas, string field) {
        LoadFieldMonsterList(datas, new string[] {field});
    }

    //실제 게임에서 쓰이는 CharacterData로 변경
    public void LoadFieldMonsterList(List<CharacterData> datas, string[] field) {
        List<Monster> temp = new List<Monster>();

        for (int i=0; i<monsterList.Length; i++) {
            foreach (string f1 in field) {
                if (Array.Find(monsterList[i].FieldList, x => x.Equals(f1)) != null) {
                    temp.Add(monsterList[i]);
                    break;
                }
            }
        }

        datas.Clear();
        for (int i=0; i<temp.Count; i++) {
            CharacterData characterData = new CharacterData();

            characterData.Save(temp[i]);
            datas.Add(characterData);
        }
    }

    public void LoadFieldMonsterList(List<CharacterData> datas) {
        datas.Clear();
        for (int i=0; i<monsterList.Length; i++) {
            CharacterData characterData = new CharacterData();

            characterData.Save(monsterList[i]);
            datas.Add(characterData);
        }
    }
}
