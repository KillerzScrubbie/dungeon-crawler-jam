using System.Collections;
using UnityEngine;
using Cinemachine;

public class camTrackSwitcher : MonoBehaviour
{
    [SerializeField] CinemachineDollyCart cart;
    [SerializeField] CinemachineSmoothPath _startPath;
    [SerializeField] CinemachineSmoothPath[] _paths;

    int _previousNum;

    void Start()
    {
        StartRandomTrack();
    }

    void StartRandomTrack()
    {
        StopAllCoroutines();
        cart.m_Path = _startPath;
        StartCoroutine(ChangeTrack());
    }

    IEnumerator ChangeTrack()
    {
        yield return new WaitForSeconds(Random.Range(3, 5));
        // cart.m_Position = Random.Range(0, 1);
        // cart.m_Speed = Random.Range(.1f, .3f);

        cart.m_Path = _paths[getNotOldNum()];
        StartCoroutine(ChangeTrack());

    }

    int getNotOldNum()
    {

        int newIndex = Random.Range(0, _paths.Length);

        while (newIndex == _previousNum)
        {
            newIndex = Random.Range(0, _paths.Length);
        }

        _previousNum = newIndex;

        return newIndex;


    }


}
