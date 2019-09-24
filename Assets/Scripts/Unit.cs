using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : RTSObject
{
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

    //protected Owner ??

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }
}
