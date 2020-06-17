$(document).ready(function () {
    if ($('#ActionName').val() == 'Index') {
        // hien thi danh sach
        jsHienThiDanhSach($('#SoKyHieu').val(), $('#TrichYeu').val(), $('#Page').val());
        $('#formview input').each(function () {
            $(this).focus(function () {
                $(this).keypress(function (e) {
                    if (e.which == 13 || e.keyCode == 13) {
                        jsHienThiDanhSach($('#SoKyHieu').val(), $('#TrichYeu').val(), 1);
                    }
                })
            })
        });
        $('#btnTimKiem').unbind('click').click(function () {
            jsHienThiDanhSach($('#SoKyHieu').val(), $('#TrichYeu').val(), 1);
        })

        $('#Create').unbind('click').click(function () {
            window.location = '/NghiQuyet/QuanLyNghiQuyet/ThemMoi';
        });
    }
    else if ($('#ActionName').val() == 'ThemMoi') {
        var NghiQuyetId = $('#NghiQuyetId').val();

        loaivb();
        loailv();

        $('#formview form').validate({
            messages: {
                SoThuTu: {
                    required: 'Số thứ tự không được bỏ trống',
                    number: 'Số thứ tự phải là số'
                },

                SoKyHieu: {
                    required: 'Số/Ký hiệu không được bỏ trống'
                },
                TrichYeu: {
                    required: "Trích yếu không được bỏ trống"
                }
            },
            rules: {
                SoThuTu: {
                    required: true,
                    number: true
                },

                SoKyHieu: {
                    required: true
                },
                TrichYeu: {
                    required: true
                }
            }
        });

        //ko select dropdown neu la thêm đối tượng mới
        slLinhVuc(0);


        // Sửa đối tượng có sẵn thì load dr down về giá trị
        if (NghiQuyetId > 0) {
            FileManager();
        }
        $("#formview").find("#btnGhiLai").unbind('click').click(function () {
            GhiLaiDuLieu(NghiQuyetId);
        });
    }
});

// load dr down về id đã chọn
function slLinhVuc(id) {
    $('#slLinhVuc').val(id);
    $('#slLinhVuc').select2().trigger('change');
}
function slLoaiVanBan(id) {
    $('#slLoaiVanBan').val(id);
    $('#slLoaiVanBan').select2().trigger('change');
}


// Load value trong dropdownlist loai van ban và lĩnh vực
function loaivb() {
    $.ajax({
        url: '/DanhMucChung/LoaiVanBan',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $("#slLoaiVanBan").select2({
                data: [{ id: 0, text: "---- Chọn loại văn bản ----" }, ...data],
                theme: "bootstrap4"
            });
            if ($('#NghiQuyetId').val() == 0) {
                slLoaiVanBan(0);
            }
            else
                slLoaiVanBan($('#LoaiVanBanId').val());

        },
        error: function () {
            alert("Lỗi!");
        }
    });
}
function loailv() {
    $.ajax({
        url: '/DanhMucChung/LinhVuc',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $("#slLinhVuc").select2({
                data: [{ id: 0, text: "---- Chọn lĩnh vực ----" }, ...data],
                theme: "bootstrap4"
            });
            if ($('#NghiQuyetId').val() == 0) {
                slLinhVuc(0);
            }
            else
                slLinhVuc($('#LinhVucId').val());
        },
        error: function () {
            alert("Lỗi!");
        }
    });
}

// ajax hien thi danh sach
function jsHienThiDanhSach(SoKyHieu, TrichYeu, page) {
    $.ajax({
        url: '/NghiQuyet/QuanLyNghiQuyet/HienThiDanhSach',
        data: {
            SoKyHieu: SoKyHieu,
            TrichYeu: TrichYeu,
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

// ajax xoá khỏi danh sách
function XoaKhoiDanhSach(NghiQuyetId) {
    $.ajax({
        url: '/NghiQuyet/QuanLyNghiQuyet/Xoa',
        data: {
            NghiQuyetId: NghiQuyetId
        },
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                toastr.success('Đã xoá nghị quyết khỏi danh sách')
                jsHienThiDanhSach($('#SoKyHieu').val(), $('#TrichYeu').val(), 1);
            } else {
                toastr.error("Có lỗi hệ thống xảy ra");
            }
        },
        error: function (data) {
            toastr.error(data.message);
        }
    })
}

// Ghi lại dữ liệu
function GhiLaiDuLieu(NghiQuyetId) {
    if ($('#formview form').valid()) {
        $('#LoaiVanBanId').val($('#slLoaiVanBan').val());
        $('#LinhVucId').val($('#slLinhVuc').val());
        debugger
        if (NghiQuyetId > 0) {

            $.ajax({
                url: "/NghiQuyet/QuanLyNghiQuyet/ThemMoi",
                data: $('#formview form').serialize(),
                type: "POST",
                dataType: "json",
                success: function (data) {
                    if (data && data.status == true) {
                        toastr.success("Đã cập nhật thông tin nghị quyết");
                    } else {
                        toastr.error("Cập nhật thông tin không thành công");
                    }
                },
                error: function () {
                    toastr.error("Cập nhật thông tin không thành công");
                }
            });
        } else {
            $("#formview form").submit();
        }
    }
}

// Đến trang hiệu chỉnh
function ThemMoi(id) {
    window.location = `/NghiQuyet/QuanLyNghiQuyet/ThemMoi/${id}`;
}

// Thao tác với file
function FileManager() {
    $.ajax({
        url: "/NghiQuyet/TaiLieu/Index",
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
    var NghiQuyetId = $('#NghiQuyetId').val();
    $.ajax({
        url: "/NghiQuyet/TaiLieu/GetListFile",
        type: "GET",
        data: {NghiQuyetId:NghiQuyetId},
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
        url: "/NghiQuyet/TaiLieu/RemoveFile",
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
    var NghiQuyetId = $("#NghiQuyetId").val();
    uploaderForm.append("NghiQuyetId", NghiQuyetId);

    // ajax upload file
    $.ajax({
        url: "/NghiQuyet/TaiLieu/SaveFile",
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