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
    Vector3 startPos = CharacterTransform.position;
    float distance = Vector3.Distance(startPos, pos);
    remainingDistance = distance;
    while (remainingDistance > 0)
    {
      CharacterTransform.transform.position = Vector3.Lerp(startPos, pos, 1 - (remainingDistance / distance));
      remainingDistance -= MoveSpeed * Time.deltaTime;
      yield return null;
    }
  }
}
