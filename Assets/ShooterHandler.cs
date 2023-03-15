using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ShooterHandler : MonoBehaviour
{
  Animator anim;

  public Transform idlePoint;
  public Transform alignPoint;
  public Transform playerTarget;
  public Transform gameTarget;
  public Transform agent;

  public NavMeshAgent navAgent;

  public Button AlignButton;
  public Button LaunchBarButton;
  public Button EngineButton;

  public Text indicator;

  private bool isIdle = true;
  private bool bar = false;
  private bool engines = false;
  private bool isWalking = false;

  private enum PlayerState
  {
    None,
    Taxi,
    LaunchBar,
    Hooked,
    Runup,
    Launch
  }

  PlayerState state;
  // Start is called before the first frame update
  void Start()
  {
    anim = GetComponent<Animator>();
    navAgent.destination = idlePoint.position;
    AlignButton.onClick.AddListener(triggered);
    LaunchBarButton.onClick.AddListener(BarButton);
    EngineButton.onClick.AddListener(RunupButton);
    state = PlayerState.None;
  }


  
  // Update is called once per frame
  void Update()
  {
    Vector3 lookPos;
    Quaternion rotation;
    print(navAgent.remainingDistance);
    if (navAgent.remainingDistance > navAgent.radius)
    {
      if (!isWalking)
      {
        anim.SetBool("walk", true);
        anim.SetBool("idle", false);
        isWalking = true;
      }
    } else
    {
      if (isWalking)
      {
        anim.SetBool("walk",false);
        anim.SetBool("idle", true);
        isWalking = false;
      }
    }
    switch (state)
    {
      case (PlayerState.Taxi):
        if (navAgent.remainingDistance < .3)
        {
          lookPos = playerTarget.transform.position - agent.transform.position;
          lookPos.y = 0;
          rotation = Quaternion.LookRotation(lookPos);
          agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * 2);
          //do alignment stuff
          Align();
        }
        break;
      case (PlayerState.LaunchBar):
        if (bar)
        {
          navAgent.SetDestination(idlePoint.position);
          anim.SetBool("bar", false);
          state = PlayerState.Hooked;
        }
        break;
      case (PlayerState.Hooked):
        if (navAgent.remainingDistance < .3)
        {
          indicator.text = "Engines";
          anim.SetBool("runup", true);
          state = PlayerState.Runup;          
        }
        break;
      case (PlayerState.Runup):
        lookPos = playerTarget.transform.position - agent.transform.position;
        lookPos.y = 0;
        rotation = Quaternion.LookRotation(lookPos);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * 2);
        if (engines)
        {
          state = PlayerState.Launch;
          anim.SetBool("runup", false);
          anim.SetBool("launch", true);
          indicator.text = "Launch";
        }
        break;
    }
  }

  void Align()
  {
    float relativeAngle = Vector2.SignedAngle(new Vector2(gameTarget.forward.x, gameTarget.forward.z), new Vector2((gameTarget.position - playerTarget.position).x, (gameTarget.position - playerTarget.position).z));
    if (relativeAngle > 5)
    {
      indicator.text = "Left";
      anim.SetBool("left", true);
      anim.SetBool("right", false);
      anim.SetBool("forward", false);
    }
    else if (relativeAngle < -5)
    {
      indicator.text = "Right";
      anim.SetBool("left", false);
      anim.SetBool("right", true);
      anim.SetBool("forward", false);
    }
    else
    {
      indicator.text = "Forward";
      anim.SetBool("left", false);
      anim.SetBool("right", false);
      anim.SetBool("forward", true);
    }
    if ((gameTarget.transform.position - playerTarget.transform.position).sqrMagnitude < 0.36 && Vector3.Dot(playerTarget.transform.forward, gameTarget.forward) > 0.5f)
    {
      indicator.text = "Bar";
      anim.SetBool("left", false);
      anim.SetBool("right", false);
      anim.SetBool("forward", false);
      anim.SetBool("bar", true);
      state = PlayerState.LaunchBar;
    }
  }

  void triggered()
  {
    if (isIdle)
    {
      AlignButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Stop Align";
      navAgent.destination = alignPoint.position;
      state = PlayerState.Taxi;
      isIdle = !isIdle;
    }
    else
    {
      AlignButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Start Align";
      navAgent.destination = idlePoint.position;
      state = PlayerState.None;
      isIdle = !isIdle;
    }
  }
  void BarButton()
  {
    bar = !bar;
  }

  void RunupButton()
  {
    engines = !engines;
  }

  void onHook()
  {
    state = PlayerState.Hooked;
    //
  }
}
