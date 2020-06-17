$(document).ready(function () {
    //------------------------- load script for view index -------------------------
    if ($("#ActionName").val() == "Index") {
        $("#btnTimKiem").click(function () {
            loadDanhSachCoQuan(1);
        })
    }
    // --------------------------------- END ----------------------------------------

    // ----------------- load script for view add new or update ---------------------
    else if ($("#ActionName").val() == "ThemMoi") {
        var coquanId = $("#CoQuanId").val();
        var thuoccoquanid = $("#CoQuan").data("coquanid");

        taviJs.load_DanhMuc_CoQuan({ id: thuoccoquanid }, 'CoQuan',$("#formview"));

        $('#formview form').validate({
            messages: {
                MaCoQuan: {
                    required: 'Mã cơ quan không được bỏ trống'
                },
                TenCoQuan: {
                    required: 'Tên cơ quan không được bỏ trống'
                },
                Email: {
                    email: 'Không đúng định dạng email'
                }
            },
            rules: {
                MaCoQuan: {
                    required: true
                },
                TenCoQuan: {
                    required: true
                },
                Email: {
                    email: true
                }
            }
        });

        $("#LaCoQuanPhatSinh").change(function () {
            if ($(this).is(":checked")) {
                $("#dateRange_Unit").show();
            } else {
                $("#dateRange_Unit").hide();
            }
        })

        if (coquanId > 0) {
            loadDanhSachPhongBan(1);
            loadDanhSachCanBo(1);
            viewDanhSachChucNang();
        }

        $("#formview").find("#btnGhiLai").unbind('click').click(function () {
            GhiLaiDuLieu(coquanId);
        });

        $("#formview").find("#btnLuuChucNang").unbind('click').click(function () {
            LuuChucNangSuDung();
        });
    }
    // --------------------------------- END ----------------------------------------

});

function ThemMoi(id) {
    location.href = "/BoMayToChuc/CoQuan/ThemMoi/" + id;
}

function GhiLaiDuLieu(coquanId) {
    $("#formview #ThuocCoQuanId").val($("#formview #ddlCoQuanId").val());
    if ($("#formview form").valid()) {
        if (coquanId > 0) {
            $.ajax({
                url: "/BoMayToChuc/CoQuan/ThemMoi",
                data: $("#formview form").serialize(),
                type: "POST",
                dataType: "json",
                success: function (data) {
                    if (data && data.status == true) {
                        toastr.success("Đã cập nhật thông tin cơ quan");
                    } else {
                        toastr.error("Cập nhật thông tin không thành công");
                    }
                },
                error: function () {
                    toastr.error("Cập nhật thông tin không thành công");
                }
            })
        } else {
            $("#formview form").submit();
        }
    }
}

function loadDanhSachCoQuan(pageNum) {
    var MaCoQuan = $("#hdMaCoQuan").val();
    var TenCoQuan = $("#hdTenCoQuan").val();
    var page = pageNum == undefined ? $("#hdPage").val() : pageNum;
    dsCoQuan(MaCoQuan, TenCoQuan, page);
}

function dsCoQuan(MaCoQuan, TenCoQuan, page) {
    $.ajax({
        url: '/BoMayToChuc/CoQuan/HienThiDanhSach',
        data: {
            MaCoQuan: MaCoQuan
            ,TenCoQuan: TenCoQuan
            ,PageCurent: page
        },
        type: "POST",
        dataType: "html",
        success: function (data) {
            $("#dataGrid").html(data);
        },
        complete: function (data) {

        }
    });
}

function ThuocCoQuan(ThuocCoQuanId) {
    $.ajax({
        url: '/DanhMuc/dsCoQuan',
        data: {
            id: ThuocCoQuanId
        },
        type: "POST",
        dataType: "html",
        success: function (data) {
            $("#CoQuan").html(data);
        },
        complete: function (data) {
        }
    });
}

function loadDanhSachCanBo(pageNum) {
    var page = pageNum == undefined ? $("#hdPageCB").val() : pageNum;
    $.ajax({
        url: '/BoMayToChuc/CanBo/DanhSachTrucThuoc',
        data: {
            CoQuanId: $("#CoQuanId").val(),
            Page: page
        },
        dataType: "json",
        success: function (data) {
            $("#count-canbotructhuoc").html(data.count);
            $("#dsCanBoTrucThuoc").html(data.content);
        },
        error: function (res) {
            toastr.error("Không lấy được danh sách cán bộ trực thuộc")
        }
    });
}

function loadDanhSachPhongBan(pageNum) {
    var page = pageNum == undefined ? $("#hdPagePB").val() : pageNum;
    $("#hdPage").val(page);
    $.ajax({
        url: '/BoMayToChuc/PhongBan/DanhSachTrucThuocCoQuan',
        data: { CoQuanId: $("#CoQuanId").val(), page: page },
        dataType: 'json',
        success: function (data) {
            $("#count-phongbantructhuoc").html(data.count);
            $("#dsPhongBanTrucThuoc").html(data.content);
        },
        error: function (res) {
            toastr.error("Không lấy được danh sách phòng ban trực thuộc")
        }
    })
}

