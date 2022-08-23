using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player_Script : MonoBehaviour
{
    private Rigidbody2D rig;
    [SerializeField] float movSpeed;
    public Camera cam;
    [SerializeField] Transform firePoint;
    [SerializeField] LineRenderer lineRenderer;
    public int weaponDamage;
    Vector3 lookDir;
    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        //Transforma a posição do mouse de pixel para posição dentro da Unity
        mousePos = Mouse.current.position.ReadValue();
        mousePos = cam.ScreenToWorldPoint(mousePos);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lookDir = mousePos - rig.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rig.rotation = angle;
    }

    public void Walk(InputAction.CallbackContext context)
    {
        rig.velocity = context.ReadValue<Vector2>() * movSpeed;
    }

    //Faz o char atirar do modo hitscan
    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //joga um raio pra frente que detecta colisão com colliders
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, lookDir);
            //se acertar executa take damage do script do inimigo
            if (hit)
            {
                Enemy_Script enemy_script = hit.transform.GetComponent<Enemy_Script>();

                if (enemy_script != null)
                {
                    enemy_script.TakeDamage(weaponDamage);
                }
                //usa a coroutine pra deixar o rastro temporario
                StartCoroutine(lineRendering());
                lineRenderer.SetPosition(0, firePoint.position);
                lineRenderer.SetPosition(1, hit.point);
            }
            else if (!hit)
            {
                StartCoroutine(lineRendering());
                lineRenderer.SetPosition(0, firePoint.position);
                lineRenderer.SetPosition(1, firePoint.position + lookDir * 100f);
            }
        }
    }
    //Coroutine que faz o rastro "piscar"
    IEnumerator lineRendering()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.05f);
        lineRenderer.enabled = false;
    }
}
