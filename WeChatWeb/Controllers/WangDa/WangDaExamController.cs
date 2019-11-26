using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using FreshCommonUtility.DataConvert;
using Newtonsoft.Json;
using WeChatCommon.CustomerAttribute;
using WeChatCommon.WebHelper;
using WeChatModel.DatabaseModel;
using WeChatModel.Message;
using WeChatService.ContentService;
using WeChatService.WangDa;

namespace WeChatWeb.Controllers.WangDa
{
    [AuthorizeIgnore]
    public class WangDaExamController : BaseController
    {
        /// <summary>
        /// 查询题目答案
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var title = HttpContext.GetStringFromParameters("title");
            if (!string.IsNullOrEmpty(title))
            {
                title = Regex.Replace(title, @"\s", "");
            }
            var server = new ContentService();
            ViewBag.TopNoList = server.GetTopNoContent(1, 20);
            ViewBag.SearchTitle = title;

            List<QuestResponseModel> models = new List<QuestResponseModel>();
            if (!string.IsNullOrEmpty(title))
            {
                WangDaService questionService = new WangDaService();
                var searchDataList = questionService.GetModels(title);
                if (searchDataList != null && searchDataList.Count > 0)
                {
                    searchDataList.ForEach(f =>
                    {
                        QuestResponseModel tempModel = new QuestResponseModel
                        {
                            Answer = f.Answer,
                            Content = f.Content,
                            QuestionAttrCopys = JsonConvert.DeserializeObject<List<QuestRequestCopyModel>>(f.QuestionAttrCopys)
                        };
                        models.Add(tempModel);
                    });
                }
            }
            return View(models);
        }

        /// <summary>
        /// 添加题库信息  
        /// </summary>
        /// <returns></returns>
        public ActionResult PutQuestionInfo()
        {

            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = "响应成功"
            };
            WangDaService service = new WangDaService();
            var examId = HttpContext.GetStringFromParameters("examId");
            var questListStr = HttpContext.GetStringFromParameters("questions");
            var courseId = HttpContext.GetStringFromParameters("courseId");
            var courseName = HttpContext.GetStringFromParameters("courseName");
            if (string.IsNullOrEmpty(questListStr))
            {
                resultMode.Message = "处理失败,请求参数错误";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            questListStr = Regex.Replace(questListStr, @"\s", "");
            List<QuestRequestModel> questionModels = JsonConvert.DeserializeObject<List<QuestRequestModel>>(questListStr);
            if (questionModels == null || questionModels.Count < 1)
            {
                resultMode.Message = "获取题目答案失败";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }

            Dictionary<string, string> chooseItem = new Dictionary<string, string>
            {
                { "0", "A" },
                { "1", "B" },
                { "2", "C" },
                { "3", "D" },
                { "4", "E" },
                { "5", "F" },
                { "6", "H" },
                { "7", "I" },
                { "8", "J" }
            };
            int number = 0;
            questionModels.ForEach(f =>
            {
                var tempOldQuestion = service.GetWangDaQuestionModelByQuestionId(f.Id);
                if (tempOldQuestion == null)
                {
                    number++;
                    var tempAnswer = "";
                    var mateChoose = "";
                    f.QuestionAttrCopys.Sort((left, right) =>
                    {
                        if (DataTypeConvertHelper.ToInt(left.Name) > DataTypeConvertHelper.ToInt(right.Name))
                        {
                            return 1;
                        }
                        return DataTypeConvertHelper.ToInt(left.Name) < DataTypeConvertHelper.ToInt(right.Name) ? -1 : 0;
                    });
                    f.QuestionAttrCopys.ForEach(r =>
                    {
                        r.Name = chooseItem[r.Name];
                        if (r.Type == "0")
                        {
                            tempAnswer += r.Name;
                        }
                        mateChoose += "|" + r.Value;
                    });
                    var tempQuestionModel = new WangdaquestionModel
                    {
                        ExamId = examId,
                        Content = f.Content,
                        QuestionAttrCopys = JsonConvert.SerializeObject(f.QuestionAttrCopys),
                        QuestionId = f.Id,
                        SearchMate = f.Content + mateChoose,
                        Answer = tempAnswer,
                        CourseId = courseId,
                        CourseName = courseName
                    };
                    service.SaveModel(tempQuestionModel);
                }
            });
            resultMode.ResultCode = ResponceCodeEnum.Success;
            resultMode.Message = "感谢你对题目做出的贡献，题目已经全部存入库，本次贡献" + number + "道题目，贡献率：" + ((double)number / questionModels.Count * 100).ToString("#0.00") + " %";
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取题课比例
        /// </summary>
        /// <returns></returns>
        public ActionResult GetQuestionCourseRate()
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = "响应成功"
            };
            var resultDic = new List<dynamic>();
            WangDaService service = new WangDaService();
            var dbDic = service.GetQuestionCourseRate();
            var initCourceDic = new Dictionary<string, Tuple<string, int>>
            {
                {"1a7a41e5-6b7c-4074-b2d7-7c85a857280e", new Tuple<string, int>("SDN与NFV技术介绍",33)},
                {"271080e4-0e39-4f7b-961c-cc0e5bffba2b", new Tuple<string, int>("CDN技术介绍",8)},
                {"33b5fe33-79ba-47dc-bec0-3b7a5ce7b2ce", new Tuple<string, int>("物联网就在你我身边",16)},
                {"46b0c273-0eb2-4cba-9117-af13f751813b", new Tuple<string, int>("人人学IT",38)},
                {"49060db8-fd8a-401e-aaf1-727281900af1", new Tuple<string, int>("探索大数据和人工智能",16)},
                {"73bfb6cd-ad81-4b9e-ac5d-c8694f49eb2d", new Tuple<string, int>("软件开发应知应会",69)},
                {"85400297-7e8b-4dab-88a4-7959cfa0fd10", new Tuple<string, int>("走进安全技术",10)},
                {"a8dfcf08-a8fa-4963-9a6e-44448a540e28", new Tuple<string, int>("5G技术发展与未来应用",8)},
                {"e8b602bb-8731-4b5b-8d4d-67c0f1b2eba7", new Tuple<string, int>("揭秘云计算",56)}
            };
            if (dbDic == null || dbDic.Count < 1)
            {
                resultMode.Message = "查询失败";
                return Json(resultMode, JsonRequestBehavior.AllowGet);
            }
            foreach (var keyValuePair in dbDic)
            {
                if (initCourceDic.ContainsKey(keyValuePair.Key))
                {
                    resultDic.Add(new
                    {
                        name = initCourceDic[keyValuePair.Key].Item1,
                        count = ((double)keyValuePair.Value / initCourceDic[keyValuePair.Key].Item2).ToString("#0.00")
                    });
                }
            }

            resultMode.Data = resultDic;
            resultMode.ResultCode = ResponceCodeEnum.Success;
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
    }
}