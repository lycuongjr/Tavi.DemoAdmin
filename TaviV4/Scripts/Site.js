$(document).ready(function () {
    $('#btnSave').click(function () {
        $("#formview form").submit();
    });
    $('#btnBack').click(function () {
        window.history.back();
    });
    $("#btnGhiLai").click(function () {
        $("#formview form").submit();
    });
    $("#btnSearch").click(function () {
        $("#formview form").submit();
    });
    $.fn.datepicker.defaults.language = 'vi';

    $(document).on('shown.lte.pushmenu', function () {
        collapseMenu(false);
    })

    $(document).on('collapsed.lte.pushmenu', function () {
        collapseMenu(true);
    })
});

function collapseMenu(isCollapse) {
    $.ajax({
        url: "/Extension/SetCollapseMenu",
        data: { isCollapse: isCollapse },
        dataType: "json",
        success: function (data) {
        },
        complete: function (data) {
        }
    });
}

function formatNumber(num) {
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
}

// set language for select 2
$.fn.select2.amd.define('select2/i18n/vi', [], function () {
    // Vie
    return {
        errorLoading: function () {
            return 'Chưa tải được dữ liệu.';
        },
        inputTooLong: function (args) {
            var overChars = args.input.length - args.maximum;
            var message = 'Tìm kiếm quá nhiều ký tự';
            return message;
        },
        inputTooShort: function (args) {
            var remainingChars = args.minimum - args.input.length;

            var message = 'Tìm kiếm quá ít ký tự';

            return message;
        },
        loadingMore: function () {
            return 'Đang tải dữ liệu…';
        },
        maximumSelected: function (args) {
            var message = 'Giới hạn số lượng lựa chọn';
            return message;
        },
        noResults: function () {
            return 'Không tìm thấy kết quả';
        },
        searching: function () {
            return 'Đang tìm kiếm…';
        }
    };
});

$.fn.select2.defaults.set("language", "vi");

$.fn.select2.amd.require(['select2/selection/search'], function (Search) {
    var oldRemoveChoice = Search.prototype.searchRemoveChoice;

    Search.prototype.searchRemoveChoice = function () {
        oldRemoveChoice.apply(this, arguments);
        this.$search.val('');
    };
});

$.fn.select2.defaults.set("templateResult", templateResult_Select);

function templateResult_Select(data) {
    var html = '<span class="select-item-text">' + data.text + '</span>';
    var desc = data.description ? data.description : data.element ? data.element.dataset['description'] : null;

    if (desc) {
        html += '<span class="select-item-desc">' + desc + '</span>'
    }

    return $("<div>" + html + "</div>");
}


// show or hide loading
jQuery.fn.showLoading = function (time) {
    var overlay = $(this).find(".overlay");
    if (overlay.length <= 0) {
        $(this).append("<div class=\"overlay\">< i class=\"fas fa-2x fa-sync-alt fa-spin\"></i></div>");
        overlay = $(this).find(".overlay");
    }
    overlay.show();
    time = (time == null || time == undefined) ? 30000 : time;
    setTimeout(function () { overlay.hide() }, time);
}

jQuery.fn.hideLoading = function () {
    var overlay = $(this).find(".overlay");
    if (overlay.length > 0) {
        overlay.hide()
    }
}

function dynamicColors() {
    var r = Math.floor(Math.random() * 255);
    var g = Math.floor(Math.random() * 255);
    var b = Math.floor(Math.random() * 255);
    return "rgb(" + r + "," + g + "," + b + ")";
}


$.taviJs = function () { }

