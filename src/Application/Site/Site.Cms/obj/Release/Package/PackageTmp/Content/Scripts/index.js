var menusc = 0;
var menuIsScroll = false;
var pageArrays = new Object();
var pageItemIndex = 1;
var curItemIndex = 0;
$(function () {
    InitPage();
    $(window).resize(function () {
        //setTimeout(InitPage,0);
        InitPage();
    });

    //菜单滚动条事件
    $("#nav-menucon").mousewheel(function (eve, dalta) {
        if (menuIsScroll) {
            return;
        }
        menuIsScroll = true;
        MenuScroll(dalta);
    });

    //向上滚动点击事件
    $("#nav-roll-top-btn").click(function () {
        MenuScroll(1);
    });

    //向下滚动点击事件
    $("#nav-roll-bottom-btn").click(function () {
        MenuScroll(-1);
    });

    //菜单事件
    BindMenuEvent();

    //菜单
    $("body").on("click", "#nav-right-inner .menu_listcon .list-group li a", function () {
        var jele = $(this);
        var tit = jele.html();
        OpenNewTabPage(jele.attr("action"), tit, false);
        //e.preventDefault();
        return false;
    });

    //页面tab左按钮
    $("#h-tabcon .left-btn").click(function () {
        var tabItems = $("#h-tabcon ul li");
        var ulWidth = $("#h-tabcon ul").width();
        PageTabConToLeft(ulWidth / tabItems.length);
    });

    //页面tab右按钮
    $("#h-tabcon .right-btn").click(function () {
        var tabItems = $("#h-tabcon ul li");
        var ulWidth = $("#h-tabcon ul").width();
        PageTabConToLeft(-(ulWidth / tabItems.length));
    });

    //页面标签tab事件
    $("body").on("click", "#h-tabcon ul li", function () {
        var jele = $(this);
        if (jele.hasClass("cur")) {
            return;
        }
        ShowPage(parseInt(jele.data("page-num")));
    });

    //关闭页面事件
    $("body").on("click", "#h-tabcon ul li i", function () {
        var jele = $(this);
        var pageNum = parseInt(jele.parent().data("page-num"));
        CloseTabPage(pageNum);
        InitPageTabLayout();
        PageTabConToLeft(jele.outerWidth());
    });

    //全屏事件
    $("#full_screebtn").click(function () {
        FullScreen();
    });

    //刷新当前页面
    $("#ref_curpagebtn").click(function () {
        RefreshCurrentPage();
    });

    //关闭其他页面
    $("#close_otpagebtn").click(function () {
        CloseOtherPage();
    });
});

//打开一个新的页面
function OpenNewTabPage(url, tit, sico) {
    url = $.trim(url);
    tit = $.trim(tit);
    if (url == "") {
        return false;
    }
    if (tit == "") {
        tit = "新页面";
    }
    var liEle = document.createElement("li");
    liEle.className = "cur";
    liEle.innerHTML = tit;
    if (!sico) {
        var icoEle = document.createElement("i");
        icoEle.className = "glyphicon glyphicon-remove";
        liEle.appendChild(icoEle);
    }
    $("#h-tabcon ul li.cur").removeClass("cur");
    $("#h-tabcon ul").css("width", "auto").append(liEle);
    $(liEle).data("page-num", pageItemIndex);
    var pageItem = document.createElement("div");
    pageItem.className = "page_item";
    var iframeEle = document.createElement("iframe");
    iframeEle.setAttribute("src", url);
    iframeEle.setAttribute("border", "0");
    iframeEle.setAttribute("style", "border:none");
    iframeEle.setAttribute("frameborder", "0");
    pageItem.appendChild(iframeEle);
    var pageCon = $("#page_innercon");
    pageCon.children(".page_item").hide();
    pageCon.append(pageItem);
    pageArrays[pageItemIndex] = {
        page: pageItem,
        tab: liEle,
        src: url
    };
    curItemIndex = pageItemIndex;
    pageItemIndex++;
    InitPageTabLayout();
};

