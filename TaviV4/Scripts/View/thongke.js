$(document).ready(function () {


    var actionName = $("#ActionName").val();

    if (actionName == "CauHinhHienThi") {
        $("#Create").click(function () {
            LoadChiTietCauHinh(0);
        })

        if ($("#dsNguoiDung").length > 0) {
            $('#dsNguoiDung').select2({
                ajax: {
                    url: '/ThongTinTongHop/ThongKe/Get_DanhSachNguoiDung',
                    data: function (params) {
                        return { search: params.term };
                    },
                    delay: 250
                },
                placeholder: "---- Hiển thị theo người dùng ----",
                allowClear: true,
            });

            $("#dsNguoiDung").change(function () {
                LoadDanhSachCauHinh();
            });
        }

        LoadDanhSachCauHinh();
    }
    else if (actionName == "ThongKeChiTiet") {
        var cauhinhId = $("#CauHinhId").val();

        // load chi tiet du lieu bieu do
        $.ajax({
            url: "/ThongTinTongHop/ThongKe/Get_DuLieuBieuDo",
            data: { cauhinhId: cauhinhId },
            dataType: "json",
            success: function (result) {
                if (result == null || result.length == 0) {
                    $("#view-chitiet-bieudo").append("<span>Dữ liệu chưa được cập nhật...</span>");
                    return true;
                }
                taviJs.createChart(result, $("#view-chitiet-bieudo"));
                $("#view-chitiet-bangdulieu").html(taviJs.create_DataTable_Chart(result,250));
            },
            error: function () {
                toastr.error("Có lỗi xảy ra")
            }
        });

        // load du lieu bieu do chi tieu truc thuoc
        if ($("#view-dulieu-tructhuoc-gop").length > 0) {
            $.ajax({
                url: "/ThongTinTongHop/ThongKe/Get_DuLieuBieuDo_ChiTiet_GopBieuDo",
                data: { cauhinhId: cauhinhId },
                dataType: "json",
                success: function (result) {
                    if (result == null || result.length == 0) {
                        return true;
                    }

                    $("#view-dulieu-tructhuoc-gop").append(`<div class="col-md-6"><div class="card card-primary card-outline"><div class="card-body p-1" id="total-chart-detail"></div></div></div>`);
                    var newEle = $("#view-dulieu-tructhuoc-gop").find("#total-chart-detail");
                    taviJs.createChart(result, newEle, 300);
                },
                error: function () {
                    toastr.error("Có lỗi xảy ra")
                }
            });
        } else {
            var listId = $("#listId").val();
            if (listId != undefined && listId != "") {
                var listChiTieuId = listId.split(",");
                $.each(listChiTieuId, function (index, id) {
                    LoadBieuDo_TrucThuoc(cauhinhId, id);
                })
            }
        }

        // load du lieu danh gia
        if ($("#view-chitiet-bieudo-dulieudanhgia").length > 0) {
            $.ajax({
                url: "/ThongTinTongHop/ThongKe/Get_BangDuLieu_DuLieuDanhGia",
                data: { cauhinhId: cauhinhId },
                dataType: "json",
                success: function (result) {
                    if (result == null || result.length == 0) {
                        $("#view-chitiet-bieudo-dulieudanhgia").html("<span>Chưa có dữ liệu đánh giá</span>");
                        return;
                    }

                    $("#view-chitiet-bieudo-dulieudanhgia").html(result);
                },
                error: function () {
                    toastr.error("Có lỗi xảy ra")
                }
            });
        }
    }
    else {
        if ($("#dsNguoiDung").length > 0) {
            $('#dsNguoiDung').select2({
                ajax: {
                    url: '/ThongTinTongHop/ThongKe/Get_DanhSachNguoiDung',
                    data: function (params) {
                        return { search: params.term };
                    },
                    delay: 250
                },
                placeholder: "---- Hiển thị theo người dùng ----",
                allowClear: true,
            });

            $("#dsNguoiDung").change(function () {
                LoadDanhSachBieuDo();
            });
        }

        LoadDanhSachBieuDo();
    }
})

function LoadDanhSachBieuDo() {
    var userId = $("#dsNguoiDung")?.val();

    $.ajax({
        url: "/ThongTinTongHop/ThongKe/Get_DanhSachBaoCao_TheoKyBaoCao",
        data: { PhanHeId: $("#hdPhanHeId").val(), UserId: userId },
        dataType: 'html',
        beforeSend: function () {
            $("#view-content-bieudo").showLoading(30000);
        },
        success: function (data) {
            $("#view-content-bieudo").hideLoading();
            $("#viewBieuDo").html(data);
            $("#viewBieuDo").find(".view-of-chart").each(function () {
                var cauhinhid = $(this).data("cauhinhid");
                LoadBieuDo(cauhinhid, $(this));
            })
        },
        error: function (data) {
            $("#view-content-bieudo").hideLoading();
            toastr.error("Chưa lấy được dữ liệu biểu đồ");
        }
    })
}

