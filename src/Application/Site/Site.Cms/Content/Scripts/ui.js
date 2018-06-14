var BodyScroll = true; //启用body滚动条
var HSearchScroll = false; //启用高级搜索滚动条
var TabConentScroll = false; //Tab内容启用滚动条
var HasDivPageCon = false; //页面有DivPage
var IsDialogPage = false; //页面作为弹框页面
var PositionListTable = false;//定位列表表格
var MinPageWidth = -1;
var pageTables = new Object();//数据表格
var LayoutCallbackEvent = null;//布局回调事件
$(function () {
    UILayout();
    $(window).resize(function () {
        setTimeout(function () {
            UILayout();
        }, 0);
    });
    //高级搜索
    $("body").on("click", ".searchboxbtn", function () {
        OpenSearchBox();
    });
    $("body").on("click", "#hsearch-box #hsb-head .c_btn", function () {
        CloseSearchBox();
    });
    CloseSearchBox();
    //时间日期
    $("body").on("click", ".Date", function () {
        WdatePicker()
    });
    $("body").on("click", ".DateTime", function () {
        WdatePicker({
            dateFmt: "yyyy-MM-dd HH:mm:ss"
        })
    });

    //Tab标签
    $('body').on('click', '.nav-tabs li a', function () {
        if (TabConentScroll) {
            setTimeout(function () {
                $(window).resize();
            }, 0);
        }
    });

    //列表选择框事件
    $("body").on('click', '.cbx_all', function () {
        var checked = $(this)[0].checked;
        var parentWrapper = $(this).parents('.DTFC_ScrollWrapper').first();
        parentWrapper.find('.dataTables_scroll .dataTables_scrollBody table tbody tr td .cbx_val').each(function (i, e) {
            $(e)[0].checked = checked;
        });
        parentWrapper.find('.DTFC_LeftWrapper .DTFC_LeftBodyWrapper .DTFC_LeftBodyLiner table tbody tr td .cbx_val').each(function (i, e) {
            $(e)[0].checked = checked;
        });
    });
    $("body").on("click", '.DTFC_LeftWrapper .DTFC_LeftBodyWrapper .DTFC_LeftBodyLiner table tbody tr td .cbx_val', function () {
        //var checked=$(this)[0].checked;
        var parentTable = $(this).parents('table').first();
        var allChecked = parentTable.find('.cbx_val:checked').length >= parentTable.find('.cbx_val').length;
        parentTable.parents('.DTFC_LeftWrapper').first().find('.DTFC_LeftHeadWrapper table thead tr th .cbx_all').each(function (i, e) {
            e.checked = allChecked;
        });
    });
});

//页面布局
function UILayout() {
    if (MinPageWidth > 0) {
        $("body").css("min-width", MinPageWidth + 'px');
    }
    var headHeight = $("#pc-head").outerHeight();
    var footHeight = $("#pc-foot").outerHeight();
    var bodyHeight = $('body').height();
    $("#pc-body").height(bodyHeight - headHeight - footHeight);

    //局部排版
    if (HasDivPageCon) {
        $(".c_pagecon").each(function (i, e) {
            var jele = $(e);
            var nowStyle = jele.attr("style");
            jele.show();
            LayoutDivPage(e, i);
            jele.attr("style", nowStyle);
        });
    }
    InitScroll();
    InitDrawPageDataTable();//表格重绘
    if (LayoutCallbackEvent) {
        LayoutCallbackEvent();
    }
};

//布局Div
function LayoutDivPage(e, i) {
    if (!e) {
        return;
    }
    var jele = $(e);
    jele.show();
    var conHeight = jele.height();
    var headHeight = jele.children('.c_head').outerHeight();
    var footHeight = jele.children('.c_foot').outerHeight();
    var bodyEle = jele.children('.c_body').first();
    bodyEle.height(conHeight - headHeight - footHeight);
    var bodyInner = bodyEle.children(".c_bodyinner").first();
    var bodyHeight = bodyEle.height();
    if (IsDialogPage) {
        bodyHeight -= 108;
    }
    ScrollElement(bodyInner, bodyHeight);
    jele.attr("style", "");
}

