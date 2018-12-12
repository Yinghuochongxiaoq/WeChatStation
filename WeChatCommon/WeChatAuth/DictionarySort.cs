using System.Collections;

namespace WeChatCommon.WeChatAuth
{
    /// <summary>
    /// 字典排序
    /// </summary>
    public class DictionarySort : IComparer
    {
        public int Compare(object oLeft, object oRight)
        {
            string sLeft = oLeft as string;
            string sRight = oRight as string;
            int iLeftLength = sLeft.Length;
            int iRightLength = sRight.Length;
            int index = 0;
            while (index < iLeftLength && index < iRightLength)
            {
                if (sLeft[index] < sRight[index])
                    return -1;
                if (sLeft[index] > sRight[index])
                    return 1;
                index++;
            }
            return iLeftLength - iRightLength;
        }
    }
}
