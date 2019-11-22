using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using FreshCommonUtility.DataConvert;
using Newtonsoft.Json;
using Senparc.Weixin;
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
                Dictionary<string, string> chooseItem = new Dictionary<string, string>
                {
                    { "0", "A、" },
                    { "1", "B、" },
                    { "2", "C、" },
                    { "3", "D、" },
                    { "4", "E、" },
                    { "5", "F、" },
                    { "6", "H、" },
                    { "7", "I、" },
                    { "8", "J、" },
                };
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
                        if (!chooseItem.ContainsKey(r.Name))
                        {
                            System.Console.Out.Write(r.Name);
                        }
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
                        Answer = tempAnswer
                    };
                    service.SaveModel(tempQuestionModel);
                }
            });
            resultMode.ResultCode = ResponceCodeEnum.Success;
            resultMode.Message = "感谢你对题目做出的贡献，题目已经全部存入库，本次贡献" + number + "道题目。";
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
    }
}