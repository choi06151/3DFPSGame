using UnityEngine;

using System.Collections.Generic;
public class ListTask : MonoBehaviour
{
    //자료구조 
    //데이터를 관리하기 위해 저장하는 방식
    //C-Creat
    //R-Read
    //U-Update
    //D-Delete
    //S-Search
    
    //자료구조
    //선형 구조와 비선형 구조로 분리
    
    //선형 구조 : 배열 ,리스트 , 큐, 스택
    //비선형 구조: 트리, 그래프 ,해쉬테이블

    private void Start()
    {
        int[] ages = new int[] { 22, 21, 20, 35 }; // 배열 선언 과 초기화 
        //1. 배열은 선언할때 지정한 배열의 크기를 변경할 수없다
        ages[0]=17;
        ages[1]=17;
        ages[2]=17;
        ages[3]=17;
        //ages[4]=17; // outofrange 발생
        // 이러한 단점 보완을 위해 자료구조는 동적배열 , 연결 리스트를 사용
        //c#은 콜렉션 네임스페이스에 LIST로 구현
        //콜렉션이란? 컴퓨터 공학의 자료구조를 C#에서 구조화하여 클래스로 구현한것들
        //List->배열과 똑같은데 크기에 따라 동적으로 증감하는 배열
        //using System.Collections.Generic;

        List<int> age2 = new List<int>() { 1, 2, 34, 5 };
        age2[0]=17;
        age2[1]=17;
        age2[2]=17;
        age2[3]=17;
        age2.Add(24);
        age2.Insert(1,21);

        for (int i = 0; i < age2.Count; i++)
        {
            Debug.Log(age2[i]); //인덱스를 통한 조회
        }

        foreach (int i in age2)
        {
            Debug.Log(i); 
        }
        
    }
}
