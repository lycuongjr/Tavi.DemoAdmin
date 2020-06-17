$(document).ready(function () {
    var phanheID = $("#hdPhanHeId").val();
    var actionName = $("#ActionName").val();
    LoadDuLieuChiTieu(phanheID, null, null, 0, null, true);

    $("#btnThemMoi").click(function () {
        document.location = "/ThongTinTongHop/DinhNghiaChiTieu/HieuChinh?PhanHeId=" + phanheID;
    })

    $("#btnRefresh").click(function () {
        LoadDuLieuChiTieu(phanheID, null, null, 0, null, true);
    })


    if (actionName == "ThemMoi") {
        var $form = $("#formview form");
        var chitieuid = $form.find("#DinhNghiaChiTieuId").val();
        var thuocchitieuid = $form.find("#ThuocDinhNghiaChiTieuId").val();
        var donvitinhid = $form.find("#DonViTinhId").val();
        HienThiGiaTriTieuChuan();

        // load don vi tinh
        taviJs.load_DanhMuc_DonViTinh("ddlDonViTinh", donvitinhid)

        $("#ddlDonViTinh").change(function () {
            var selectdata = $("#ddlDonViTinh").select2("data")
            if (selectdata.length == 0 || selectdata[0].id == "") {
                $form.find("#DonViTinhId").val("");
                $form.find("#TenDonViTinh").val("");
            } else {
                $form.find("#DonViTinhId").val(selectdata[0].id);
                $form.find("#TenDonViTinh").val(selectdata[0].text);
            }
        })

        $("#btnGhiLai").unbind('click').click(function () {
            if (!$form.valid()) return false;
            if (chitieuid > 0) {
                $.ajax({
                    url: "/ThongTinTongHop/DinhNghiaChiTieu/GhiLaiHieuChinh",
                    data: $form.serialize(),
                    type: 'POST',
                    dataType: 'json',
                    success: function (data) {
                        if (data.Success) {
                            toastr.success("Chỉ tiêu đã được cập nhật");
                        } else {
                            toastr.error("Chưa lưu được dữ liệu")
                        }
                    },
                    error: function (data) {
                        toastr.error("Có lỗi xảy ra");
                    }
                })
            } else {
                $form.attr("action", "/ThongTinTongHop/DinhNghiaChiTieu/GhiLaiHieuChinh");
                $form.submit();
            }
        })

        $("#btnDanhSach").click(function () {
            document.location = "/ThongTinTongHop/DinhNghiaChiTieu/Index?id=" + phanheID;
        })

        $form.validate({
            messages: {
                TenChiTieu: {
                    required: "Tên chỉ tiêu không được bỏ trống"
                }
            },
            rules: {
                TenChiTieu: {
                    required: true
                } 
            }
        })

        $.ajax({
            url: "/ThongTinTongHop/DinhNghiaChiTieu/DanhSachChiTieu_Select",
            data: { PhanHeId: phanheID, KhacChiTieuId: chitieuid, selectedid: thuocchitieuid },
            dataType: 'json',
            success: function (data) {
                $("#dllThuocChiTieuId").select2({
                    data: data
                })
            },
            error: function () {
                toastr.error("Có lỗi dữ liệu");
            }
        })

        $("#dllThuocChiTieuId").change(function () {
            $form.find("#ThuocDinhNghiaChiTieuId").val($("#dllThuocChiTieuId").val());
        })

        if (chitieuid > 0) {
            LoadDuLieuChiTieuCon(phanheID, chitieuid);
        }

        $("#LoaiKetQua").change(function () {
            HienThiGiaTriTieuChuan();
        })
    }
});

function LoadDuLieuChiTieuCon(PhanHeId, ThuocChiTieuID) {
    $.ajax({
        url: "/ThongTinTongHop/DinhNghiaChiTieu/DanhSachChiTieuCon",
        data: { PhanHeId: PhanHeId, ThuocChiTieuId: ThuocChiTieuID },
        dataType: 'html',
        type: 'GET',
        success: function (data) {
            $("#gridChiTieuCon").html(data);
        },
        error: function (data) {
            toastr.error("Có lỗi xảy ra")
        }
    })
}

