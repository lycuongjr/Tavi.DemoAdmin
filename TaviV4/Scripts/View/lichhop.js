
$(document).ready(function () {
    if ($('#ActionName').val() == 'Index') {
        // Hiển thị danh sách
        //jsHienThiDanhSach($('#TenCuocHop').val(), $('#Page').val());
        $('#formview input').each(function () {
            $(this).focus(function () {
                $(this).keypress(function (e) {
                    if (e.which == 13 || e.keyCode == 13) {
                        //jsHienThiDanhSach($('#TenCuocHop').val(), 1);
                        alert($('#TenCuocHop').val());
                    }
                })
            })
        });

        $('#btnTimKiem').unbind('click').click(function () {
            //jsHienThiDanhSach($('#TenCuocHop').val(), 1);

        });

        $('#Create').click(function () {
            window.location = '/QuanLyCuocHop/LichHop/ThemMoi';
        });

        // Calendar và Events hiện lên Calendar
        RenderCalendar();
    }
    else if ($('#ActionName').val() == 'ThemMoi') {
        var thuoccoquanid = $("#CoQuan").data("coquanid");

        taviJs.load_DanhMuc_CoQuan({ id: thuoccoquanid }, 'CoQuan', $("#formview"));
        var cuochopId = $('#CuocHopId').val();
        $('#formview form').validate({
            messages: {
                TenCuocHop: {
                    required: 'Tên cuộc họp không được bỏ trống'
                },

                DiaDiem: {
                    required: 'Địa điểm không được bỏ trống'
                },
                ThoiGianCuocHop: {
                    required: "Thời gian cuộc họp không được bỏ trống"
                },
                NguoiChuTri: {
                    required: 'Người chủ trì không dược bỏ trống'
                }
            },
            rules: {
                TenCuocHop: {
                    required: true
                },

                DiaDiem: {
                    required: true
                },
                NguoiChuTri: {
                    required: true
                },
                ThoiGianCuocHop: {
                    required: true
                }
            }
        });



        if (cuochopId > 0) {

            FileManager();
            loadThanhPhanThamGia(1);
        }

        $("#formview").find("#btnGhiLai").unbind('click').click(function () {
            GhiLaiDuLieu(cuochopId);
        });
    }
});

// ajax hiển thị danh sách
// ko dùng
function jsHienThiDanhSach(TenCuocHop, page) {
    $.ajax({
        url: '/QuanLyCuocHop/LichHop/HienThiDanhSach',
        data: {
            TenCuocHop: TenCuocHop,
            Page: page
        },
        type: "POST",
        dataType: "html",
        success: function (data) {
            console.log(data);
        },
        complete: function (data) {

        }
    });
}

// ajax getdata
function getData(TenCuocHop, page, calendar) {
    // Thêm event vào bảng
    $.ajax({
        url: '/QuanLyCuocHop/LichHop/GetData',
        data: {
            TenCuocHop: TenCuocHop,
            Page: page
        },
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            // Hàm gì đó để chuẩn hoá và return data để đưa lên calendar
            var events = ChuyenHoa(data);
            calendar.addEventSource(events);
            // Hiện chức năng điều chỉnh cho lịch họp
            addSettingToEvents();
        },
        error: function (data) {

        }
    });
    // Thêm chức năng vào event
}

// Thuộc đơn vị trong view
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

// Render Calendar
function RenderCalendar() {
    //Date for the calendar events (dummy data)
    var Calendar = FullCalendar.Calendar;

    var calendarEl = document.getElementById('calendar');

    var calendar = new Calendar(calendarEl, {
        plugins: ['bootstrap', 'interaction', 'dayGrid', 'timeGrid'],
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        locale: 'vi',
    });

    calendar.render();

    // để tránh việc lỗi dữ liệu thì vẫn lên lịch chứ ko nên lên trang trắng
    getData($('#TenCuocHop').val(), 1, calendar)

}