// common tavi java script
$.extend(true, taviJs = {
    showDialog: function (options) {
        var defaultOptions = {
            id: 'popup',
            url: '',
            content: '',
            width: 500
        };
        options = $.extend(defaultOptions, options);

        var popup = $('#' + options.id);
        if (!popup.length) {
            $('body').append('<div class="modal fade" id="' + options.id + '"><div class="modal-dialog modal-dialog-centered" style="max-width:' + options.width + 'px"></div></div>');
            popup = $('#' + options.id);
        }
        if (options.url != '') {
            popup.find(".modal-dialog").load(options.url);
        }
        if (options.content != '') {
            popup.find(".modal-dialog").html(options.content);
        }
        popup.modal({ backdrop: 'static', keyboard: false });
        return popup;
    },
    showConfirmDialog: function (content) {
        return taviJs.showDialogCustom({
            id: 'confirmPopup',
            title: 'Xác nhận',
            type: 'warning',
            content: content,
            textYes: 'Tiếp tục'
        });
    },
    showDialogCustom: function (options) {
        var defaultOptions = {
            id: 'popup',
            title: 'Thông tin',
            type: 'default', // default | primary | secondary | info | warning | success | danger
            overlay: false,
            url: '',
            content: '',
            width: 500,
            height: null,
            textYes: 'Ghi lại',
            textNo: 'Đóng'
        };
        options = $.extend(defaultOptions, options);

        // remove old dialog if exists
        var popup = $('#' + options.id);
        if (popup.length) {
            popup.remove();
        }

        // create new dialog
        $('body').append('<div class="modal fade" id="' + options.id + '"><div class="modal-dialog modal-dialog-centered" style="max-width:' + options.width + 'px"></div></div>');
        popup = $('#' + options.id);
        popup.find('.modal-dialog').append('<div class="modal-content"></div>');
        var content = popup.find('.modal-content');

        // overlay
        if (options.overlay) {
            content.append(`<div class="overlay d-flex justify-content-center align-items-center">
                <i class= "fas fa-2x fa-sync fa-spin" ></i></div>`);
        }

        // init header
        content.append(`<div class="modal-header bg-` + options.type + `">
                            <h4 class="modal-title">` + options.title + `</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Đóng">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>`);

        //init body
        content.append(`<div class="modal-body" ` + (options.height != null ? (`style="max-height:` + options.height + `px;overflow-y:auto"`) : ``) + `></div>`);
        if (options.url != '') {
            content.find(".modal-body").load(options.url);
        }
        if (options.content != '') {
            content.find(".modal-body").html(options.content);
        }

        //init footer
        content.append(`<div class="modal-footer justify-content-between">
                            <button type="button" class="btn btn-default cmd-close" data-dismiss="modal">` + options.textNo + `</button>
                            <button type="button" class="btn btn-primary cmd-save">` + options.textYes + `</button>
                        </div>`);
        popup.modal({ backdrop: 'static', keyboard: false });
        return popup;
    },


    // script load danh muc
    url_DanhMuc_CoQuan: "/BoMayToChuc/DanhMuc/dsCoQuan",
    url_DanhMuc_ChucVu: "/BoMayToChuc/DanhMuc/dsChucVu",
    url_DanhMuc_PhongBan: "/BoMayToChuc/DanhMuc/dsPhongBan",
    url_DanhMuc_Tinh: "/DanhMucChung/DonVihanhChinhCap1",
    url_DanhMuc_Huyen: "/DanhMucChung/DonVihanhChinhCap2",
    url_DanhMuc_Xa: "/DanhMucChung/DonVihanhChinhCap3",
    url_DanhMuc_DonViTinh: "/DanhMucChung/DonViTinh",

    // common
    load_DanhMuc: function (url, data, eleId, $form) {
        $.ajax({
            url: url,
            data: data,
            dataType: "html",
            success: function (data) {
                if ($form == undefined)
                    $("#" + eleId).html(data);
                else
                    $form.find("#" + eleId).html(data);
            },
            complete: function (data) {
            }
        });
    },

    // danh muc co quan
    load_DanhMuc_CoQuan: function (data, eleId, $form) {
        this.load_DanhMuc(this.url_DanhMuc_CoQuan, data, eleId, $form);
    },
    // danh muc phong ban
    load_DanhMuc_PhongBan: function (data, eleId) {
        this.load_DanhMuc(this.url_DanhMuc_PhongBan, data, eleId);
    },
    // danh muc chuc vu
    load_DanhMuc_ChucVu: function (data, eleId) {
        this.load_DanhMuc(this.url_DanhMuc_ChucVu, data, eleId);
    },

    // danh muc don vi hanh chinh cap tinh
    load_DanhMuc_Tinh: function (eleId, selectedId, addData) {
        $.ajax({
            url: this.url_DanhMuc_Tinh,
            data: { id: selectedId },
            dataType: "JSON",
            success: function (data) {
                if (addData != undefined) data.unshift(addData);
                data.unshift({ id: "", text: "-- Chọn tỉnh --" })
                $("#" + eleId).empty().select2({
                    data: data
                });
            },
            error: function () {
                toastr.error("Chưa lấy được danh sách tỉnh")
            }
        })
    },

    // danh muc don vi hanh chinh cap tinh
    load_DanhMuc_Huyen: function (eleId, selectedId, parentId, addData) {
        $.ajax({
            url: this.url_DanhMuc_Huyen,
            data: { id: selectedId, parentId: parentId },
            dataType: "JSON",
            success: function (data) {
                if (addData != undefined) data.unshift(addData);
                data.unshift({ id: "", text: "-- Chọn huyện --" })
                $("#" + eleId).empty().select2({
                    data: data
                });
            },
            error: function () {
                toastr.error("Chưa lấy được danh sách huyện")
            }
        })
    },

    // danh muc don vi hanh chinh cap tinh
    load_DanhMuc_Xa: function (eleId, selectedId, parentId, addData) {
        $.ajax({
            url: this.url_DanhMuc_Xa,
            data: { id: selectedId, parentId: parentId },
            dataType: "JSON",
            success: function (data) {
                if (addData != undefined) data.unshift(addData);
                data.unshift({ id: "", text: "-- Chọn xã --" })
                $("#" + eleId).empty().select2({
                    data: data
                });
            },
            error: function () {
                toastr.error("Chưa lấy được danh sách xã")
            }
        })
    },

    // danh muc don vi tinh
    load_DanhMuc_DonViTinh: function (eleId, selectedId) {
        $.ajax({
            url: this.url_DanhMuc_DonViTinh,
            data: { id: selectedId },
            dataType: "JSON",
            success: function (data) {
                $("#" + eleId).select2({
                    data: data,
                    templateResult: function (data) {
                        if (!data.id) {
                            return data.text;
                        }
                        var $temp = $("<span>" + data.text + (data.name != "" ? (" <i>(" + data.name + ")</i>") : "") + "</span>");
                        return $temp;
                    }
                });
            },
            error: function () {
                toastr.error("Chưa lấy được danh sách đơn vị tính")
            }
        })
    },
    //-------------------------------------------------------

    // helper
    displayFileSize: function (fileSize) {
        var sizes = ["B", "KB", "MB", "GB", "TB"];
        var order = 0;
        while (fileSize >= 1024 && order < sizes.length - 1) {
            order++;
            fileSize = fileSize / 1024;
        }
        var result = fileSize.toFixed(2) + " " + sizes[order];
        return result;
    },

    // create chart
    createChart: function (chartData, ele, height) {
        var func = this;
        height = height == undefined || height == "" ? 250 : height;
        $(ele).empty();
        $.each(chartData, function (index, value) {
            if (value.ChartType == "table") {
                var tblContent = $(ele).append(`<div class="chart-table-data" style="min-height: ` + height + `px;"></div>`).children("div").last();
                $(tblContent).html(func.create_DataTable_Chart(chartData, height));
            } else {
                var canvasEle = $(ele).append(`<canvas class="chart-preview" style="min-height: ` + height + `px; height:` + height + `px; max-height:` + height + `px; max-width:100%;"></canvas>`).children("canvas").last();
                if (value.ChartType == "area") {
                    func.create_Area_Chart(value, canvasEle);
                }
                if (value.ChartType == "line") {
                    func.create_Line_Chart(value, canvasEle);
                }
                if (value.ChartType == "pie") {
                    func.create_Pie_Chart(value, canvasEle);
                }
                if (value.ChartType == "bar") {
                    func.create_Bar_Chart(value, canvasEle);
                }
            }
        });
    },

    // function create area chart
    create_Area_Chart: function (data, ele) {
        var areaChartCanvas = $(ele).get(0).getContext('2d');
        var labels = data.Labels;
        var datasets = [];

        var areaChartData = {
            labels: labels,
            datasets: datasets
        };

        var areaChartOptions = {
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        var label = data.datasets[tooltipItem.datasetIndex].label || '';
                        var unit = data.datasets[tooltipItem.datasetIndex].unit || '';

                        if (label) {
                            label += ': ';
                        }
                        label += tooltipItem.yLabel.toLocaleString() + " " + unit;
                        return label;
                    },
                    footer: function (tooltipItem, data) {
                        return data.datasets[tooltipItem[0].datasetIndex].footer || '';
                    }
                },
            },
            maintainAspectRatio: false,
            responsive: true,
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    gridLines: {
                        display: false,
                    }
                }],
                yAxes: [{
                    gridLines: {
                        display: false,
                    }
                }]
            },
            title: {
                display: true,
                text: data.ChartTitle
            }
        }

        if (data.Datasets2 && data.Datasets2.length > 0 && data.Datasets2[0].Data.length > 0) {
            // add yAxes option:
            $.each(data.Datasets2, function (index, value) {
                var color = dynamicColors();
                datasets.push({
                    label: value.Label,
                    borderColor: color,
                    backgroundColor: color,
                    borderborderWidth: 3,
                    data: value.Data,
                    fill: false,
                    type: value.Type,
                    yAxisID: "RIGHT",
                    unit: value.Unit
                })
            });

            $.each(data.Datasets, function (index, value) {
                datasets.push({
                    label: value.Label,
                    backgroundColor: dynamicColors(),
                    data: value.Data,
                    yAxisID: "LEFT",
                    unit: value.Unit,
                    footer: value.Footer,
                })
            });

            areaChartOptions.scales.yAxes = [
                {
                    display: true,
                    id: 'LEFT',
                    type: 'linear',
                    position: 'left',
                },
                {
                    display: true,
                    id: 'RIGHT',
                    type: 'linear',
                    position: 'right',
                    gridLines: {
                        display: false
                    }
                }
            ]
        } else {
            $.each(data.Datasets, function (index, value) {
                datasets.push({
                    label: value.Label,
                    backgroundColor: dynamicColors(),
                    data: value.Data,
                    unit: value.Unit,
                    footer: value.Footer,
                })
            });
        }

        var areaChart = new Chart(areaChartCanvas, {
            type: 'line',
            data: areaChartData,
            options: areaChartOptions
        })
        return areaChart;
    },

    // function create line chart
    create_Line_Chart: function (data, ele) {
        var lineChartCanvas = $(ele).get(0).getContext('2d');
        var labels = data.Labels;
        var datasets = [];

        var areaChartData = {
            labels: labels,
            datasets: datasets
        };

        var areaChartOptions = {
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        var label = data.datasets[tooltipItem.datasetIndex].label || '';
                        var unit = data.datasets[tooltipItem.datasetIndex].unit || '';

                        if (label) {
                            label += ': ';
                        }
                        label += tooltipItem.yLabel.toLocaleString() + " " + unit;
                        return label;
                    },
                    footer: function (tooltipItem, data) {
                        return data.datasets[tooltipItem[0].datasetIndex].footer || '';
                    }
                },
            },
            maintainAspectRatio: false,
            responsive: true,
            legend: {
                display: true
            },
            scales: {
                xAxes: [{
                    gridLines: {
                        display: true,
                    }
                }],
                yAxes: [{
                    gridLines: {
                        display: true,
                    }
                }]
            },
            datasetFill: false,
            title: {
                display: true,
                text: data.ChartTitle
            }
        }

        if (data.Datasets2 && data.Datasets2.length > 0 && data.Datasets2[0].Data.length > 0) {
            // add yAxes option:
            $.each(data.Datasets2, function (index, value) {
                var color = dynamicColors();
                datasets.push({
                    label: value.Label,
                    borderColor: color,
                    backgroundColor: color,
                    borderborderWidth: 3,
                    data: value.Data,
                    fill: false,
                    type: value.Type,
                    yAxisID: "RIGHT",
                    unit: value.Unit
                })
            });

            $.each(data.Datasets, function (index, value) {
                datasets.push({
                    label: value.Label,
                    backgroundColor: dynamicColors(),
                    borderColor: dynamicColors(),
                    data: value.Data,
                    fill: false,
                    yAxisID: "LEFT",
                    footer: value.Footer,
                    unit: value.Unit
                })
            });

            areaChartOptions.scales.yAxes = [
                {
                    display: true,
                    id: 'LEFT',
                    type: 'linear',
                    position: 'left',
                },
                {
                    display: true,
                    id: 'RIGHT',
                    type: 'linear',
                    position: 'right',
                    gridLines: {
                        display: false
                    }
                }
            ]
        } else {
            $.each(data.Datasets, function (index, value) {
                datasets.push({
                    label: value.Label,
                    backgroundColor: dynamicColors(),
                    borderColor: dynamicColors(),
                    data: value.Data,
                    fill: false,
                    unit: value.Unit,
                    footer: value.Footer,
                })
            });
        }


        var lineChart = new Chart(lineChartCanvas, {
            type: 'line',
            data: areaChartData,
            options: areaChartOptions
        })
        return lineChart;
    },

    create_Pie_Chart: function (data, ele) {
        var pieChartCanvas = $(ele).get(0).getContext('2d')
        var labels = data.Labels;
        var datasets = [];
        var backgroundColor = [];
        var flg_setBackGroundColor = true;
        $.each(data.Datasets, function (index, value) {
            if (flg_setBackGroundColor) {
                $.each(value.Data, function (index, value) {
                    backgroundColor.push(dynamicColors());
                })
                flg_setBackGroundColor = false;
            }

            datasets.push({
                label: value.Label,
                backgroundColor: backgroundColor,
                data: value.Data,
                fill: false,
                unit: value.Unit
            })
        });

        var pieData = {
            labels: labels,
            datasets: datasets
        };
        var pieOptions = {
            tooltips: {
                callbacks: {
                    title: function (arr_tooltipItem, data) {
                        var tooltipItem = arr_tooltipItem[0];
                        var lb_text = data.datasets[tooltipItem.datasetIndex].label || '';
                        return lb_text;
                    },
                    label: function (tooltipItem, data) {
                        var lb_text = data.labels[tooltipItem.index];
                        var lb_value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toLocaleString();
                        var unit = data.datasets[tooltipItem.datasetIndex].unit || '';
                        return lb_text + ": " + lb_value + " " + unit;
                    },
                    afterLabel: function (tooltipItem, data) {
                        var sum = 0;
                        var dataArr = data.datasets[tooltipItem.datasetIndex].data;
                        var value = dataArr[tooltipItem.index];
                        dataArr.map(data => {
                            sum += data;
                        });
                        var percentage = (value * 100 / sum).toFixed(2) + "%";
                        return "(" + percentage + ")";
                    },
                },
            },
            maintainAspectRatio: false,
            responsive: true,
            title: {
                display: true,
                text: data.ChartTitle
            }
        }

        var pieChart = new Chart(pieChartCanvas, {
            type: 'pie',
            data: pieData,
            options: pieOptions
        })
        return pieChart;
    },

    create_Bar_Chart: function (data, ele) {
        var barChartCanvas = $(ele).get(0).getContext('2d')
        var labels = data.Labels;
        var datasets = [];

        var barChartOptions = {
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        var label = data.datasets[tooltipItem.datasetIndex].label || '';
                        var unit = data.datasets[tooltipItem.datasetIndex].unit || '';

                        if (label) {
                            label += ': ';
                        }
                        label += tooltipItem.yLabel.toLocaleString() + " " + unit;
                        return label;
                    },
                    footer: function (tooltipItem, data) {
                        return data.datasets[tooltipItem[0].datasetIndex].footer || '';
                    }
                },
            },
            responsive: true,
            maintainAspectRatio: false,
            datasetFill: false,
            title: {
                display: true,
                text: data.ChartTitle
            },
            scales: {
                xAxes: [{
                    //barThickness: 50,  // number (pixels) or 'flex'
                    maxBarThickness: 100 // number (pixels),

                }],
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }

        // if has multi yAxes
        if (data.Datasets2 && data.Datasets2.length > 0 && data.Datasets2[0].Data.length > 0) {
            // add yAxes option:
            $.each(data.Datasets2, function (index, value) {
                var color = dynamicColors();
                datasets.push({
                    label: value.Label,
                    borderColor: color,
                    backgroundColor: color,
                    borderborderWidth: 3,
                    data: value.Data,
                    fill: false,
                    type: value.Type,
                    yAxisID: "RIGHT",
                    unit: value.Unit
                })
            });

            $.each(data.Datasets, function (index, value) {
                datasets.push({
                    label: value.Label,
                    backgroundColor: dynamicColors(),
                    data: value.Data,
                    fill: false,
                    yAxisID: "LEFT",
                    footer: value.Footer,
                    unit: value.Unit
                })
            });

            barChartOptions.scales.yAxes = [
                {
                    display: true,
                    id: 'LEFT',
                    type: 'linear',
                    position: 'left',
                },
                {
                    display: true,
                    id: 'RIGHT',
                    type: 'linear',
                    position: 'right',
                    gridLines: {
                        display: false
                    }
                }
            ]
        } else {
            $.each(data.Datasets, function (index, value) {
                datasets.push({
                    label: value.Label,
                    backgroundColor: dynamicColors(),
                    data: value.Data,
                    fill: false,
                    footer: value.Footer,
                    unit: value.Unit
                })
            });
        }

        var barChartData = {
            labels: labels,
            datasets: datasets
        };

        var barChart = new Chart(barChartCanvas, {
            type: 'bar',
            data: barChartData,
            options: barChartOptions
        })
        return barChart;
    },

    create_DataTable_Chart: function (lstChartData, height) {
        var html = '';
        height = height == undefined || height == "" ? 300 : height;
        if (lstChartData.length > 0) {
            var isFirstTable = true;
            var col_count = 1;
            $.each(lstChartData, function (index, chartData) {
                // create header
                if (isFirstTable) {
                    html += `<div class="table-responsive" style="max-height:` + (height * lstChartData.length) + `px">
                        <table class="table table-bordered table-head-fixed table-dulieu-bieudo" width="100%">
                            <thead>
                                <tr>
                                    <th>Tiêu chí</th>`;

                    col_count += chartData.Datasets.length;

                    $.each(chartData.Datasets, function (dtsetIndex, data) {
                        html += `<th class='text-right'>${data.Label}${data.Unit ? ("<br/><span style='font-weight:normal;font-style:italic'>(" + data.Unit + ")</span>") : ""}</th>`;
                    });

                    if (chartData.Datasets2 && chartData.Datasets2.length > 0 && chartData.Datasets2[0].Data && chartData.Datasets2[0].Data.length > 0) {
                        col_count += chartData.Datasets2.length;

                        $.each(chartData.Datasets2, function (dtsetIndex, data) {
                            html += `<th class='text-right'>${data.Label}${data.Unit ? ("<br/><span style='font-weight:normal;font-style:italic'>(" + data.Unit + ")</span>") : ""}</th>`;
                        });
                    }

                    html += `</tr></thead><tbody>`;
                }

                if (lstChartData.length > 1 && chartData.ChartTitle) {
                    html += `<tr style="background-color:#d3f8f0;"><td colspan="${col_count}"><b>${chartData.ChartTitle}</b></td></tr>`;
                }
                
                $.each(chartData.Labels, function (labelIndex, label) {
                    html += "<tr><td>" + label + "</td>"
                    $.each(chartData.Datasets, function (dtsetIndex, data) {
                        html += "<td class='text-right'>" + data.Data[labelIndex].toLocaleString() + "</td>";
                    });

                    if (chartData.Datasets2 && chartData.Datasets2.length > 0 && chartData.Datasets2[0].Data && chartData.Datasets2[0].Data.length > 0) {
                        $.each(chartData.Datasets2, function (dtsetIndex, data) {
                            html += "<td class='text-right'>" + data.Data[labelIndex].toLocaleString() + "</td>";
                        });
                    }

                    html += "</tr>";
                });
                isFirstTable = false;
            })
            html += `</tbody></table></div>`;
        }
        return html;
    },

    dynamicColors: function () {
        var r = Math.floor(Math.random() * 255);
        var g = Math.floor(Math.random() * 255);
        var b = Math.floor(Math.random() * 255);
        return "rgb(" + r + "," + g + "," + b + ")";
    }
})
// Button CLose
$(document).on('click', '.popover button.close', function () {
    $(this).parents(".popover").popover('hide');
})