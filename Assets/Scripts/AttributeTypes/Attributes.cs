using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("Unit")]
[System.Serializable]
public class Attributes
{
    [System.Serializable]
    public struct Bounty
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
    public struct Attack
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
    public struct Movement
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
    public struct Armor
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
    public struct Cost
    {
        public ResourceTypes types;
        public int cost;

        public Cost(ResourceTypes types, int cost)
        {
            this.types = types;
            this.cost = cost;
        }
    }
    [XmlArray("bounties")]
    [XmlArrayItem("bounty")]
    public List<Bounty> bounties = new List<Bounty>();

    [XmlArray("attacks")]
    [XmlArrayItem("attack")]
    public List<Attack> attacks = new List<Attack>();

    [XmlArray("movements")]
    [XmlArrayItem("movement")]
    public List<Movement> movements = new List<Movement>();

    [XmlArray("costs")]
    [XmlArrayItem("cost")]
    public List<Cost> costs = new List<Cost>();

    [XmlElement("armor")]
    public Armor armor = new Armor();

    //[XmlArray("abilities")]
    //[XmlArrayItem("ability")]
    //public List<Ability> abilities = new List<Ability>();

    [XmlAttribute("level")]
    public int level;
    [XmlAttribute("viewDistance")]
    public float viewDistance;
    //[XmlAttribute("icon")]
    //public Sprite icon;
    //[XmlAttribute("owner")]
    //public RTSPlayer Owner;
}
