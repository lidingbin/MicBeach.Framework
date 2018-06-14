var searchOptionsDic = new Object();
var ajaxPro = 0;
//打开新窗体
function OpenNewPage(url, tit, sico) {
    window.top.OpenNewTabPage(url, tit, sico);
};

//Ajax全局设置
$.ajaxSetup({
    global: false,
    beforeSend: function (xhr, o) {
        AjaxBeforeSend(xhr, this);
    },
    complete: function () {
        AjaxComplete();
    }
});

function AjaxBeforeSend(xhr, options) {
    ajaxPro++;
    xhr.setRequestHeader("Http-Request-Type", "ajax-request");
    if (options && options.data && (options.data.NotShowLoading || options.data.indexOf("NotShowLoading=true") >= 0)) {

    } else {
        window.ShowLoading();
    }
}

function AjaxComplete() {
    ajaxPro--;
    if (ajaxPro <= 0) {
        window.HideLoading();
    }
}

function SuccessCallback(res) {
    window.HideLoading();
    if (!res) {
        return;
    }
    if (res.Success) {
        if (res.Data && res.Data.Close) {
            art.dialog.close(true);
        }
        SuccessMsg(res.Message);
    } else {
        ErrorMsg(res.Message);
    }
}

function FailedCallback(res) {
    window.HideLoading();
    ErrorMsg("数据提交失败");
}

//分页搜索
function PageSearch(options) {
    if (!options) {
        return;
    }
    var defaults = {
        url: '',
        data: { page: 1, pageSize: 20 },
        listEle: "#tabe_page_list",
        pagerEle: "#pc-foot",
        selectPage: false,
        callback: undefined,
        init: true,
        showPageNum: true
    };
    var pageListId = !options.listEle ? defaults.listEle : options.listEle;
    var searchOptions = searchOptionsDic[pageListId];
    searchOptions = $.extend(true, {}, defaults, options);
    //if (options.data.init==undefined||options.data.init==null) {
    //    options.data.init = true;
    //}
    if (searchOptions.init) {
        searchOptions.data.page = 1;
    }
    //searchOptions = $.extend(true, {}, !searchOptions ? defaults : searchOptions, options);
    if (!searchOptions.url || $.trim(searchOptions.url) == "") {
        return;
    }
    searchOptionsDic[pageListId] = searchOptions;
    $.post(searchOptions.url, searchOptions.data, function (res) {
        var listItemEle = $(searchOptions.listEle);
        //listItemEle.html(res.View);
        var dataValues = JSON.parse(res.Datas);
        ReplaceDataTableData(searchOptions.listEle, dataValues);
        InitDataTableChecked(searchOptions.listEle);
        CreatePageControl(res.TotalCount, searchOptions.data.page, searchOptions.data.pageSize, pageListId);
        //$(searchOptions.pagerEle).html("");
        if (searchOptions.callback) {
            searchOptions.callback(dataValues);
        }
    })
}

//分页控件点击事件
function PagerBtnSearch(page, pageListId) {
    if (isNaN(page) || page <= 0 || !pageListId || $.trim(pageListId) == "") {
        return;
    }
    var searchOptions = searchOptionsDic[pageListId];
    if (!searchOptions || page == searchOptions.data.page) {
        return;
    }
    var newOptions = $.extend(true, {}, searchOptions, { data: { page: page }, init: false });
    PageSearch(newOptions);
}

