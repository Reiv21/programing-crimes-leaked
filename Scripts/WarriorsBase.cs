using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorsBase : MonoBehaviour
{
    public LineRenderer circleRenderer;
    public GameObject Warrior;
    public Transform Spawn;
    public BuildingScript BuildingScript;
    GameObject SpawnedWarrior;
    public GameObject FlagPrefab;
    GameObject NodeGameObj;
    public GameObject Flag;
    public List<Vector3> FlagPositions = new List<Vector3>();
    public Transform Exit;
    public GameObject FlagButTransparent;
    public GameObject FlagButTransparentRed;
    public GameObject FlagButTransparentGreen;

    public List<int> HowManyWarriorsAreDead = new List<int>();
    public bool CanSpawnWarrior = false;
    public int MaxWarriors = 3;
    bool IsCoroutineOn = false;
    public float CooldownOfSpawning;
    GameObject[] nodes;
    public Camera camera;
    bool CanPlaceFlag = false;
    public bool FlagPosChoose = false;
    float range = 10f;
    public bool isSpawning = false;

    //WOJOWNICY KTÓRZY SĄ STWORZENI PRZEZ BAZE
    GameObject[] Warriors = new GameObject[3];
    GameObject Warrior1;
    GameObject Warrior2;
    GameObject Warrior3;

    void Start()
    {
        SetMaxWarriors();
        //warriors spawn

        //circle
        DrawCircle(100);
        // flag
        FlagButTransparentGreen = Instantiate(FlagButTransparentGreen, transform.position, new Quaternion(0f, 0f, 0f, 0f));
        FlagButTransparentRed = Instantiate(FlagButTransparentRed, transform.position, new Quaternion(0f, 0f, 0f, 0f));
        FlagButTransparent = FlagButTransparentGreen;
        FlagButTransparentGreen.transform.parent = transform;
        FlagButTransparentRed.transform.parent = transform;
        FlagButTransparent.SetActive(false);
        FlagButTransparentGreen.SetActive(false);
        FlagButTransparentRed.SetActive(false);

        //FlagButTransparent.SetActive(false);
        camera = FindObjectOfType<Camera>();
        StartCoroutine(WarriorsSpawn());
        FindClosestNode();
        
    }

    void Update()
    {
        if (HowManyWarriorsAreDead.ToArray().Length != 0 && !isSpawning)
        {
            StartCoroutine("WarriorsSpawn");
        }
        if(FlagPosChoose)
        {
            if(Input.GetMouseButtonDown(0)&&CanPlaceFlag)
            {
                Flag.transform.position = FlagButTransparent.transform.position;
                FlagButTransparent.SetActive(false);
                CanPlaceFlag = false;
                BuildingScript.FlagPlaced();
                CancelInvoke("ShowItWithMouse");
                int HowManyTimesMultiply = 1;
                FlagPositions.Clear();
                for (int i = 0; i < MaxWarriors; i++)
                {
                    
                    if (i == 0)
                    {
                        FlagPositions.Add(Flag.transform.position + new Vector3(0f, 0f, 2f));
                    }
                    if (i % 2 == 0)
                    {
                        //Parzyste
                        FlagPositions.Add(Flag.transform.position + new Vector3(1f, HowManyTimesMultiply, 0f));
                        HowManyTimesMultiply++;
                    }
                    else
                    {
                        //Nieparzyste 
                        FlagPositions.Add(Flag.transform.position + new Vector3(-1f, HowManyTimesMultiply, 0f));
                    }
                }
            }
        }
        else
        {

        }
    }
    public IEnumerator WarriorsSpawn()
    {
        isSpawning = true;
        int Number = 0;
        Vector3 SpawnV3 = new Vector3(Spawn.transform.position.x, Spawn.transform.position.y, Spawn.transform.position.z);
        if(HowManyWarriorsAreDead.Count <= 0)
        {
            yield return null;
            isSpawning = false;
        }
        // Tu co� nie ten :(
        for (int żyweWojowniki = -1; żyweWojowniki < MaxWarriors;)
        {
            if(żyweWojowniki == -1)
            {
                żyweWojowniki = MaxWarriors - HowManyWarriorsAreDead.Count;
            }
            if (CanSpawnWarrior)
            {

                SpawnedWarrior = Instantiate(Warrior, SpawnV3, new Quaternion(0f, 0f, 0f, 0f));
                SpawnedWarrior.GetComponent<Warrior>().Exit = Exit;
                SpawnedWarrior.GetComponent<Warrior>().WarriorsBase = this;
                SpawnedWarrior.GetComponent<Warrior>().posNumber = Number;
                SpawnedWarrior.transform.SetParent(transform);
                CanSpawnWarrior = false;

                Warriors[HowManyWarriorsAreDead[0]] = SpawnedWarrior;
                HowManyWarriorsAreDead.RemoveAt(0);
                żyweWojowniki++;
                Number++;
            }
            if (IsCoroutineOn == false && CanSpawnWarrior == false)
            {
                IsCoroutineOn = true;
                yield return new WaitForSeconds(CooldownOfSpawning);
                CanSpawnWarrior = true;
                IsCoroutineOn = false;
            }
        }
        isSpawning = false;

    }

    public void FindClosestNode()
    {
        nodes = GameObject.FindGameObjectsWithTag("node");
        float distanceToClosestNode = Mathf.Infinity;

        foreach (GameObject node in nodes)
        {
            float distanceToNode = Vector3.Distance(transform.position, node.transform.position);
            if (distanceToClosestNode > distanceToNode)
            {
                distanceToClosestNode = distanceToNode;
                NodeGameObj = node;
            }
        }
        Flag = Instantiate(FlagPrefab, NodeGameObj.transform.position, new Quaternion(0f, 0f, 0f, 0f));
        Flag.transform.position += new Vector3(0, 2f, 0);
        Flag.transform.parent = transform;
        int HowManyTimesMultiply = 0;
        FlagPositions.Clear();
        Vector3 Vector = Flag.transform.position;
        float WarriorHeight = 0.3f;
        for (int i = 0; i < MaxWarriors; i++)
        {
            
            if (i == 0)
            {

                FlagPositions.Add(Flag.transform.position + new Vector3(0, Flag.transform.localPosition.y * -1 - WarriorHeight, 2));
                
                // zrobic jak tu wszedzie
            }
            else
            {
                if (i % 2 == 0)
                {
                    //Parzyste
                    FlagPositions.Add(Flag.transform.position + new Vector3(1f, Flag.transform.localPosition.y * -1 - WarriorHeight, HowManyTimesMultiply));

                    HowManyTimesMultiply++;
                }
                else
                {
                    //Nieparzyste
                    FlagPositions.Add(Flag.transform.position + new Vector3(-1f, Flag.transform.localPosition.y * -1 - WarriorHeight, HowManyTimesMultiply));
                }
            }
        }
    }
    public void ShowItWithMouse()
    {
        if(!FlagPosChoose)
        {
            return;
        }
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo);
        if (hitInfo.collider == null)
        {
            Debug.Log("No HitInfo");
            return;
        }
        if (hitInfo.collider.tag == "node")
        {
            float distanceToFlag = Vector3.Distance(transform.position, FlagButTransparent.transform.position);
            if (distanceToFlag > range)
            {
                FlagButTransparent = FlagButTransparentRed;
                FlagButTransparent.SetActive(true);
                FlagButTransparentGreen.SetActive(false);
                FlagButTransparent.transform.position = hitInfo.point + new Vector3(0, 1.5f, 0);
                return;

            }
            FlagButTransparent = FlagButTransparentGreen;
            FlagButTransparent.transform.position = hitInfo.point + new Vector3(0, 1.5f, 0);

            FlagButTransparent.SetActive(true);
            FlagButTransparentRed.SetActive(false);
            CanPlaceFlag = true;
        }
        else
        {
            CanPlaceFlag = false;
            FlagButTransparent.transform.position = hitInfo.point + new Vector3(0, 1.5f, 0);
            FlagButTransparent = FlagButTransparentRed;
            FlagButTransparent.SetActive(true);
            FlagButTransparentGreen.SetActive(false);
            
        }
        
    }
    void DrawCircle(int steps)
    {
        float radius = range / 3;
        circleRenderer.positionCount = steps + 1;
        for(int currentStep = 0; currentStep <= steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);
            float x = xScaled * radius;
            float y = yScaled * radius;
            
            Vector3 currentPosition = new Vector3(x, y, 0.65f);

            circleRenderer.SetPosition(currentStep, currentPosition);
            circleRenderer.enabled = false;
        }
    }
    void SetMaxWarriors()
    {
        for(int number = 0; number < MaxWarriors; number++)
        {
            // nie zapisuje aktualnie martwych lol
            //if max warriors = 3 to robi liste (0,1,2)
            HowManyWarriorsAreDead.Add(number);
            //Debug.Log("Aktualny martwy wojownik to2 " + HowManyWarriorsAreDead[number]);
        }
    }
}
