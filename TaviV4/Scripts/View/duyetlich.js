$(document).ready(function () {
    // Hiển thị danh sách
    if ($("#ActionName").val() == "Index") {

        jsHienThiDanhSach($('#TenCuocHop').val(), $('#Page').val());
        $('#formview input').each(function () {
            $(this).focus(function () {
                $(this).keypress(function (e) {
                    if (e.which == 13 || e.keyCode == 13) {
                        jsHienThiDanhSach($('#TenCuocHop').val(), 1);

                    }
                })
            })
        });

        $('#btnTimKiem').unbind('click').click(function () {
            jsHienThiDanhSach($('#TenCuocHop').val(), 1);
        });

        $('#Create').unbind('click').click(function () {
            location.href = "/QuanLyCuocHop/LichHop/ThemMoi";
        });


    }
    else if ($("#ActionName").val() == "ChiTiet") {

        var CuocHopId = $("#CuocHopId").val();
        if (CuocHopId > 0) {
            FileManager();
            
            loadThanhPhanThamGia(1);

            // change datetime VN
            var dateStart = moment($("#NgayBatDau").val()).format('DD/MM/YYYY HH:mm');
            var dateEnd = moment($("#NgayKetThuc").val()).format('DD/MM/YYYY HH:mm');
            $("#NgayBatDau").val(dateStart);
            $("#NgayKetThuc").val(dateEnd);
        }
    }

});

// Hàm đổi hết td sang VN 
function ChuyenDoiTd() {
    var tds = $('table tr td[style="width:20%"]');
    $.each(tds, function (index, td) {
        if (td.innerHTML != '&nbsp;') {
            $(td).html(ChuyenDoiVN(td.innerHTML));
        }
    })
}

// hàm chuyển đổi datetime VN
function ChuyenDoiVN(datetime) {
    return moment(datetime).format('DD/MM/YYYY HH:mm') + '';
}

// ajax hiển thị danh sách
function jsHienThiDanhSach(TenCuocHop, page) {
    $.ajax({
        url: '/QuanLyCuocHop/DuyetLich/HienThiDanhSach',
        data: {
            TenCuocHop: TenCuocHop,
            Page: page
        },
        type: "POST",
        dataType: "html",
        success: function (data) {
            $("#dataGrid").html(data);
        },
        complete: function () {
            ChuyenDoiTd();
        }
    });
}

// ajax duyệt lịch họp
function DuyetCuocHop(CuocHopId) {
    $.ajax({
        url: '/QuanLyCuocHop/DuyetLich/DuyetLich',
        type: 'POST',
        data: { CuocHopId: CuocHopId },
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                toastr.success('Cuộc họp đã được duyệt')
                jsHienThiDanhSach($('#TenCuocHop').val(), $('#Page').val());
            } else {
                toastr.error("Có lỗi hệ thống xảy ra");
            }
        },
        error: function (data) {
            toastr.error(data.message);
        }
    })
}
// ajax xoá lịch họp
function XoaKhoiDanhSach(CuocHopId) {
    $.ajax({
        url: '/QuanLyCuocHop/DuyetLich/Xoa',
        type: 'POST',
        data: { CuocHopId: CuocHopId },
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                toastr.success('Đã xoá cuộc họp khỏi danh sách')
                jsHienThiDanhSach($('#TenCuocHop').val(), $('#Page').val());
            } else {
                toastr.error("Có lỗi hệ thống xảy ra");
            }
        },
        error: function (data) {
            toastr.error(data.message);
        }
    })
}
// ajax hiển thị thông tin chi tiết
function ChiTietCuocHop(CuocHopId) {
    location.href = "/QuanLyCuocHop/DuyetLich/ChiTiet/" + CuocHopId;
}



function dsTaiLieuCuocHop(CuocHopId, page) {

    $.ajax({
        url: '/QuanLyCuocHop/TaiLieuCuocHop/HienThiDanhSach',
        data: {
            CuocHopId: CuocHopId,
            Page: page
        },
        type: "POST",
        dataType: "json",
        success: function (data) {
            $("#count-tailieucuochop").html(data.count);

            $("#dsTaiLieuCuocHop").html(data.content);
        },
        error: function () {
            alert('Lỗi! Vui lòng liên hệ Tâm Việt');
        },
        complete: function (data) {

        }
    });
}

function loadThanhPhanThamGia(pageNum) {
    var page = pageNum == undefined ? $("#hdPageTPTG").val() : pageNum;
    var TPTGId = $("#CuocHopId").val();
    dsThanhPhanThamGia(TPTGId, page);

}

function dsThanhPhanThamGia(CuocHopId, page) {
    $.ajax({
        url: '/QuanLyCuocHop/ThanhPhanThamGia/HienThiDanhSach',
        data: {
            CuocHopId: CuocHopId,
            Page: page
        },
        type: "POST",
        dataType: "json",
        success: function (data) {
            $("#count-thanhphanthamgia").html(data.count);

            $("#dsThanhPhanThamGia").html(data.content);
        },
        error: function () {
            alert('Lỗi! Vui lòng liên hệ Tâm Việt');
        },
        complete: function (data) {

        }
    });
}

// Thao tác với file
function FileManager() {
    $.ajax({
        url: "/QuanLyCuocHop/TaiLieuCuocHop/IndexDuyet",
        type: "GET",
        dataType: "html",
        success: function (data) {
            $('#dsTaiLieu').html(data);
        },
        error: function () {
            console.log("Lỗi");
        },
        complete: function () {
            
            LoadListFileUploaded();

        }

    });
}


function LoadListFileUploaded() {
    var CuocHopId = $('#CuocHopId').val();
    $.ajax({
        url: "/QuanLyCuocHop/TaiLieuCuocHop/GetListFileDuyet",
        type: "GET",
        data: { CuocHopId: CuocHopId },
        dataType: "html",
        success: function (data) {
            $("#dataGrid").html(data);
        },
        error: function (data) {
            toastr.error("Không lấy được danh sách file")
        }
    });
}

function ViewFile(filePath) {

}