function DisplayThoiGianBaoCao(value) {
    if (value == undefined || value == 1) { // ky bao cao nam
        $("#Thang").parent().hide();
        $("#Quy").parent().hide();

    } else if (value == 2) { // ky bao cao quy
        $("#Quy").parent().show();
        $("#Thang").parent().hide();
    } else {
        $("#Quy").parent().hide();
        $("#Thang").parent().show();
    }
    checkPhuongPhapTinh();
}

function GetCauHinhData() {
    var CauHinhId = $("#CauHinhHienThiId").val()
    var PhanHeId = $("#hdPhanHeId").val();
    var DinhNghiaChiTieuIds = $("#DinhNghiaChiTieuIds").val();
    var tinhData = $("#ddlTinh").select2("data")[0];
    var huyenData = $("#ddlHuyen").select2("data")[0];
    var xaData = $("#ddlXa").select2("data")[0];
    var KyBaoCao = $("input[name=KyBaoCao]:checked").val();
    var Nam = $("#Nam").val();
    var Thang = $("#Thang").val();
    var Quy = $("#Quy").val();
    var LoaiDuLieu = $("#LoaiDuLieu").val();
    var DangBieuDo = $("#DangBieuDo").val();
    var ConSuDung = $("#ConSuDung").prop("checked");
    var TenChiTieu = $("#ChiTieuId").val();
    var UuTien = $("#ThongKeUuTien").val();
    var PhuongPhapTinh = $("#PhuongPhapTinh").val();
    var ThuTu = $("#ThuTu").val();
    var GopBieuDo = $("#GopBieuDo").prop("checked");
    var CanBoId = $("#dsNguoiDung").val();
    return {
        CauHinhHienThiId: CauHinhId,
        PhanHeThongTinId: PhanHeId == 0 ? null : PhanHeId,
        DinhNghiaChiTieuIds: DinhNghiaChiTieuIds,
        TinhId: tinhData.id,
        TenTinh: tinhData.id == null || tinhData.id == "" ? null : tinhData.text,
        HuyenId: huyenData.id,
        TenHuyen: huyenData.id == null || huyenData.id == "" ? null : huyenData.text,
        XaId: xaData.id,
        TenXa: xaData.id == null || xaData.id == "" ? null : xaData.text,
        KyBaoCao: KyBaoCao,
        Nam: Nam,
        Thang: Thang,
        Quy: Quy,
        LoaiDuLieu: LoaiDuLieu,
        DangBieuDo: DangBieuDo,
        ConSuDung: ConSuDung,
        TenChiTieu: TenChiTieu,
        ThongKeUuTien: UuTien,
        PhuongPhapTinh: PhuongPhapTinh,
        ThuTu: ThuTu,
        GopBieuDo: GopBieuDo,
        CanBoId: CanBoId
    }
}

function LoadDanhSachCauHinh() {
    var userid = $("#dsNguoiDung")?.val();
    $.ajax({
        url: "/ThongTinTongHop/ThongKe/CauHinhHienThi",
        data: { phanheId: $("#hdPhanHeId").val(), userId: userid },
        dataType: 'html',
        beforeSend: function () {
            $("#card-gridCauHinh").showLoading(30000);
        },
        success: function (data) {
            $("#grid-CauHinh").html(data);
            $("#card-gridCauHinh").hideLoading();
        },
        error: function (data) {
            toastr.error("Chưa lấy được dữ liệu biểu đồ");
            $("#card-gridCauHinh").hideLoading();
        }
    })
}

function LoadChiTietCauHinh_Onclick(cauhinhId, event) {
    if (event) {
        target = event.target;
        if (target.tagName == "A" || target.tagName == "I") {
            return true;
        }
    }
    LoadChiTietCauHinh(cauhinhId);
}

