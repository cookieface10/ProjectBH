using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject PlaceHolderWeapon;

    public GameObject[] PlayerInv = new GameObject[16];
    public GameObject[] TitanInv = new GameObject[1];

    public GameObject[] WeaponHotBar = new GameObject[3];

    public GameObject CurrentItem;
    public GameObject ActiveItem;

    public GameObject HoldingSpot;

    public int currentSlot;

    private void Start()
    {
        for(int i = 0; i < 16; i++)
        {
            PlayerInv[i] = null;
        }

        PlayerInv[0] = PlaceHolderWeapon;
    }
    private void Update()
    {
        GameManager.PlayerInv = PlayerInv;
        GameManager.TitanInv = TitanInv;

        if (!GameManager.IsTitan)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && PlayerInv[0] != null)
            {

                currentSlot = 0;
                if(PlayerInv[currentSlot] != ActiveItem)
                {
                    CurrentItem = Instantiate(PlayerInv[currentSlot], HoldingSpot.transform.position, HoldingSpot.transform.rotation, HoldingSpot.transform);
                    ActiveItem = PlayerInv[currentSlot];
                }
                
            }
        }
        else
        {

        }
    }
}

/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InvantoryManager : MonoBehaviour
{
    
    public GameObject[] inv = new GameObject[4];


    public GameObject Camera;


    public GameObject key;
    public GameObject picklock;
    public GameObject flashlight;

    public GameObject dropKey;
    public GameObject dropPicklock;
    public GameObject dropFlashlight;

    public GameObject HoldingSpot;


    public RawImage[] slots = new RawImage[4];
    public RawImage[] flashlights = new RawImage[4];
    public RawImage[] keys = new RawImage[4];
    public RawImage[] pickLocks = new RawImage[4];

    private bool grabFlashlight;
    private bool grabKey;
    private bool grabPickLock;
    public Color inUse = Color.white;
    public Color notInUse = Color.white;
    private float pickupRange = 2.0f;
    private GameObject currentItem;
    private GameObject activeItem;
    private int currentSlot = 0;

    private void Update()
    {
        slots[currentSlot].color = inUse;
        if (inv[0] == null && inv[1] == null && inv[2] == null && inv[3] == null)
        {
            GameManager.hasInventory = false;
        }
        else
        {
            GameManager.hasInventory = true;
        }
        ///////////////////// SLOT SYSTEM ////////////////////////
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentSlot = 0;
            slots[1].color = notInUse;
            slots[2].color = notInUse;
            slots[3].color = notInUse;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentSlot = 1;
            slots[0].color = notInUse;
            slots[2].color = notInUse;
            slots[3].color = notInUse;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentSlot = 2;
            slots[0].color = notInUse;
            slots[1].color = notInUse;
            slots[3].color = notInUse;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentSlot = 3;
            slots[0].color = notInUse;
            slots[1].color = notInUse;
            slots[2].color = notInUse;
        }
        if (GameManager.dead)
        {
            slots[0].enabled = false;
            slots[1].enabled = false;
            slots[2].enabled = false;
            slots[3].enabled = false;
            inv[0] = null;
            inv[1] = null;
            inv[2] = null;
            inv[3] = null;
        }
        else
        {
            slots[0].enabled = true;
            slots[1].enabled = true;
            slots[2].enabled = true;
            slots[3].enabled = true;
        }
        //////////////// Item INTERACT RAYCAST //////////////////////////
        RaycastHit hit;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, pickupRange))
        {
            if (hit.transform.gameObject.tag == "invItem")
            {
                GameManager.interactible = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (currentItem == null)
                    {
                        Destroy(hit.transform.gameObject);
                        if (hit.transform.gameObject.name == "torch")
                        {
                            grabFlashlight = true;
                        }
                        if (hit.transform.gameObject.name == "key")
                        {
                            grabKey = true;
                        }
                        if (hit.transform.gameObject.name == "pickLock")
                        {
                            grabPickLock = true;
                        }
                        if (hit.transform.gameObject.name == "gold")
                        {
                            GameManager.gold += 10;
                        }
                    }
                    else if (hit.transform.gameObject.name == "gold")
                    {
                        Destroy(hit.transform.gameObject);
                        GameManager.gold += 10;
                    }

                }

            }
            else if (hit.transform.gameObject.tag == "StorageContainer")
            {
                GameManager.interactible = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Animator ani = hit.transform.GetComponent<Animator>();
                    ani.Play("LockerOpen", 0, 0.0f);
                }
            }
            else
            {
                GameManager.interactible = false;
            }
        }
        else
        {
            GameManager.interactible = false;
        }

        RaycastHit hitMonster;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hitMonster, 20, ~0, QueryTriggerInteraction.Ignore))
        {
            if(hitMonster.transform.gameObject.tag == "Greed")
            {
                GameManager.lookingAtGreed = true;
            }
            else
            {
                GameManager.lookingAtGreed = false;
            }
        }
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hitMonster, 20))
        {
            if (hitMonster.transform.gameObject.tag == "RedEye")
            {
                GameManager.lookingAtRed = true;
            }
            else
            {
                GameManager.lookingAtRed = false;
            }
        }
        ////////// item drop ////////

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(activeItem == flashlight)
            {
                GameObject dr = Instantiate(dropFlashlight, HoldingSpot.transform.position, HoldingSpot.transform.rotation);
                dr.name = "torch";
                inv[currentSlot] = null;
            }
            else if(activeItem == key)
            {
                GameObject dr = Instantiate(dropKey, HoldingSpot.transform.position, HoldingSpot.transform.rotation);
                dr.name = "key";
                inv[currentSlot] = null;
            }
            else if (activeItem == picklock)
            {
                GameObject dr = Instantiate(dropPicklock, HoldingSpot.transform.position, HoldingSpot.transform.rotation);
                dr.name = "pickLock";
                inv[currentSlot] = null;
            }
        }


        /////// inv manage ///////

        if (inv[currentSlot] != activeItem)
        {
            Destroy(currentItem);
            if (inv[currentSlot] != null)
            {
                currentItem = Instantiate(inv[currentSlot], HoldingSpot.transform.position, HoldingSpot.transform.rotation, HoldingSpot.transform);
            }
            activeItem = inv[currentSlot];
            UpdateSlots();
            
        }

        if (grabFlashlight)
        {
            inv[currentSlot] = flashlight;
            grabFlashlight = false;
        }
        if (grabKey)
        {
            inv[currentSlot] = key;
            grabKey = false;
        }
        if (grabPickLock)
        {
            inv[currentSlot] = picklock;
            grabPickLock = false;
        }

        if (GameManager.dead)
        {
            UpdateSlots();
        }



        if(activeItem == key || activeItem == picklock)
        {
            GameManager.holdingKey = true;
        }
        else
        {
            GameManager.holdingKey = false;
        }
        if(GameManager.holdingKey && GameManager.removeKey)
        {
            inv[currentSlot] = null;
            Destroy(currentItem);
            GameManager.removeKey = false;
            GameManager.holdingKey = false;
        }

    }
    //////////////   INV ICONS ////////////////////////////
    void UpdateSlots()
    {
        for (int i = 0; i < 4; i++)
        {
            if (inv[i] == flashlight)
            {
                flashlights[i].enabled = true;
            }
            else
            {
                flashlights[i].enabled = false;
            }

            if (inv[i] == key)
            {
                keys[i].enabled = true;
            }
            else
            {
                keys[i].enabled = false;
            }

            if (inv[i] == picklock)
            {
                pickLocks[i].enabled = true;
            }
            else
            {
                pickLocks[i].enabled = false;
            }
        }
    }
}


*/