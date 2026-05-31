$(document).ready(function () {

    // ============================================================
    // 1. MODAL THÊM MỚI: ẩn/hiện vùng Thuốc / Vật tư
    // ============================================================
    $('#selectLoaiSP').change(function () {
        var loai = $(this).val();
        if (loai === "THUOC") {
            $('#divThuoc').removeClass('d-none');
            $('#divVatTu').addClass('d-none');
        } else {
            $('#divThuoc').addClass('d-none');
            $('#divVatTu').removeClass('d-none');
        }
    });

    // ============================================================
    // 2. NÚT SỬA: gọi AJAX lấy chi tiết rồi đổ vào modal
    // ============================================================
    $(document).on('click', '.btn-sua-sp', function () {
        var maSp = $(this).data('id');

        // Gọi action GetDetail trả về JSON
        $.ajax({
            url: '/SanPhams/GetDetail',
            type: 'GET',
            data: { id: maSp },
            success: function (data) {
                if (!data.success) {
                    alert('Lỗi: ' + data.message);
                    return;
                }

                // Đổ dữ liệu vào form modal sửa
                $('#edit_MaSp').val(data.maSp);
                $('#edit_TenSp').val(data.tenSp);
                $('#edit_LoaiSp_display').val(data.loaiSp === 'THUOC' ? 'THUỐC' : 'VẬT TƯ Y TẾ');
                $('#edit_GiaBan').val(data.giaBan);
                $('#edit_MucCanhBao').val(data.mucCanhBao);
                $('#edit_MaDvt').val(data.maDvt);

                // Ẩn/hiện vùng Thuốc hoặc Vật tư
                if (data.loaiSp === 'THUOC') {
                    $('#editDivThuoc').removeClass('d-none');
                    $('#editDivVatTu').addClass('d-none');
                    $('#edit_MaLoai').val(data.maLoai);
                    $('#edit_CanToa').prop('checked', data.canToa);
                    $('#edit_GhiChu').val(data.ghiChu);
                } else {
                    $('#editDivThuoc').addClass('d-none');
                    $('#editDivVatTu').removeClass('d-none');
                    $('#edit_MaLoaiVt').val(data.maLoaiVt);
                    $('#edit_NhaSanXuat').val(data.nhaSanXuat);
                }

                // Mở modal
                var modal = new bootstrap.Modal(document.getElementById('modalSuaSP'));
                modal.show();
            },
            error: function () {
                alert('Không thể tải thông tin sản phẩm. Vui lòng thử lại.');
            }
        });
    });

});