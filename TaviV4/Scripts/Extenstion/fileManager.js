$(document).ready(function () {

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
    })

    $("#btnClearAllFile").click(function () {
        arrayFile = [];
        LoadFileUploading(arrayFile);
    })
})

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
    $.ajax({
        url: "/Extension/GetListFile",
        type: "GET",
        dataType: "html",
        success: function (data) {
            $("#dataGrid").html(data);
        },
        error: function (data) {
            toastr.error("Không lấy được danh sách file")
        }
    })
}

function ViewFile(filePath) {

}

function RemoveFile(filePath) {
    $.ajax({
        url: "/Extension/RemoveFile",
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
    })
}

function UploadSingleFile(file, index) {
    var fileId = index;

    var uploaderForm = new FormData();
    uploaderForm.append("file", file);

    // ajax upload file
    $.ajax({
        url: "/Extension/SaveFile",
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
