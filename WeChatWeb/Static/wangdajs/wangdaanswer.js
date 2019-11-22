var examId = '';
$(function () {
    var urls = window.location.href.split('/');
    var courseId = urls[urls.length - 1];
    var versionId = "";
    $.ajax({
        type: "POST",
        dataType: "JSON",
        data: { courseId: courseId },
        url: 'https://wangda.andedu.net/api/v1/course-study/course-front/register',
        success: function (data) {
            versionId = data.versionId;
            $.ajax({
                type: "GET",
                dataType: "JSON",
                data: { courseId: courseId },
                url: 'https://wangda.andedu.net/api/v1/course-study/course-front/chapter-progress?courseId=' + courseId + '&versionId=' + versionId + '&isRegister=false' + '&_=' + new Date().getTime(),
                success: function (data) {
                    var lastInfo = data[data.length - 1].courseChapterSections;
                    examId = lastInfo[0].resourceId;
                    if (lastInfo[0].progress && lastInfo && lastInfo.length > 0) {
                        $.ajax({
                            type: "GET",
                            dataType: "JSON",
                            async: false,
                            url: 'https://wangda.andedu.net/api/v1/exam/exam-register/newenergy-score-detail?examId=' + examId + '&_=' + new Date().getTime(),
                            success: function (data) {
                                var questions = data.paper.questions;
                                var paperStr = "";
                                var answerMap = ["A", "B", "C", "D","E","F","H","I","J"];
                                questions.forEach((item, index) => {
                                    let title = (index + 1) + "、" + item.content;
                                    let answer = "";
                                    let chooseItem = "";
                                    item.questionAttrCopys = item.questionAttrCopys.sort((a, b) => {
                                        return a.name - b.name;
                                    });
                                    item.questionAttrCopys.forEach((aItem, aIndex) => {
                                        if (aItem.type == "0") {
                                            answer = answer + answerMap[aItem.name];
                                        }
                                        if (aIndex % 2 == 0) {
                                            chooseItem = chooseItem + "\n";
                                        }
                                        if (aIndex % 2 == 1) {
                                            chooseItem = chooseItem + "\t";
                                        }
                                        chooseItem = chooseItem + answerMap[aItem.name] + "、" + aItem.value;
                                    });
                                    paperStr += title + "  答案：" + answer + chooseItem + "\n\n";
                                });
                                console.log(paperStr);
                                $.ajax({
                                    type: "POST",
                                    dataType: "JSON",
                                    data: {
                                        examId: examId,
                                        questions: JSON.stringify(questions)
                                    },
                                    async: false,
                                    url: 'http://localhost:8003/WangDaExam/PutQuestionInfo',
                                    success: function (data) {
                                        console.log(data.Message);
                                        console.log('如果你需要可以去 https://aivabc.com/WangDaExam 检索参考答案');
                                    }
                                });
                            }
                        });
                    } else {
                        console.log('没有获取到考试记录');
                    }

                }
            });
        }
    });
});
