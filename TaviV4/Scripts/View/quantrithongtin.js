
$(document).ready(function () {
    var actionName = $("#ActionName").val();

    // init tree view
    $("#jsTreeChiTieu").jstree({
        core: {
            data: {
                url: '/ThongTinTongHop/QuanTriThongTin/DanhSachChiTieu',
                data: function (node) {
                    return { PhanHeId: $("#hdPhanHeId").val(), ThuocChiTieuId: node.id }
                }
            },
            multiple: false
        },
        search: {
            show_only_matches: true,
            ajax: {
                url: '/ThongTinTongHop/QuanTriThongTin/DanhSachChiTieu_Search?PhanHeId=1',
                data: function (str) {
                    return { PhanHeId: $("#hdPhanHeId").val(), TenChiTieu: str }
                }  
            }
        },
        themes: {
            variant: "large"
        },
        plugins: ['state','search'],
    });

    // event change
    $("#jsTreeChiTieu").on("changed.jstree", function (e, data) {
        if (data.node) {
            // load danh sach bao bao
            LoadDanhSachBaoCao(data.node.id);
        }
    })

    $("#btnSearchTree").click(function () {
        var str = $("#txtSearchTree").val();
        $("#jsTreeChiTieu").jstree(true).search(str);
    })

    if (actionName == "ThemMoi") {
        var $form = $("#formview form");

        // hidden value
        var baocaoId = $("#KetQuaBaoCaoId").val();
        var chitieuId = $("#ThuocChiTieuId").val();
        var tinhId = $("#ThuocTinhId").val();
        var huyenId = $("#ThuocHuyenId").val();
        var xaId = $("#ThuocXaId").val();

        $form.validate({
            errorPlacement: function (error, element) {
                //debugger;
            },
            highlight: function (element, errorClass, validClass) {
                //debugger;
            },

            //When removing make the same adjustments as when adding
            unhighlight: function (element, errorClass, validClass) {
                //debugger;
            },
            rules: {
                ddlTinh: {
                    required: true
                },
                Nam: {
                    required: true
                }
            },
            messages: {
                ddlTinh: {
                    required: "Chưa chọn tỉnh"
                },
                ddlNam: {
                    required: "Chưa chọn năm"
                }
            }
        })

        // load danh sach ky bao cao
        LoadDanhSachKyBaoCao(baocaoId, 0);

        LoadDanhSachChiTietBaoCaoTrucThuoc(chitieuId, tinhId, huyenId, xaId, 0, 0, 0, 0);


        // set select 2
        $("#Nam").select2();
        $("#Thang").select2();
        DisplayThoiGianBaoCao($("input[name=KyBaoCao]:checked").val());

        $("#CachTinhGiaTri").select2({
            //placeholder: "-- Chọn cách tính --",
            //allowClear: true,
            minimumResultsForSearch: -1,
            dropdownAutoWidth: true,
        })

        $("#CachTinhGiaTri").change(function () {
            var cachtinh = $(this).val();
            if (cachtinh == 1) {
                $("#GiaTriKetQuaSo").attr('readonly', true);
                $("#GiaTriKetQuaSo").val(Calc_Sum_Value());
            } else if (cachtinh == 2) {
                $("#GiaTriKetQuaSo").attr('readonly', true);

                var nam = $("#Nam").val();
                var quy = $("#Quy").val();
                var thang = $("#Thang").val();
                var kybaocao = $("input[name=KyBaoCao]:checked").val();

                $.ajax({
                    url: "/ThongTinTongHop/QuanTriThongTin/Get_TongGiaTri_TheoDonViHanhChinh",
                    data: { ChiTieuId: chitieuId, TinhId: tinhId, HuyenId: huyenId, XaId: xaId, KyBaoCao: kybaocao, NamBaoCao: nam, QuyBaoCao: quy, ThangBaoCao: thang },
                    dataType: "json",
                    success: function (data) {
                        $("#GiaTriKetQuaSo").val(data);
                    },
                    error: function () {
                        toastr.error("Chưa lấy được tổng giá trị");
                    }
                })
            } else {
                $("#GiaTriKetQuaSo").attr('readonly', false);
            }
        });

        $("input[name=KyBaoCao]").change(function () {
            DisplayThoiGianBaoCao($("input[name=KyBaoCao]:checked").val());
            LoadChiTietKyBaoCao_ByData()
        })
        $("#Nam").on("select2:select", function (e) { LoadChiTietKyBaoCao_ByData() });
        $("#Thang").on("select2:select", function (e) { LoadChiTietKyBaoCao_ByData() });
        $("#Quy").change(function () { LoadChiTietKyBaoCao_ByData() });
        

        // load danh mục
        taviJs.load_DanhMuc_Tinh("ddlTinh", tinhId);
        taviJs.load_DanhMuc_Huyen("ddlHuyen", huyenId, tinhId);
        taviJs.load_DanhMuc_Xa("ddlXa", xaId, huyenId);

        $("#ddlTinh").change(function () {
            var id = $("#ddlTinh").val();
            taviJs.load_DanhMuc_Huyen("ddlHuyen", 0, id);
            taviJs.load_DanhMuc_Xa("ddlXa", 0, 0);
        })

        $("#ddlHuyen").change(function () {
            var id = $("#ddlHuyen").val();
            taviJs.load_DanhMuc_Xa("ddlXa", 0, id);
        })

        $("#ddlTinh").attr("disabled", true);
        $("#ddlHuyen").attr("disabled", true);
        $("#ddlXa").attr("disabled", true);

        $("#btnLuuLai").click(function () {
            if ($form.valid()) {
                LuuKetQuaBaoCao();
            }
        })

        $("#btnQuayLai").click(function () {
            window.history.back();
        })

        $("#btnLamMoiKyBaoCao").click(function () {
            LoadDanhSachKyBaoCao(baocaoId, 0);
        })
    }
});

