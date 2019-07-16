using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DataLayer
{
    /// <summary>
    /// 数据库操作参数类
    /// </summary>
    [Serializable]
    public partial class AccessParameter
    {
        public AccessParameter()
        {
            m_storedProcedures = new List<string>();
            m_storedParams = new List<Hashtable>();
        }

        /// <summary>
        /// 存储过程名称
        /// </summary>
        private List<string> m_storedProcedures;

        /// <summary>
        /// 存储过程输入参数
        /// </summary>
        private List<Hashtable> m_storedParams;

        /// <summary>
        /// 获取或设置存储过程名称列表
        /// </summary>
        public List<string> StoredProcedures
        {
            get { return m_storedProcedures; }
            set { m_storedProcedures = value; }
        }

        /// <summary>
        /// 获取或设置存储过程输入参数列表
        /// </summary>
        public List<Hashtable> StoredParams
        {
            get { return m_storedParams; }
            set { m_storedParams = value; }
        }
    }
}
