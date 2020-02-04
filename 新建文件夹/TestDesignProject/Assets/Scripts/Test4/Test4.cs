using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test4 : MonoBehaviour
{
    
}




//-------------     工厂方法模式   ----------------


//============》例子一
public interface IWalker{
    void Walk(Transform target);
}

public class LeftWalker : MonoBehaviour, IWalker
{
    Transform targetTr;
    public void Walk(Transform target){
        targetTr = target;
        WalkLeft();
    }
    void WalkLeft() { }
}

public class RightWalker : MonoBehaviour, IWalker
{
    Transform targetTr;
    public void Walk(Transform target){
        targetTr = target;
        WalkRight();
    }
    void WalkRight() { }
}

public interface IWalkerFactory{
    IWalker Create();
}
public class LeftWalkerFactory : IWalkerFactory
{
    public IWalker Create(){
        return new GameObject().AddComponent<LeftWalker>();
    }
}
public class RightWalkerFactory : IWalkerFactory
{
    public IWalker Create(){
        return new GameObject().AddComponent<RightWalker>();
    }
}

//============》例子二

public enum PizzaType{
    pizza1,
    pizza2,
    pizza3,

    Null
}


//===>产品基类
public abstract class Pizza{
    string name;
    string dough;
    string sauce;
    ArrayList toppings = new ArrayList();
    public virtual void Prepare() { }
    public virtual void Bake() { }
    public virtual void Cut() { }
    public virtual void Box() { }
    public string getName(){
        return name;
    }
}


//===》衍生产品，可以自己重写表现
public class NY_Pizza1 : Pizza { }
public class NY_Pizza2 : Pizza { }
public class NY_Pizza3 : Pizza { }

public class Chi_Pizza1 : Pizza { }
public class Chi_Pizza2 : Pizza { }
public class Chi_Pizza3 : Pizza { }



//===》工厂基类
public abstract class PizzeFactory{
    public Pizza orderPizza(PizzaType type){
        Pizza pizza = createPizza(type);
        pizza.Prepare();
        pizza.Bake();
        pizza.Cut();
        pizza.Box();

        return pizza;
    }
    public abstract Pizza createPizza(PizzaType type);
}

//衍生工厂，根据类型生成对应披萨
public class NY_PizzeFactory : PizzeFactory
{
    public override Pizza createPizza(PizzaType type){
        Pizza pizza = null;
        switch (type)
        {
            case PizzaType.pizza1:
                pizza = new NY_Pizza1();
            break;
            case PizzaType.pizza2:
                pizza = new NY_Pizza2();
            break;
            case PizzaType.pizza3:
                pizza = new NY_Pizza3();
            break;
        }
        return pizza;
    }
}

public class Chi_PizzeFactory : PizzeFactory
{
    public override Pizza createPizza(PizzaType type){
        Pizza pizza = null;
        switch (type)
        {
            case PizzaType.pizza1:
                pizza = new Chi_Pizza1();
            break;
            case PizzaType.pizza2:
                pizza = new Chi_Pizza2();
            break;
            case PizzaType.pizza3:
                pizza = new Chi_Pizza3();
            break;
        }
        return pizza;
    }
}
public class UseFactory{
    public void UseIt(){
        NY_PizzeFactory nY_PizzeFactory = new NY_PizzeFactory();
        Chi_PizzeFactory chi_PizzeFactory = new Chi_PizzeFactory();

        Pizza NYpizza1 = nY_PizzeFactory.createPizza(PizzaType.pizza1);
        Pizza NYpizza2 = nY_PizzeFactory.createPizza(PizzaType.pizza2);
        Pizza NYpizza3 = nY_PizzeFactory.createPizza(PizzaType.pizza3);

        //etc............
    }
}


//-------------     抽象工厂模式   ----------------

namespace AbstractFactory
{

    //=======》原料接口
    public interface Dough { }
    public interface Sauce { }
    public interface Cheese { }
    public interface Veggies { }
    public interface Pepperoni { }
    public interface Clams { }

    //========》原料衍生的
    public class Dough1 : Dough { }
    public class Sauce1 : Sauce { }
    public class Cheese1 : Cheese { }
    public class Veggies1 : Veggies { }
    public class Pepperoni1 : Pepperoni { }
    public class Clams1 : Clams { }

