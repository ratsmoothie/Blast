using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public WeaponScriptableObject weaponStats;

    protected Vector3 projectileDirection;
    public float durationUntilDestroy;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, durationUntilDestroy);
        weaponStats = FindObjectOfType<WeaponScriptableObject>();
    }

    public void ProjectileDirectionFixer(Vector3 direction)
    {
        projectileDirection = direction;

        float directionX = projectileDirection.x;
        float directionY = projectileDirection.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        //default direction is right so no need to orient it for that one
        if (directionX < 0 && directionY == 0) //face left
        {
            scale.x *= -1;
            scale.y *= -1;
        }
        else if (directionX > 0 && directionY == 0) //face right
        {
            //default
        }
        else if (directionX == 0 && directionY < 0) //down
        {
            scale.y *= -1;
        }
        else if (directionX == 0 && directionY > 0) //up
        {
            scale.x *= -1;
        }
        else if (directionX > 0 && directionY > 0) //up-right
        {
            rotation.z = 0f;
        }
        else if (directionX > 0 && directionY < 0) //down-right
        {
            rotation.z = -90f;
        }
        else if (directionX < 0 && directionY > 0) //up-left
        {
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = -90f;
        }
        else if (directionX < 0 && directionY < 0) //down-left
        {
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = 0f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation); //conversion
    }
}