function LoadChiTietCauHinh(cauhinhId, reloadChart, reloadList) {
    $.ajax({
        url: "/ThongTinTongHop/ThongKe/ChiTietCauHinh",
        data: { cauhinhId: cauhinhId },
        type: 'GET',
        dataType: 'html',
        success: function (data) {
            var $dialog = taviJs.showDialog({
                id: "dialogHieuChinhCauHinh",
                content: data,
                width: 900,
                height: 800
            });

            var chitieuIds = $dialog.find("#DinhNghiaChiTieuIds").val();
            var tinhId = $dialog.find("#TinhId").val();
            var huyenId = $dialog.find("#HuyenId").val();
            var xaId = $dialog.find("#XaId").val();

            // load danh muc
            $("#Nam").select2();
            $("#Thang").select2();
            checkPhuongPhapTinh();

            $("input[name=KyBaoCao]").change(function () {
                DisplayThoiGianBaoCao($("input[name=KyBaoCao]:checked").val());
            })

            $("#Nam,#Quy,#Thang").change(function () {
                checkPhuongPhapTinh();
            })

            // load danh mục
            taviJs.load_DanhMuc_Tinh("ddlTinh", tinhId);
            taviJs.load_DanhMuc_Huyen("ddlHuyen", huyenId, tinhId, { id: -1, text: "Tất cả", selected: huyenId == -1 });
            taviJs.load_DanhMuc_Xa("ddlXa", xaId, huyenId, { id: -1, text: "Tất cả", selected: xaId == -1 });

            $("#ddlTinh").change(function () {
                var id = $("#ddlTinh").val();
                taviJs.load_DanhMuc_Huyen("ddlHuyen", 0, id, { id: -1, text: "Tất cả", selected: huyenId == -1 });
                taviJs.load_DanhMuc_Xa("ddlXa", 0, 0, { id: -1, text: "Tất cả", selected: xaId == -1 });
            })

            $("#ddlHuyen").change(function () {
                var id = $("#ddlHuyen").val();
                taviJs.load_DanhMuc_Xa("ddlXa", 0, id, { id: -1, text: "Tất cả", selected: xaId == -1 });
            })

            $("#btnChonChiTieu").click(function () {
                $dialogChonChiTieu = taviJs.showDialogCustom({
                    id: "popupChonChiTieu",
                    title: "Chọn chỉ tiêu",
                    type: "info",
                    textYes: "Chọn",
                    content: "<div id='treeChonChiTieu'></div>",
                    height: 400
                });

                $dialogChonChiTieu.find("#treeChonChiTieu").jstree({
                    core: {
                        data: {
                            url: '/ThongTinTongHop/QuanTriThongTin/DanhSachChiTieu',
                            data: function (node) {
                                return { PhanHeId: $("#hdPhanHeId").val(), ThuocChiTieuId: node.id }
                            }
                        }
                    },
                    themes: {
                        variant: "large"
                    }
                });

                $dialogChonChiTieu.find(".cmd-save").click(function () {
                    var chitieuIds = "";
                    var names = "";
                    var listSelectedData = $dialogChonChiTieu.find("#treeChonChiTieu").jstree().get_selected(true);
                    if (listSelectedData.length > 0) {
                        var listId = listSelectedData.map(m => m.id);
                        var listName = listSelectedData.map(m => m.text);
                        chitieuIds = listId.join(",");
                        names = listName.join(", ");
                    }
                    $dialog.find("#DinhNghiaChiTieuIds").val(chitieuIds);
                    $dialog.find("#ChiTieuId").val(names);
                    $dialogChonChiTieu.modal("hide");
                });

            })

            // event save 
            $dialog.find(".cmd-save").unbind('click').click(function () {
                $.ajax({
                    url: "/ThongTinTongHop/ThongKe/LuuCauHinh",
                    data: { cauhinh: GetCauHinhData() },
                    dataType: "json",
                    type: "POST",
                    success: function (data) {
                        if (reloadList != false) {
                            LoadDanhSachCauHinh();
                        }
                        if (data) toastr.success("Đã ghi lại dữ liệu")
                        if (reloadChart) {
                            LoadBieuDo(cauhinhId, $("#chart-" + cauhinhId));
                        }
                        $dialog.modal("hide");
                    },
                    error: function () {
                        toastr.error("Có lỗi xảy ra")
                    }
                })
            })
        },
        error: function (data) {
            toastr.error("Có lỗi xảy ra");
        }
    })
}

function LoadBieuDo(cauhinhId, ele) {
    $.ajax({
        url: "/ThongTinTongHop/ThongKe/Get_DuLieuBieuDo",
        data: { cauhinhId: cauhinhId },
        dataType: "json",
        success: function (result) {
            if (result == null || result.length == 0) {
                $(ele).empty().append("<span>Dữ liệu chưa được cập nhật...</span>");
                return true;
            }
            taviJs.createChart(result, ele, 300);
        },
        error: function () {
            toastr.error("Có lỗi xảy ra")
        }
    });
}

