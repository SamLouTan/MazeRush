using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _frontWall;
    [SerializeField] private GameObject _backWall;

    [SerializeField] private GameObject _unvisitedBlock;

    public bool IsVisited { get; set; }

   public void Visit()
    {
        IsVisited = true;

        // Ensure SetActive is called from the main thread
        
            _unvisitedBlock.SetActive(false);
      
    }

    public void ClearLeftWall()
    {
       
            _leftWall.SetActive(false);
 
    }

    public void ClearRightWall()
    {
       
            _rightWall.SetActive(false);
       
    }

    public void ClearFrontWall()
    {
        
            _frontWall.SetActive(false);
        
    }

    public void ClearBackWall()
    {
       
            _backWall.SetActive(false);
     
    }
   
}