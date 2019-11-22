using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using EventAggregation;
using System.Xml.Serialization;

public class Unit : RTSObject, IDestructable, IItemDropable
{
    private Vector3 previousPosition = Vector3.zero;
    private Ray visibility;

    public UnitVisionInfo unitVisionInfo;

    public Attributes attributes = new Attributes();

    public OrderHandler orderHandler;

    //props
    [SerializeField]
    public int health;
    public int Health { get; set; }
    public List<Item> Items { get; set; }

    public int MaxHealth { get; set; }

    protected override void Awake()
    {
        base.Awake();
        if (!(attributes.Owner is null)) attributes.Owner.units.Add(this);
    }

    protected override void Start()
    {
        base.Start();
        GameMode.instance.Units.Add(this);
        Items = new List<Item>();
        //tools.SerializeToXml(attributes, Name);
        //tools.ParseXmlToObject("Assets/UnitAttirbutes/lol.txt", ref attributes);
        //XmlTools.SerializeToXml(attributes, Name);
        orderHandler = GetComponent<OrderHandler>();
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
        Health = health;
    }

    public override Dictionary<string, object> GetInfo()
    {
        info.Add("attacks", attributes.attacks);
        info.Add("movements", attributes.movements);
        info.Add("armor", attributes.armor);
        //info.Add("abilities", attributes.abilities);
        info.Add("level", attributes.level);
        //info.Add("icon", attributes.icon);
        info.Add("health", Health);
        info.Add("maxHealth", MaxHealth);
        info.Add("items", Items);

        return base.GetInfo();
    }

    private void VisionCalculation()
    {
        if (previousPosition != transform.position)
        {
            previousPosition = transform.position;
            List<Vector3> points = new List<Vector3>();
            List<Vector3> borderPoints = new List<Vector3>();
            Collider[] blockers = Physics.OverlapSphere(transform.position, attributes.viewDistance);
            points.Add(new Vector3(transform.position.x + attributes.viewDistance, transform.position.y, transform.position.z - attributes.viewDistance));
            points.Add(new Vector3(transform.position.x + attributes.viewDistance, transform.position.y, transform.position.z + attributes.viewDistance));
            points.Add(new Vector3(transform.position.x - attributes.viewDistance, transform.position.y, transform.position.z + attributes.viewDistance));
            points.Add(new Vector3(transform.position.x - attributes.viewDistance, transform.position.y, transform.position.z - attributes.viewDistance));
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
                    if (!Physics.Raycast(visibility, out rh, attributes.viewDistance))
                    {
                        borderPoints.Add(visibility.GetPoint(attributes.viewDistance * 1.41421356237f));
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
            unitVisionInfo = new UnitVisionInfo(borderPoints, transform.position, attributes.viewDistance);
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
