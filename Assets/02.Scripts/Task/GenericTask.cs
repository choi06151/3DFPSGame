using System;
using UnityEngine;


//제네릭은 타입에 상관없는
// 이자료형을 사용할떄 자료형이 무엇인지 정의하지 않고 그때그때 사용
// Cup of Drink

public class Cup<T>
{
    public T Drink;
    
    public Cup(T Drink)
    {
        this.Drink = Drink;
    }
    
}
public class GenericTask : MonoBehaviour
{
    private void Start()
    {
        Cup<int> cup1=new Cup<int> (17);
        Cup<float> cup2=new Cup<float> (12);
    }
}