function TaoBangDuLieuBieuDo(lstChartData) {
    var html = '';
    if (lstChartData.length > 0) {
        $.each(lstChartData, function (index,chartData) {
            html += `<div class="table-responsive" style="max-height:250px">
                        <table class="table table-bordered table-head-fixed table-dulieu-bieudo" width="100%">
                            <thead>
                                <tr>
                                    <th>Tiêu chí</th>`;
            $.each(chartData.Datasets, function (dtsetIndex,data) {
                html += `<th>${data.Label}<br/><>${data.Unit ? ("(" + data.Unit + ")") : ""}</th>`;
            });
            html += `</tr></thead><tbody>`;
            $.each(chartData.Labels, function (labelIndex, label) {
                html += "<tr><td>" + label + "</td>"
                $.each(chartData.Datasets, function (dtsetIndex,data) {
                    html += "<td>" + data.Data[labelIndex].toLocaleString() + "</td>";
                });
                html += "</tr>";
            });
            html += `</tbody></table></div>`;
        })
    }
    return html;
}

function LoadBieuDo_TrucThuoc(cauhinhId, chitieuId) {
    $.ajax({
        url: "/ThongTinTongHop/ThongKe/Get_DuLieuBieuDo_ChiTiet",
        data: { cauhinhId: cauhinhId, chitieuId: chitieuId },
        dataType: "json",
        success: function (result) {
            if (result == null || result.length == 0) {
                return true;
            }

            $("#view-dulieu-tructhuoc").append(`<div class="col-md-6"><div class="card card-primary card-outline"><div class="card-body p-1" id="chart-detail-` + chitieuId + `"></div></div></div>`);
            var newEle = $("#view-dulieu-tructhuoc").find("#chart-detail-" + chitieuId);
            taviJs.createChart(result, newEle, 300);
        },
        error: function () {
            toastr.error("Có lỗi xảy ra")
        }
    });
}

function isMultiTime() {
    var KyBaoCao = $("input[name=KyBaoCao]:checked").val();
    var Nam = $("#Nam").val();
    var Thang = $("#Thang").val();
    var Quy = $("#Quy").val();
    if (KyBaoCao == 1) {
        return Nam == null || Nam == "";
    } else if (KyBaoCao == 2) {
        return Nam == null || Nam == "" || Quy == null || Quy == "";
    } else if (KyBaoCao == 3) {
        return Nam == null || Nam == "" || Thang == null || Thang == "";
    }
}

function checkPhuongPhapTinh() {
    if (isMultiTime()) {
        $("#PhuongPhapTinh").attr("disabled", false);
    } else {
        $("#PhuongPhapTinh").val("");
        $("#PhuongPhapTinh").attr("disabled", true);
    }
}

function XoaCauHinh(cauhinhId, reloadList) {
    var dialog = taviJs.showConfirmDialog("Bạn có chắc chắn muốn xóa cấu hình này không?");
    dialog.find(".cmd-save").click(function () {
        $.ajax({
            url: "/ThongTinTongHop/ThongKe/XoaCauHinh",
            data: { cauhinhId: cauhinhId },
            type: "POST",
            dataType: "json",
            success: function (result) {
                if (result) {
                    if (reloadList != false) {
                        LoadDanhSachCauHinh();
                    }
                    toastr.success("Đã xóa cấu hình hiển thị");
                } else {
                    toastr.error("Chưa xóa được");
                }
                dialog.modal("hide");
            },
            error: function () {
                toastr.error("Có lỗi xảy ra")
            }
        });
    })
}

function ApplyForCustomUser() {
    var $dialog = taviJs.showDialogCustom({
        id: 'dialogApplyCustom',
        title: 'Chọn người dùng',
        type: 'info',
        content: '<select id="selectlist_CustomUser" class="form-control" multiple="multiple"></select>',
        width: 600,
        textYes: 'Áp dụng',
        textNo: 'Đóng'
    });

    $dialog.find("#selectlist_CustomUser").select2({
        ajax: {
            url: '/ThongTinTongHop/ThongKe/Get_DanhSachNguoiDung',
            data: function (params) {
                return { search: params.term };
            },
            delay: 250
        },
        placeholder: "---- Hiển thị theo người dùng ----",
        allowClear: true,
        multiple: true
    });

    $dialog.find(".cmd-save").click(function () {
        if ($("#selectlist_CustomUser").val() == null || $("#selectlist_CustomUser").val().length <= 0) {
            toastr.error("Chưa chọn người dùng !");
            return;
        }

        var userIds = Array.prototype.join.call($("#selectlist_CustomUser").val(), ",");

        $.ajax({
            url: "/ThongTinTongHop/ThongKe/AppDungCauHinh_ChoNguoiDung",
            data: { PhanHeId: $("#hdPhanHeId").val(), UserIds: userIds },
            type: "POST",
            dataType: "json",
            success: function (result) {
                if (result) {
                    toastr.success("Dữ liệu đã được cập nhật");
                } else {
                    toastr.error("Chưa cập nhật được dữ liệu");
                }
                $dialog.modal("hide");
            },
            error: function () {
                toastr.error("Có lỗi xảy ra")
            }
        });
    })
}