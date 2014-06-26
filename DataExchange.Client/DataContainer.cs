using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataExchange.Client
{
    public class DataContainer
    {
        /// <summary>
        /// 模拟数据列表
        /// </summary>
        public static List<string> MsgList { get; set; }

        static DataContainer()
        {
            MsgList = new List<string>();
        }
    }
}