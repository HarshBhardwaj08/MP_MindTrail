using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePort : MonoBehaviour
{
    public GameObject player;
    public GameObject face2;
    public GameObject face4;
    public GameObject directionSignRight;
    public GameObject directionSignLeft; 
    public GameObject BlockPassage;
    public GameObject vTrucks;
    public Transform teleportLocation;
    [SerializeField] BoxCollider2D mainFaceboxCollider;
    private void OnEnable()
    {
        transform.position = player.transform.position;
       
    }
  
   public void TelePortPlayer()
    {
        if (vTrucks != null)
        {  
            Vtruck vtruck = vTrucks.GetComponent<Vtruck>();
            vtruck.boxCollider.isTrigger = true;
            vtruck.AllQuestionSolved = true;
            vtruck.solveQuizes.gameObject.SetActive(false);
            mainFaceboxCollider.enabled = false;
        }
        player.transform.position = teleportLocation.position;
        face2.SetActive(false);
      
        
        face4.SetActive(true);
        
        if(directionSignLeft != null && directionSignRight != null)
        {
            directionSignRight.SetActive(true);
            BlockPassage.gameObject.SetActive(true);    
            directionSignLeft.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
