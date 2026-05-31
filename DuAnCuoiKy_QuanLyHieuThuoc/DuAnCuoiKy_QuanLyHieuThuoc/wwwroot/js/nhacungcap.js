$(document).ready(function () {

    // ============================================================
    // HÀM DÙNG CHUNG: gọi AJAX lấy chi tiết NCC
    // ============================================================
    function layChiTietNCC(maNcc, callback) {
        $.ajax({
            url: '/NhaCungCaps/GetDetail',
            type: 'GET',
            data: { id: maNcc },
            success: function (data) {
                if (!data.success) {
                    alert('Lỗi: ' + data.message);
                    return;
                }
                callback(data);
            },
            error: function () {
                alert('Không thể tải thông tin. Vui lòng thử lại.');
            }
        });
    }

    // ============================================================
    // 1. CLICK VÀO CARD HOẶC HÀNG BẢNG → MỞ MODAL XEM CHI TIẾT
    //    Bỏ qua nếu click xuất phát từ nút con (bút chì, thùng rác)
    // ============================================================
    $(document).on('click', '.btn-xem-ncc', function (e) {
        // Nếu click vào button hoặc form bên trong → bỏ qua
        if ($(e.target).closest('button, form, a').length) return;

        var maNcc = $(this).data('id');
        layChiTietNCC(maNcc, function (data) {
            $('#xem_MaNcc').text('Mã: ' + data.maNcc);
            $('#xem_TenNcc').text(data.tenNcc);
            $('#xem_SoDienThoai').text(data.soDienThoai || '—');
            $('#xem_Email').text(data.email || '—');
            $('#xem_TinhThanh').text(data.tenTinhThanh || '—');
            $('#xem_DiaChi').text(data.diaChi || '—');

            if (data.trangThai) {
                $('#xem_TrangThai').html('<span class="badge bg-success bg-opacity-10 text-success rounded-pill px-3">Hoạt động</span>');
            } else {
                $('#xem_TrangThai').html('<span class="badge bg-danger bg-opacity-10 text-danger rounded-pill px-3">Tạm ngưng</span>');
            }

            $('#btnChuyenSangSua').data('id', data.maNcc);

            var modal = new bootstrap.Modal(document.getElementById('modalXemNCC'));
            modal.show();
        });
    });

    // ============================================================
    // 2. NÚT "CHỈNH SỬA" TRONG MODAL XEM → CHUYỂN SANG MODAL SỬA
    // ============================================================
    $('#btnChuyenSangSua').on('click', function () {
        var maNcc = $(this).data('id');
        bootstrap.Modal.getInstance(document.getElementById('modalXemNCC')).hide();
        $('#modalXemNCC').one('hidden.bs.modal', function () {
            moModalSua(maNcc);
        });
    });

    // ============================================================
    // 3. NÚT BÚT CHÌ TRONG BẢNG → MỞ THẲNG MODAL SỬA
    // ============================================================
    $(document).on('click', '.btn-sua-ncc', function (e) {
        e.stopPropagation();
        var maNcc = $(this).data('id');
        moModalSua(maNcc);
    });

    function moModalSua(maNcc) {
        layChiTietNCC(maNcc, function (data) {
            $('#edit_MaNcc').val(data.maNcc);
            $('#edit_TenNcc').val(data.tenNcc);
            $('#edit_SoDienThoai').val(data.soDienThoai);
            $('#edit_Email').val(data.email);
            $('#edit_DiaChi').val(data.diaChi);
            $('#edit_TrangThai').prop('checked', data.trangThai);

            var modal = new bootstrap.Modal(document.getElementById('modalSuaNCC'));
            modal.show();
        });
    }

});