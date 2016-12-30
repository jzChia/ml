using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace MLCommon.Entities
{

    [Serializable]
    [Class(Table = "UserInfo", Name = "UserInfo", NameType = typeof(UserInfo))]
    public class UserInfo
    {
        [Id(0, Name = "userId", Column = "userId", TypeType = typeof(String))]
        [Key(1)]
        [Generator(2, Class = "uuid.hex")]
        public virtual  string userId { get; set; }
        [Property]
        public virtual  string userName { get; set; }
        [Property]
        public virtual  string passWord { get; set; }

        /// <summary>
        /// 0:未登录 1:已登陆
        /// </summary>
        [Property]
        public virtual  int isLogined { get; set; }
        /// <summary>
        /// 0:已停用 1:已启用
        /// </summary>
        [Property]
        public virtual  int state { get; set; }
        [Property]
        public virtual  string stuNo { get; set; }

        [Property]
        public virtual  string fullName { get; set; }
        [Property]
        public virtual  string department { get; set; }
        [Property]
        public virtual  string mobilePhone { get; set; }
        [Property]
        public virtual  DateTime createTime { get; set; }
        [Property]
        public virtual  DateTime? lastLoginTime { get; set; }
        [Property]
        public virtual DateTime updateTime { get; set; }
    }
}
