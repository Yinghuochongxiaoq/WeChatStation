/*|========================================================|
  |=============This code is auto by CodeBuilder===========|
  |================ Organization:FreshManIT+  =============|
  |==========Any Question please tell me:FreshManIT========|
  |===https://github.com/FreshManIT/CodeBuilder/issues ====|
  |===============OR Email:qinbocai@sina.cn================|
  |========================================================|
**/

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// wangdaquestion table in MySQL5
    /// </summary>
    [Table("wangdaquestion")]
    public class WangdaquestionModel
    {
        /// <summary>
        /// Id 主键
        /// </summary>
        [Key]
        public Int64 Id { get; set; }
        /// <summary>
        /// ExamId 课程id
        /// </summary>
        public String ExamId { get; set; }
        /// <summary>
        /// QuestionId 问题id
        /// </summary>
        public String QuestionId { get; set; }
        /// <summary>
        /// Content 问题内容
        /// </summary>
        public String Content { get; set; }
        /// <summary>
        /// QuestionAttrCopys 答案
        /// </summary>
        public String QuestionAttrCopys { get; set; }
        /// <summary>
        /// SearchMate 搜索项
        /// </summary>
        public String SearchMate { get; set; }
        /// <summary>
        /// Answer 题目答案
        /// </summary>
        public String Answer { get;set; }
        /// <summary>
        /// 课程id
        /// </summary>
        public String CourseId { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public String CourseName { get; set; }
    }
}