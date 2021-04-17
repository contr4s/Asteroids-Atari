using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerShip: Singleton<PlayerShip>
{
    [Header("Set in Inspector")]
    public float shipSpeed = 10f;

    [SerializeField] private int _startlives = 3;
    private int _curLives;
    public int Lives 
    { 
        get => _curLives;
        set
        {
            _curLives = value;
            if (_curLives <= 0)
            {
                GameManager.S.GameOver();
            }
            else
            {
                GameManager.S.mangerUI.RefreshUILives(_curLives);
            }
        }
    }

    [SerializeField] private ProjectilePool _projectilePool;

    [SerializeField] private float _immoratalityTime = 2f;
    private bool _immortality = false;

    private Rigidbody _rigid;
    private Animator _animator;
    private Camera _mainCam;

    static public Vector3 position
    {
        get
        {
            return S.transform.position;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _mainCam = Camera.main;

        if (_projectilePool == null)
            _projectilePool = GetComponentInChildren<ProjectilePool>();                   
    }

    private void Start()
    {
        Lives = _startlives;
    }

    void Update()
    {
        float aX = Input.GetAxis("Horizontal");
        float aY = Input.GetAxis("Vertical");

        Vector3 vel = new Vector3(aX, aY);
        if (vel.magnitude > 1)
        {
            // Avoid speed multiplying by sqrt(2) when moving at a diagonal
            vel.Normalize();
        }

        _rigid.velocity = vel * shipSpeed;

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDestroyable destroyable))
        {
            if (!destroyable.CreatedByPlayer && !_immortality)
            {
                Lives--;
                destroyable.DestroyMe();
                StartCoroutine(ImmortalityAfterCollision(_immoratalityTime));
            }               
        }
    }

    private void Fire()
    {
        Vector3 mPos = Input.mousePosition;
        mPos.z = -_mainCam.transform.position.z;
        Vector3 mPos3D = _mainCam.ScreenToWorldPoint(mPos);

        var projectile = _projectilePool.GetAvailableObject();
        projectile.CreatedByPlayer = true;
        projectile.transform.position = transform.position;
        projectile.transform.LookAt(mPos3D);
        projectile.gameObject.SetActive(true);

        GameManager.S.audioManager.PlayShootSound();
    }

    private IEnumerator ImmortalityAfterCollision(float immortalTime)
    {
        _immortality = true;
        _animator.SetBool("wound", true);
        yield return new WaitForSeconds(immortalTime);
        _animator.SetBool("wound", false);
        _immortality = false;
    }
}
