
$(document).ready(function () {
    $.ajax({
        url: '/BoMayToChuc/NhomNguoiDung/DanhSachCoQuan',
        dataType: 'html',
        success: function (data) {
            $("#formview #list-coquan").html(data);
            var selectedId = $("#SelectCoQuanId").val()
            if (selectedId != undefined && selectedId != null & selectedId > 0) {
                $(".div-coquan[data-coquanid=" + selectedId + "]").trigger('click');
            } else {
                $(".div-coquan").first().trigger('click');
            }
        },
        error: function (res) {
            toastr.error("Có lỗi xảy ra")
        }
    })

    $("#card-dsNguoiDung").on('expanded.lte.cardwidget', collapseCardChucNang);
    $("#card-dsChucNang").on('expanded.lte.cardwidget', collapseCardNguoiDung);
});

function collapseCardNguoiDung() {
    $("#card-dsNguoiDung").CardWidget('collapse')
}

function collapseCardChucNang() {
    $("#card-dsChucNang").CardWidget('collapse')
}

function activeCoQuan(coquanId) {
    $("div.div-coquan.active").each(function () {
        $(this).removeClass('active');
    })
    $("#coquanId-" + coquanId).find(".div-coquan").addClass('active');
}

function loadDanhSachNhom(coquanid, e) {
    activeCoQuan(coquanid)
    $.ajax({
        url: '/BoMayToChuc/NhomNguoiDung/HienThiDanhSach',
        data: { CoQuanId: coquanid },
        dataType: 'html',
        success: function (data) {
            $("#formview #table-nhomnguoidung").html(data);

            $("#table-nhomnguoidung tbody tr").unbind('click').click(function () {
                var coquanId = $(this).attr('coquanId');
                var nhomId = $(this).attr('nhomnguoidungId');
                if (coquanId == 0 || nhomId == 0) return false;
                // active row
                $(this).closest('table').find('tr.active').removeClass('active');
                $(this).addClass('active');

                // load data by row
                loadChucNangSuDung(nhomId, coquanId);
                loadDanhSachNguoiDung(nhomId, coquanId);
            })

            var firstRow = $("#table-nhomnguoidung tbody tr").first();
            if (firstRow.attr('nhomnguoidungid') == 0) {
                // remove content in role table
                $("#table-dsChucNang table tbody").remove();
            } else {
                firstRow.trigger("click");
            }
        },
        error: function (res) {
            toastr.error("Có lỗi xảy ra")
        }
    })
}