function LoadDuLieuChiTieu(PhanHeId, ThuocChiTieuId, prefix, level, afterEle, isRefresh) {
    $.ajax({
        url: "/ThongTinTongHop/DinhNghiaChiTieu/DanhSachChiTieu",
        data: { PhanHeId: PhanHeId, ThuocChiTieuId: ThuocChiTieuId, prefix: prefix, level: level },
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            $("#card-DanhSachChiTieu").showLoading(30000);
        },
        success: function (data) {
            $("#card-DanhSachChiTieu").hideLoading();
            var parentid = ThuocChiTieuId == null ? "" : ThuocChiTieuId;
            if (isRefresh) {
                $("#dataGrid").find("table tbody").html(data);
            } else {
                $(data).insertAfter(afterEle);
            }
            $("input[name=cbxChiTieu-" + parentid + "]").change(function () {
                var cbk = $(this);
                var row = cbk.closest("tr");
                var chitieuID = row.data("id");
                var item_level = row.data("level");
                var item_prefix = row.find(".prefix").html() + ".";
                if (cbk.prop("checked")) {
                    LoadDuLieuChiTieu(PhanHeId, chitieuID, item_prefix, item_level, row)
                } else {
                    var nextRow = row.next();
                    while (nextRow != undefined && nextRow.length > 0 && nextRow.data("level") > item_level) {
                        currentRow = nextRow;
                        nextRow = nextRow.next();
                        currentRow.remove();
                    }
                }
            })
        },
        error: function (data) {
            $("#card-DanhSachChiTieu").hideLoading();
            toastr.error("Không lấy được danh sách chỉ tiêu");
        }
    })
}

function showDialogHieuChinhChiTieu(chitieuId,thuocchitieuId, phanhethongtinId) {
    $.ajax({
        url: "/ThongTinTongHop/DinhNghiaChiTieu/HieuChinhChiTieuCon",
        data: { ChiTieuId: chitieuId },
        type: 'GET',
        dataType: 'html',
        success: function (data) {
            var $dialog = taviJs.showDialog({
                id: "dialogHieuChinhChiTieu",
                content: data,
                width: 850,
                height: 800
            });

            //set default value
            if (chitieuId == undefined || chitieuId == null || chitieuId == 0) {
                setDefaultValue($("#formview form"), $dialog);
            }

            HienThiGiaTriTieuChuan1();

            // load donvitinh
            var donvitinhid = $($dialog).find("#DonViTinhId").val();
            taviJs.load_DanhMuc_DonViTinh("ddlDonViTinh1", donvitinhid)
            $("#ddlDonViTinh1").change(function () {
                var selectdata = $($dialog).find("#ddlDonViTinh1").select2("data")
                if (selectdata.length == 0 || selectdata[0].id == "") {
                    $($dialog).find("form").find("#DonViTinhId").val("");
                    $($dialog).find("form").find("#TenDonViTinh").val("");
                } else {
                    $($dialog).find("form").find("#DonViTinhId").val(selectdata[0].id);
                    $($dialog).find("form").find("#TenDonViTinh").val(selectdata[0].text);
                }
            })

            $("#LoaiKetQua1").change(function () {
                HienThiGiaTriTieuChuan1();
            })

            // validate form
            $dialog.find("form").validate({
                messages: {
                    TenChiTieu: {
                        required: 'Tên chỉ tiêu không được bỏ trống'
                    }
                },
                rules: {
                    TenChiTieu: {
                        required: true
                    }
                }
            });

            // event save 
            $dialog.find(".cmd-save").unbind('click').click(function () {
                if ($dialog.find("form").valid()) {
                    var form = $dialog.find("form");
                    form.find("#LaPhanNhanhQuocGia").val(form.find("#LaPhanNhanhQuocGia1").prop("checked"));
                    form.find("#LaPhanNhanhHuyen").val(form.find("#LaPhanNhanhHuyen1").prop("checked"));
                    form.find("#LaPhanNhanhTinh").val(form.find("#LaPhanNhanhTinh1").prop("checked"));
                    form.find("#KyBaoCaoNam").val(form.find("#KyBaoCaoNam1").prop("checked"));
                    form.find("#KyBaoCaoGiaiDoan").val(form.find("#KyBaoCaoGiaiDoan1").prop("checked"));
                    form.find("#KyBaoCaoQuy").val(form.find("#KyBaoCaoQuy1").prop("checked"));
                    form.find("#KyBaoCaoThang").val(form.find("#KyBaoCaoThang1").prop("checked"));
                    form.find("#LaNhomTieuChi").val(form.find("#LaNhomTieuChi1").prop("checked"));
                    form.find("#ConHieuLuc").val(form.find("#ConHieuLuc1").prop("checked"));
                    form.find("#LoaiKetQua").val(form.find("#LoaiKetQua1").val());
                    form.find("#GiaTriTieuChuan").val(form.find("#GiaTriTieuChuan1").val());

                    $dialog.find("#ThuocDinhNghiaChiTieuId").val(thuocchitieuId);
                    $dialog.find("#PhanHeThongTinId").val(phanhethongtinId);
                    $.ajax({
                        url: "/ThongTinTongHop/DinhNghiaChiTieu/GhiLaiHieuChinh",
                        type: "POST",
                        data: $dialog.find("form").serialize(),
                        dataType: 'json',
                        success: function (data) {
                            if (data && data.Success) {
                                toastr.success("Đã ghi lại dữ liệu cán bộ");
                                LoadDuLieuChiTieuCon(phanhethongtinId, thuocchitieuId);
                            } else {
                                toastr.error("Chưa lưu được dữ liệu");
                            }
                            $dialog.modal("hide");
                        },
                        error: function () {
                            toastr.error("Chưa lưu được dữ liệu");
                        }
                    })
                } else {
                    toastr.error("Thiếu thông tin yêu cầu của chỉ tiêu");
                }
            })
        },
        error: function (data) {
            toastr.error("Có lỗi xảy ra");
        }
    })
}