// function phong ban truc thuoc co quan --------------------
function viewPhongBanTrucThuoc(phongbanId, coquanId) {
    $.ajax({
        url: "/BoMayToChuc/CoQuan/ViewHieuChinhPhongBan",
        data: { id: phongbanId },
        dataType: 'html',
        success: function (data) {
            // view thong tin can bo
            var $dialog = taviJs.showDialog({
                id: "dialogHieuChinhPhongBan",
                content: data,
                width: 850
            });

            // validate form
            $dialog.find("form").validate({
                messages: {
                    MaPhongBan: {
                        required: 'Mã phòng ban không được bỏ trống'
                    },
                    TenPhongBan: {
                        required: 'Tên phòng ban không được bỏ trống'
                    }
                },
                rules: {
                    MaPhongBan: {
                        required: true
                    },
                    TenPhongBan: {
                        required: true
                    }
                }
            });

            // event save 
            $dialog.find(".cmd-save").unbind('click').click(function () {
                if ($dialog.find("form").valid()) {
                    $dialog.find("#ThuocCoQuanId").val(coquanId);
                    $dialog.find("#ConHoatDong").val($dialog.find("#PB_ConHoatDong").prop("checked"));
                    $.ajax({
                        url: "/BoMayToChuc/PhongBan/GhiLai",
                        type: "POST",
                        data: $dialog.find("form").serialize(),
                        dataType: 'json',
                        success: function (data) {
                            if (data && data.status) {
                                toastr.success("Đã ghi lại dữ liệu cán bộ");
                                loadDanhSachPhongBan(1);
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
                    toastr.error("Thiếu thông tin yêu cầu của phòng ban");
                }
            })
        },
        error: function () {
            toastr.error("Không lấy được thông tin phòng ban")
        }
    })
}
function ThayDoiTrangThaiPhongBan(canboId) {
    $.ajax({
        url: '/BoMayToChuc/PhongBan/ThayDoiTrangThai',
        type: 'POST',
        data: { id: canboId },
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                toastr.success('Trạng thái cán bộ đã được thay đổi.')
                loadDanhSachPhongBan();
            } else {
                toastr.error(data.message);
            }
        },
        error: function (data) {
            toastr.error(data.message);
        }
    })
}
// remove position by id
function XoaPhongBan(canboId) {
    $.ajax({
        url: '/BoMayToChuc/PhongBan/XoaPhongBan',
        type: 'POST',
        data: { id: canboId },
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                toastr.success(data.message)
                loadDanhSachPhongBan();
            } else {
                toastr.error(data.message);
            }
        },
        error: function (data) {
            toastr.error(data.message);
        }
    })
}
// ----------------------------------------------------------

//***********************************************************

// function can bo truc thuoc co quan -----------------------
function viewCanBoTrucThuoc(canboId, coquanId) {
    $.ajax({
        url: "/BoMayToChuc/CoQuan/ViewHieuChinhCanBo",
        data: { id: canboId },
        dataType: 'html',
        success: function (data) {
            // view thong tin can bo
            var $dialog = taviJs.showDialog({
                id: "dialogHieuChinhCanBo",
                content: data,
                width: 850
            });

            // get hidden value
            var phongbanid = $dialog.find("#PhongBan").data("phongbanid");
            var chucvuid = $dialog.find("#ChucVu").data("chucvuid");

            taviJs.load_DanhMuc_PhongBan({ id: phongbanid, coquanid: coquanId }, 'PhongBan', $dialog);
            taviJs.load_DanhMuc_ChucVu({ id: chucvuid }, 'ChucVu', $dialog);

            // validate form
            $dialog.find("form").validate({
                messages: {
                    HoVaTen: {
                        required: "Tên cán bộ không được bỏ trống"
                    },
                    TenDangNhap: {
                        required: "Tên đăng nhập không được bỏ trống"
                    },
                    MatKhau: {
                        required: "Mật khẩu không được bỏ trống"
                    },
                    ddlPhongBanId: {
                        required: "Chưa chọn phòng ban"
                    },
                    ddlChucVuId: {
                        required: "Chưa chọn chức vụ"
                    }
                },
                rules: {
                    HoVaTen: {
                        required: true
                    },
                    TenDangNhap: {
                        required: true
                    },
                    MatKhau: {
                        required: true
                    },
                    ddlPhongBanId: {
                        required: true
                    },
                    ddlChucVuId: {
                        required: true
                    }
                }
            });

            // event save 
            $dialog.find(".cmd-save").unbind('click').click(function () {
                if ($dialog.find("form").valid()) {
                    $dialog.find("#CoQuanHienTaiId").val(coquanId);
                    $dialog.find("#ChucVuHienTaiId").val($dialog.find("#ddlChucVuId").val());
                    $dialog.find("#PhongBanHienTaiId").val($dialog.find("#ddlPhongBanId").val());
                    $dialog.find("#ConHoatDong").val($dialog.find("#CB_ConHoatDong").prop("checked"));
                    
                    $.ajax({
                        url: "/BoMayToChuc/CanBo/GhiLai",
                        type: "POST",
                        data: $dialog.find("form").serialize(),
                        dataType: 'json',
                        success: function (data) {
                            if (data && data.status) {
                                toastr.success("Đã ghi lại dữ liệu cán bộ");
                                loadDanhSachCanBo(1);
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
                    toastr.error("Thiếu thông tin yêu cầu của cán bộ");
                }
            })
        },
        error: function () {
            toastr.error("Không lấy được thông tin cán bộ")
        }
    })
}
// active or inactive user
function ThayDoiTrangThai(canboId) {
    $.ajax({
        url: '/BoMayToChuc/CanBo/ThayDoiTrangThai',
        type: 'POST',
        data: { id: canboId },
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                toastr.success('Trạng thái cán bộ đã được thay đổi.')
                loadDanhSachCanBo();
            } else {
                toastr.error(data.message);
            }
        },
        error: function (data) {
            toastr.error(data.message);
        }
    })
}
// remove position by id
function XoaCanBo(canboId) {
    $.ajax({
        url: '/BoMayToChuc/CanBo/XoaCanBo',
        type: 'POST',
        data: { id: canboId },
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                toastr.success(data.message)
                loadDanhSachCanBo();
            } else {
                toastr.error(data.message);
            }
        },
        error: function (data) {
            toastr.error(data.message);
        }
    })
}
// ----------------------------------------------------------

