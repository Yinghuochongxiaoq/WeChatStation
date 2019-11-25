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
                url: 'https://wangda.andedu.net/api/v1/course-study/course-front/chapter-progress?courseId=' + courseId + '&versionId=' + versionId + '&isRegister=false' + '&_=' + new Date().getTime(),
                success: function (data) {
                    if(!data || data.length<1){
                        console.log('\u83b7\u53d6\u8bfe\u7a0b\u8fdb\u5ea6\u5931\u8d25\uff0c\u8bf7\u5237\u65b0\u9875\u9762\u91cd\u8bd5');
                        return;
                    }
                    let resourceIds='';
                    data.forEach((chapter,index)=>{
                        if(chapter && chapter.courseChapterSections && chapter.courseChapterSections.length>0){
                            let lastInfo = chapter.courseChapterSections[chapter.courseChapterSections.length-1];
                            if (lastInfo && lastInfo.sectionType==9 ) {
                                let examId = lastInfo.resourceId;
                                resourceIds+=examId+",";
                            }
                        }
                    });
                    if(resourceIds.length>0){
                        resourceIds=resourceIds.substr(0,resourceIds.length-1);
                    }
                    $.ajax({
                        type: "GET",
                        dataType: "JSON",
                        url: 'https://wangda.andedu.net/api/v1/exam/exam/basic-by-ids',
                        data:{ids:resourceIds,_:new Date().getTime()},
                        success:function(data){
                            if(data && data.length>0){
                                data.forEach((examInfo,index)=>{
                                    if(examInfo && examInfo.examRecord && examInfo.examRecord.status){
                                        let examId = examInfo.id;
                                        getAndPutData(examId);
                                    }
                                });
                            }
                        },
                        fail:function(){
                            console.log('\u83b7\u53d6\u8003\u8bd5\u5217\u8868\u5931\u8d25');
                        }
                    });
                },
                fail: function() {
                    console.log('\u83b7\u53d6\u8bfe\u7a0b\u8fdb\u5ea6\u5931\u8d25\uff0c\u8bf7\u5237\u65b0\u9875\u9762\u91cd\u8bd5');
                }
            });
        },
        fail: function() {
            console.log('\u521d\u59cb\u5316\u5931\u8d25\uff0c\u8bf7\u5237\u65b0\u9875\u9762\u91cd\u8bd5');
        }
    });
});

function getAndPutData(examId) {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        async: false,
        url: 'https://wangda.andedu.net/api/v1/exam/exam-register/newenergy-score-detail?examId=' + examId + '&_=' + new Date().getTime(),
        success: function (data) {
            var questions = data.paper.questions;
            var paperStr = "";
            var answerMap = ["A", "B", "C", "D", "E", "F", "H", "I", "J"];
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
                paperStr += title + "  \u7b54\u6848\uff1a" + answer + chooseItem + "\n\n";
            });
            console.log(paperStr);
            let jsonData=JSON.stringify(questions).replace(/<[^>]+>/g,"");
            $.ajax({
                type: "POST",
                dataType: "JSON",
                data: {
                    examId: examId,
                    questions: jsonData
                },
                async: false,
                url: 'https://aivabc.com/WangDaExam/PutQuestionInfo',
                success: function (data) {
                    console.log(data.Message);
                    console.log('\u5982\u679c\u4f60\u9700\u8981\u53ef\u4ee5\u53bb https://aivabc.com/WangDaExam \u68c0\u7d22\u53c2\u8003\u7b54\u6848');
                }
            });
        }
    });
}