//生成分页控件
function CreatePageControl(totalCount, currentPage, pageSize, pageListId) {
    var searchOptions = searchOptionsDic[pageListId];
    $(searchOptions.pagerEle + " .pager-ctrol").remove();
    if (isNaN(totalCount) || totalCount <= 0) {
        $(searchOptions.listEle).parent().addClass("b_b_none");
        return;
    }
    $(searchOptions.listEle).parent().removeClass("b_b_none");
    currentPage = isNaN(currentPage) || currentPage < 1 ? 1 : currentPage;
    pageSize = isNaN(pageSize) || pageSize < 1 ? 1 : pageSize;
    var pageCount = Math.ceil(totalCount / pageSize);
    currentPage = currentPage > pageCount ? pageCount : currentPage;
    var isFirstPage = currentPage == 1;
    var isLastPage = currentPage == pageCount;
    var pagerConClass = searchOptions.selectPage ? "pager-ctrol select_pager" : "pager-ctrol";
    pagerConClass += ' row pd-0 mg-0';
    var pagerCon = GetDivByClass(pagerConClass);
    var pcRight = GetDivByClass("pc-right column col-lg-9 col-md-9 txt-right", pagerCon);
    var pcLeft = GetDivByClass("pc-left hidden-sm hidden-xs pdl-5", pagerCon);
    var btnUrl = "javascript:void(0)";
    //if (!isFirstPage) {
    var firstBtn = GetLinkByClass(btnUrl, "", pcRight);
    var prevBtn = GetLinkByClass(btnUrl, "", pcRight);
    firstBtn.innerHTML = "首页";
    prevBtn.innerHTML = "上一页";
    if (isFirstPage) {
        firstBtn.className = "dis";
        prevBtn.className = "dis";
    } else {
        firstBtn.onclick = function () {
            PagerBtnSearch(1, pageListId);
        };
        prevBtn.onclick = function () {
            PagerBtnSearch(currentPage - 1, pageListId);
        };
    }
    //}
    if (searchOptions.showPageNum) {
        if (pageCount <= 10) {
            for (var p = 1; p <= pageCount; p++) {
                var btn = GetLinkByClass(btnUrl, currentPage == p ? "cur" : "", pcRight);
                btn.innerHTML = p;
                btn.onclick = function () {
                    var npage = parseInt(this.innerHTML);
                    PagerBtnSearch(npage, pageListId);
                };
            }
        } else if (currentPage <= 5) {
            for (var p = 1; p <= 8; p++) {
                var btn = GetLinkByClass(btnUrl, currentPage == p ? "cur" : "", pcRight);
                btn.innerHTML = p;
                btn.onclick = function () {
                    var npage = parseInt(this.innerHTML);
                    PagerBtnSearch(npage, pageListId);
                };
            }
            var btnPoint = GetLinkByClass(btnUrl, "", pcRight);
            btnPoint.innerHTML = "...";
            btnPoint.onclick = function () {
                PagerBtnSearch(9, pageListId);
            };
            var lastBtn = GetLinkByClass(btnUrl, "", pcRight);
            lastBtn.innerHTML = pageCount;
            lastBtn.onclick = function () {
                var npage = parseInt(this.innerHTML);
                PagerBtnSearch(npage, pageListId);
            };
        } else {
            var firstBtn = GetLinkByClass(btnUrl, "", pcRight);
            firstBtn.innerHTML = "1";
            firstBtn.onclick = function () {
                var npage = parseInt(this.innerHTML);
                PagerBtnSearch(npage, pageListId);
            };
            var btnPoint = GetLinkByClass(btnUrl, "", pcRight);
            btnPoint.innerHTML = "...";
            btnPoint.onclick = function () {
                PagerBtnSearch(2, pageListId);
            };
            var beginPage = currentPage - 3;
            var endPage = (currentPage + 4) > pageCount ? pageCount : currentPage + 3;
            for (var p = beginPage; p <= endPage; p++) {
                var btn = GetLinkByClass(btnUrl, currentPage == p ? "cur" : "", pcRight);
                btn.innerHTML = p;
                btn.onclick = function () {
                    var npage = parseInt(this.innerHTML);
                    PagerBtnSearch(npage, pageListId);
                };
            }
            if (endPage < pageCount) {
                var btnPoint2 = GetLinkByClass(btnUrl, "", pcRight);
                btnPoint2.innerHTML = "...";
                btnPoint2.onclick = function () {
                    PagerBtnSearch(endPage + 1, pageListId);
                };
                var lastBtn = GetLinkByClass(btnUrl, "", pcRight);
                lastBtn.innerHTML = pageCount;
                lastBtn.onclick = function () {
                    PagerBtnSearch(pageCount, pageListId);
                };
            }
        }
    }
    //if (!isLastPage) {
    var nextBtn = GetLinkByClass(btnUrl, "", pcRight);
    var lastBtn = GetLinkByClass(btnUrl, "", pcRight);
    nextBtn.innerHTML = "下一页";
    lastBtn.innerHTML = "末页";
    if (isLastPage) {
        nextBtn.className = "dis";
        lastBtn.className = "dis";
    } else {
        lastBtn.onclick = function () {
            PagerBtnSearch(pageCount, pageListId);
        };
        nextBtn.onclick = function () {
            PagerBtnSearch(currentPage + 1, pageListId);
        };
    }
    //}

    $(pcLeft).append('共<span class="txt_num">' + totalCount + '</span>条数据<span class="txt-split">|</span>每页显示');
    var selectCon = GetElementByClass("span", "page_select", null);
    var sizeSelect = GetElementByClass("select", "form-control", selectCon);
    var option10 = GetElementByClass("option", "", sizeSelect);
    option10.innerHTML = "10";
    option10.setAttribute("value", 10);

    var option20 = GetElementByClass("option", "", sizeSelect);
    option20.innerHTML = "20";
    option20.setAttribute("value", 20);

    var option50 = GetElementByClass("option", "", sizeSelect);
    option50.innerHTML = "50";
    option50.setAttribute("value", 50);

    $(pcLeft).append(selectCon);
    $(sizeSelect).val(pageSize);
    sizeSelect.onchange = function () {
        var npageSize = parseInt($(this).val());
        if (isNaN(npageSize) || npageSize <= 0) {
            return;
        }
        var newPageCount = Math.ceil(totalCount / npageSize);
        var cnPage = currentPage > newPageCount ? newPageCount : currentPage;
        var newOptions = $.extend(true, {}, searchOptions, { data: { page: cnPage, pageSize: npageSize } });
        PageSearch(newOptions);
    }
    $(searchOptions.pagerEle).append(pagerCon);
    UILayout();
    InitScroll();
}


