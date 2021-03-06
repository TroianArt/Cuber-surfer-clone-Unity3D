using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager _instance;

    [Header("General Variables")]
    public float confettiLoopRate;

    [Header("VFX Prefabs")]
    public GameObject levelSuccessConfettiVFX;
    public GameObject cubeCollectedVFX;
    public GameObject gemCollectedVFX;

    private void Awake()
    {
        _instance = this;
    }

    public void SpawnCubeCollectedVFX(Transform _transform)
    {
        Vector3 pos = new Vector3(_transform.position.x, _transform.position.y, _transform.position.z - 0.5f);

        //Instantiate(cubeCollectedVFX, pos, Quaternion.Euler(0f, 180f, 0f), _transform);

    } // SpawnCubeCollectedVFX()

    public void SpawnGemCollectedVFX(Transform _transform)
    {
        Vector3 pos = new Vector3(_transform.position.x, _transform.position.y + 1f, _transform.position.z);

        Instantiate(gemCollectedVFX, pos, Quaternion.Euler(0f, 180f, 0f), _transform);

    } // SpawnCubeCollectedVFX()

    public void StartConfettiLoop(Transform _transform)
    {
        StartCoroutine(ConfettiLoop(_transform.position));

    } // SpawnLevelSuccessConfettiVFX()

    IEnumerator ConfettiLoop(Vector3 _pos)
    {
        while (true)
        {
            yield return new WaitForSeconds(confettiLoopRate);

            //Vector3 rndPos = new Vector3(Random.Range(_pos.x - 2f, _pos.x + 2f), _pos.y + 10f, Random.Range(_pos.z - 2f, _pos.z + 2f));
            Vector3 rndPos = new Vector3(_pos.x , _pos.y + 10f, _pos.z);
            Instantiate(levelSuccessConfettiVFX, rndPos, Quaternion.Euler(-90f, 0f, 0f));
        }

    } // ConfettiLoop()

} // class