//打开高级搜索框
function OpenSearchBox() {
    var boxEle = $("#hsearch-box");
    boxEle.stop();
    boxEle.show();
    //ScrollElement($("#hsb-binner"));
    boxEle.animate({
        "right": "0px"
    }, 500);
};

//关闭高级搜索框
function CloseSearchBox() {
    var boxEle = $("#hsearch-box");
    var boxWidth = boxEle.width();
    boxEle.stop();
    boxEle.animate({
        "right": -boxWidth + "px"
    }, 500, function () {
        boxEle.hide();
    });
};

//初始化滚动条
function InitScroll() {
    //页面滚动条
    if (BodyScroll) {
        ScrollElement($("#pc-body-inner"));
    }
    //高级搜索框
    if (HSearchScroll) {
        ScrollElement($("#hsb-binner"), undefined);
    }
    //tab页滚动条
    if (TabConentScroll) {
        $(".tab-pane-inner").each(function (i, e) {
            ScrollElement($(e));
        });
    }
    //div滚动条
    if (HasDivPageCon) {
        $(".c_pagecon").each(function (i, e) {
            var jele = $(e);
            var bodyEle = jele.children('.c_body').first();
            var bodyInner = bodyEle.find(".c_bodyinner").first();
            var nowStyle = jele.attr("style");
            jele.show();
            ScrollElement(bodyInner, undefined);
            jele.attr("style", nowStyle);
        });
    }
}

//给指定元素添加滚动条
function ScrollElement(e, height) {
    if (!e) {
        return;
    }
    $(e).slimScroll({
        width: 'auto',
        height: isNaN(height) ? 'auto' : height + 'px',
        size: '5px',
        position: 'right',
        color: '#0a85d7',
        alwaysVisible: true,
        opacity: 1,
        distance: '0px',
        //start: $('#child_image_element'),
        //railVisible: true,
        railColor: '#222',
        railOpacity: 0.9,
        wheelStep: 10,
        allowPageScroll: false,
        disableFadeOut: false
    });
};

//弹出页面
function DialogPage(options) {
    var defaultOps = {
        title: "新页面",
        width: "800px",
        height: "500px",
        opacity: 0.6,
        duration: 0,
        background: "#f5f5f5",
        lock: true,
        cancel: true,
        resize: false,
        closeLoading: true,
        ok: function () {
            var iframe = this.iframe.contentWindow;
            if (!iframe.document.body) {
                return false;
            };
            var selValue = iframe.ArtCallback();
            options.okCallback(selValue);
        }
    };
    defaultOps = $.extend(defaultOps, options);
    artDialog.open(defaultOps.url, defaultOps);
};

//初始化数据表
function InitDataTable(options) {
    if (!options) {
        return;
    }
    var tableEle = $(options.TableEle);
    if (tableEle.length <= 0) {
        return;
    }
    var tableContentHeight = tableEle.parent().height() - 34;
    var defaultTableOptions = {
        scrollY: tableContentHeight + 'px',
        fixedColumns: {
            rightColumns: 1
        },
        language: {
            infoEmpty: '',
            emptyTable: '暂无数据...'
        },
        searching: false,
        paging: false,
        autoWidth: true,
        dom: 'rtlp',
        ordering: false,
        sScrollX: '100%',
        sScrollXInner: "110%",
        bScrollCollapse: true,
    };
    var initOptions = $.extend(true, defaultTableOptions, options);
    var table = tableEle.DataTable(initOptions);
    pageTables[options.TableEle] = { options: initOptions, table: table };
}

//初始重绘页面表格
function InitDrawPageDataTable() {
    if (!pageTables) {
        return;
    }
    var tableSelectorArray = new Array();
    for (var t in pageTables) {
        tableSelectorArray.push(t);
    }
    InitDrawNowDataTable(tableSelectorArray);
}

