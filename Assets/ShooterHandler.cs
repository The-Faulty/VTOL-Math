using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ShooterHandler : MonoBehaviour
{
  public Transform idlePoint;
  public Transform alignPoint;
  public NavMeshAgent agent;
  public Button button;

  private bool isIdle = true;

  // Start is called before the first frame update
  void Start()
  {
    agent.destination = idlePoint.position;
    button.onClick.AddListener(triggered);
  }

  // Update is called once per frame
  void Update()
  {

  }

  void triggered()
  {
    if (isIdle)
    {
      button.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Stop Align";
      agent.destination = alignPoint.position;
      isIdle = !isIdle;
    } else
    {
      button.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Start Align";
      agent.destination = idlePoint.position;
      isIdle = !isIdle;
    }
  }
}
