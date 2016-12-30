using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NHibernate.Mapping.Attributes;

namespace MLCommon.Entities
{
    [Serializable]
    [Class(Table = "ProspectingModel", Name = "ProspectingModel", NameType = typeof(ProspectingModel))]
    public class ProspectingModel
    {
        [Id(0, Name = "modelId", Column = "modelId", TypeType = typeof(String))]
        [Key(1)]
        [Generator(2, Class = "uuid.hex")]
        public virtual  string modelId { get; set; }
        [Property]
        public virtual  string name { get; set; }
        [Property(Update=false)]
        public virtual  DateTime createTime { get; set; }
        [Property]
        public virtual DateTime updateTime { get; set; }
        [Property]
        public virtual String detail { get; set; } 
        [Property]
        public virtual  string author { get; set; }
        [XmlIgnore]
        [Bag(0, Name = "factors", Table = "ProspectingModelFactor",Generic=true, Inverse = true, Cascade = "all",Lazy=CollectionLazy.True)]
        [Key(1,Column="modelId")]
        [ManyToMany(2,Column="factorId",ClassType=typeof(Factor))]
        public virtual IList<Factor> factors { get; set; }
    }
}
