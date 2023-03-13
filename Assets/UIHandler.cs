using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
  public Transform characterTarget;
  public Transform gameTarget;

  public RectTransform playerUI;
  public RectTransform backgroundUI;

  public Text angleDisplay;
  public Text alignDisplay;
  public Text moveDisplay;

  public bool isAssigned;

  Vector2 backgroundSize;
  Vector2 relativeDistance;

  int maxDistance = 10;
  float worldUIConversion;
  float uiOffset;

  float relativeAngle;

  bool isAligned = false;

  // Start is called before the first frame update
  void Start()
  {
    backgroundSize = new Vector2(backgroundUI.rect.width, backgroundUI.rect.height);
    uiOffset = backgroundSize.y - playerUI.rect.height;
    worldUIConversion = backgroundSize.y / maxDistance;
    //playerUI.anchoredPosition = new Vector2(playerUI.anchoredPosition.x + 300, playerUI.anchoredPosition.y + 300);

    //pythag(playerUI.anchoredPosition.x, playerUI.anchoredPosition.y - uiOffset);

    //print(playerUI.rect);
    print(worldUIConversion);
  }

  // Update is called once per frame
  void Update()
  {
    if (isAssigned)
    {
      relativeAngle = Vector2.SignedAngle(new Vector2(gameTarget.forward.x, gameTarget.forward.z), new Vector2((gameTarget.position - characterTarget.position).x, (gameTarget.position - characterTarget.position).z));
      angleDisplay.text = relativeAngle.ToString();

      worldToUI(characterTarget, gameTarget);
      howMove();
      checkAlign();
    }
    //print((characterTarget.position - gameTarget.position).sqrMagnitude);
    //print(Vector3.Dot(characterTarget.forward, gameTarget.forward));
  }

  float pythag(float a, float b)
  {
    return (float)(Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2))); //a^2 + b^2 = c^2
  }

  //Unity has y and z flipped from standard convention
  void worldToUI(Transform player, Transform target)
  {
    relativeDistance = new Vector2(player.position.x - target.position.x, player.position.z - (target.position.z));

    //playerUIsetPosition(relativeDistance.x, -relativeDistance.y);
    playerUIsetPosition(Mathf.Sin(Mathf.Deg2Rad * relativeAngle) * relativeDistance.magnitude, Mathf.Cos(Mathf.Deg2Rad * relativeAngle) * relativeDistance.magnitude);
    //print(pythag(relativeDistance.x, relativeDistance.y));
  }

  void playerUIsetPosition(float x, float y)
  {
    x *= worldUIConversion;
    y = uiOffset - (y * worldUIConversion);

    //check collisions
    if (y < playerUI.rect.height) y = playerUI.rect.height;
    if (x < -(backgroundSize.x / 2) + playerUI.rect.width / 2) x = -(backgroundSize.x / 2) + (playerUI.rect.width / 2);
    if (x > (backgroundSize.x / 2) - playerUI.rect.width / 2) x = (backgroundSize.x / 2) - (playerUI.rect.width / 2);

    playerUI.anchoredPosition = new Vector2(x, y);
    playerUI.eulerAngles = new Vector3(0, 0, Vector3.SignedAngle(characterTarget.forward, gameTarget.forward, Vector3.up));
  }

  void checkAlign()
  {
    if (Vector3.Dot(characterTarget.forward, gameTarget.forward) > 0.5 && !isAligned)
    {
      isAligned = true;
      alignDisplay.text = "true";
    }
    else if (Vector3.Dot(characterTarget.forward, gameTarget.forward) < 0.5 && isAligned)
    {
      isAligned = false;
      alignDisplay.text = "false";
    }
  }

  void howMove()
  {
    if (relativeAngle > 10)
    {
      moveDisplay.text = "left";
    }
    else if (relativeAngle < -10)
    {
      moveDisplay.text = "right";
    }
    else
    {
      moveDisplay.text = "forward";
    }
  }
}