function setDefaultValue(formParent, formChild) {
    formChild.find("#LaPhanNhanhQuocGia1").prop("checked", formParent.find("#LaPhanNhanhQuocGia").prop("checked"));
    formChild.find("#LaPhanNhanhHuyen1").prop("checked", formParent.find("#LaPhanNhanhHuyen").prop("checked"));
    formChild.find("#LaPhanNhanhTinh1").prop("checked", formParent.find("#LaPhanNhanhTinh").prop("checked"));
    formChild.find("#KyBaoCaoNam1").prop("checked", formParent.find("#KyBaoCaoNam").prop("checked"));
    formChild.find("#KyBaoCaoGiaiDoan1").prop("checked", formParent.find("#KyBaoCaoGiaiDoan").prop("checked"));
    formChild.find("#KyBaoCaoQuy1").prop("checked", formParent.find("#KyBaoCaoQuy").prop("checked"));
    formChild.find("#KyBaoCaoThang1").prop("checked", formParent.find("#KyBaoCaoThang").prop("checked"));
    formChild.find("#CoQuanThuThap").val(formParent.find("#CoQuanThuThap").val());
    formChild.find("#DonViTinhId").val(formParent.find("#DonViTinhId").val());
    formChild.find("#TenDonViTinh").val(formParent.find("#TenDonViTinh").val());
}

function XoaChiTieu(chitieuId) {
    var dialog = taviJs.showConfirmDialog("Xóa chỉ tiêu bao gồm tất cả chỉ tiêu con của chỉ tiêu này. Bạn muốn tiếp tục không?");
    dialog.find(".cmd-save").click(function () {
        $.ajax({
            url: '/ThongTinTongHop/DinhNghiaChiTieu/XoaChiTieu',
            data: { ChiTieuId: chitieuId },
            type: 'post',
            dataType: 'json',
            success: function (data) {
                if (data == true) {
                    toastr.success("Đã xóa chỉ tiêu");
                    dialog.modal("hide");
                    var phanheId = $("#hdPhanHeId").val();
                    var thuocchitieuid = $("#formview form").find("#DinhNghiaChiTieuId").val();
                    LoadDuLieuChiTieuCon(phanheId, thuocchitieuid);
                }
            },
            error: function (data) {
                toastr.error("Có lỗi xảy ra");
            }
        })
    })
    
}

function HienThiGiaTriTieuChuan() {
    var loaiketqua = $("#LoaiKetQua").val();
    if (loaiketqua == 1 || loaiketqua == 2) {
        $(".giatritieuchuan").hide();
    } else {
        $(".giatritieuchuan").show();
    }
}

function HienThiGiaTriTieuChuan1() {
    var loaiketqua = $("#LoaiKetQua1").val();
    if (loaiketqua == 1 || loaiketqua == 2) {
        $(".giatritieuchuan1").hide();
    } else {
        $(".giatritieuchuan1").show();
    }
}