// funtion danh sach chuc nang
function viewDanhSachChucNang() {
    $.ajax({
        url: '/BoMayToChuc/CoQuan/ChucNangSuDung',
        data: { CoQuanId: $("#CoQuanId").val() },
        dataType: 'html',
        success: function (data) {
            var isCheck = true;
            $("#dsChucNangSuDung").html(data);
            $("#dsChucNangSuDung #checkAllFunction").change(function () {
                $("#dsChucNangSuDung input[name=ckbFunction]").prop("checked", $(this).prop('checked'));
            })
            $("#dsChucNangSuDung input[name=ckbFunction]").change(function () {
                // check child row
                if (isCheck) {
                    $("#dsChucNangSuDung input[name=ckbFunction][thuocchucnangId=" + $(this).val() + "]").prop("checked", $(this).prop('checked'));
                    $("#dsChucNangSuDung input[name=ckbFunction][thuocchucnangId=" + $(this).val() + "]").trigger('change');
                    isCheck = true;
                }
                // check parent row
                if ($(this).prop('checked')) {
                    isCheck = false;
                    $("#dsChucNangSuDung #chucnangId-" + $(this).attr("thuocchucnangId")).prop("checked", true);
                    $("#dsChucNangSuDung #chucnangId-" + $(this).attr("thuocchucnangId")).trigger('change');
                    isCheck = true;
                }
            })
        },
        error: function (res) {
            toastr.error("Không lấy được danh sách chức năng")
        }
    })
}

function LuuChucNangSuDung() {
    $.ajax({
        url: '/BoMayToChuc/CoQuan/LuuChucNang',
        data: { CoQuanId: $("#CoQuanId").val(), dsChucNang: $("input:checkbox[name=ckbFunction]:checked").map(function () { return Number($(this).val()) }).get() },
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data = true) {
                toastr.success("Dữ liệu đã được ghi lại")
            } else {
                toastr.error("Có lỗi xảy ra")
            }
        },
        error: function (res) {
            toastr.error("Có lỗi xảy ra")
        }
    })
}

function PhanQuyenCaNhan(canboId) {
    $.ajax({
        url: '/BoMayToChuc/CanBo/DsPhanQuyenCaNhan',
        data: { CanBoId: canboId },
        dataType: 'html',
        success: function (data) {
            var $dialog = taviJs.showDialogCustom({
                id: "dialogPhanQuyenCaNhan",
                content: data,
                width: 900,
                title: 'Danh sách phân quyền',
                textYes: 'Ghi lại',
                textNo: 'Đóng'
            });

            $dialog.find(".cmd-save").unbind('click').click(function () {
                if ($dialog.find("input:checkbox[name=ckbFunction1]").not(":disabled").length > 0) {
                    $.ajax({
                        url: '/BoMayToChuc/CanBo/GhiLaiPhanQuyen',
                        data: { CanBoId: canboId, dsChucNang: $dialog.find("input:checkbox[name=ckbFunction1]:checked").map(function () { return Number($(this).val()) }).get() },
                        type: 'POST',
                        dataType: 'json',
                        success: function (data) {
                            if (data = true) {
                                toastr.success("Dữ liệu đã được ghi lại")
                            } else {
                                toastr.error("Có lỗi xảy ra")
                            }
                            $dialog.modal("hide");
                        },
                        error: function (res) {
                            toastr.error("Có lỗi xảy ra")
                            $dialog.modal("hide");
                        }
                    })
                } else {
                    toastr.error("Chưa có dữ liệu thay đổi");
                    $dialog.modal("hide");
                }
            })
        },
        error: function (res) {
            toastr.error("Có lỗi xảy ra")
        }
    })
}