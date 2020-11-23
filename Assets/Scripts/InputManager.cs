using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{

    bool isMobile;

    public event System.Action<Tile> OnPointerStay = delegate { };
    public event System.Action<Tile> OnPointerRelease = delegate { };
  

    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            isMobile = true;
        }
        else // as i expect this to be only mobile or desctop this should be good enouth
        {
            isMobile = false;
        }
    }

    private void Start()
    {
        
    }

    void Update()
    {
        if (isMobile)
        {
            if (Input.touchCount > 0 && Input.touchCount < 2)
            {
                /*
                 * this might be better to consider later
                 *for (int i = 0; i < Input.touchCount; ++i)
        {   
                 */
                if (Input.touches[0].phase == TouchPhase.Moved)
                {
                    if(checkTouch(Input.GetTouch(0).position) != null)
                        OnPointerStay(checkTouch(Input.GetTouch(0).position));
                }

                if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    if (checkTouch(Input.GetTouch(0).position) != null)
                        OnPointerRelease(checkTouch(Input.GetTouch(0).position));
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                if (checkTouch(Input.mousePosition) != null)
                    OnPointerStay(checkTouch(Input.mousePosition));
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (checkTouch(Input.mousePosition) != null)
                    OnPointerRelease(checkTouch(Input.mousePosition));
            }
        }

        
    }
    
    Tile checkTouch(Vector3 input)
    {
        Ray ray = Camera.main.ScreenPointToRay(input);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        // Create a particle if hit
        if (hit.collider != null) // only collider in the scene are tiles
        {
            if (hit.transform.GetComponent<Tile>())
            {
                return hit.transform.GetComponent<Tile>();
            }
        }

        return null;
    }
}
