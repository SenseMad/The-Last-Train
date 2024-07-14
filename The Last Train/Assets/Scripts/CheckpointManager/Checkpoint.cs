using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TLT.CharacterManager;

namespace TLT.Checkpoints
{
  public class Checkpoint : MonoBehaviour
  {


    //===================================



    //===================================

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if (!collision.GetComponent<Character>())
        return;

      Vector3 position = transform.position;
      Quaternion rotation = transform.rotation;
      string levelName = SceneManager.GetActiveScene().name;


    }

    //===================================



    //===================================
  }
}