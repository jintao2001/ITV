//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ITV.MDL
{
    using System;
    using System.Collections.Generic;
    
    public partial class OPERATORS
    {
        public OPERATORS()
        {
            this.ROLES = new HashSet<ROLES>();
        }
    
        public int OPER_ID { get; set; }
        public string OPER_CODE { get; set; }
        public string OPER_PW { get; set; }
        public string OPER_NAME { get; set; }
        public Nullable<byte> OPER_TYPE { get; set; }
        public string OPER_REG_TIME { get; set; }
        public string OPER_LAST_TIME { get; set; }
        public Nullable<int> OPER_LOGIN_ONCE { get; set; }
        public byte OPER_IS_USE { get; set; }
        public Nullable<int> Orders { get; set; }
    
        public virtual ICollection<ROLES> ROLES { get; set; }
    }
}
