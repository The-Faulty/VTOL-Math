using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewNav : MonoBehaviour
{
  public Transform CharacterTransform;

  public float remainingDistance;
  public float MoveSpeed = 1f;

  public CrewNav(Transform charT)
  {
    CharacterTransform = charT;
  }

  public void SetDestination(Vector3 pos)
  {
    StartCoroutine(MoveToAsync(pos));
  }

  private IEnumerator MoveToAsync(Vector3 pos)
  {
    print("Move to" + pos);
    Vector3 lookPos;
    Quaternion rotation;
    Vector3 startPos = CharacterTransform.position;
    float distance = Vector3.Distance(startPos, pos);
    remainingDistance = distance;
    while (remainingDistance > 0)
    {
      lookPos = pos - CharacterTransform.position;
      lookPos.y = 0;
      rotation = Quaternion.LookRotation(lookPos);
      CharacterTransform.rotation = Quaternion.Slerp(CharacterTransform.rotation, rotation, Time.deltaTime * 2);
      CharacterTransform.position = Vector3.Lerp(startPos, pos, 1 - (remainingDistance / distance));
      remainingDistance -= MoveSpeed * Time.deltaTime;
      yield return null;
    }
  }
}
