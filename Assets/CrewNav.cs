using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewNav : MonoBehaviour
{
  public Transform CharacterTransform;

  public float remainingDistance;
  public float MoveSpeed = 2;

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
    //CharacterTransform.rotation = Vector3.
    Vector3 lookPos;
    Quaternion rotation;
    Vector3 startPos = CharacterTransform.position;
    float distance = Vector3.Distance(startPos, pos);
    remainingDistance = distance;
    while (remainingDistance > 0)
    {
      lookPos = CharacterTransform.transform.position - pos;
      lookPos.y = 0;
      rotation = Quaternion.LookRotation(lookPos);
      CharacterTransform.transform.rotation = Quaternion.Slerp(CharacterTransform.transform.rotation, rotation, Time.deltaTime * 2);
      CharacterTransform.transform.position = Vector3.Lerp(startPos, pos, 1 - (remainingDistance / distance));
      remainingDistance -= MoveSpeed * Time.deltaTime;
      yield return null;
    }
  }
}