// Chuyển hoá dữ liệu lấy từ server về để hiển thị lên calendar
function ChuyenHoa(data) {
    var newdata = [];
    $.each(data, function (index, item) {
        // lấy datetime bắt đầu
        var dateStart = item.NgayBatDau + "";
        dateStart = dateStart.substring(6, 19);
        dateStart = parseInt(dateStart);
        dateStart = new Date(dateStart);
        // lấy datetime kết thúc
        var dateEnd = item.NgayKetThuc + "";
        dateEnd = dateEnd.substring(6, 19);
        dateEnd = parseInt(dateEnd);
        dateEnd = new Date(dateEnd);
        // lấy màu
        var bgColor = "#0073b7"; // biển sẽ
        var now = new Date();

        if (now.valueOf() > dateEnd.valueOf()) {
            bgColor = "#00a65a"; // lá đã xong
        }
        else if (now.valueOf() > dateStart.valueOf()) {
            bgColor = "#f39c12"; // cam đang
        }

        // cho ra event tương ứng
        var event = {
            title: `${item.TenCuocHop} - ${item.NguoiChuTri}`,
            start: dateStart,
            end: dateEnd,
            backgroundColor: bgColor,
            borderColor: bgColor,
            textColor: '#fff',
            url: `/QuanLyCuocHop/LichHop/ThemMoi/${item.CuocHopId}`,
        };
        newdata.push(event);
    });
    return newdata;
}


