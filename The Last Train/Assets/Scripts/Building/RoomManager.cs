using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TLT.InteractionObjects;
using System;

namespace TLT.Building
{
  public sealed class RoomManager : MonoBehaviour
  {
    [SerializeField] private SpriteRenderer _closedRoom;

    [SerializeField] private List<DoorManager> _listDoors = new();

    [SerializeField] private List<Transform> _listSpawnPoints = new();

    [SerializeField] private List<GameObject> _spawnables = new();

    [Space]
    [SerializeField] private bool _isRoomOpenStart = false;

    //===================================

    public bool IsRoomOpen { get; private set; }

    //===================================

    public event Action OnOpenRoom;

    //===================================

    private void Start()
    {
      if (_isRoomOpenStart)
      {
        IsRoomOpen = true;

        if (_closedRoom != null)
          _closedRoom.gameObject.SetActive(false);
      }
    }

    private void OnEnable()
    {
      foreach (var door in _listDoors)
        {
          door.OnDoorOpen += OpenRoom;
        }
    }

    private void OnDisable()
    {
      foreach (var door in _listDoors)
      {
        door.OnDoorOpen -= OpenRoom;
      }
    }

    //===================================

    public void OpenRoom()
    {
      if (IsRoomOpen)
        return;

      IsRoomOpen = true;

      StartCoroutine(FadeOut());
    }

    public void ActionOnOpenRoom()
    {
      OnOpenRoom?.Invoke();
    }

    //===================================

    private void SpawnItems()
    {
      if (_listSpawnPoints.Count == 0 && _spawnables.Count == 0)
        return;

      System.Random random = new();

      int numberSpawns = random.Next(0, _listSpawnPoints.Count + 1);

      if (numberSpawns == 0)
        return;

      List<int> indexSelectedPoints = new();

      for (int i = 0; i < numberSpawns; i++)
      {
        int index = random.Next(0, _listSpawnPoints.Count);
        
        while (indexSelectedPoints.Contains(index))
        {
          index = random.Next(0, _listSpawnPoints.Count);
        }

        indexSelectedPoints.Add(index);
      }

      foreach (var indexSelectedPoint in indexSelectedPoints)
      {
        int indexSpawnables = random.Next(0, _spawnables.Count);

        Instantiate(_spawnables[indexSpawnables], _listSpawnPoints[indexSelectedPoint].position, Quaternion.identity);
      }
    }

    //===================================

    private IEnumerator FadeOut()
    {
      SpawnItems();

      float elapsedTime = 0f;
      Color color = _closedRoom.color;

      while (elapsedTime < 1)
      {
        elapsedTime += Time.deltaTime;
        color.a = Mathf.Clamp01(1 - elapsedTime);
        _closedRoom.color = color;

        yield return null;
      }

      color.a = 0;
      _closedRoom.color = color;
      _closedRoom.gameObject.SetActive(false);
    }

    //===================================
  }
}