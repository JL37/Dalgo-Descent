using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour, IObjectPooling
{
    [Header("Objects")]
    [SerializeField] TMP_Text m_NameText;
    [SerializeField] Canvas m_Canvas;
    [SerializeField] ParticleSystem m_ParticleSystem;
    [SerializeField] Animator m_ChestAnimation;

    protected GameUI m_GameUI;

    [Header("Pool manager")]
    [SerializeField] string m_PoolManagerStr;
    protected ObjectPoolManager m_ItemPoolManager;

    [Header("Variables to adjust")]
    [SerializeField] int m_MaxFontSize = 80;
    [SerializeField] float m_HeightOffset = 1.5f;

    protected float m_CurrFontSize;

    protected bool m_WithinRange = false;
    protected bool m_Opened = false;
    protected bool m_ItemSpawned = false;

    protected float m_Radian = 0;
    protected int m_AbValue = 0;
    protected int m_Cost = 1;
    protected float m_ImageDimensions = 100;

    protected PlayerStats m_PlayerStats = null;
    protected ItemPickup m_Item = null;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<MeshRenderer>().material.color = Color.yellow;
        m_Canvas.gameObject.SetActive(true);
        m_CurrFontSize = 0;

        //Randomise cost
        m_Cost = 1;
        //m_Cost = Random.Range(15, 30);

        //Set text to the cost
        m_NameText.text = "<color=yellow>$</color>" + m_Cost + "\n<color=yellow>(F)</color>";

        m_GameUI = GameObject.FindGameObjectWithTag("HUD").GetComponent<GameUI>();
        m_PlayerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        //Get pool manager
        m_ItemPoolManager = GameObject.Find(m_PoolManagerStr).GetComponent<ObjectPoolManager>();
    }

    public ParticleSystem GetParticleSystem() { return m_ParticleSystem; }

    public void Initialise()
    {
        m_Cost = Random.Range(15, 30);
        m_ChestAnimation.Play("Closed");
    }

    public void RemoveSpawnedItem()
    {
        m_Item = null;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateNameTextPos();

        if (m_WithinRange && !m_Opened)
            LerpFontSize(m_MaxFontSize);
        else
            LerpFontSize(0);
    }

    private void Update()
    {
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Return when paused
            return;

        m_WithinRange = m_PlayerStats.GetChest() == this ? true : false;

        if (m_Item)
            m_Item.InRange = m_WithinRange;

        //Debug to check if reset works
        //if (Input.GetMouseButtonDown(1))
        //   Reset();
    }

    protected bool AnimationNotPlaying(string name)
    {
        AnimatorStateInfo stateInfo = m_ChestAnimation.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName(name))
            return true;

        if (m_ChestAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            return true;

        return false;
    }

    protected void LerpFontSize(int newSize)
    {
        if (GameStateManager.Get_Instance.CurrentGameState == GameState.Paused) //Return when paused
            return;

        if (m_NameText.fontSize == newSize)
        {
            m_CurrFontSize = newSize;
            return;
        }

        float spd = 0.3f;
        m_CurrFontSize = Mathf.Lerp(m_CurrFontSize, newSize, spd);
        m_NameText.fontSize = (int)m_CurrFontSize;
    }

    protected void UpdateSineGraph(ref Vector2 ogPos)
    {
        //Move angle by speed
        float angleClamp = 315f;
        float xAxisRatio = 100f; //1 bump/ 180 degrees 
        float spd = Mathf.Deg2Rad * 300f * Time.fixedDeltaTime;

        //Sine graoh
        float height = 300f;
        if (Mathf.Deg2Rad * 180 >= m_Radian)
        {
            //Wide tall sine graph
            float y = height * Mathf.Sin(m_Radian);
            float x = (m_Radian / (Mathf.Deg2Rad * 180)) * xAxisRatio;

            ogPos.x += (x * m_AbValue);
            ogPos.y += y;
        } 
        else if (Mathf.Deg2Rad * angleClamp >= m_Radian)
        {
            float y = (height/2) * Mathf.Sin(2 * (m_Radian - Mathf.Deg2Rad * 90));
            float x = (m_Radian / (Mathf.Deg2Rad * 180)) * xAxisRatio;

            ogPos.x += (x * m_AbValue);
            ogPos.y += y;
        }

        m_Radian += spd;
    }

    protected void UpdateNameTextPos()
    {
        //Update positioning of UI above text
        //Offset Y axis
        float offsetPosY = gameObject.transform.position.y + m_HeightOffset;

        //Final position of marker above chest in world space
        Vector3 offsetPos = new Vector3(gameObject.transform.position.x, offsetPosY, gameObject.transform.position.z);

        //Calculate "screen" position (NOT a canvas/recttransform position)
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        //Convert screen pos to canvas/recttransform space (LEAVE CAMERA NULL IF SCREEN SPACE OVERLAY)
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);

        m_NameText.transform.localPosition = canvasPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_PlayerStats = other.GetComponent<PlayerStats>();
            if (!m_PlayerStats)
            {
                print("Player has no player stats!");
                return;
            }

            //m_WithinRange = true;
            m_PlayerStats.AddChest(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //m_WithinRange = false;
            m_PlayerStats.RemoveChest(this);
        }
    }

    public void OnInteract()
    {
        if (!m_WithinRange) //If within range or not
            return;

        if (m_Opened)
        {
            if (m_Item)
                m_Item.OnInteract(); //Interacting with items
            return;
        }

        //Check if player has enough coins la
        if (m_PlayerStats.DeductCoin(m_Cost))
        {
            //SpawnRing();
            m_Opened = true;

            m_ChestAnimation.SetTrigger("Opening");
            StartCoroutine(Coroutine_SpawnRing(1.5f));
        }
        else
        {
            //Show error
            m_GameUI.ShowError("Not enough coins!");
        }
    }

    protected IEnumerator Coroutine_SpawnRing(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        //Spawning of ring
        SpawnRing();

        //Changing particle effects accordingly and then playing it
        Color rarityCol = m_Item.GetItemRef().GetRarityColor();
        var main = m_ParticleSystem.main;
        main.startColor = rarityCol;
        m_ParticleSystem.Play();
    }

    protected void SpawnRing()
    {
        m_ItemSpawned = true;
        m_Opened = true;

        GetComponent<MeshRenderer>().material.color = Color.red;
        print("CHEST HAS BEEN OPENED");

        //Start Create item
        GameObject itemObj = m_ItemPoolManager.GetFromPool();
        itemObj.transform.position = transform.position;
        itemObj.GetComponent<ItemPickup>().Initialise(this);

        m_Item = itemObj.GetComponent<ItemPickup>();
    }
}