function GhiLaiDuLieu(CuocHopId) {
    $("#formview #ThuocCoQuanId").val($("#formview #ddlCoQuanId").val());
    if ($("#formview form").valid()) {
        if (CuocHopId > 0) {
            $.ajax({
                url: "/QuanLyCuocHop/LichHop/ThemMoi",
                data: $("#formview form").serialize(),
                type: "POST",
                dataType: "json",
                success: function (data) {
                    if (data && data.status == true) {
                        toastr.success("Đã cập nhật thông tin cuộc họp");
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

function ThemMoi(id) {
    location.href = "/QuanLyCuocHop/LichHop/ThemMoi/" + id;
}

function XoaKhoiDanhSach(CuocHopId) {
    $.ajax({
        url: '/QuanLyCuocHop/LichHop/Xoa',
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
    });
}


// Thêm sửa xoá tài Liệu cuộc họp
function loadTaiLieuCuocHop(pageNum) {
    var page = pageNum == undefined ? $("#hdPageTLCH").val() : pageNum;
    var TLCHId = $("#CuocHopId").val();
    dsTaiLieuCuocHop(TLCHId, page);

}
function dsTaiLieuCuocHop(CuocHopId, page) {
    $.ajax({
        url: '/QuanLyCuocHop/TaiLieuCuocHop/HienThiDanhSachTaiLieu',
        data: {
            CuocHopId: CuocHopId,
            Page: page
        },
        type: 'POST',
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

function XoaTaiLieu(TaiLieuCuocHopId) {
    $.ajax({
        url: '/QuanLyCuocHop/TaiLieuCuocHop/Xoa',
        data: { TaiLieuCuocHopId: TaiLieuCuocHopId },
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                toastr.success('Đã xoá tài liệu cuộc họp khỏi danh sách')
                loadTaiLieuCuocHop(1);
            } else {
                toastr.error("Có lỗi hệ thống xảy ra");
            }
        },
        error: function (data) {
            toastr.error(data.message);
        }
    });

}

function HieuChinhTaiLieu(tailieuId, cuochopId) {
    $.ajax({
        url: "/QuanLyCuocHop/TaiLieuCuocHop/HieuChinh",
        data: { id: tailieuId },
        dataType: 'html',
        success: function (data) {
            // view thong tin tai lieu
            var $dialog = taviJs.showDialog({
                id: 'dialogHieuChinhTaiLieu',
                content: data,
                width: 550
            });

            // validate form
            $dialog.find("form").validate({
                messages: {
                    customFile: {
                        required: "Tài liệu không được bỏ trống"
                    },

                },
                rules: {
                    customFile: {
                        required: true
                    },

                }
            });

            // event save
            $dialog.find(".cmd-save").unbind('click').click(function () {
                if ($dialog.find("form").valid()) {
                    $dialog.find('#CuocHopId').val(cuochopId);
                    $dialog.find("#ConSuDung").val($dialog.find("#TLCH_ConSuDung").prop("checked"));

                    $.ajax({
                        url: '/QuanLyCuocHop/TaiLieuCuocHop/GhiLai',
                        type: 'POST',
                        data: $dialog.find("form").serialize(),
                        dataType: 'json',
                        success: function (data) {
                            if (data && data.status) {
                                toastr.success("Đã ghi lại tài liệu cuộc họp");
                                loadTaiLieuCuocHop(1);
                            } else {
                                toastr.error("Chưa lưu được tài liệu");
                            }
                            $dialog.modal("hide");
                        },
                        error: function () {
                            toastr.error("Chưa lưu được dữ liệu");
                        }
                    });
                } else {
                    toastr.error("Thiếu thông tin yêu cầu của tài liệu");
                }
            })
        },
        error: function () {
            toastr.error("Không hiệu chỉnh được tài liệu cuộc họp")
        }
    });
}
// Thêm sửa xoá thành phần tham gia
function loadThanhPhanThamGia(pageNum) {
    var page = pageNum == undefined ? $("#hdPageTPTG").val() : pageNum;
    var TPTGId = $("#CuocHopId").val();
    dsThanhPhanThamGia(TPTGId, page);

}
function dsThanhPhanThamGia(CuocHopId, page) {
    $.ajax({
        url: '/QuanLyCuocHop/ThanhPhanThamGia/HienThiDanhSachThanhPhan',
        data: {
            CuocHopId: CuocHopId,
            Page: page
        },
        type: 'POST',
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

function XoaThanhPhan(ThanhPhanThamGiaId) {
    $.ajax({
        url: '/QuanLyCuocHop/ThanhPhanThamGia/Xoa',
        data: { ThanhPhanThamGiaId: ThanhPhanThamGiaId },
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                toastr.success('Đã xoá thành phần khỏi danh sách')
                loadThanhPhanThamGia(1);
            } else {
                toastr.error("Có lỗi hệ thống xảy ra");
            }
        },
        error: function (data) {
            toastr.error(data.message);
        }
    });

}

function HieuChinhThanhPhan(thanhphanId, cuochopId) {

    $.ajax({
        url: "/QuanLyCuocHop/ThanhPhanThamGia/HieuChinh",
        data: { id: thanhphanId },
        dataType: 'html',
        success: function (data) {
            // view thong tin tai lieu
            var $dialog = taviJs.showDialog({
                id: 'dialogHieuChinhThanhPhan',
                content: data,
                width: 550
            });


            // validate form
            $dialog.find("form").validate({
                messages: {
                    Ten: {
                        required: "Tên người tham gia không được bỏ trống"
                    },
                    LoaiThanhPhan: {
                        required: "Loại thành phần không được bỏ trống"
                    },
                },
                rules: {
                    Ten: {
                        required: true
                    },
                    LoaiThanhPhan: {
                        required: true
                    }
                }
            });

            // event save
            $dialog.find(".cmd-save").unbind('click').click(function () {
                if ($dialog.find("form").valid()) {
                    $dialog.find('#CuocHopId').val(cuochopId);
                    $dialog.find('#ThuocCoQuanId').val(ThuocCoQuanId);
                    $dialog.find("#ConSuDung").val($dialog.find("#TPTG_ConSuDung").prop("checked"));

                    $.ajax({
                        url: '/QuanLyCuocHop/ThanhPhanThamGia/GhiLai',
                        type: 'POST',
                        data: $dialog.find("form").serialize(),
                        dataType: 'json',
                        success: function (data) {
                            if (data && data.status) {
                                toastr.success("Đã ghi lại thành phần tham gia cuộc họp");
                                loadThanhPhanThamGia(1);
                            } else {
                                toastr.error("Chưa lưu được người tham gia");
                            }
                            $dialog.modal("hide");
                        },
                        error: function () {
                            toastr.error("Chưa lưu được dữ liệu");
                        }
                    });
                } else {
                    toastr.error("Thiếu thông tin yêu cầu của người tham gia");
                }
            })
        },
        error: function () {
            toastr.error("Không hiệu chỉnh được thành phần tham gia cuộc họp")
        }
    });
}

// Thao tác với file
function FileManager() {
    $.ajax({
        url: "/QuanLyCuocHop/TaiLieuCuocHop/Index",
        type: "GET",
        dataType: "html",
        success: function (data) {
            $('#dsTaiLieu').html(data);
        },
        error: function () {
            console.log("Lỗi");
        },
        complete: function () {
            var arrayFile = [];

            LoadListFileUploaded();

            $('#customFile').change(function (e) {
                var files = e.target.files;
                if (files.length > 0) {
                    $.each(files, function (index, value) {
                        arrayFile.push(value);
                    })
                }
                LoadFileUploading(arrayFile);
            });

            $("#btnUploadAllFile").click(function () {
                $.each(arrayFile, function (index, file) {
                    UploadSingleFile(file, index);
                })
            });

            $("#btnClearAllFile").click(function () {
                arrayFile = [];
                LoadFileUploading(arrayFile);
            });
        }

    });
}

function LoadFileUploading(arrayFile) {
    var html = "";
    $.each(arrayFile, function (index, file) {
        html += `<tr id="fileUpload_` + index + `" data-fileId="` + index + `">
                    <td>` + file.name + `</td>
                    <td class="text-right">` + taviJs.displayFileSize(file.size) + `</td>
                    <td class="align-middle"><div class="progress active">
                          <div class="progress-bar bg-primary progress-bar-striped" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                            <span class="sr-only">0% Complete (success)</span>
                          </div>
                        </div>
                    </td>
                    <td class="text-center"><span><i class='fa fa-trash text-danger'></i></span></td>
                </tr>`;
    })
    $("#dataGridFileLoading").find("tbody").html(html);
}

function LoadListFileUploaded() {
    var CuocHopId = $('#CuocHopId').val();
    $.ajax({
        url: "/QuanLyCuocHop/TaiLieuCuocHop/GetListFile",
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

function RemoveFile(filePath) {
    $.ajax({
        url: "/QuanLyCuocHop/TaiLieuCuocHop/RemoveFile",
        type: "GET",
        data: { filePath: filePath },
        dataType: "json",
        success: function (data) {
            if (data) {
                toastr.success("Đã xóa file");
                LoadListFileUploaded();
            } else {
                toastr.error("Chưa xóa được file");
                LoadListFileUploaded();
            }
        },
        error: function (data) {
            toastr.error("Có lỗi xảy ra");
        }
    });
}

function UploadSingleFile(file, index) {
    var fileId = index;

    var uploaderForm = new FormData();
    uploaderForm.append("file", file);
    var CuocHopId = $("#CuocHopId").val();
    uploaderForm.append("CuocHopId", CuocHopId);

    // ajax upload file
    $.ajax({
        url: "/QuanLyCuocHop/TaiLieuCuocHop/SaveFile",
        data: uploaderForm,
        type: "post",
        dataType: "json",
        processData: false,
        contentType: false,
        xhr: function () {
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) {
                // progressing event
                myXhr.upload.addEventListener('progress', function (e) {
                    var percent = (e.loaded / e.total) * 100;
                    $("#dataGridFileLoading").find("#fileUpload_" + fileId).find(".progress-bar").css("width", percent + "%")
                }, false);

                // loaded event
                myXhr.upload.addEventListener('load', function (e) {
                    var percent = (e.loaded / e.total) * 100;
                    $("#dataGridFileLoading").find("#fileUpload_" + fileId).find(".progress-bar").css("width", "100%");
                    $("#dataGridFileLoading").find("#fileUpload_" + fileId).find(".progress-bar").removeClass("progress-bar-striped");
                }, false);
            }
            return myXhr;
        },
        success: function () {
            LoadListFileUploaded();
        },
        error: function () {
            toastr.error("Tải lên file " + file.name + " thất bại");
        },
    })
}