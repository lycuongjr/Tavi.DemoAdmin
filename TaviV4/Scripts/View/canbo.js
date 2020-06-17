$(document).ready(function () {
    //------------------------- load script for view index -------------------------
    if ($("#ActionName").val() == "Index") {
        // get positions and bind to grid
        dsCanBo($('#hdHoVaTen').val(), $('#hdTenDangNhap').val(), $('#hdPage').val());

        // handle event create
        $('#Create').unbind('click').click(function () {
            location.href = "/BoMayToChuc/CanBo/ThemMoi";
        });

        // handle event search
        $('#btnTimKiem').unbind('click').click(function () {
            $("#hdPage").val(1);
            loadDanhSachCanBo();
        })
    }
    // --------------------------------- END ----------------------------------------

    // ----------------- load script for view add new or update ---------------------
    else if ($("#ActionName").val() == "ThemMoi") {
        // get hidden value
        var coquanid = $("#CoQuan").data("coquanid");
        var phongbanid = $("#PhongBan").data("phongbanid");
        var chucvuid = $("#ChucVu").data("chucvuid");

        taviJs.load_DanhMuc_CoQuan({ id: coquanid }, 'CoQuan');
        taviJs.load_DanhMuc_PhongBan({ id: phongbanid, coquanid: coquanid }, 'PhongBan');
        taviJs.load_DanhMuc_ChucVu({ id: chucvuid }, 'ChucVu');

        $("#CoQuan").click(function () {
            $("#ddlCoQuanId").change(function () {
                taviJs.load_DanhMuc_PhongBan({ coquanid: $(this).val() }, 'PhongBan');
            })
            $(this).unbind('click');
        })

        $("#btnGhiLai").unbind('click').click(function () {
            GhiLaiCanBo();
        })

        // validate form
        $("#formview form").validate({
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
                ddlCoQuanId: {
                    required: "Chưa chọn cơ quan"
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
                ddlCoQuanId: {
                    required: true
                },            
                ddlPhongBanId: {
                    required: true
                },            
                ddlChucVuId: {
                    required: true
                }
            }
        })
    }
    // --------------------------------- END ----------------------------------------
    
});

// function load all possition
function loadDanhSachCanBo(pageNum) {
    var HoVaTen = $("#hdHoVaTen").val();
    var TenDangNhap = $("#hdTenDangNhap").val();
    var page = pageNum == undefined ? $("#hdPage").val() : pageNum;
    $("#hdPage").val(page);
    dsCanBo(HoVaTen, TenDangNhap, page);
}

// call ajax get all user by page
function dsCanBo(HoVaTen,TenDangNhap,page) {
    $.ajax({
        url: '/BoMayToChuc/CanBo/HienThiDanhSach',
        data: {
            HoVaTen: HoVaTen,
            TenDangNhap: TenDangNhap,
            Page: page
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

// dialog edit/add position
function HieuChinhCanBo(id) {
    location.href = "/BoMayToChuc/CanBo/ThemMoi/" + id;
}

// save change position
function GhiLaiCanBo() {
    if ($("#formview form").valid()) {
        $("#CoQuanHienTaiId").val($("#ddlCoQuanId").val());
        $("#ChucVuHienTaiId").val($("#ddlChucVuId").val());
        $("#PhongBanHienTaiId").val($("#ddlPhongBanId").val());
        $("#formview form").submit();
    } else {
        toastr.error("Thiếu thông tin yêu cầu của cán bộ");
    }
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