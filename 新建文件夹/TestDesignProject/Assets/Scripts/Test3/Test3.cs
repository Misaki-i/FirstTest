using UnityEngine;

public class Test3 : MonoBehaviour{

    //被装饰对象
    Beverage beverage = new Espresso();
    Beverage beverage2 = new HouseBlend();

    private void Start() {
        DebugBeverage();
    }
    
    public void DebugBeverage(){
        
        //装饰者包装的
        beverage2 = new Mocha(beverage2);
        beverage2 = new Mocha(beverage2);
        beverage2 = new Whip(beverage2);

        Debug.Log(beverage.GetDes() + "     "+ beverage.cost());
        Debug.Log(beverage2.GetDes() + "     "+ beverage2.cost());
    }


}
//====》基类
public abstract class Beverage{
    protected string des = "Unknow beverage";
    public virtual string GetDes() { return des; }
    public abstract double cost();
}

//====》装饰者基类
public abstract class CondimentDecorator : Beverage{
    public override string GetDes() { return des; }
}

//====》被装饰者对象1
public class Espresso : Beverage{
    public Espresso(){
        des = "Espresso";
    }
    public override double cost(){
        return 1.99f;
    }
}

//====》被装饰者对象2
public class HouseBlend : Beverage{
    public HouseBlend(){
        des = "House Blend Coffee";
    }
    public override double cost(){
        return 0.89f;
    }
}

public class Mocha : CondimentDecorator{
    Beverage beverage;
    public Mocha(Beverage beverage){
        this.beverage = beverage;
    }
    public override string GetDes(){
        return beverage.GetDes() + ",Mocha";
    }
    public override double cost(){
        return 0.2f + beverage.cost();
    }
}

public class Whip : CondimentDecorator{
    Beverage beverage;
    public Whip(Beverage beverage){
        this.beverage = beverage;
    }
    public override string GetDes(){
        return beverage.GetDes() + " ,Whip";
    }
    public override double cost(){
        return 0.8f+beverage.cost();
    }
}




