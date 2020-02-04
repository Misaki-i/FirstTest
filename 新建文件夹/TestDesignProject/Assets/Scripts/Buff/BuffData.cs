using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffData
{
    /// <summary>
    /// 缓存栈
    /// </summary>
    private static Stack<BuffData> poolCache = new Stack<BuffData>();
    /// <summary>
    /// BuffData下一个ID
    /// </summary>
    public static int buffIndex { get; private set; }
    /// <summary>
    /// ID
    /// </summary>
    public int buffDataID;
    /// <summary>
    /// 配置表ID
    /// </summary>
    public int BuffID;
    /// <summary>
    /// buff类型
    /// </summary>
    public BuffType BuffType;
    /// <summary>
    /// 叠加类型
    /// </summary>
    public BuffOverlap BuffOverlap = BuffOverlap.StackedLayer;
    /// <summary>
    /// 执行次数
    /// </summary>
    public BuffCalculateType BuffCalculate = BuffCalculateType.Loop;
    /// <summary>
    /// 关闭类型
    /// </summary>
    public BuffShutDownType BuffShutDownType = BuffShutDownType.All;
    /// <summary>
    /// 最大限制
    /// </summary>
    public int MaxLimit;
    /// <summary>
    /// 当前数据
    /// </summary>
    [SerializeField]
    private int _Limit;
    public int GetLimit { get { return _Limit; } }
    /// <summary>
    /// 执行时间
    /// </summary>
    [SerializeField]
    private float PersistentTime;
    public float GetPersistentTime { get { return PersistentTime; } }
    /// <summary>
    /// 当前时间
    /// </summary>
    [SerializeField]
    private float _CurTime;
    /// <summary>
    /// 事件参数
    /// </summary>
    public object Data;
    /// <summary>
    /// 调用频率
    /// </summary>
    public float CallFrequency { get; set; }
    /// <summary>
    /// 当前频率
    /// </summary>
    private float _curCallFrequency { get; set; }
    /// <summary>
    /// 执行次数
    /// </summary>
    [SerializeField]
    private int index = 0;
    /// <summary>
    /// 根据 CallFrequency 间隔 调用 结束时会调用一次 会传递 Data数据
    /// </summary>
    public Action<object> OnCallBackParam;
    /// <summary>
    ///   /// <summary>
    /// 根据 CallFrequency 间隔 调用 结束时会调用一次 会传递 Data数据 int 次数
    /// </summary>
    /// </summary>
    public Action<object, int> OnCallBackParamIndex;
    /// <summary>
    /// 根据 CallFrequency 间隔 调用 结束时会调用一次
    /// </summary>
    public Action OnCallBack;
    /// <summary>
    /// 根据 CallFrequency 间隔 调用 结束时会调用一次 int 次数
    /// </summary>
    public Action<int> OnCallBackIndex;

    /// <summary>
    /// 当改变时间
    /// </summary>
    public Action<BuffData> OnChagneTime;
    /// <summary>
    /// 当添加层
    /// </summary>
    public Action<BuffData> OnAddLayer;
    /// <summary>
    /// 当删除层
    /// </summary>
    public Action<BuffData> OnSubLayer;
    /// <summary>
    /// 开始调用
    /// </summary>
    public Action OnStart;
    /// <summary>
    /// 结束调用
    /// </summary>
    public Action OnFinsh;
    [SerializeField]
    private bool _isFinsh;

    /// <summary>
    /// 构造方法
    /// </summary>
    private BuffData() {
        buffDataID = buffIndex++;
        CallFrequency = 1;
        PersistentTime = 0;
    }

    private BuffData(float persistentTime,Action onCallBack)
    {

        PersistentTime = persistentTime;
        OnCallBack = onCallBack;
        buffDataID = buffIndex++;
    }

    /// <summary>
    /// 重置时间
    /// </summary>
    public void ResterTime()
    {
        _CurTime = 0;
    }

    /// <summary>
    /// 修改 时间
    /// </summary>
    /// <param name="time"></param>
    public void ChangePersistentTime(float time)
    {
        PersistentTime = time;
        if (PersistentTime >= MaxLimit)
            PersistentTime = MaxLimit;
        if (OnChagneTime != null)
            OnChagneTime(this);
    }

    /// <summary>
    /// 加一层
    /// </summary>
    public void AddLayer()
    {
        _Limit++;
        _CurTime = 0;
        if (_Limit > MaxLimit)
        {
            _Limit = MaxLimit;
            return;
        }
        if (OnAddLayer != null)
            OnAddLayer(this);
    }

    /// <summary>
    /// 减一层
    /// </summary>
    public void SubLayer()
    {
        _Limit--;
        if (OnSubLayer != null)
            OnSubLayer(this);
    }

    /// <summary>
    /// 开始Buff
    /// </summary>
    public void StartBuff()
    {
        //ChangeLimit(MaxLimit);
        _isFinsh = false;
        if (OnStart != null)
            OnStart();
    }

    /// <summary>
    /// 执行中
    /// </summary>
    /// <param name="delayTime"></param>
    public void OnTick(float delayTime)
    {
        _CurTime += delayTime;
        //判断时间是否结束
        if (_CurTime >= PersistentTime)
        {
            ///调用事件
            CallBack();
            //判断结束类型 为层方式
            if (BuffShutDownType == BuffShutDownType.Layer)
            {
                SubLayer();
                //判读层数小于1 则结束
                if (_Limit <= 0)
                {
                    _isFinsh = true;
                    return;
                }
                //重置时间
                _curCallFrequency = 0;
                _CurTime = 0;
                return;
            }
            _isFinsh = true;
            return;
        }

        //如果是按频率调用
        if (CallFrequency > 0)
        {
            _curCallFrequency += delayTime;
            if (_curCallFrequency >= CallFrequency)
            {
                _curCallFrequency = 0;
                CallBack();
            }
            return;
        }
        ///调用回调
        CallBack();
    }

    /// <summary>
    /// 获取当前执行时间
    /// </summary>
    public float GetCurTime
    {
        get { return _CurTime; }
    }
    /// <summary>
    /// 是否结束
    /// </summary>
    public bool IsFinsh
    {
        get { return _isFinsh; }
    }

    /// <summary>
    /// 调用回调
    /// </summary>
    private void CallBack()
    {
        //次数增加
        index++;
        //判断buff执行次数 
        if (BuffCalculate == BuffCalculateType.Once)
        {
            if (index > 1) { index = 2; return; }
        }

        if (OnCallBack != null)
            OnCallBack();
        if (OnCallBackIndex != null)
            OnCallBackIndex(index);
        if (OnCallBackParam != null)
            OnCallBackParam(Data);
        if (OnCallBackParamIndex != null)
            OnCallBackParamIndex(Data, index);
    }

    /// <summary>
    /// 关闭buff
    /// </summary>
    public void CloseBuff()
    {
        if (OnFinsh != null)
            OnFinsh();
        Clear();
    }

    public void Clear()
    {
        _Limit = 0;
        BuffID = -1;
        index = 0;
        PersistentTime = 0;
        _CurTime = 0;
        Data = null;
        CallFrequency = 0;
        _curCallFrequency = 0;
        OnCallBackParam = null;
        OnCallBack = null;
        OnStart = null;
        OnFinsh = null;
        _isFinsh = false;
        Push(this);
    }

    /// <summary>
    /// 创建BuffData
    /// </summary>
    /// <returns></returns>
    public static BuffData Create()
    {
        if (poolCache.Count < 1)
            return new BuffData();
        BuffData buffData = poolCache.Pop();
        return buffData;
    }

    public static BuffData Create(BuffBase buffBase,Action onCallBack)
    {
       return Create(buffBase,onCallBack,null,null);
    }

    public static BuffData Create(BuffBase buffBase, Action onCallBack,Action<BuffData> addLayerAcion,Action<BuffData> subLayerAction)
    {
        BuffData db = Pop();
        db.BuffCalculate = buffBase.BuffCalculate;
        db.BuffID = buffBase.BuffID;
        db.CallFrequency = buffBase.CallFrequency;
        db.PersistentTime = buffBase.Time;
        db.BuffOverlap = buffBase.BuffOverlap;
        db.BuffShutDownType = buffBase.BuffShutDownType;
        db.BuffType = buffBase.BuffType;
        db.MaxLimit = buffBase.MaxLimit;
        db.OnCallBack = onCallBack;
        db.OnAddLayer = addLayerAcion;
        db.OnSubLayer = subLayerAction;
        db._Limit = 1;
        return db;
    }

    /// <summary>
    /// 弹出
    /// </summary>
    /// <returns></returns>
    private static BuffData Pop()
    {
        if (poolCache.Count < 1)
        {
            BuffData bd = new BuffData();
            return bd;
        }
        BuffData buffData = poolCache.Pop();
        return buffData;
    }

    /// <summary>
    /// 压入
    /// </summary>
    /// <param name="buffData"></param>
    private static void Push(BuffData buffData)
    {
        poolCache.Push(buffData);
    }
}
