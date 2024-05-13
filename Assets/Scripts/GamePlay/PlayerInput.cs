using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    private Player player;

    [SerializeField] private float highSpeedShootingRate;
    [SerializeField] private float highSpeedShootingTime;

    private float shootingTimer;
    private float startTime;
    private bool isShooting;

    public UnityEvent<float> OnContinuousShooting;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Attack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            player.Nuke();
        }

        // continuous shooting
        if (Input.GetMouseButtonDown(0) && player.IsHighSpeedShootingEnabled())
        {
            StartShooting();
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopShooting();
        }
        HandleContinuousShooting();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 direction = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y, 10));
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        player.Move(new Vector2(horizontalInput, verticalInput), angle);

        //Vector2 direction = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y, 10));
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //player.Move(direction, angle);
    }

    private void StartShooting()
    {
        isShooting = true;
        startTime = Time.time;
    }

    private void StopShooting()
    {
        isShooting = false;
    }

    private void EndShooting()
    {
        isShooting = false;
        shootingTimer = 0f;
        player.DisableHighSpeedShooting();
    }

    private void HandleContinuousShooting()
    {
        if (isShooting)
        {
            shootingTimer += Time.deltaTime;
            if (shootingTimer <= highSpeedShootingTime)
            {
                if (Time.time - startTime >= highSpeedShootingRate)
                {
                    player.Attack();
                    startTime = Time.time;
                }
                OnContinuousShooting.Invoke(1 - shootingTimer / highSpeedShootingTime);
            }
            else
            {
                EndShooting();
            }
        }
    }

    public void ResetHighSpeedShootingTimer()
    {
        shootingTimer = 0;
    }
}
