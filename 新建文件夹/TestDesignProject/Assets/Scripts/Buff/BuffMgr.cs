using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMgr : MonoBehaviour
{
    private static BuffMgr _instance;
    public static BuffMgr Instance
    {
        get { return _instance; }
    }

    public List<BuffBase> buffBase = new List<BuffBase>();


    #region GUI
    [SerializeField]
    private Actor TestActor;
    #endregion

    void Awake()
    {
        _instance = this;

        // GUITools.ResterWindowAction(new Rect(300, 0, 220, 120), delegate(GUIAction action)
        // {
        //     action.Rect = GUI.Window(action.Id, action.Rect, delegate
        //     {
        //         action.Param[0] = GUI.TextField(new Rect(10, 20, 200, 20), action.Param[0]);
        //         action.Param[1] = GUI.TextField(new Rect(10, 45, 200, 20), action.Param[1]);
        //         if (GUI.Button(new Rect(10, 70, 200, 30), "AddBuff"))
        //         {
        //             if (TestActor == null || !TestActor.gameObject.Equals(action.Param[1]))
        //             {
        //                 GameObject obj = GameObject.Find(action.Param[1]);
        //                 if (obj != null)
        //                     TestActor = obj.GetComponent<Actor>();
        //             }
        //             if (TestActor != null)
        //                 DoBuff(TestActor, int.Parse(action.Param[0]));
        //             else
        //                 GUI.Label(new Rect(10, 105, 200, 10), "测试Actor 为 null ，请检查Actor Name是否正确！");
        //         }
        //         GUI.DragWindow();
        //     }, action.Id+ " - Buff Test|BuffManager");

        // },"0","Player");
    }

    /// <summary>
    /// 执行buff
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="buffID"></param>
    public void DoBuff(Actor actor,int buffID)
    {
        DoBuff(actor,GetBuffBase(buffID));
    }

    /// <summary>
    /// 执行buff
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="buff"></param>
    public void DoBuff(Actor actor, BuffBase buff)
    {
        if (buff == null) return;
        BuffData db = null;

        switch (buff.BuffType)
        {
            case BuffType.AddHp: //增加血量
                if (!IsAdd(actor, buff))
                {
                    db = BuffData.Create(buff, delegate
                    {
                        // actor.ActorAttr.AddHP((int)buff.Num);
                    });

                }
                break;
            case BuffType.AddMaxHp: //增加最大血量
                if (!IsAdd(actor, buff))
                {
                    db = BuffData.Create(buff, delegate
                    {
                        // actor.ActorAttr.AddMaxHP((int)buff.Num);
                    }, delegate {
                        // actor.ActorAttr.AddMaxHP((int)buff.Num);
                    }, delegate {
                        // actor.ActorAttr.SubMaxHp((int)buff.Num);
                    });
                }
                break;
            case BuffType.SubHp: //减少血量
                if (!IsAdd(actor, buff))
                { 
                    db = BuffData.Create(buff, delegate
                    {
                        // actor.ActorAttr.SubHp((int)buff.Num);
                    });
                }
                break;
            case BuffType.SubMaxHp: //减少最大血量
                if (!IsAdd(actor, buff))
                {
                    db = BuffData.Create(buff, delegate
                    {
                        // actor.ActorAttr.SubMaxHp((int)buff.Num);
                    }, delegate
                    {
                        // actor.ActorAttr.SubMaxHp((int)buff.Num);
                    }, delegate
                    {
                        // actor.ActorAttr.AddMaxHP((int)buff.Num);
                    });
                }
                break;
            case BuffType.AddDamageFloated: //浮空
                if (!IsAdd(actor, buff))
                {
                    db = BuffData.Create(buff, delegate
                    {
                        // if (actor.ActorState != ActorState.DamageRise)
                        //     actor.ActorAttr.DamageRiseAbility = buff.Num;
                        // actor.SetDamageRiseState();
                    });
                }
                break;
            case BuffType.AddFloated:
                if (!IsAdd(actor, buff))
                {
                    db = BuffData.Create(buff, delegate
                    {
                        Vector3 moveDir = Vector3.up;
                        moveDir *= buff.Num;
                        // actor.CharacterController.Move(moveDir*Time.deltaTime);
                    });
                }
                 break;
            case BuffType.AddSprint:
                if (!IsAdd(actor,buff))
                {
                    db = BuffData.Create(buff, delegate {
                        Vector3 moveDir = actor.transform.forward;
                        moveDir *= buff.Num;
                        moveDir.y += -20;
                        // actor.CharacterController.Move(moveDir*Time.deltaTime);
                        //actor.Translate(Vector3.forward * buff.Num * Time.deltaTime);
                    });
                }
                break;
            case BuffType.AddIsIgnoreGravity:
                if (!IsAdd(actor, buff))
                {
                    db = BuffData.Create(buff, null);
                    // db.OnStart = delegate { actor.ActorPhysical.IsIgnoreGravity = true; };
                    db.OnFinsh = delegate
                    {
                        // actor.ActorPhysical.IsIgnoreGravity = false;
                    };
                }
                break;
        }
        if (db != null){}
            // actor.ActorBuff.AddBuff(db);
    }

    /// <summary>
    /// 玩家是否已经有此buff
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="buff"></param>
    /// <returns></returns>
    private bool IsAdd(Actor actor,BuffBase buff)
    {
        // BuffData oldBuff = actor.ActorBuff.GetBuffByBaseID(buff.BuffID);
        //  =====  》角色的buff
        BuffData oldBuff = null;

        if (oldBuff != null)
        {
            switch (buff.BuffOverlap)
            {
                case BuffOverlap.ResterTime:
                    oldBuff.ResterTime();
                    break;
                case BuffOverlap.StackedLayer:
                    oldBuff.AddLayer();
                    break;
                case BuffOverlap.StackedTime:
                    oldBuff.ChangePersistentTime(oldBuff.GetPersistentTime + buff.Time);
                    break;
                default:
                    break;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取配置数据
    /// </summary>
    /// <param name="buffID"></param>
    /// <returns></returns>
    public BuffBase GetBuffBase(int buffID)
    {
        for (int i = 0; i < buffBase.Count; i++)
        {
            if (buffBase[i].BuffID == buffID)
                return buffBase[i];
        }
        return null;
    }

}
