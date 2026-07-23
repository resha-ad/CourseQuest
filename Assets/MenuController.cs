using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI elements

public class MenuController : MonoBehaviour
{
     public GameObject[] objSites;
    // // public GameObject canvasMainMenu;
    public bool isCameraMove=true;
    // // Start is called before the first frame update
    // void Start()
    // {
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    //     // if(Input.GetMouseButtonDown(0)){
    //     //     RaycastHit hit;
    //     //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     //     if(Physics.Raycast(ray, out hit, 100.0f)){
    //     //         if(hit.transform.gameObject.tag=="NextSite"){
    //     //             LoadSite(hit.transform.gameObject.GetComponent<NextSite>().GetSiteToLoad());
    //     //         }
    //     //     }
    //     // }
    // }
    public void LoadSite(int siteNumber){
        for(int i=0; i<objSites.Length;i++){
            objSites[i].SetActive(false);
        }
        objSites[siteNumber].SetActive(true);
        // canvasMainMenu.SetActive(false);
        isCameraMove=true;
        // GetComponent<CameraController>().ResetCamera();
    }
    
    // public void ReturnToSite(){
    //     isCameraMove=true;
    // }
    // public void OpenMedia(){
    //     isCameraMove=true;
    // }


    public Canvas canvas; // Reference to the Canvas

    void Start()
    {
        
    }

    public void HideCanvas()
    {
        // Hide the canvas
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }
    }
}