//初始化页面布局
function InitPage() {
    var winHeight = $(window).height();
    var headHeight = $("#cms-head").outerHeight();
    var bodyHeight = winHeight - headHeight;
    $("#cms-body").height(bodyHeight);
    if ($("#body-content-col").hasClass('full')) {
        $("#body-content-col").width('100%').height('100%');
    } else {
        //内容
        var bodyWidth = $("#cms-body").width();
        if (bodyWidth >= 800) {
            $("#body-nav-col").show();
            var navWidth = $("#body-nav-col").outerWidth();
            $("#body-content-col").width(bodyWidth - navWidth);
        }
        else {
            $("#body-nav-col").hide();
            $("#body-content-col").width(bodyWidth);
        }
        //		if($("#body-nav-col").is(":visible")){
        //			var navWidth=$("#body-nav-col").outerWidth();
        //			$("#body-content-col").width(bodyWidth-navWidth);
        //		}
        //		else{
        //			$("#body-content-col").width(bodyWidth);
        //		}
        ScrollNav();
    }
    var contentBodyHeight = $("#body-content-col").height();
    var bodyHeadHeight = $("#cbr-head").outerHeight();
    $("#cbr-pagecon").height(contentBodyHeight - bodyHeadHeight);
    InitPageTabLayout();
}

//菜单滚动
function MenuScroll(dalta, noBindEvent) {
    var menuConHeight = $(window).height(); //$("#nav-menucon").height();
    var ulEle = $("#nav-menucon>ul");
    var ulEleHeight = ulEle.height();
    //alert(menuConHeight+","+ulEleHeight);
    var cheight = ulEleHeight - menuConHeight + 89; //菜单高度和容器高度差
    cheight = cheight < 0 ? 0 : cheight;
    menusc += dalta * 160;
    menusc = menusc > 0 ? 0 : menusc;
    menusc = menusc < -cheight ? -cheight : menusc;
    $("#nav-menucon>ul>li .menu_list:visible").hide().css("height", "auto").css("top", "0px");
    $("#nav-menucon>ul li.foc").removeClass("foc");
    ulEle.css("top", menusc + "px");
    if (!noBindEvent) {
        setTimeout(BindMenuEvent, 50);
    }
    menuIsScroll = false;
};

//绑定菜单事件
function BindMenuEvent() {
    $("#nav-menucon>ul>li").each(function (i, e) {
        var jele = $(e);
        jele.find('.menu_list .menul_inner .menugroup_title').each(function (ni, ne) {
            var jnele = $(ne);
            if (jnele.next('.menu_listcon').find('ul li').length <= 0) {
                jnele.next('.menu_listcon').remove();
                jnele.remove();
            }
            else {
                jnele.next('.menu_listcon').find('ul li a').each(function (li, le) {
                    var jlele = $(le);
                    var url = jlele.attr("href");
                    jlele.attr("action", url);
                    jlele.attr("href", 'javascript:void(0)');
                });
            }
        });
        if (jele.find('.menu_list .menul_inner .menugroup_title').length <= 0) {
            jele.remove();
        }
        jele.data("m-num", i);
        jele.click(function () {
            var rightNav = $("#nav-right");
            rightNav.html("");
            var navInner = $('<div id="nav-right-inner" class="wp-100 hp-100"></div>');
            rightNav.html(navInner);
            var menuHtml = jele.find(".menu_list .menul_inner").html();
            navInner.html(menuHtml);
            if (!rightNav.is(':visible')) {
                $('#body-nav-col').removeClass('w-100').addClass('w-190');
                InitPage();
                $("#nav-right").removeClass('hide');
                ScrollNav();
            }
            navInner.accordion({
                collapsible: true,
                heightStyle: "content"
            });
        });
    });
};
//菜单滚动
function ScrollNav() {
    $('#nav-right-inner').slimScroll({
        width: 'auto',
        height: 'auto',
        size: '3px',
        position: 'right',
        color: '#0a85d7',
        alwaysVisible: true,
        opacity: 0.8,
        distance: '0px',
        //start: $('#child_image_element'),
        //railVisible: true,
        railColor: '#222',
        railOpacity: 0.9,
        wheelStep: 10,
        allowPageScroll: false,
        disableFadeOut: false
    });
}

//重置页面Tab按钮布局
function InitPageTabLayout() {
    var tabCon = $("#h-tabcon");
    var tabs = tabCon.find("ul li");
    var allWidth = 0;
    var maxLenthTxt = "";
    tabs.each(function (i, e) {
        var txt = $(e).text();
        if (txt.length > maxLenthTxt.length) {
            maxLenthTxt = txt;
        }
        allWidth += $(e).outerWidth(true);
    });
    allWidth += maxLenthTxt.length;
    tabCon.children("ul").width(allWidth);
    var conWidth = $("#h-tabcon").width();
    var ulWidth = $("#h-tabcon>ul").width();
    if (ulWidth > conWidth) {
        PageTabConToLeft(-(ulWidth - conWidth));
    } else {
        PageTabConToLeft(0);
    }
};