function LoadDanhSachBaoCao(chitieuId, page) {
    var phanheId = $("#hdPhanHeId").val();
    $.ajax({
        url: "/ThongTinTongHop/QuanTriThongTin/DanhSachBaoBao",
        data: { PhanHeId: phanheId, ThuocChiTieuId: chitieuId, page: page },
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            $("#card-dsBaoCao").showLoading();
        },
        success: function (data) {
            $("#card-dsBaoCao").hideLoading();
            $("#gridDanhSachBaoCao").html(data);
        },
        error: function () {
            toastr.error("Không  lấy được danh sách báo cáo");
        }
    });
}

function ThemBaoCao(id) {
    var phanheId = $("#hdPhanHeId").val();
    var selectedNodes = $("#jsTreeChiTieu").jstree("get_selected", true);
    if (selectedNodes.length <= 0) {
        toastr.warning("Chưa chọn chỉ tiêu để cập nhập báo cáo");
        return false;
    }
    // get data
    var chitieu = selectedNodes[0];
    var chitieuId = chitieu.id;
    var tenchitieu = chitieu.text;

    // view detail
    if (id != undefined && id > 0) {
        document.location = "/ThongTinTongHop/QuanTriThongTin/ChiTietBaoCao?PhanHeId=" + phanheId + "&BaoCaoId=" + id + "&ChiTieuId=" + chitieuId;
        return;
    }

    // create new report dialog
    var $dialog = taviJs.showDialogCustom({
        id: "dialogTaoBaoCao",
        type: "info",
        title: "Tạo báo cáo",
        width: 700,
        content: `<form>
                        <div class="form-group row">
                            <div class="col-md-2">
                                <label class="col-form-label">Chỉ tiêu</label>
                            </div>
                            <div class="col-md-10">
                                <input type="text" name="TenChiTieu" class="form-control" readonly value="` + tenchitieu + `"/>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-md-2">
                                <label class="col-form-label">Địa bàn</label>
                            </div>
                            <div class="col-md-10 row p-0 m-0">
                                <div class="col-md-4 mb-1">
                                    <select class="form-control" name="ddlTinh" id="ddlTinh"></select>
                                </div>
                                <div class="col-md-4 mb-1">
                                    <select class="form-control" name="ddlHuyen" id="ddlHuyen"></select>
                                </div>
                                <div class="col-md-4">
                                    <select class="form-control" name="ddlXa" id="ddlXa"></select>
                                </div>
                            </div>
                        </div>
                    </form>`
    });

    // load danh mục
    taviJs.load_DanhMuc_Tinh("ddlTinh", 0);
    taviJs.load_DanhMuc_Huyen("ddlHuyen", 0, 0);
    taviJs.load_DanhMuc_Xa("ddlXa", 0, 0);
    $("#ddlTinh").change(function () {
        var id = $("#ddlTinh").val();
        taviJs.load_DanhMuc_Huyen("ddlHuyen", 0, id);
        taviJs.load_DanhMuc_Xa("ddlXa", 0, 0);
    })
    $("#ddlHuyen").change(function () {
        var id = $("#ddlHuyen").val();
        taviJs.load_DanhMuc_Xa("ddlXa", 0, id);
    })

    $dialog.find(".cmd-save").click(function () {
        var tinhData = $("#ddlTinh").select2("data")[0];
        if (tinhData.id == "" || tinhData.id == 0) {
            toastr.warning("Hãy chọn địa bàn để tạo báo cáo");
            return;
        }
        var huyenData = $("#ddlHuyen").select2("data")[0];
        var xaData = $("#ddlXa").select2("data")[0];

        $.ajax({
            url: "/ThongTinTongHop/QuanTriThongTin/TaoBaoCao",
            data: {
                ChiTieuId: chitieuId,
                TinhId: tinhData.id,
                TenTinh: tinhData.id == null || tinhData.id == "" ? null : tinhData.text,
                HuyenId: huyenData.id,
                TenHuyen: huyenData.id == null || huyenData.id == "" ? null : huyenData.text,
                XaId: xaData.id,
                TenXa: xaData.id == null || xaData.id == "" ? null : xaData.text
            },
            dataType: 'json',
            success: function (res) {
                if (res.Success == true) {
                    document.location = "/ThongTinTongHop/QuanTriThongTin/ChiTietBaoCao?PhanHeId=" + res.Data.PhanHeId + "&BaoCaoId=" + res.Data.BaoCaoId + "&ChiTieuId=" + res.Data.ChiTieuId;
                } else {
                    toastr.error(res.Message);
                }
            },
            error: function (res) {
                toastr.error("Có lỗi xảy ra");
            }
        })
    })


}

