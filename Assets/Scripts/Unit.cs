using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : RTSObject
{
    private Ray visibility;

    [System.Serializable]
    protected struct Bounty
    {
        public string resource;
        public int count;

        public Bounty(string resource, int count)
        {
            this.resource = resource;
            this.count = count;
        }
    }

    [System.Serializable]
    protected struct Attack
    {
        public Attacks attack;
        public DamageTypes type;
        public int damage;

        public Attack(Attacks attack, DamageTypes type, int damage)
        {
            this.attack = attack;
            this.type = type;
            this.damage = damage;
        }
    }

    [System.Serializable]
    protected struct Movement
    {
        public MovementTypes type;
        public float speed;

        public Movement(MovementTypes type, float speed)
        {
            this.type = type;
            this.speed = speed;
        }
    }

    [System.Serializable]
    protected struct Armor
    {
        public ArmorTypes type;
        public float count;

        public Armor(ArmorTypes type, float count)
        {
            this.type = type;
            this.count = count;
        }
    }

    [System.Serializable]
    protected struct Cost
    {
        public ResourceTypes type;
        public int count;

        public Cost(ResourceTypes type, int count)
        {
            this.type = type;
            this.count = count;
        }
    }

    [SerializeField]
    protected List<Bounty> bounties = new List<Bounty>();

    [SerializeField]
    protected List<Attack> attacks = new List<Attack>();

    [SerializeField]
    protected List<Movement> movements = new List<Movement>();

    [SerializeField]
    protected List<Cost> costs = new List<Cost>();

    [SerializeField]
    protected Armor armor = new Armor();

    [SerializeField]
    protected List<Ability> abilities = new List<Ability>();

    [SerializeField]
    protected int level;

    [SerializeField]
    protected float viewDistance;

    [SerializeField]
    protected Sprite icon;

    [SerializeField]
    protected RTSPlayer Owner;

    public override void Awake()
    {
        base.Awake();
        StartVisionCheck();
    }

    public override void Start()
    {
        base.Start();
    }

    protected void StartVisionCheck()
    {
        StopCoroutine("Vision");
        StartCoroutine("Vision");
    }

    protected IEnumerator Vision()
    {
        List<Vector3> points=new List<Vector3>();
        List<Vector3> borderPoints = new List<Vector3>();
        Collider[] blockers= Physics.OverlapSphere(transform.position, viewDistance);
        foreach(Collider block in blockers)
        {
            BoxCollider blocker = block.transform.GetComponent<BoxCollider>();
            if (blocker is null || (!(visibilityBlocker is null ) && blocker.gameObject.Equals(gameObject))) continue;
            Vector3 pos = blocker.transform.position;
            points.Add(new Vector3(pos.x + blocker.size.x * blocker.transform.lossyScale.x / 2, pos.y, pos.z + blocker.size.z * blocker.transform.lossyScale.z / 2));
            points.Add(new Vector3(pos.x - blocker.size.x * blocker.transform.lossyScale.x / 2, pos.y, pos.z + blocker.size.z * blocker.transform.lossyScale.z / 2));
            points.Add(new Vector3(pos.x - blocker.size.x * blocker.transform.lossyScale.x / 2, pos.y, pos.z - blocker.size.z * blocker.transform.lossyScale.z / 2));
            points.Add(new Vector3(pos.x + blocker.size.x * blocker.transform.lossyScale.x / 2, pos.y, pos.z - blocker.size.z * blocker.transform.lossyScale.z / 2));
            yield return null;
        }
        foreach(Vector3 point in points)
        {
            visibility.origin = new Vector3(transform.position.x, point.y, transform.position.z);
            Vector3 original = (point - transform.position);
            original = new Vector3(original.x, 0, original.z).normalized;
            float angle = 10/original.magnitude;
            for (float i = -angle; i <= angle; i += angle) {
                visibility.direction = Quaternion.Euler(0, i, 0) * original;
                RaycastHit rh;
                if(!Physics.Raycast(visibility, out rh, viewDistance)) {
                    borderPoints.Add(visibility.GetPoint(viewDistance));
                    //Debug.DrawLine(visibility.origin, visibility.GetPoint(viewDistance), Color.green, 1);
                }
                else {
                    borderPoints.Add(rh.point);
                    //Debug.DrawLine(visibility.origin, rh.point,Color.green,1);
                }
            }
            yield return null;
        }
        if(!(Owner is null))Owner.visionUpdateRun(points, transform.position, viewDistance);
        StartVisionCheck();
    }
}
