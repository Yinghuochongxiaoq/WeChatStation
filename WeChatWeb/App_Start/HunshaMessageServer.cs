using System.Collections.Generic;
using System.Linq;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;

namespace WeChatWeb
{
    public class HunshaMessageServer
    {
        public List<HunshaMessage> GetAllHunshaMessages(string prince)
        {
            List<HunshaMessage> messageList;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                messageList = conn.GetList<HunshaMessage>(new { Prince=prince}).OrderByDescending(f => f.Id).ToList();
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