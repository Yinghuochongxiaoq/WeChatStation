var mySwiper = new Swiper('.swiper-container',
    {
        direction: 'vertical',
        pagination: '.swiper-pagination',
        mousewheelControl: true,
        onInit: function(swiper) {
            swiperAnimateCache(swiper);
            swiperAnimate(swiper);
        },
        onSlideChangeEnd: function(swiper) {
            swiperAnimate(swiper);
        },
        onTransitionEnd: function(swiper) {
            swiperAnimate(swiper);
        },


        watchSlidesProgress: true,

        onProgress: function(swiper) {
            for (var i = 0; i < swiper.slides.length; i++) {
                var slide = swiper.slides[i];
                var progress = slide.progress;
                var translate = progress * swiper.height / 4;
                scale = 1 - Math.min(Math.abs(progress * 0.5), 1);
                var opacity = 1 - Math.min(Math.abs(progress / 2), 0.5);
                slide.style.opacity = opacity;
                es = slide.style;
                es.webkitTransform = es.MsTransform = es.msTransform = es.MozTransform = es.OTransform = es.transform =
                    'translate3d(0,' + translate + 'px,-' + translate + 'px) scaleY(' + scale + ')';

            }
        },

        onSetTransition: function(swiper, speed) {
            for (var i = 0; i < swiper.slides.length; i++) {
                es = swiper.slides[i].style;
                es.webkitTransitionDuration = es.MsTransitionDuration = es.msTransitionDuration =
                    es.MozTransitionDuration = es.OTransitionDuration = es.transitionDuration = speed + 'ms';

            }
        },


    });
//初始化地图
var map = new ylmap.init;
map.mapInit;

/**
 * 绑定事件
 * @param {any} e
 */
function changeOpen(e) {
    $(".swiper-container").on('mousemove touchmove', page_touchmove);
}

/**
 *打开音乐
 * @param {any} e
 */
function page_touchmove(e) {
    //开启声音
    if ($("#car_audio").length > 0 && audio_switch_btn) {
        $("#car_audio")[0].play();
        audio_loop = true;
        $(".audio_close").css("display", "none");
        $(".audio_open").css("display", "inline-block");
    }
}

/**
 * 声音控制
 */
var audio_switch_btn = true;//声音开关控制值
var audio_btn = true, //声音播放完毕
    audio_loop = true, //声音循环
    audioTime = null, //声音播放延时
    audioTimeT = null, //记录上次播放时间
    audio_interval = null, //声音循环控制器
    audio_start = null, //声音加载完毕
    audio_stop = null, //声音是否在停止
    mousedown = null; //PC鼠标控制鼠标按下获取值
//关闭声音
function audio_close() {
    if (audio_btn && audio_loop) {
        audio_btn = false;
        audioTime = Number($("#car_audio")[0].duration - $("#car_audio")[0].currentTime) * 1000;
        if (audioTime < 0) { audioTime = 0; }
        if (audio_start) {
            if (isNaN(audioTime)) {
                audioTime = audioTimeT;
            } else {
                audioTime > audioTimeT ? audioTime = audioTime : audioTime = audioTimeT;
            }
        };
        if (!isNaN(audioTime) && audioTime != 0) {
            audio_btn = false;
            setTimeout(
                function () {
                    $("#car_audio")[0].pause();
                    $("#car_audio")[0].currentTime = 0;
                    audio_btn = true;
                    audio_start = true;
                    if (!isNaN(audioTime) && audioTime > audioTimeT) audioTimeT = audioTime;
                }, audioTime);
        } else {
            audio_interval = setInterval(function () {
                if (!isNaN($("#car_audio")[0].duration)) {
                    if ($("#car_audio")[0].currentTime != 0 &&
                        $("#car_audio")[0].duration != 0 &&
                        $("#car_audio")[0].duration == $("#car_audio")[0].currentTime) {
                        $("#car_audio")[0].currentTime = 0;
                        $("#car_audio")[0].pause();
                        clearInterval(audio_interval);
                        audio_btn = true;
                        audio_start = true;
                        if (!isNaN(audioTime) && audioTime > audioTimeT) audioTimeT = audioTime;
                    }
                }
            },
                20);
        }
    }
}
//页面声音播放
$(function () {
    //获取声音元件
    var btn_au = $(".fn-audio").find(".btn");
    //绑定点击事件
    btn_au.on('click', audio_switch);
    function audio_switch() {
        if ($("#car_audio") == undefined) {
            return;
        }
        if (audio_switch_btn) {
            //关闭声音
            $("#car_audio")[0].pause();
            audio_switch_btn = false;
            $("#car_audio")[0].currentTime = 0;
            btn_au.find("span").eq(1).css("display", "none");
            btn_au.find("span").eq(0).css("display", "inline-block");
        }
        //开启声音
        else {
            audio_switch_btn = true;
            btn_au.find("span").eq(0).css("display", "none");
            btn_au.find("span").eq(1).css("display", "inline-block");
        }
    }
    changeOpen();
});


