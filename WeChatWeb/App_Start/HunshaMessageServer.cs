using System.Collections.Generic;
using System.Linq;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;

namespace WeChatWeb
{
    public class HunshaMessageServer
    {
        public List<HunshaMessage> GetAllHunshaMessages()
        {
            List<HunshaMessage> messageList;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                messageList = conn.GetList<HunshaMessage>().ToList();
            }

            return messageList;
        }

        public bool InsertMessage(HunshaMessage model)
        {
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                conn.Insert(model);
            }

            return true;
        }
    }
}