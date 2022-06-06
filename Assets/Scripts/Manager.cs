using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    

    private int i = 0;
    private Vector3[,] plansza = new Vector3[8,8];
    public TextMeshProUGUI boardID;
    public TextMeshProUGUI iterationPanel;
    public TextMeshProUGUI successPanel;
    public TextMeshProUGUI collisionPanel;
    public TextMeshProUGUI failPanel;
    public TextMeshProUGUI failedAttemptPanel;
    public int iteration =0;
    public int success = 0;
    public int collision = 0;
    void Start()
    {
        for(int x = 0; x < 8; x++){
            for(int z = 0; z < 8; z++){
                plansza[x,z] = new Vector3(x*40f, 30f,(140f - z*40f)); 
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow))
            i = (i==63)? 0 : i+1;
        else if(Input.GetKeyUp(KeyCode.LeftArrow))
            i = (i==0)? 63 : i-1;
        transform.position = plansza[i/8,i%8];
        boardID.text = $"{i}";

        iterationPanel.text = $"Iteration: {iteration}";
        successPanel.text = $"Success: {success} [{Mathf.Round(((float)success/(float)iteration) * 10000f)/100f}%]";
        float fail = iteration-success;
        failPanel.text = $"Fail: {fail} [{Mathf.Round((fail/(float)iteration) * 10000f)/100f}%]";
        collisionPanel.text = $"    Collision: {collision} [{Mathf.Round(((float)collision/(float)iteration) * 10000f)/100f}]%";
        float noFault = fail-collision;
        failedAttemptPanel.text = $"    No-fault: {noFault} [{Mathf.Round((noFault/(float)iteration) * 10000f)/100f}]%";
         
    }
}
