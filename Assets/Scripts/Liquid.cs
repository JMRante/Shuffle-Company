using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public bool isFall = false;

    public GameObject liquidCell;
    public GameObject liquidFallCell;
    public GameObject model;
    private float liquidAmount;

    private int liquidHeight = 1;
    public int liquidMaxHeight;

    public float fillSpeed = 1f;

    private Sensor sensor;

    private Vector3[] fillDirections = new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left, Vector3.up };
    private bool[] exemptDirections = new bool[] { false, false, false, false, false};

    private int totalMask;
    private int filledMask;
    private int liquidMask;
    private int solidMask;
    private int fallMask;

    void Start()
    {
        sensor = GetComponentInChildren<Sensor>();
        liquidAmount = 0f;

        totalMask = LayerMask.GetMask("Solid", "Water", "Waterfall");
        filledMask = LayerMask.GetMask("Solid", "Water");
        liquidMask = LayerMask.GetMask("Water");
        solidMask = LayerMask.GetMask("Solid");
        fallMask = LayerMask.GetMask("Waterfall");

        if (!isFall)
        {
            model.transform.localPosition = new Vector3(0f, Mathf.Lerp(-0.5f, 0.3f, liquidAmount), 0f);
        }
    }

    void FixedUpdate()
    {
        if (liquidAmount < 1f)
        {
            liquidAmount += fillSpeed * Time.deltaTime;
        }

        if (liquidAmount >= 1f)
        {
            liquidAmount = 1f;

            if (!sensor.IsCellBlocked(Vector3.zero, Vector3.one * 0.49f, solidMask))
            { 
                if (isFall)
                {
                    if (sensor.IsCellBlocked(Vector3.zero, Vector3.one * 0.49f, liquidMask))
                    {
                        Destroy(gameObject);
                    }

                    if (!sensor.IsCellBlocked(Vector3.up, Vector3.one * 0.49f, fallMask) && sensor.IsCellBlocked(-transform.forward, Vector3.one * 0.49f, solidMask))
                    {
                        Destroy(gameObject);
                    }

                    GameObject liquidSeedObject = null;

                    if (!sensor.IsCellBlocked(Vector3.down, Vector3.one * 0.49f, filledMask))
                    {
                        bool fallInSameDirectionExists = false;
                        Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.down, Vector3.one * 0.49f, Quaternion.identity, fallMask);

                        foreach (Collider collider in colliders)
                        {
                            Transform tf = collider.transform.GetComponent<Transform>();

                            if (tf.forward == transform.forward)
                            {
                                fallInSameDirectionExists = true;
                                break;
                            }
                        }

                        if (!fallInSameDirectionExists)
                        {
                            liquidSeedObject = Instantiate(liquidFallCell, transform.position + Vector3.down, transform.rotation, transform.parent);
                            liquidSeedObject.name = "WCF";
                        }
                    }
                    else if (sensor.IsCellBlocked(Vector3.down, Vector3.one * 0.49f, solidMask) && !sensor.IsCellBlocked(Vector3.zero, Vector3.one * 0.49f, liquidMask))
                    {
                        liquidSeedObject = Instantiate(liquidCell, transform.position, Quaternion.identity, transform.parent);
                        liquidSeedObject.name = "WC";
                        Destroy(gameObject);
                    }

                    if (liquidSeedObject != null)
                    {
                        Liquid liquidSeed = liquidSeedObject.GetComponent<Liquid>();
                        liquidSeed.liquidHeight = 1;
                        liquidSeed.liquidMaxHeight = this.liquidMaxHeight;
                    }
                }
                else
                {
                    for (int i = 0; i < fillDirections.Length; i++)
                    {
                        Vector3 direction = fillDirections[i];

                        if (exemptDirections[i] == true || sensor.IsCellBlocked(direction, Vector3.one * 0.49f, liquidMask))
                        {
                            exemptDirections[i] = true;
                        }
                        else if (direction != Vector3.up || liquidHeight < liquidMaxHeight)
                        {
                            if (!sensor.IsCellBlocked(direction, Vector3.one * 0.49f, filledMask))
                            {
                                GameObject liquidSeedObject = null;

                                if (sensor.IsCellBlocked(direction + Vector3.down, Vector3.one * 0.49f, filledMask))
                                {
                                    Liquid liquidBelow = (Liquid) sensor.GetComponentFromCell(direction + Vector3.down, typeof(Liquid), liquidMask);

                                    if (liquidBelow == null || liquidBelow.liquidHeight < this.liquidMaxHeight)
                                    {
                                        liquidSeedObject = Instantiate(liquidCell, transform.position + direction, Quaternion.identity, transform.parent);
                                        liquidSeedObject.name = "WC";
                                    }
                                }
                                else
                                {
                                    bool fallInSameDirectionExists = false;
                                    Collider[] colliders = Physics.OverlapBox(transform.position + direction, Vector3.one * 0.49f, Quaternion.identity, fallMask);

                                    foreach (Collider collider in colliders)
                                    {
                                        Transform tf = collider.transform.GetComponent<Transform>();

                                        if (tf.forward == direction)
                                        {
                                            fallInSameDirectionExists = true;
                                            break;
                                        }
                                    }

                                    if (!fallInSameDirectionExists)
                                    {
                                        liquidSeedObject = Instantiate(liquidFallCell, transform.position + direction, Quaternion.LookRotation(direction, Vector3.up), transform.parent);
                                        liquidSeedObject.name = "WCF";
                                    }
                                }

                                if (liquidSeedObject != null)
                                {
                                    Liquid liquidSeed = liquidSeedObject.GetComponent<Liquid>();

                                    if (direction == Vector3.up)
                                    {
                                        liquidSeed.liquidHeight = this.liquidHeight + 1;
                                        liquidSeed.liquidMaxHeight = this.liquidMaxHeight;
                                    }
                                    else
                                    {
                                        Liquid liquidBelow = (Liquid)sensor.GetComponentFromCell(direction + Vector3.down, typeof(Liquid), liquidMask);
                                        
                                        if (liquidBelow != null)
                                        {
                                            liquidSeed.liquidHeight = liquidBelow.liquidHeight + 1;
                                        }
                                        else
                                        {
                                            liquidSeed.liquidHeight = this.liquidHeight;
                                        }

                                        liquidSeed.liquidMaxHeight = this.liquidMaxHeight;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (!isFall)
        {
            if (liquidHeight < liquidMaxHeight - 1)
            {
                model.transform.localPosition = new Vector3(0f, Mathf.Lerp(-0.5f, 0.5f, liquidAmount), 0f);
            }
            else
            {
                model.transform.localPosition = new Vector3(0f, Mathf.Lerp(-0.5f, 0.3f, liquidAmount), 0f);
            }

            if (liquidHeight < liquidMaxHeight)
            {
                model.SetActive(false);
            }
            else
            {
                model.SetActive(true);
            }
        }
    }
}
