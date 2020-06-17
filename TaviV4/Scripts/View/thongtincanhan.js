$(document).ready(function () {


    var readURL = function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('.avatar').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }

    var saveImage = function (input) {
        if (input.files && input.files[0]) {
            // save data
            var fd = new FormData();
            var file = input.files[0];
            fd.append('file', file);
            $.ajax({
                url: '/BoMayToChuc/CanBo/LuuAnhDaiDien',
                type: 'post',
                data: fd,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response) {
                        toastr.success("Đã thay đổi ảnh đại diện")
                    } else {
                        toastr.error('Chưa thay đổi được ');
                    }
                },
            });
        }
    }

    $(".file-upload").on('change', function () {
        readURL(this);
        saveImage(this);
    });

    $("#us_info form").validate({
        rules: {
            HoVaTen: {
                required: true,
            },
            Email: {
                email: true
            }
        },
        messages: {
            HoVaTen: {
                required: "Họ tên không được bỏ trống",
            },
            Email: {
                email: "Email chưa đúng định dạng"
            }
        }
    });

    $("#us_account form").validate({
        rules: {
            MatKhau: {
                required: true,
            },
            MatKhauMoi: {
                required: true,
            },
            XacThucMatKhau: {
                required: true,
                equalTo: "#MatKhauMoi"
            }
        },
        messages: {
            MatKhau: {
                required: "Chưa nhập mật khẩu cũ",
            },
            MatKhauMoi: {
                required: "Chưa nhập mật khẩu mới",
            },
            XacThucMatKhau: {
                required: "Chưa nhập xác thực mật khẩu",
                equalTo: "Mật khẩu xác thực không trùng khớp"
            }
        }
    });
});

function CapNhatThongTin() {
    var form = $("#us_info form");
    if (form.valid()) {
        $.ajax({
            url: '/BoMayToChuc/CanBo/CapNhatThongTin',
            type: 'post',
            data: form.serialize(),
            dataType: "json",
            success: function (response) {
                if (response.Success) {
                    toastr.success("Đã thay đổi thông tin")
                } else {
                    toastr.error('Chưa thay đổi được thông tin');
                }
            },
            error: function (response) {
                toastr.error("Có lỗi xảy ra");
            }
        });
    }
}

function CapNhatMatKhau() {
    var form = $("#us_account form");
    if (form.valid()) {
        $.ajax({
            url: '/BoMayToChuc/CanBo/CapNhatMatKhau',
            type: 'post',
            data: form.serialize(),
            dataType: "json",
            success: function (response) {
                if (response.Success) {
                    toastr.success(response.Message)
                } else {
                    toastr.error(response.Message);
                }
            },
            error: function (response) {
                toastr.error("Có lỗi xảy ra");
            }
        });
    }
}