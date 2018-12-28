var ylmap = ylmap || {};
ylmap.init = function () {
    var mapBox = $('.ylMap'),
        mapVal = mapBox.next(".mapVal").find('.address').val(),
        latTarget = Number(mapBox.next(".mapVal").find('.latitude').val()),
        lngTarget = Number(mapBox.next(".mapVal").find('.longitude').val()),
        way = null,
        transit = null,
        driving = null,
        location_point = null;
    var mapInit = function () {
        if (mapBox.size() > 0) {
            var map = new BMap.Map(mapBox.attr("id")),
                point = new BMap.Point(lngTarget, latTarget),
                marker = new BMap.Marker(point);
            window.map = map;
            window.point = point;
            window.marker = marker;
            map.enableScrollWheelZoom();
            map.enableInertialDragging();
            map.centerAndZoom(point, 15);
            map.addOverlay(marker);
            mapOpenInfo();
            marker.addEventListener("click",
                function (e) {
                    mapOpenInfo();
                });
            map.addEventListener("click",
                function (e) {
                    return false;
                });
        }
    },
        mapOpenInfo = function () {
            var data = eval('(' + mapVal + ')');
            mapAddInfo(marker, data);
        },
        mapAddInfo = function (marker_, data) {
            var content_infoWindow = $('<div class="infoWindow"></div>');
            content_infoWindow.append('<h4>' + data.sign_name + '</h4>');
            content_infoWindow.append('<p class="tel"><a href="tel:' +
                data.contact_tel +
                '">' +
                data.contact_tel +
                '</a></p>');
            content_infoWindow.append('<p class="address">' + data.address + '</p>');
            content_infoWindow.append(
                '<div class="window_btn"><button class="open_navigate open_bus" onclick="open_navigate(this)">公交</button><button class="open_navigate open_car" onclick="open_navigate(this)">自驾</button><span class="State"></span></div>');
            var opts = {
                width: 400,
                height: 0,
                title: ' '
            }
            var info = new BMap.InfoWindow(content_infoWindow[0], opts);
            marker_.openInfoWindow(info, map.getCenter());
        };
    open_navigate = function (obj) {
        $(obj).hasClass("open_bus") ? way = 'bus' : way = 'car';
        navigate();
        $('.infoWindow').find('span.State').html('正在定位您的位置！');
    },
        navigate = function () {
            if (window.navigator.geolocation) {
                window.navigator.geolocation.getCurrentPosition(handleSuccess, handleError, {
                    timeout: 10000
                });
            } else {
                alert('sorry！您的设备不支持定位功能');
            }
        },
        handleError = function (error) {
            var msg;
            switch (error.code) {
                case error.TIMEOUT:
                    msg = "获取超时!请稍后重试!";
                    break;
                case error.POSITION_UNAVAILABLE:
                    msg = '无法获取当前位置!';
                    break;
                case error.PERMISSION_DENIED:
                    msg = '您已拒绝共享地理位置!';
                    break;
                case error.UNKNOWN_ERROR:
                    msg = '无法获取当前位置!';
                    break;
            }
            if ($('.infoWindow').find('span.State').length > 0) {
                $('.infoWindow').find('span.State').html(msg);
            } else {
                alert(msg);
            }
        },
        handleSuccess = function (position) {
            var coords = position.coords;
            var lat = coords.latitude;
            var lng = coords.longitude;
            location_point = new BMap.Point(lng, lat);
            $('.infoWindow').find('span.State').html('获取信息成功，正在加载中！');
            if (way == "bus") bus_transit();
            else self_transit();
            mapBox.parent().addClass("open");
            mapBox.parent().find(".close_map").click(function () {
                mapBox.parent().removeClass("open");
                if (transit) transit.clearResults();
                if (driving) driving.clearResults();
                map.reset();
                map.centerAndZoom(point, 15);
                mapBox.parent().find(".close_map").hide();
                mapOpenInfo();
                $(".m-page").on('mousedown touchstart', page_touchstart);
                $(".m-page").on('mousemove touchmove', page_touchmove);
                $(".m-page").on('mouseup touchend mouseout', page_touchend);
                $('.fn-audio').show();
                $('#transit_result').removeClass("open");
                $(".transitBtn").hide();
            });
            $(".m-page").off('mousedown touchstart');
            $(".m-page").off('mousemove touchmove');
            $(".m-page").off('mouseup touchend mouseout');
        };
    $(".m-map .tit p").click(function () {
        mapBox.parent().toggleClass("open");
        if (mapBox.parent().hasClass("open")) {
            $('.fn-audio').hide();
            $(".m-page").off('mousedown touchstart');
            $(".m-page").off('mousemove touchmove');
            $(".m-page").off('mouseup touchend mouseout');
        } else {
            $('.fn-audio').show();
            $(".m-page").on('mousedown touchstart', page_touchstart);
            $(".m-page").on('mousemove touchmove', page_touchmove);
            $(".m-page").on('mouseup touchend mouseout', page_touchend);
        }
    });
    bus_transit = function () {
        if (transit) transit.clearResults();
        if (driving) driving.clearResults();
        if (!location_point) {
            alert('抱歉：定位失败！');
            return;
        }
        $('.fn-audio').hide();
        if (typeof (loadingPageShow) == "function") loadingPageShow();
        $('.infoWindow').find('span.State').html('正在绘制出导航路线');
        var transit_result = $("#transit_result") || $('<div id="transit_result"></div>');
        transit_result.appendTo(mapBox);
        transit = new BMap.TransitRoute(map, {
            renderOptions: {
                map: map,
                panel: "transit_result",
                autoViewport: true
            },
            onSearchComplete: searchComplete
        });
        transit.search(location_point, point);
    },
        self_transit = function () {
            if (transit) transit.clearResults();
            if (driving) driving.clearResults();
            if (!location_point) {
                alert('抱歉：定位失败！');
                return;
            }
            $('.fn-audio').hide();
            if (typeof (loadingPageShow) == "function") loadingPageShow();
            $('.infoWindow').find('span.State').html('正在绘制出导航路线');
            var transit_result = $("#transit_result") || $('<div id="transit_result"></div>');
            transit_result.appendTo(mapBox);
            driving = new BMap.DrivingRoute(map, {
                renderOptions: {
                    map: map,
                    panel: transit_result.attr('id'),
                    autoViewport: true
                },
                onSearchComplete: searchComplete
            });
            driving.search(location_point, point);
        },
        searchComplete = function (result) {
            if (result.getNumPlans() == 0) {
                alert('非常抱歉,未搜索到可用路线');
                map.reset();
                map.centerAndZoom(point, 15);
                mapBox.parent().find(".close_map").hide();
                mapOpenInfo();
                $('#transit_result').removeClass("open").hide();
                $(".transitBtn").hide();
            } else {
                $('#transit_result').addClass("open");
                $('.infoWindow').find('span.State').html('');
                if (!$('.transitBtn').length > 0) {
                    $('#transit_result').after($('<p class="transitBtn close" onclick="transit_result_close()"><a href="javascript:void(0)">关闭</a></p>'));
                    $('#transit_result').after($('<p class="transitBtn bus" onclick="bus_transit()"><a href="javascript:void(0)">公交</a></p>'));
                    $('#transit_result').after($('<p class="transitBtn car" onclick="self_transit()"><a href="javascript:void(0)">自驾</a></p>'));
                }
                mapBox.parent().find(".close_map").show();
                $("#transit_result").addClass("open");
                $(".transitBtn").show();
            }
            if (typeof (loadingPageHide) == "function") loadingPageHide();
            if ($("#transit_result").hasClass("open")) {
                $(".close").find("a").html("关闭");
            } else {
                $(".close").find("a").html("打开");
            }
        };
    transit_result_close = function () {
        if ($("#transit_result").hasClass("open")) {
            $('#transit_result').removeClass("open");
            $(".close").find("a").html("打开");
        } else {
            $('#transit_result').addClass("open");
            $(".close").find("a").html("关闭");
        }
    };
    window.mapInit = mapInit;
    function loadfunction() {
        var script = document.createElement("script");
        script.src = "http://api.map.baidu.com/api?v=1.4&callback=mapInit";
        document.head.appendChild(script);
        var Style = document.createElement("style");
        Style.type = "text/css";
        var style_map = "#transit_result{display:none;position:absolute;top:0;left:0;width:100%;height:100%;z-index:1000;overflow-y:scroll;}" + "#transit_result.open{display:block;}" + "#ylMap img{width:auto;height:auto;}" + "#ylMap .transitBtn {display:none;position:absolute;z-index:3000;}" + "#ylMap .transitBtn a{display:block;width:80px;box-shadow:0 0 2px rgba(0,0,0,0.6) inset, 0 0 2px rgba(0,0,0,0.6);height:80px;border-radius:80%;color:#fff;background:rgba(230,45,36,0.8);text-align:center;line-height:80px;font-size:14px; font-weight:bold}" + "#ylMap .transitBtn.close {top:10px;right:10px;}" + "#ylMap .transitBtn.bus {top:10px;right:110px;}" + "#ylMap .transitBtn.car {top:110px;right:10px;}" + "#ylMap .transitBtn.bus a{background:rgba(28,237,235,0.8);}" + "#ylMap .transitBtn.car a{background:rgba(89,237,37,0.8);}" + ".m-map.open{height:92%;width:100%;}" + "#transit_result h1{font-size:26px!important;}" + "#transit_result div[onclick^='Instance']{background:none!important;}" + "#transit_result span{display:inline-block;font-size:20px;padding:0 5px;}" + "#transit_result table {font-size:20px!important;}" + "#transit_result table td{padding:5px 10px!important;line-height:150%!important;}" + ".infoWindow h4{font-size:14px;}" + ".infoWindow p{margin-bottom:10px;font-size:14px;}" + ".infoWindow .window_btn{font-size:14px;}" + ".infoWindow .window_btn .open_navigate{display:inline-block;padding:10px 15px;margin-right:10px;border:1px solid #ccc;border-radius:6px;text-align:center;cursor:pointer;}" + ".anchorBL{display:none!important;}";
        Style.innerHTML = style_map;
        document.head.appendChild(Style);
    }
    loadfunction();
};