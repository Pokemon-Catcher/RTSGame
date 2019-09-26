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
        points.Add(new Vector3(transform.position.x + viewDistance, 0, transform.position.z - viewDistance));
        points.Add(new Vector3(transform.position.x + viewDistance, 0, transform.position.z + viewDistance));
        points.Add(new Vector3(transform.position.x - viewDistance, 0, transform.position.z + viewDistance));
        points.Add(new Vector3(transform.position.x - viewDistance, 0, transform.position.z - viewDistance));
        foreach (Collider block in blockers)
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
            visibility.origin = new Vector3(transform.position.x, point.y+0.1f, transform.position.z);
            Vector3 original = (point - visibility.origin);
            original = new Vector3(original.x, 0.1f, original.z).normalized;
            float angle = 1/original.magnitude;
            for (float i = -angle; i <= angle; i += angle) {
                visibility.direction = Quaternion.Euler(0, i, 0) * original;
                RaycastHit rh;
                if(!Physics.Raycast(visibility, out rh, viewDistance)) {
                    borderPoints.Add(visibility.GetPoint(viewDistance* 1.41421356237f));
                    //Debug.DrawLine(visibility.origin, visibility.GetPoint(viewDistance), Color.green, 1);
                }
                else {
                    borderPoints.Add(rh.point);
                    //Debug.DrawLine(visibility.origin, rh.point,Color.green,1);
                }
            }
            yield return null;
        }
        borderPoints.Sort(new ReverserClass(transform.position));
        int z = 0;
        foreach(Vector3 point in borderPoints)
        {
            z++;
            Debug.Log(point + " " + z + " "+ Vector3.Angle(point, transform.position));
        }
        if (!(Owner is null))Owner.visionUpdateRun(borderPoints, transform.position, viewDistance);
        StartVisionCheck();
    }
    public class ReverserClass : IComparer<Vector3>
    {
        private Vector2 origin;
        public ReverserClass(Vector3 sender)
        {
            origin = new Vector2(sender.x,sender.z);
        }

        int IComparer<Vector3>.Compare(Vector3 x, Vector3 y)
        {
            float angle1 = Vector2.SignedAngle(origin, new Vector2(x.x, x.z)) + 180;
            float angle2 = Vector2.SignedAngle(origin, new Vector2(y.x, y.z)) + 180;
            if (angle1 > angle2) return 1;
            else if (angle1 < angle2) return -1;
            return 0;
        }
    }
}
