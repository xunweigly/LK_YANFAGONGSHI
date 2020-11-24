using System;
using System.Collections.Generic;
using System.Text;

namespace fuzhu
{
    public class SearchInfo
    {
        public SearchInfo() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段的值</param>
        /// <param name="sqlOperator">字段的Sql操作符号</param>
        //public SearchInfo(string fieldName, object fieldValue, SqlOperator sqlOperator)            
        //{ }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段的值</param>
        /// <param name="sqlOperator">字段的Sql操作符号</param>
        /// <param name="excludeIfEmpty">如果字段为空或者Null则不作为查询条件</param>
        public SearchInfo(string fieldName, object fieldValue, SqlOperator sqlOperator)
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
            SqlOperator = sqlOperator;

        }



        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
       
        public object FieldValue    
        {
            get ; 
            set ; 
        }

        /// <summary>
        /// 字段的Sql操作符号
        /// </summary>
        public SqlOperator SqlOperator
        {
            get;
            set;
        }

        ///// <summary>
        ///// 如果字段为空或者Null则不作为查询条件
        ///// </summary>
        //public bool ExcludeIfEmpty
        //{
        //    get { return excludeIfEmpty; }
        //    set { excludeIfEmpty = value; }
        //}
    }
}
