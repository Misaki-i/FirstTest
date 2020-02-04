using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBase : MonoBehaviour
{
    [SerializeField]
    private List<BuffData> buffs = new List<BuffData>();

    public virtual void OnInit()
    {
        InitBuff();
    }

    void InitBuff()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// 执行buff
    /// </summary>
    void FixedUpdate()
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            buffs[i].OnTick(Time.deltaTime);
            if (buffs[i].IsFinsh)
            {
                buffs[i].CloseBuff();
                buffs.Remove(buffs[i]);
            }
        }
    }

    /// <summary>
    /// 添加buff
    /// </summary>
    /// <param name="buffData"></param>
    public void AddBuff(BuffData buffData)
    {
        if (!buffs.Contains(buffData))
        {
            buffs.Add(buffData);
            buffData.StartBuff();
        }
    }

    /// <summary>
    /// 移除buff
    /// </summary>
    /// <param name="buffDataID"></param>
    public void RemoveBuff(int buffDataID)
    {
        BuffData bd = GetBuff(buffDataID);
        if(bd!=null)
            bd.CloseBuff();
    }

    /// <summary>
    /// 移除buff
    /// </summary>
    /// <param name="buffData"></param>
    public void RemoveBuff(BuffData buffData)
    {
        if (buffs.Contains(buffData))
        {
            buffData.CloseBuff();
        }
    }

    /// <summary>
    /// 获取buff
    /// </summary>
    /// <param name="buffDataID"></param>
    /// <returns></returns>
    public BuffData GetBuff(int buffDataID)
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            if (buffs[i].buffDataID == buffDataID)
                return buffs[i];
        }
        return null;
    }

    /// <summary>
    /// 获取buff
    /// </summary>
    /// <param name="buffBaseID"></param>
    /// <returns></returns>
    public BuffData GetBuffByBaseID(int buffBaseID)
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            if (buffs[i].BuffID == buffBaseID)
                return buffs[i];
        }
        return null;
    }

    /// <summary>
    /// 获取buff
    /// </summary>
    /// <param name="buffType"></param>
    /// <returns></returns>
    public BuffData[] GetBuff(BuffType buffType)
    {
        List<BuffData> buffdatas = new List<BuffData>();
        for (int i = 0; i < buffs.Count; i++)
        {
            if (buffs[i].BuffType == buffType)
                buffdatas.Add(buffs[i]);
        }
        return buffdatas.ToArray();
    }

    /// <summary>
    /// 执行buff
    /// </summary>
    /// <param name="buffID"></param>
    public void DoBuff(int buffID)
    {
        // BuffMgr.Instance.DoBuff(Actor,buffID);
    }

}