//创建一个指定Class的Div
function GetDivByClass(className, parentElement) {
    return GetElementByClass("div", className, parentElement);
}

//创建一个指定Id的div
function GetDivById(id, parentElement) {
    return GetElementById("div", id, parentElement);
}

//创建一个指定Class的a标签
function GetLinkByClass(href, className, parentElement) {
    var linkElement = GetElementByClass("a", className, parentElement);
    linkElement.href = href;
    return linkElement;
}

//创建一个指定Id的a标签
function GetLinkById(href, id, parentElement) {
    var linkElement = GetElementById("a", id, parentElement);
    linkElement.href = href;
    return linkElement;
}

//创建一个指定Class的Img标签
function GetImgByClass(src, className, parentElement) {
    var imgElement = GetElementByClass("img", className, parentElement);
    imgElement.src = src;
    return imgElement;
}

//创建一个指定Id的img标签
function GetImgById(src, id, parentElement) {
    var imgElement = GetElementById("img", id, parentElement);
    imgElement.src = src;
    return imgElement;
}

//用指定的类名创建一个指定类型的元素对象
function GetElementByClass(tagName, className, parentElement) {
    if (!tagName) {
        return;
    }
    var elementObject = document.createElement(tagName);
    elementObject.className = className;
    if (parentElement) {
        try {
            parentElement.appendChild(elementObject);
        } catch (e) {

        }
    }
    return elementObject;
}

//使用指定的ID创建一个元素对象
function GetElementById(tagName, id, parentElement) {
    if (!tagName) {
        return;
    }
    var elementObject = document.createElement(tagName);
    elementObject.id = id;
    if (parentElement) {
        try {
            parentElement.appendChild(elementObject);
        } catch (e) {

        }
    }
    return elementObject;
}

//表单验证成功事件
function ValidSuccess(label, element) {
    var eleVal = $(element).val();
    if (eleVal == "") {
        return;
    }
    var tipEle = $(element).next("span");
    tipEle.removeClass("error").removeClass("prompt").removeClass("ajax").addClass("success tip").html("");
}

//表单验证失败事件
function ValidError(label, element) {
    var tipEle = $(element).next("span");
    tipEle.removeClass("success").removeClass("prompt").removeClass("ajax").addClass("error tip");
    tipEle.html(label.html());
}

//成功消息
function SuccessMsg(msg) {
    TipMsg(msg, 1);
};

//失败消息
function ErrorMsg(msg) {
    TipMsg(msg, 2);
};

//结果消息
function ResultMsg(res) {
    if (!res) {
        return;
    }
    TipMsg(res.Message, res.Success ? 1 : 2);
    if (!res.success && res.NeedLogin) {
        window.top.RedirectLoginPage();
    }
};

//显示消息
function TipMsg(msg, type) {
    var style = "padding:5px;color:#fff;background:#32d61c";
    switch (type) {
        case 2:
            style = "padding:5px;color:#fff;background:#ee272c";
            break;
    }
    art.dialog.tips2({
        content: msg,
        style: style
    });
};

//询问框
function ConfirmMsg(msg, fun) {
    art.dialog.confirm(msg, fun, function () { });
}

//序列form为Json
function GetFormJson(formEle) {
    if (!formEle) {
        return {};
    }
    var formData = $(formEle).serializeArray();
    var jsonData = '';
    $.each(formData, function (i, e) {
        jsonData += e.name + ':' + e.value;
        if (i < formData.length - 1) {
            jsonData += ',';
        }
    });
    jsonData = '{' + jsonData + '}';
    return JSON.parse(jsonData);
}

//显示Ajax等待框
function ShowLoading() {
    $("#loading_con").show();
}

//关闭Ajax等待框
function HideLoading() {
    $("#loading_con").hide();
}