//重新初始化当前表格
function InitDrawNowDataTable(tableSelectorArray) {
    if (!tableSelectorArray || tableSelectorArray.length <= 0) {
        return;
    }
    for (var t in tableSelectorArray) {
        InitDrawSingleDataTable(tableSelectorArray[t]);
    }
}

//重新初始化单个数据表格
function InitDrawSingleDataTable(tableSelector) {
    if (!tableSelector) {
        return;
    }
    var nowTableItem = pageTables[tableSelector];
    if (!nowTableItem) {
        return;
    }
    var tableWrapper = $(tableSelector + "_wrapper:visible");//user_table_wrapper
    if (tableWrapper.length <= 0) {
        return;
    }
    var containerHeight = tableWrapper.parent().height();
    var wapperInner = tableWrapper.find('.DTFC_ScrollWrapper');
    var tableContentHeight = containerHeight - wapperInner.find('.dataTables_scroll .dataTables_scrollHead').first().height();
    wapperInner.height(containerHeight);
    wapperInner.find('.dataTables_scroll .dataTables_scrollBody').css("height", tableContentHeight + "px");
    nowTableItem.table.draw();
    var maxHeight = parseInt(wapperInner.find('.dataTables_scroll .dataTables_scrollBody').css("max-height"));
    wapperInner.find('.dataTables_scroll .dataTables_scrollBody').css("max-height", tableContentHeight + "px");
}

//数据表添加新数据
function AddDataTableData(tableSelector, datas) {
    if (!tableSelector || !datas || datas.length <= 0) {
        return;
    }
    var nowTableItem = pageTables[tableSelector];
    if (!nowTableItem) {
        return;
    }
    nowTableItem.table.rows.add(datas).draw();
}

//清除数据表数据
function ClearDataTableData(tableSelector) {
    if (!tableSelector) {
        return;
    }
    var nowTableItem = pageTables[tableSelector];
    if (!nowTableItem) {
        return;
    }
    nowTableItem.table.clear().draw();
}

//替换数据
function ReplaceDataTableData(tableSelector, datas) {
    if (!tableSelector) {
        return;
    }
    var nowTableItem = pageTables[tableSelector];
    if (!nowTableItem) {
        return;
    }
    nowTableItem.table.clear().rows.add(datas).draw();
}

//获取表格选择数据
function GetDataTableCheckedValues(tableSelector) {
    if (!tableSelector) {
        return new Array();
    }
    var dataArray = new Array();
    var tableWrapper = $(tableSelector + "_wrapper");
    if (tableWrapper.length <= 0) {
        return dataArray;
    }
    tableWrapper.find('.DTFC_ScrollWrapper .DTFC_LeftWrapper .DTFC_LeftBodyWrapper .DTFC_LeftBodyLiner table tbody tr td .cbx_val:checked').each(function (i, e) {
        dataArray.push($(e).val());
    });
    return dataArray;
}

//初始化表格选择控件
function InitDataTableChecked(tableSelector) {
    if (!tableSelector) {
        return;
    }
    var tableWrapper = $(tableSelector + "_wrapper");
    var cbxLength = tableWrapper.find('.DTFC_ScrollWrapper .DTFC_LeftWrapper .DTFC_LeftBodyWrapper .DTFC_LeftBodyLiner table tbody tr td .cbx_val').length;
    var checkedLength = tableWrapper.find('.DTFC_ScrollWrapper .DTFC_LeftWrapper .DTFC_LeftBodyWrapper .DTFC_LeftBodyLiner table tbody tr td .cbx_val:checked').length;
    var allChecked = checkedLength >= cbxLength && cbxLength > 0;
    tableWrapper.find('.DTFC_ScrollWrapper .DTFC_LeftWrapper .DTFC_LeftHeadWrapper table thead tr th .cbx_all').each(function (i, e) {
        e.checked = allChecked;
    });
}