function DisplayThoiGianBaoCao(value) {
    if (value == undefined || value == 1) { // ky bao cao nam
        $("#Thang").parent().hide();
        $("#Quy").parent().hide();
        // remove validate in hidden element
        changeRequiredValidate($("#Thang"), "remove");
        changeRequiredValidate($("#Quy"), "remove");

    } else if (value == 2) { // ky bao cao quy
        $("#Quy").parent().show();
        $("#Thang").parent().hide();
        // remove validate in hidden element
        changeRequiredValidate($("#Thang"), "aremove");
        changeRequiredValidate($("#Quy"), "add","Chưa chọn quý");
    } else {
        $("#Quy").parent().hide();
        $("#Thang").parent().show();
        // remove validate in hidden element
        changeRequiredValidate($("#Quy"), "aremove");
        changeRequiredValidate($("#Thang"), "add", "Chưa chọn tháng");
    }
}

function changeRequiredValidate($ele, type, message) {
    isRequired = $ele.rules().required;
    if (type == "add") {
        if (!isRequired)
            $ele.rules("add", {
                required: true,
                messages: {
                    required: message
                }
            })
    } else if (type == "remove") {
        if (isRequired)
            $ele.rules("remove", "required");
    }
}

function LoadDanhSachKyBaoCao(baocaoId, selectedId) {
    $.ajax({
        url: "/ThongTinTongHop/QuanTriThongTin/DanhSachKyBaoCao",
        data: { baocaoId: baocaoId },
        dataType: "html",
        beforeSend: function () {
            $("#card-GridKyBaoCao").showLoading(30000);
        },
        success: function (data) {
            $("#card-GridKyBaoCao").hideLoading();
            $("#grid-KyBaoCao").html(data);
            if (selectedId == 0) {
                var firstRow = $("#grid-KyBaoCao table tbody tr").first();
                if (firstRow.attr("chitietId") != 0) {
                    firstRow.trigger("click");
                }
            } else {
                var selectedRow = $("#grid-KyBaoCao table tbody tr[chitietId=" + selectedId + "]");
                selectedRow.trigger("click");
            }
        },
        error: function (data) {
            $("#card-GridKyBaoCao").hideLoading();
            toastr.error("Chưa lấy được sanh sách kỳ báo cáo")
        }
    });
}