//页面标签滚动
function PageTabConToLeft(value) {
    var conWidth = $("#h-tabcon").width();
    var tabUlEle = $("#h-tabcon>ul");
    var tabUILeft = parseInt(tabUlEle.css("margin-left"));
    if (isNaN(tabUILeft)) {
        tabUILeft = 0;
    }
    var tabUlWidth = tabUlEle.width();
    if (tabUlWidth <= conWidth) {
        //ShowAndHideTabScrollBtn();
        tabUILeft = 0;
    } else {
        tabUILeft += value;
        var cvalue = conWidth - tabUlWidth;
        if (tabUILeft < cvalue) {
            tabUILeft = cvalue;
        }
        if (tabUILeft > 0) {
            tabUILeft = 0;
        }
    }
    tabUlEle.css("margin-left", tabUILeft + "px");
    ShowAndHideTabScrollBtn();
}

//显示隐藏Tab按钮
function ShowAndHideTabScrollBtn() {
    var conWidth = $("#h-tabcon").width();
    var tabUlEle = $("#h-tabcon>ul");
    var ulWidth = tabUlEle.width();
    if (conWidth >= tabUlEle.width()) {
        $("#h-tabcon .left-btn,#h-tabcon .right-btn").hide();
    } else {
        var uiLeft = parseInt(tabUlEle.css("margin-left"));
        if (isNaN(uiLeft)) {
            uiLeft = 0;
        }
        if (uiLeft >= 0) {
            $("#h-tabcon .left-btn").hide();
            $("#h-tabcon .right-btn").show();
        } else if (uiLeft <= conWidth - ulWidth) {
            $("#h-tabcon .left-btn").show();
            $("#h-tabcon .right-btn").hide();
        } else {
            $("#h-tabcon .left-btn,#h-tabcon .right-btn").show();
        }
    }
}

//切换页面
function ShowPage(pageNum) {
    if (isNaN(pageNum) || pageNum <= 0) {
        return;
    }
    var nowPageObj = pageArrays[pageNum];
    if (!nowPageObj) {
        return;
    }
    $("#h-tabcon ul li.cur").removeClass("cur");
    $(nowPageObj.tab).addClass("cur");
    $("#page_innercon .page_item").hide();
    $(nowPageObj.page).show();
    curItemIndex = pageNum;
};

//关闭页面
function CloseTabPage(pageNum) {
    if (isNaN(pageNum) || pageNum <= 0) {
        return;
    }
    var nowPageObj = pageArrays[pageNum];
    if (!nowPageObj) {
        return;
    }
    var jtabEle = $(nowPageObj.tab);
    var prevTab = jtabEle.prev();
    if (!prevTab) {
        prevTab = jtabEle.next();
    }
    jtabEle.remove();
    $(nowPageObj.page).remove();
    pageArrays[pageNum] = null;
    if (prevTab) {
        ShowPage(parseInt(prevTab.data("page-num")));
    }
};

//全屏
function FullScreen() {
    var contentEle = $("#body-content-col");
    contentEle.toggleClass("full");
    InitPage();
};

//刷新当前页面
function RefreshCurrentPage() {
    if (curItemIndex <= 0) {
        return;
    }
    var nowPageObj = pageArrays[curItemIndex];
    if (!nowPageObj) {
        return;
    }
    var iframeEle = $(nowPageObj.page).find("iframe")[0];
    iframeEle.src = nowPageObj.src;
};

//关闭其他页面
function CloseOtherPage() {
    var notDeletePages = new Object();
    for (var p in pageArrays) {
        var pnum = parseInt(p);
        if (pnum != 1 && pnum != curItemIndex) {
            var nowPageObj = pageArrays[p];
            if (!nowPageObj) {
                continue;
            }
            $(nowPageObj.tab).remove();
            $(nowPageObj.page).remove();
            pageArrays[p] = null;
        } else {
            notDeletePages[p] = pageArrays[p];
        }
    }
    pageArrays = notDeletePages;
    var ulWidth = $("#h-tabcon>ul").width();
    InitPageTabLayout();
    PageTabConToLeft(ulWidth);
};

//当前页面重定向
function RedirectCurrentPage(url, tit) {
    var nowPageObj = pageArrays[curItemIndex];
    if (!nowPageObj) {
        return;
    }
    var iframeEle = $(nowPageObj.page).find("iframe")[0];
    iframeEle.src = url;
    nowPageObj.src = url;
    var btnEle = $(nowPageObj.tab).find("i").clone();
    $(nowPageObj.tab).html(tit).append(btnEle);
    InitPageTabLayout();
}