var data = [];
/**
 * 获取数据
 */
function getAllMessage() {
    $.ajax({
        url: "/Home/GetAllMessage",
        type: "POST",
        success: function (dataStr) {
            Obj.data = dataStr;
        }
    });
}

/**
 * 添加数据
 */
function addMessage(strMessage) {
    var postData = {};
    postData.message = strMessage;
    $.ajax({
        url: "/Home/InserInfo",
        data: postData,
        type: "POST",
        success: function (dataStr) {
        }
    });
}


var data = [];


$.fn.barrage = function (opt) {
    var _self = $(this);

    var opts = { //默认参数
        data: [], //数据列表
        row: 5, //显示行数
        time: 2000, //间隔时间
        gap: 5, //每一个的间隙
        position: 'fixed', //绝对定位
        direction: 'bottom right', //方向
        ismoseoverclose: false, //悬浮是否停止
        ajaxTime: 3000,
    }

    var settings = $.extend({}, opts, opt); //合并参数
    var M = {},
        Obj = {};
    Obj.data = settings.data;
    M.vertical = settings.direction.split(/\s+/)[0]; //纵向
    M.horizontal = settings.direction.split(/\s+/)[1]; //横向
    M.bgColors = ['#edbccc', '#edbce7', '#c092e4', '#9b92e4', '#92bae4', '#92d9e4', '#92e4bc', '#a9e492', '#d9e492', '#e4c892']; //随机背景色数组
    Obj.arrEle = []; //预计存储dom集合数组
    M.barrageBox = $('<div id="barrage" style="z-index:999;max-width:100%;position:' + settings.position + ';' + M.vertical + ':0px;' + M.horizontal + ':0;"></div>'); //存所有弹幕的盒子
    M.timer = null;
    M.ajaxTime = null;
    var createView = function () {
        var randomIndex = Math.floor(Math.random() * M.bgColors.length);
        var ele = $('<a class="overflow-text" target="_blank" style="opacity:0;text-align:' + settings.direction.split(/\s+/)[1] + ';font-size:14px;float:' + settings.direction.split(/\s+/)[1] + ';background-color:' + M.bgColors[randomIndex] + '"; href="javascript:;' + '">' + (Obj.data && Obj.data.length > 0 ? Obj.data[0].Message : "") + '</a>');
        var str = Obj.data.shift();
        if (M.vertical == 'top') {
            ele.animate({
                'opacity': 1,
                'margin-top': settings.gap,
            },
                1000);
            M.barrageBox.prepend(ele);
        } else {
            ele.animate({
                'opacity': 1,
                'margin-bottom': settings.gap,
            },
                1000);
            M.barrageBox.append(ele);
        }
        Obj.data.push(str);
        if (M.barrageBox.children().length > settings.row) {
            M.barrageBox.children().eq(0).animate({
                'opacity': 0,
            },
                300,
                function () {
                    $(this).css({
                        'margin': 0,
                    }).remove();
                });
        }
    }
    M.mouseClose = function () {
        settings.ismoseoverclose &&
            (function () {
                M.barrageBox.mouseover(function () {
                    clearInterval(M.timer);
                    M.timer = null;
                    M.ajaxTime = null;
                }).mouseout(function () {
                    M.timer = setInterval(function () { //循环
                        createView();
                    },
                        settings.time);
                    M.ajaxTime = setInterval(function () {
                        getAllMessage();
                    }, settings.ajaxTime);
                });
            })();
    }
    Obj.close = function () {
        M.barrageBox.remove();
        clearInterval(M.timer);
        clearInterval(M.ajaxTime);
        M.timer = null;
        M.ajaxTime = null;
    }
    Obj.start = function () {
        if (M.timer) return;
        _self.append(M.barrageBox); //把弹幕盒子放到页面中
        createView(); //创建试图并开始动画
        M.timer = setInterval(function () { //循环
            createView();
        },
            settings.time);
        M.ajaxTime = setInterval(function () {
            getAllMessage();
        }, settings.ajaxTime);
        M.mouseClose();
    }
    return Obj;
}

// 数据初始化
var Obj = $('body').barrage({
    data: data, //数据列表
    row: 5, //显示行数
    time: 2000, //间隔时间
    gap: 20, //每一个的间隙
    position: 'fixed', //绝对定位
    direction: 'bottom left', //方向
    ismoseoverclose: true, //悬浮是否停止
});
getAllMessage();
Obj.start();

//添加评论
$("#submit_barraget").click(function () {

    var val = $("#barrage_content").val();
    if (val == '') {
        return;
    }
    //此格式与dataa.js的数据格式必须一致
    var addVal = {
        Message: val
    }
    //添加进数组
    Obj.data.unshift(addVal);
    addMessage(val);
    alert('谢谢您的祝福');
    $("#barrage_content").val('');
    $("#barrage_content").blur();
});