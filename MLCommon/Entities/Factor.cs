using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NHibernate.Mapping.Attributes;
using NHibernate.Type;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MLCommon.Entities
{
    [Serializable]
    [Class(Table = "Factor", Name = "Factor", NameType = typeof(Factor))]
    public  class Factor
    {
        [Id(0, Name = "factorId", Column = "factorId", TypeType = typeof(String))]
        [Key(1)]
        [Generator(2, Class = "uuid.hex")]
        public virtual  string factorId { get; set; }
        [Property]
        public virtual  string name { get; set; }
        [Property(Update = false)]
        public virtual  DateTime createTime { get; set; }
        [Property]
        public virtual  DateTime updateTime { get; set; }
        [Property]
        public virtual string author { get; set; }
        [Property]
        public virtual string category { get; set; }
        [Property]
        public virtual  string classification { get; set; }
        [Property]
        public virtual int referenceMatch { get; set; }
        [Property]
        public virtual String detail { get; set; } 
        [XmlIgnore]
        [Bag(0, Name = "prospectingModels", Table = "ProspectingModelFactor", Generic = true, Lazy = CollectionLazy.True)]
        [Key(1,Column = "factorId")]
        [ManyToMany(2, Column = "modelId", ClassType = typeof(ProspectingModel))]
        public virtual IList<ProspectingModel> prospectingModels { get; set; }

        public virtual int frequency { get; set; }
        public virtual double sig { get { return Math.Round(significance,2); } }
        public virtual double significance { get; set; }

        public virtual string displayName
        {
            get { return classification + ":" + category + ":" + name; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || (obj.GetType().Equals(this.GetType())) == false) return false;
            Factor f = obj as Factor;
            if (!String.IsNullOrEmpty(f.factorId) && !String.IsNullOrEmpty(this.factorId))
                return f.factorId.Equals(this.factorId);
            else
                return this.name.Equals(f.name) && this.classification.Equals(f.classification) && this.category.Equals(f.category);
        }

        public override int GetHashCode()
        {
            return (this.factorId+"#"+this.classification+"#"+this.category+"#"+this.name).GetHashCode();
        }

        public virtual Factor clone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream) as Factor;
        }
    }
}