// show dialog hieu chinh nhom 
function viewHieuChinhNhom(nhomId) {
    $.ajax({
        url: "/BoMayToChuc/NhomNguoiDung/HieuChinh",
        data: { id: nhomId },
        dataType: 'html',
        success: function (data) {
            // view thong tin can bo
            var $dialog = taviJs.showDialog({
                id: "dialogHieuChinhNhom",
                content: data,
                width: 450
            });

            // get hidden value
            var coquanid = $dialog.find("#CoQuan").data("coquanid");

            if (coquanid == "" || coquanid == undefined) {
                if ($(".div-coquan.active").length > 0) {
                    coquanid = $(".div-coquan.active").data("coquanid");
                }
            }

            taviJs.load_DanhMuc_CoQuan({ id: coquanid }, 'CoQuan', $dialog);

            // validate form
            $dialog.find("form").validate({
                messages: {
                    TenNhomNguoiDung: {
                        required: "Tên cán bộ không được bỏ trống"
                    },
                    ddlCoQuanId: {
                        required: "Chưa chọn cơ quan"
                    }
                },
                rules: {
                    TenNhomNguoiDung: {
                        required: true
                    },
                    ddlCoQuanId: {
                        required: true
                    }
                }
            });

            // event save 
            $dialog.find(".cmd-save").unbind('click').click(function () {
                if ($dialog.find("form").valid()) {
                    $dialog.find("#CoQuanId").val($dialog.find("#ddlCoQuanId").val());
                    $.ajax({
                        url: "/BoMayToChuc/NhomNguoiDung/LuuHieuChinh",
                        type: "POST",
                        data: $dialog.find("form").serialize(),
                        dataType: 'json',
                        success: function (result) {
                            if (result && result.success) {
                                toastr.success("Đã ghi lại dữ liệu nhóm");
                                loadDanhSachNhom(result.data.CoQuanId, $("#coquanId-" + result.data.CoQuanId + ".div-coquan"));
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
                    toastr.error("Thiếu thông tin yêu cầu của nhóm");
                }
            })
        },
        error: function () {
            toastr.error("Không lấy được thông tin nhóm")
        }
    })
}

// load danh sach chuc nang theo nhom nguoi dung
function loadChucNangSuDung(nhomId, coquanId){
    $.ajax({
        url: '/BoMayToChuc/NhomNguoiDung/DanhSachChucNang',
        data: { CoQuanId: coquanId, NhomNguoiDungId: nhomId },
        dataType: 'html',
        success: function (data) {
            $("#formview #table-dsChucNang").html(data);
            $("#btnLuuPhanQuyen").unbind('click').click(function () {
                if ($("input:checkbox[name=ckbFunction]").length > 0) {
                    var nhomnguoidungid = $("#table-nhomnguoidung tbody tr.active").first().attr('nhomnguoidungid');
                    $.ajax({
                        url: '/BoMayToChuc/NhomNguoiDung/LuuChucNang',
                        data: { NhomNguoiDungId: Number(nhomnguoidungid), dsChucNang: $("input:checkbox[name=ckbFunction]:checked").map(function () { return Number($(this).val()) }).get() },
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
                } else {
                    toastr.error("Chưa có dữ liệu");
                }
            })
        },
        error: function (res) {
            toastr.error("Có lỗi xảy ra")
        }
    })
}

// load sanh sach nguoi dung
function loadDanhSachNguoiDung(nhomId, coquanId) {
    $.ajax({
        url: '/BoMayToChuc/NhomNguoiDung/DanhSachNguoiDung',
        data: { NhomNguoiDungId: nhomId },
        dataType: 'html',
        success: function (data) {
            $("#formview #table-dsNguoiDung").html(data);

            // event add data
            $("#btnThemNguoiDung").unbind('click').click(function () {
                addDanhSachNguoiDung(nhomId, coquanId);
            })
        },
        error: function (res) {
            toastr.error("Có lỗi xảy ra")
        }
    })
}

// them nguoi dung vao nhom
function addDanhSachNguoiDung(nhomId, coquanId) {
    $.ajax({
        url: '/BoMayToChuc/NhomNguoiDung/DanhSachNguoiDungThemMoi',
        data: { NhomNguoiDungId: nhomId, CoQuanId: coquanId },
        dataType: 'html',
        success: function (data) {
            var $dialog = taviJs.showDialogCustom({
                id: "dialogThemNguoiDung",
                content: data,
                width: 900,
                title: 'Danh sách người dùng',
                textYes: 'Thêm',
                textNo: 'Đóng'
            });

            $dialog.find("#checkAllUser").change(function () {
                $dialog.find("input[name=ckbUser]").prop("checked", $(this).prop('checked'));
            })

            $dialog.find(".cmd-save").unbind('click').click(function () {
                var userIds = $("input:checkbox[name=ckbUser]:checked").map(function () { return Number($(this).val()) }).get();
                if (userIds != undefined && userIds.length > 0) {
                    $.ajax({
                        url: '/BoMayToChuc/NhomNguoiDung/ThemNguoiDung',
                        data: { NhomNguoiDungId: nhomId, dsNguoiDung: userIds },
                        type: 'POST',
                        dataType: 'json',
                        success: function (data) {
                            if (data = true) {
                                toastr.success("Dữ liệu đã được ghi lại");
                                loadDanhSachNguoiDung(nhomId, coquanId);
                            } else {
                                toastr.error("Có lỗi xảy ra");
                            }
                            $dialog.modal("hide");
                        },
                        error: function (res) {
                            toastr.error("Có lỗi xảy ra");
                        }
                    })
                }
            })
        },
        error: function (res) {
            toastr.error("Có lỗi xảy ra")
        }
    })
}

function xoaDanhSachNguoiDung(userId, xoatatca) {
    var nhomnguoidungId = $("#table-nhomnguoidung tbody tr.active").first().attr('nhomnguoidungid');
    var coquanid = $("#table-nhomnguoidung tbody tr.active").first().attr('coquanid');
    xoatatca = xoatatca == undefined || xoatatca == null ? false : xoatatca
    $.ajax({
        url: '/BoMayToChuc/NhomNguoiDung/XoaNguoiDung',
        data: { NhomNguoiDungId: nhomnguoidungId, NguoiDungId: userId, XoaTatCa: xoatatca },
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data = true) {
                toastr.success("Dữ liệu đã được ghi lại");
                loadDanhSachNguoiDung(nhomnguoidungId, coquanid);
            } else {
                toastr.error("Có lỗi xảy ra");
            }
        },
        error: function (res) {
            toastr.error("Có lỗi xảy ra");
        }
    })
}