    public class Dough2 : Dough { }
    public class Sauce2 : Sauce { }
    public class Cheese2 : Cheese { }
    public class Veggies2 : Veggies { }
    public class Pepperoni2 : Pepperoni { }
    public class Clams2 : Clams { }

    //======》披萨抽象类
    public abstract class Pizza{
        protected string name;
        protected Dough dough;
        protected Sauce sauce;
        protected Cheese cheese;
        protected Veggies veggies;
        protected Pepperoni pepperoni;
        protected Clams clams;

        public virtual void Bake() { }
        public virtual void Cut() { }
        public virtual void Box() { }

        
        public abstract void Prepare();
        public void SetName(string _name){
            name = _name;
        }
        public string GetName(){
            return name;
        }
    }
    //====》披萨衍生具体类
    public class CheesePizza : Pizza{
        PizzaIngredientFactory ingredientFactory;
        public CheesePizza(PizzaIngredientFactory _ingredientFactory){
            ingredientFactory = _ingredientFactory;
        }
        public override void Prepare(){
            dough = ingredientFactory.createDough();
            sauce = ingredientFactory.createSauce();
            cheese = ingredientFactory.createCheese();
        }
    }
    public class ClamPizza : Pizza{
        PizzaIngredientFactory ingredientFactory;
        public ClamPizza(PizzaIngredientFactory _ingredientFactory){
            ingredientFactory = _ingredientFactory;
        }
        public override void Prepare(){
            dough = ingredientFactory.createDough();
            sauce = ingredientFactory.createSauce();
            cheese = ingredientFactory.createCheese();
            clams = ingredientFactory.createClams();
        }
    }

    //=====》原料工厂接口
    public interface PizzaIngredientFactory{
        Dough createDough();
        Sauce createSauce();
        Cheese createCheese();
        Veggies[] createVeggies();
        Pepperoni createPepperoni();
        Clams createClams();
    }

    //======》原料工厂衍生具体类
    public class NY_PizzaIngredientFactory : PizzaIngredientFactory{
        public Cheese createCheese() { return new Cheese1(); }
        public Clams createClams() { return new Clams1(); }
        public Dough createDough() { return new Dough1(); }
        public Pepperoni createPepperoni() { return new Pepperoni1(); }
        public Sauce createSauce() { return new Sauce1(); }
        public Veggies[] createVeggies(){
            Veggies[] veggiess = { new Veggies1(), new Veggies1() };
            return veggiess;
        }
    }
    public class Chi_PizzaIngredientFactory : PizzaIngredientFactory{
        public Cheese createCheese() { return new Cheese2(); }
        public Clams createClams() { return new Clams2(); }
        public Dough createDough() { return new Dough2(); }
        public Pepperoni createPepperoni() { return new Pepperoni2(); }
        public Sauce createSauce() { return new Sauce2(); }
        public Veggies[] createVeggies(){
            Veggies[] veggiess = { new Veggies2(), new Veggies2() };
            return veggiess;
        }
    }
    //===》工厂基类
    public abstract class PizzeFactory{
        public Pizza orderPizza(PizzaType type){
            Pizza pizza = createPizza(type);
            pizza.Prepare();
            pizza.Bake();
            pizza.Cut();
            pizza.Box();

            return pizza;
        }
        public abstract Pizza createPizza(PizzaType type);
    }
    //衍生工厂，根据类型生成对应披萨
    public class NY_PizzeFactory : PizzeFactory
    {
        public override Pizza createPizza(PizzaType type){
            Pizza pizza = null;
            PizzaIngredientFactory ingredientFactory = new NY_PizzaIngredientFactory();
            switch (type)
            {
                case PizzaType.pizza1:
                    pizza = new CheesePizza(ingredientFactory);
                break;
                case PizzaType.pizza2:
                    // pizza = new NY_Pizza2();
                    pizza = new ClamPizza(ingredientFactory);
                break;
                case PizzaType.pizza3:
                    // pizza = new NY_Pizza3();
                break;
            }
            return pizza;
        }
    }



}



