function getData_KyBaoCao()
{
    var baocaoId = $("#KetQuaBaoCaoId").val();
    var tinhData = $("#ddlTinh").select2("data")[0];
    var huyenData = $("#ddlHuyen").select2("data")[0];
    var xaData = $("#ddlXa").select2("data")[0];
    var phanheId = $("#hdPhanHeId").val();
    var chitieuId = $("#ThuocChiTieuId").val();
    var trangthai = $("input[name=TrangThaiXuLy]:checked").val();
    var consudung = $("#ConHieuLuc").prop("checked");
    var hienthi = $("#HienThiTongHopTrangChu").prop("checked");
    return {
        KetQuaBaoCaoId: baocaoId,
        PhanHeThongTinId: phanheId,
        DinhNghiaChiTieuId: chitieuId,
        ThuocTinhId: tinhData.id,
        TenTinh: tinhData.id == null || tinhData.id == "" ? null : tinhData.text,
        ThuocHuyenId: huyenData.id,
        TenHuyen: huyenData.id == null || huyenData.id == "" ? null : huyenData.text,
        ThuocXaId: xaData.id,
        TenXa: xaData.id == null || xaData.id == "" ? null : xaData.text,
        TrangThai: trangthai,
        ConHieuLuc: consudung,
        HienThiTongHopTrangChu: hienthi
    }
}

function getData_GiaTri_KyBaoCao() {
    var nam = $("#Nam").val();
    var quy = $("#Quy").val();
    var thang = $("#Thang").val();
    var kybaocao = $("input[name=KyBaoCao]:checked").val();
    var giatriso = $("#GiaTriKetQuaSo").val();
    var giatrikhangdinh = $("#GiaTriKetQuaKhangDinh").prop("checked");
    var giatritieuchuan = $("#GiaTriTieuChuan").val();
    var cachtinhgiatri = $("#CachTinhGiaTri").val();
    return {
        NamBaoCao: nam,
        ThangBaoCao: thang,
        QuyBaoCao: quy,
        KyBaoCao: kybaocao,
        GiaTriSo: giatriso,
        GiaTriKhangDinh: giatrikhangdinh,
        GiaTriTieuChuan: giatritieuchuan,
        CachTinhGiaTri: cachtinhgiatri
    }
}

