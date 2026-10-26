using System;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField]private Camera _miniMapcCamera;
    
   [SerializeField]private Transform _target;
   [SerializeField] private float _height = 8f;
   [SerializeField] private float _size = 5f;



   public void ZoomIn()
   {
       _size += 1;
       RefreshCamera();

   }

   public void ZoomOut()
   {
       _size -= 1;
       RefreshCamera();

   }
   private void RefreshCamera()
   {
       _miniMapcCamera.orthographicSize = _size;
   }
   
   private void LateUpdate()
   {
       //1. 대상의 머리위로 위치를 옮긴다
       transform.position = _target.position+Vector3.up*_height;
       transform.eulerAngles = new Vector3(90, _target.eulerAngles.y, 0);
   }
}
