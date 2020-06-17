$(document).ready(function () {
    // get positions and bind to grid
    dsChucVu($('#hdMaChucVu').val(), $('#hdTenChucVu').val(), $('#hdPage').val());

    // handle event create
    $('#Create').unbind('click').click(function () {
        HieuChinhChucVu();
    });

    // handle event search
    $('#btnTimKiem').unbind('click').click(function () {
        $("#hdPage").val(1);
        loadDanhSachChucVu();
    })
});

// function load all possition
function loadDanhSachChucVu(pageNum) {
    var MaChucVu = $("#hdMaChucVu").val();
    var TenChucVu = $("#hdTenChucVu").val();
    var page = pageNum == undefined ? $("#hdPage").val() : pageNum;
    $("#hdPage").val(page);
    dsChucVu(MaChucVu, TenChucVu, page);
}

// call ajax get all possition by page
function dsChucVu(MaChuCVu, TenChucVu, page) {
    $.ajax({
        url: 'HienThiDanhSach',
        data: {
            MaChucVu: MaChuCVu,
            TenChucVu: TenChucVu,
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

function onPageLoaded(pageNum) {
    $("#hdPage").val(pageNum);
}

// dialog edit/add position
function HieuChinhChucVu(id) {
    $.ajax({
        url: '/BoMayToChuc/ChucVu/HieuChinh/' + id,
        dataType: 'html',
        success: function (data) {
            var $popup = taviJs.showDialog({
                id: 'dialogThemMoiChucVu',
                content: data
            })
            // validate for form
            $popup.find("form").validate({
                messages: {
                    MaChucVu: {
                        required: "Mã chức vụ không được bỏ trống"
                    },
                    TenChucVu: {
                        required: "Tên chức vụ không được bỏ trống"
                    }
                },
                rules: {
                    MaChucVu: {
                        required: true
                    },
                    TenChucVu: {
                        required: true
                    }
                }
            })
        },
        error: function () {
            toastr.error("Không lấy được thông tin chức vụ")
        }
    })
}

// save change position
function GhiLaiChucVu(e) {
    $form = $(e).closest('form');
    if (!$form.valid()) return false;
    $.ajax({
        url: 'GhiLai',
        type: 'POST',
        data: $form.serialize(),
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                toastr.success('Thông tin chức vụ đã được ghi lại.')
                loadDanhSachChucVu();
                $("#dialogThemMoiChucVu").modal("hide");
            } else {
                toastr.error("Có lỗi hệ thống xảy ra");
            }
        },
        error: function (data) {
            toastr.error(data.message);
        }
    })
}

// active or inactive position
function ThayDoiTrangThai(chucvuId) {
    $.ajax({
        url: 'ThayDoiTrangThai',
        type: 'POST',
        data: { id: chucvuId },
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                toastr.success('Trạng thái chức vụ đã được thay đổi.')
                loadDanhSachChucVu();
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
function XoaChucVu(chucvuId) {
    $.ajax({
        url: 'XoaChucVu',
        type: 'POST',
        data: { id: chucvuId },
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                toastr.success('Trạng thái chức vụ đã được thay đổi.')
                loadDanhSachChucVu();
            } else {
                toastr.error(data.message);
            }
        },
        error: function (data) {
            toastr.error(data.message);
        }
    })
}