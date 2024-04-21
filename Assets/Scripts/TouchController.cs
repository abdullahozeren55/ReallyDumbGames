using UnityEngine;

public class TouchController : MonoBehaviour
{
    public GameObject[] diamondPrefabs; // Prefabınızı sürükleyip bırakın
    public GameObject[] superDiamondPrefabs;
    public GameObject[] otherCollectablePrefabs;
    public float repeatDelay = 0.5f; // Tekrarlama gecikmesi (saniye)
    private BoxCollider2D touchArea; // Dokunma alanı

    private bool isTouchSupported;
    private Vector3 touchPos;

    private float lastTouchTime;
    private int prefabListNumber;
    private int diamondCounter;
    private int randomPrefabNumber;

    void Awake()
    {
        touchArea = GetComponent<BoxCollider2D>();
        diamondCounter = 0;
        lastTouchTime = 0f;
    }

    void Start()
    {
        isTouchSupported = Input.touchSupported;
    }

    void Update()
    {
        if(!GameManager.Instance.isGameOver)
        {
            // Dokunmatik veya fare giriş kontrolü
            if (isTouchSupported ? Input.touchCount > 0 : Input.GetMouseButton(0))
            {
                // Dokunma veya fare pozisyonunu dünya konumuna çevir
                GetTouchPosition();

                // Dokunma alanı içinde mi kontrol et
                if (touchArea.OverlapPoint(touchPos) && Time.time >= lastTouchTime + repeatDelay)
                {
                    // Dokunma veya fare girişine bağlı olarak elmas instantiate et
                    InstantiateDiamond(touchPos);
                }
            }
        }
        
    }

    void InstantiateDiamond(Vector3 position)
    {
        float randomRot = Random.Range(0f, 360f);

        if(diamondCounter < 10)
        {
            prefabListNumber = Random.Range(0, diamondPrefabs.Length);
            Instantiate(diamondPrefabs[prefabListNumber], position, Quaternion.Euler(0f, 0f, randomRot));
            diamondCounter++;
        }
        else
        {
            GetRandomPrefabNumber();

            if(randomPrefabNumber < otherCollectablePrefabs.Length)
            {
                Instantiate(otherCollectablePrefabs[randomPrefabNumber], position, Quaternion.Euler(0f, 0f, randomRot));
            }
            else
            {
                prefabListNumber = Random.Range(0, superDiamondPrefabs.Length);
                Instantiate(superDiamondPrefabs[prefabListNumber], position, Quaternion.Euler(0f, 0f, randomRot));
            }

            
            diamondCounter = 0;
        }

        lastTouchTime = Time.time;
        
    }

    void GetRandomPrefabNumber()
    {
        randomPrefabNumber = Random.Range(0, otherCollectablePrefabs.Length + 1);
    }

    void GetTouchPosition()
    {
        touchPos = isTouchSupported
            ? Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)
            : Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPos.z = 0f;
    }
}
