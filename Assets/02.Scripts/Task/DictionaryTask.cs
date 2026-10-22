using System;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryTask : MonoBehaviour
{
    //지금까지 배웠던 배열이나 List는 인덱스를 이용해 요소에 접근한다
    // 딕셔너리는 요소를 저장할떄 키와 값의 쌍으로 저장
    // 해쉬테이블은 키와 값을 쌍으로 관리
    
    
    private void Start()
    {
        Dictionary<string,int> students=new Dictionary<string, int>(){{"오나현",13}};
        students.Add("최재원", 13);
        students["안녕"] = 15;
    }
}
