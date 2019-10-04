using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using EventAggregation;

public class Unit : RTSObject, IDestructable, IItemDropable, ISelectable
{
    private Vector3 previousPosition = Vector3.zero;

    private Ray visibility;
    public UnitVisionInfo unitVisionInfo;

    [System.Serializable]
    protected struct Bounty
    {
        public ResourceTypes resource;
        public int count;

        public Bounty(ResourceTypes resource, int count)
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
        public ResourceTypes types;
        public int cost;

        public Cost(ResourceTypes types, int cost)
        {
            this.types = types;
            this.cost = cost;
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

    //props
    public int Health { get; set; }
    public List<Item> Items { get; set; }
    public Dictionary<string, object> Info { get; set; }

    public bool Selected { get; set; }
    public int MaxHealth { get; set; }

    [Range(0, 5)]
    [SerializeField]
    private int hp;

    protected override void Awake()
    {
        base.Awake();
        if (!(Owner is null)) Owner.units.Add(this);

        EventAggregator.Subscribe<SelectEvent>(OnSelect);
        EventAggregator.Subscribe<MultiSelectEvent>(OnMultiSelect);
        EventAggregator.Subscribe<DeselectEvent>(OnDeselect);
    }

    protected override void Start()
    {
        base.Start();
        GameMode.instance.Units.Add(this);
        Info = new Dictionary<string, object>();
        Items = new List<Item>();
    }

    public void TakeDamage(int count)
    {

    }

    public void DropItem()
    {

    }

    protected override void Update()
    {
        base.Update();
        //VisionCalculation();
        Health = hp;
    }

    protected void OnSelect(IEventBase eventBase)
    {
        SelectEvent ev = eventBase as SelectEvent;
        if(ev != null && ev.hit.collider.gameObject == gameObject)
        {
            Selected = true;
            Debug.Log(Selected);
        }
    }

    protected void OnMultiSelect(IEventBase eventBase)
    {
        MultiSelectEvent selectEvent = eventBase as MultiSelectEvent;

        float x = transform.position.x;
        float z = transform.position.z;

        Vector3 StartSelectingPoint = selectEvent.firstCorner;
        Vector3 EndSelectingPoint = selectEvent.secondCorner;

        if (x > StartSelectingPoint.x && x < EndSelectingPoint.x || (x < StartSelectingPoint.x && x > EndSelectingPoint.x))
        {
            if (z > StartSelectingPoint.z && z < EndSelectingPoint.z || (z < StartSelectingPoint.z && z > EndSelectingPoint.z))
            {
                Selected = true;
                Debug.Log(Selected);
            }
        }
    }

    protected void OnDeselect(IEventBase eventBase)
    {
        DeselectEvent ev = eventBase as DeselectEvent;

        if(ev != null)
        {
            Selected = false;
            Debug.Log(Selected);
        }
    }

    public virtual Dictionary<string, object> GetInfo()
    {
       // Debug.Log("dada");
        //for HUDManager
        Info.Add("attacks", attacks);
        Info.Add("movements", movements);
        Info.Add("armor", armor);
        Info.Add("abilities", abilities);
        Info.Add("level", level);
        Info.Add("icon", icon);
        Info.Add("health", Health);
        Info.Add("maxHealth", MaxHealth);
        Info.Add("items", Items);
        Info.Add("name", Name);

        return Info;
    }

    private void VisionCalculation()
    {
        if (previousPosition != transform.position)
        {
            previousPosition = transform.position;
            List<Vector3> points = new List<Vector3>();
            List<Vector3> borderPoints = new List<Vector3>();
            Collider[] blockers = Physics.OverlapSphere(transform.position, viewDistance);
            points.Add(new Vector3(transform.position.x + viewDistance, transform.position.y, transform.position.z - viewDistance));
            points.Add(new Vector3(transform.position.x + viewDistance, transform.position.y, transform.position.z + viewDistance));
            points.Add(new Vector3(transform.position.x - viewDistance, transform.position.y, transform.position.z + viewDistance));
            points.Add(new Vector3(transform.position.x - viewDistance, transform.position.y, transform.position.z - viewDistance));
            foreach (Collider block in blockers)
            {
                BoxCollider blocker = block.transform.GetComponent<BoxCollider>();
                if (blocker is null || (!(visibilityBlocker is null) && blocker.gameObject.Equals(gameObject))) continue;
                Vector3 pos = blocker.transform.position;
                points.Add(new Vector3(pos.x + blocker.size.x * blocker.transform.lossyScale.x / 2, transform.position.y, pos.z + blocker.size.z * blocker.transform.lossyScale.z / 2));
                points.Add(new Vector3(pos.x - blocker.size.x * blocker.transform.lossyScale.x / 2, transform.position.y, pos.z + blocker.size.z * blocker.transform.lossyScale.z / 2));
                points.Add(new Vector3(pos.x - blocker.size.x * blocker.transform.lossyScale.x / 2, transform.position.y, pos.z - blocker.size.z * blocker.transform.lossyScale.z / 2));
                points.Add(new Vector3(pos.x + blocker.size.x * blocker.transform.lossyScale.x / 2, transform.position.y, pos.z - blocker.size.z * blocker.transform.lossyScale.z / 2));
            }
            int counter = 0;
            foreach (Vector3 point in points)
            {
                counter++;
                visibility.origin = new Vector3(transform.position.x, point.y + 0.1f, transform.position.z);
                Vector3 original = (point - visibility.origin);
                original = new Vector3(original.x, 0.1f, original.z).normalized;
                float angle = 10 / original.magnitude;
                for (float i = -angle; i <= angle; i += angle)
                {
                    if (angle != 0)
                        visibility.direction = Quaternion.Euler(0, i, 0) * original;
                    else
                        visibility.direction = original;
                    RaycastHit rh;
                    if (!Physics.Raycast(visibility, out rh, viewDistance))
                    {
                        borderPoints.Add(visibility.GetPoint(viewDistance * 1.41421356237f));
                        //Debug.DrawLine(visibility.origin, visibility.GetPoint(viewDistance), Color.green, 1);
                    }
                    else
                    {
                        borderPoints.Add(rh.point);
                        //Debug.DrawLine(visibility.origin, rh.point,Color.green,1);
                    }
                }
            }
            borderPoints.Sort(new ReverserClass(transform.position));
            //if (!(Owner is null)) Owner.minimap.VisionUpdateRun(borderPoints, transform.position, viewDistance);
            unitVisionInfo = new UnitVisionInfo(borderPoints, transform.position, viewDistance);
        }
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
            return Mathf.Atan2(origin.x-x.x, origin.y - x.z).CompareTo(Mathf.Atan2(origin.x - y.x, origin.y - y.z));
        }
    }
}
