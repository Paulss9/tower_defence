using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;

    //separate visually in the unity panel the specific turret attributes
    [Header("General")]

    public float range = 15f;

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Use Laser")]
    public bool useLaser = false;

    public int damageOverTime = 30;
    public float slowAmount = .5f;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("UnitySetupFields")]

    public string enemyTag = "Enemy";

    public Transform partToRotate;
    public float turnSpeed = 10f;

    
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    //find target function
    void UpdateTarget()
    {
        //find all of our enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        //for each enemy found, get distance, see if distance is shortest
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            //if we're using the laser but target is not existant disable the laser beam
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
                    
            }

            return;
        }

        LockOnTarget();

        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
        
    }

    void LockOnTarget()
    {
        //target lock on
        //calculate the direction in which I want the turret to look
        Vector3 dir = target.position - transform.position;
        // compute how the roation needs to happen based on the direction
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        //convert back to vector3 / eurler angles the rotation
        //added lerping functionality to smoothen out the rotation
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        //moving the part to rotate
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Laser()
    {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        //activate the laser beam if it's disabled (it can get disabled when there is no target)
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            //this is how you indicate the start playing the particle animation
            impactEffect.Play();
            // enable light effect
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        //generate the vector betweeb enemy and the firing point
        Vector3 dir = firePoint.position - target.position;

        //set the position of the effect with a delta of vector norm,
        //we do this because we want the effect to not spawn in the middle of the enemy but right outside the radius of the enemy, 
        //note that the enemy radius is 2
        //dir.normalized returns the vector with a magnitude of one
        impactEffect.transform.position = target.position + dir.normalized;

        //this will set the orientation of the impact effect to the dir vector
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void Shoot ()
    {
        GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