function getData_Grid_TrucThuoc() {
    var listdata = [];
    var table = $("#grid-ChiTietDuLieuTrucThuoc table");
    var rows = table.find("tbody").find("tr");
    if (rows.length > 0) {
        rows.each(function (index, row) {
            var ketquabaocaoid = $(row).data("ketquabaocaoid");
            var dinhnghiachitieuid = $(row).data("dinhnghiachitieuid");
            var giatriso = $(row).find("#inp-" + dinhnghiachitieuid) == undefined ? null : $(row).find("#inp-" + dinhnghiachitieuid).val();
            var giatrikhangdinh = $(row).find("#cbxKetQuaDanhGia-" + dinhnghiachitieuid) == undefined ? null : $(row).find("#cbxKetQuaDanhGia-" + dinhnghiachitieuid).prop("checked");
            var consudung = $(row).find("#cbxConSuDungKetQua-" + dinhnghiachitieuid).prop("checked");
            var giatritieuchuan = $(row).find("#inp-GiaTriTieuChuan-" + dinhnghiachitieuid).val();
            listdata.push({
                KetQuaBaoCaoId: ketquabaocaoid,
                DinhNghiaChiTieuId: dinhnghiachitieuid,
                GiaTriSo: giatriso,
                GiaTriKhangDinh: giatrikhangdinh,
                GiaTriTieuChuan: giatritieuchuan,
                ConSuDung: consudung
            })
        })
    }
    return listdata;
}

function LuuKetQuaBaoCao() {
    var ketqua = getData_KyBaoCao();
    var chitiet = getData_GiaTri_KyBaoCao();
    var tructhuoc = getData_Grid_TrucThuoc();
    $.ajax({
        url: "/ThongTinTongHop/QuanTriThongTin/LuuKetQua",
        data: { KetQuaBaoCao: ketqua, ChiTietBaoCao: chitiet, DanhSachTrucThuoc: tructhuoc },
        dataType: "json",
        type: "POST",
        success: function (result) {
            if (result.success) {
                $("#KetQuaBaoCaoId").val(result.data.baocaoId);
                LoadDanhSachKyBaoCao(result.data.baocaoId, result.data.chitietId);
                toastr.success("Dữ liệu đã được ghi lại");
            } else {
                toastr.error("Chưa lưu được dữ liệu");
            }
        },
        error: function (result) {
            toastr.error("Chưa lưu được dữ liệu")
        }
    })
}

function OnClick_KyBaoCao(row, event) {
    if (event) {
        target = event.target;
        if (target.tagName == "A" || target.tagName == "I") {
            return true;
        }
    }
    var chitietId = $(row).attr("chitietId");
    if (chitietId == "" || chitietId == 0) return;
    $(row).parent().find("tr.active").removeClass("active");
    $(row).addClass("active");
    LoadChiTietKyBaoCao(chitietId);
}

function LoadChiTietKyBaoCao(chitietId) {
    $("#formChiTietBaoCao").showLoading();
    $.ajax({
        url: "/ThongTinTongHop/QuanTriThongTin/ChiTietKetQuaBaoCao",
        data: { chitietId: chitietId },
        dataType: 'json',
        type: "GET",
        success: function (data) {
            LoadIntoForm(data);
        },
        error: function (data) {
            toastr.error("Không lấy được dữ liệu");
        }
    });
}

function LoadChiTietKyBaoCao_ByData() {
    var bcId = $("#KetQuaBaoCaoId").val();
    var kybc = $("input[name=KyBaoCao]:checked").val();
    var nam = $("#Nam").val();
    var quy = $("#Quy").val();
    var thang = $("#Thang").val();
    $.ajax({
        url: "/ThongTinTongHop/QuanTriThongTin/Get_ChiTietKetQuaBaoCao",
        data: { BaoCaoId: bcId, KyBaoCao: kybc, Nam: nam, Thang: thang, Quy: quy },
        dataType: 'json',
        type: "GET",
        success: function (data) {
            LoadIntoForm(data);
        },
        error: function (data) {
            toastr.error("Không lấy được dữ liệu");
        }
    });
}

