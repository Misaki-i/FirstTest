using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主题基类
/// </summary>
public interface Subject{
    void RegisterObserver(Observer o);              //添加观察者
    void RemoveObserver(Observer o);                //移除观察者
    void NotifyObserver();                          //统一收到通知
}
/// <summary>
/// 观察者基类
/// </summary>
public interface Observer{
    void update(float temp, float hum, float pressure);     //观察者更新
}
public interface DisplayElement{
    void display();                                         //观察者公布方法
}

/// <summary>
/// 主题具体类
/// </summary>
public class WeatherData : Subject{
    private ArrayList observers;
    float temperature;
    float humidity;
    float pressure;
    public WeatherData(){
        observers = new ArrayList();
    }
    public void RegisterObserver(Observer o){
        observers.Add(o);
    }

    public void RemoveObserver(Observer o){
       int i = observers.IndexOf(o);
       if(i>=0){
           observers.RemoveAt(i);
       }
    }

    public void NotifyObserver()
    {
        for (int i = 0; i < observers.Count; i++){
            Observer observer = (Observer)observers[i];
            observer.update(temperature, humidity, pressure);
        }
    }
    public void SetMeasureData(float temp, float hum, float press){
        temperature = temp;
        humidity = hum;
        press = pressure;
        NotifyObserver();
    }
}

/// <summary>
/// 观察者具体类
/// </summary>
public class CurrentConditionDisplay : Observer, DisplayElement{
    float temp;
    float hum;
    Subject weatherData;

    public CurrentConditionDisplay(Subject weatherData){
        this.weatherData = weatherData;
        weatherData.RegisterObserver(this);
    }
    public void display(){
        Debug.Log("嘤嘤嘤===》" + temp + "===》" + hum);
    }
    public void update(float temp, float hum, float pressure){
        this.temp = temp;
        this.hum = hum;
        display();
    }
}




public class Test2 : MonoBehaviour
{
    public void Test(){
        Subject weatherData = new WeatherData();
        Observer currentConditionDisplay = new CurrentConditionDisplay(weatherData);
        weatherData.RegisterObserver(currentConditionDisplay);
        weatherData.NotifyObserver();               //发布....
    }



    //=====》Unity 内置委托使用
    public delegate void OnCallBackEnter();
    public OnCallBackEnter callBackDelegate;

    public void AddCallBack(){
        Debug.Log("添加委托！！");
        callBackDelegate += TestCallBack1;
        callBackDelegate += TestCallBack2;
        callBackDelegate += TestCallBack3;
    }
    public void RemoveCallBack(){
        callBackDelegate -= TestCallBack1;
        callBackDelegate -= TestCallBack2;
        callBackDelegate -= TestCallBack3;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.P)){
            //获得委托列表
            Delegate[] delArr = callBackDelegate.GetInvocationList();
            for (int i = 0; i < delArr.Length; i++)
            {
                delArr[i].DynamicInvoke();
            }
            Debug.Log("获得委托列表");
        }
        if(Input.GetKeyDown(KeyCode.O)){
            //清空委托的
            Delegate[] delArr = callBackDelegate.GetInvocationList();
            for (int i = 0; i < delArr.Length; i++)
            {
                callBackDelegate -= delArr[i] as OnCallBackEnter;
                Debug.Log("??");
            }
             Debug.Log("委托清空");
        }

        if(Input.GetKeyDown(KeyCode.K)){
            AddCallBack();
        }

        if(Input.GetKeyDown(KeyCode.L)){
            callBackDelegate();
        }
    }
    public void TestCallBack1() { 
        Debug.Log("1111111111");
    }
    public void TestCallBack2() { 
        Debug.Log("2222222222");
    }
    public void TestCallBack3() { 
        Debug.Log("33333333333");
    }


    
}


