using UnityEngine;

/// <summary>
/// 行为1接口
/// </summary>
public interface QuackBehavior{
    void Quack();
}
/// <summary>
/// 行为2接口
/// </summary>
public interface FlyBehavior{
    void Fly();
}
public class FlyBehavior1 : FlyBehavior{
    public void Fly() { Debug.Log("Flyyyyyyyyyyy"); }
}
public class QuackBehavior1 : QuackBehavior{
    public void Quack() { Debug.Log("Quackkkkkkk"); }
}




public class Test01 : MonoBehaviour
{

    private void Start()
    {
        CreateNewDuck();
    }

    public void CreateNewDuck(){
        Duck testDuck = new TestDuck1();
        testDuck.FlyEvent();
        testDuck.QuackEvent();
    }
}

/// <summary>
/// 抽象基类
/// </summary>
public abstract class Duck{
    protected FlyBehavior flyBehavior;
    protected QuackBehavior quackBehavior;
    public Duck(){
    }
    public void FlyEvent(){
        flyBehavior.Fly();
    }
    public void QuackEvent(){
        quackBehavior.Quack();
    }

    public void SetFlyBehavior(FlyBehavior _flyBehavior){
        flyBehavior = _flyBehavior;
    }
    public void SetQuackBehavior(QuackBehavior _quackBehavior){
        quackBehavior = _quackBehavior;
    }
}

public class TestDuck1 : Duck{
    public TestDuck1(){
        flyBehavior = new FlyBehavior1();
        quackBehavior = new QuackBehavior1();
    }
}