function LoadIntoForm(data) {
    $("input[name=KyBaoCao][value=" + data.KyBaoCao + "]").prop("checked", true);
    DisplayThoiGianBaoCao($("input[name=KyBaoCao]:checked").val());
    if (data.NamBaoCao != null) {
        $("#Nam").val(data.NamBaoCao).trigger("change");
    }
    if (data.QuyBaoCao != null)
        $("#Quy").val(data.QuyBaoCao);
    if (data.ThangBaoCao != null)
        $("#Thang").val(data.ThangBaoCao).trigger("change");
    $("#GiaTriKetQuaSo").val(data.GiaTriSo);
    $("#GiaTriKetQuaKhangDinh").prop("checked", data.GiaTriKhangDinh == true ? true : false);
    $("#GiaTriTieuChuan").val(data.GiaTriTieuChuan);
    $("#CachTinhGiaTri").val(data.CachTinhGiaTri).trigger("change");
    var baocaoId = $("#KetQuaBaoCaoId").val();
    var chitieuId = $("#ThuocChiTieuId").val();
    var tinhId = $("#ThuocTinhId").val();
    var huyenId = $("#ThuocHuyenId").val();
    var xaId = $("#ThuocXaId").val();
    LoadDanhSachChiTietBaoCaoTrucThuoc(chitieuId, tinhId, huyenId, xaId, data.KyBaoCao, data.NamBaoCao, data.ThangBaoCao, data.QuyBaoCao)
}

function LoadDanhSachChiTietBaoCaoTrucThuoc(chitieuId, tinhid, huyenid, xaid, ky, nam, thang, quy) {
    $.ajax({
        url: "/ThongTinTongHop/QuanTriThongTin/DanhSachKetQuaTrucThuoc",
        data: { ChiTieuId: chitieuId, TinhId: tinhid, HuyenId: huyenid, XaId: xaid, KyBaoCao: ky, NamBaoCao: nam, QuyBaoCao: quy, ThangBaoCao: thang },
        dataType: "html",
        success: function (data) {
            $("#grid-ChiTietDuLieuTrucThuoc").html(data);
            $("#formChiTietBaoCao").hideLoading();
            Register_OnchangeEvent_To_Calc_Value();
        },
        error: function () {
            toastr.error("Chưa lấy được dữ liệu trực thuộc")
        }
    })
}

function XoaKetQuaBaoCao(ketquaId, chitieuId) {
    var dialog = taviJs.showConfirmDialog("Bạn có chắc chắn muốn xóa kết quả báo cáo này không?");
    dialog.find(".cmd-save").click(function () {
        $.ajax({
            url: '/ThongTinTongHop/QuanTriThongTin/XoaKetQua',
            data: { KetQuaId: ketquaId },
            type: 'post',
            dataType: 'json',
            success: function (data) {
                if (data == true) {
                    toastr.success("Đã xóa kết quả báo cáo");
                    dialog.modal("hide");
                    LoadDanhSachBaoCao(chitieuId, 1);
                }
            },
            error: function (data) {
                toastr.error("Có lỗi xảy ra");
            }
        })
    })
}

function XoaChiTietKetQuaBaoCao(chitietId, baocaoId) {
    var dialog = taviJs.showConfirmDialog("Bạn có chắc chắn muốn xóa kết quả kỳ báo cáo này không?");
    dialog.find(".cmd-save").click(function () {
        $.ajax({
            url: '/ThongTinTongHop/QuanTriThongTin/XoaChiTietKetQua',
            data: { ChiTietKetQuaId: chitietId },
            type: 'post',
            dataType: 'json',
            success: function (data) {
                if (data == true) {
                    toastr.success("Đã xóa kỳ báo cáo");
                    dialog.modal("hide");
                    LoadDanhSachKyBaoCao(baocaoId, 0);
                }
            },
            error: function (data) {
                toastr.error("Có lỗi xảy ra");
            }
        })
    })
}

function Calc_Sum_Value() {
    var sum = 0;
    $(".inp-giatri").each(function () {
        sum += + $(this).val();
    });
    return sum;
}

function Register_OnchangeEvent_To_Calc_Value() {
    $(".inp-giatri").unbind("change").change(function () {
        if ($("#CachTinhGiaTri").val() == 1) { // tong gia tri truc thuoc
            $("#GiaTriKetQuaSo").val(Calc_Sum_Value());
        }
    })
}

function Set_TongGiaTri_TheoDVHC() {

}