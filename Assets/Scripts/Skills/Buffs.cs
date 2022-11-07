using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Buffs
{
    [SerializeField]
    public List<Buff> buffs = new List<Buff>();

    public void Reset() {
        buffs.Clear();
    }

    public void AddBuff(Effect effect, float duration, bool infinity=false) {
        Buff buff = new Buff();

        buff.effect = effect;
        buff.duration = duration;
        buff.infinity = false;

        buffs.Add(buff);
    }

    public void AddPassive(Effect effect) {
        Buff buff = new Buff();

        buff.effect = effect;
        buff.infinity = true;

        buffs.Add(buff);
    }

    IEnumerator DecreaseDuration() {
        List<Buff> removeList = new List<Buff>();

        while (true) {
            removeList.Clear();

            for (int i=0; i<buffs.Count; i++) {
                buffs[i].duration -= Time.deltaTime;

                if (buffs[i].duration <= 0)
                    removeList.Add(buffs[i]);
            }

            foreach (var temp in removeList)
                buffs.Remove(temp);

            yield return null;
        }
    }
}
