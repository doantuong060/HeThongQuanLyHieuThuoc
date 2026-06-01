// nhanvien.js - MedVault Staff Management

$(document).ready(function () {

    // ================================================================
    // 1. CLICK VÀO HÀNG -> MỞ MODAL CHI TIẾT
    // ================================================================
    $(document).on("click", ".staff-row", function () {
        openChiTietModal($(this));
    });

    function openChiTietModal(row) {
        const hoTen = row.data("hoten") || "";
        const maNv = row.data("manv") || "";
        const gioiTinh = row.data("gioitinh") || "—";
        const sdt = row.data("sdt") || "—";
        const email = row.data("email") || "";
        const ngayVao = row.data("ngayvaolam") || "—";
        const trangThai = row.data("trangthai") || "—";
        const vaiTro = row.data("vaitro") || "—";

        $("#detail-avatar").text(hoTen.charAt(0).toUpperCase());
        $("#detail-hoten").text(hoTen);
        $("#detail-manv").text(maNv);
        $("#detail-gioitinh").text(gioiTinh);
        $("#detail-sdt").text(sdt);
        $("#detail-email").text(email !== "" ? email : "—");
        $("#detail-ngayvaolam").text(ngayVao);
        $("#detail-vaitro").text(vaiTro);

        const isActive = (trangThai === "Đang làm");
        $("#detail-trangthai").html(
            `<span class="badge ${isActive ? "bg-success" : "bg-danger"} bg-opacity-10
             text-${isActive ? "success" : "danger"} rounded-pill px-3">${trangThai}</span>`
        );

        bootstrap.Modal.getOrCreateInstance(document.getElementById("modalChiTietNV")).show();
    }

    // ================================================================
    // 2. NÚT XEM CHI TIẾT (icon info) - stopPropagation
    // ================================================================
    $(document).on("click", ".btn-xem-nv", function (e) {
        e.stopPropagation();
        const maNv = $(this).data("manv");
        const row = $(`tr.staff-row[data-manv="${maNv}"]`);
        if (row.length) openChiTietModal(row);
    });

    // ================================================================
    // 3. NÚT CHỈNH SỬA (icon bút chì) -> MỞ MODAL SỬA qua AJAX
    // ================================================================
    $(document).on("click", ".btn-sua-nv", function (e) {
        e.stopPropagation(); // ngăn bubble lên <tr> mở modal chi tiết

        const maNv = $(this).data("manv");

        // Reset modal về trạng thái loading
        $("#edit-loading").show();
        $("#edit-content").hide();
        $("#btnLuuSua").hide();
        $("#edit-manv").val("");

        // Mở modal trước để user thấy spinner
        bootstrap.Modal.getOrCreateInstance(document.getElementById("modalSuaNV")).show();

        // AJAX gọi GET /NhanViens/GetForEdit?id=NV0001
        $.ajax({
            url: "/NhanViens/GetForEdit",
            type: "GET",
            data: { id: maNv },
            success: function (data) {
                // Đổ dữ liệu vào form
                $("#edit-manv").val(data.maNv);
                $("#edit-badge-manv").text(data.maNv);
                $("#edit-hoten").val(data.hoTen);
                $("#edit-sdt").val(data.soDienThoai);
                $("#edit-email").val(data.email || "");

                // Giới tính
                $("#edit-gioitinh").val(data.gioiTinh || "Nam");

                // Ngày sinh: API trả về ISO string, cần format thành yyyy-MM-dd cho input[type=date]
                if (data.ngaySinh) {
                    const d = new Date(data.ngaySinh);
                    if (!isNaN(d)) {
                        const yyyy = d.getFullYear();
                        const mm = String(d.getMonth() + 1).padStart(2, "0");
                        const dd = String(d.getDate()).padStart(2, "0");
                        $("#edit-ngaysinh").val(`${yyyy}-${mm}-${dd}`);
                    } else {
                        $("#edit-ngaysinh").val("");
                    }
                } else {
                    $("#edit-ngaysinh").val("");
                }

                // Vai trò
                $("#edit-vaitro").val(data.maVaiTro);

                // Reset mật khẩu mới
                $("#edit-matkhaumoi").val("").attr("type", "password");
                $(".toggle-password i").removeClass("bi-eye").addClass("bi-eye-slash");

                // Ẩn spinner, hiện nội dung + nút lưu
                $("#edit-loading").hide();
                $("#edit-content").show();
                $("#btnLuuSua").show();
            },
            error: function () {
                $("#edit-loading").html(
                    `<div class="text-danger"><i class="bi bi-exclamation-triangle me-2"></i>
                     Không tải được thông tin nhân viên. Vui lòng thử lại.</div>`
                );
            }
        });
    });

    // ================================================================
    // 4. NÚT KHÓA / MỞ KHÓA - xác nhận + stopPropagation
    // ================================================================
    $(document).on("click", ".btn-toggle-status", function (e) {
        e.stopPropagation();

        const hoTen = $(this).data("hoten");
        const trangThai = $(this).data("trangthai");
        const action = (trangThai.toString().toLowerCase() === "true") ? "khóa" : "mở khóa";

        if (!confirm(`Bạn có chắc muốn ${action} tài khoản của nhân viên "${hoTen}"?`)) {
            e.preventDefault();
        }
    });

    // ================================================================
    // 5. TÌM KIẾM NHANH PHÍA CLIENT
    // ================================================================
    $("#staffSearch").on("keyup", function () {
        const keyword = $(this).val().toLowerCase().trim();
        $(".table-staff tbody tr.staff-row").each(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(keyword) > -1);
        });
    });

    // ================================================================
    // 6. ẨN / HIỆN MẬT KHẨU TRONG MODAL THÊM VÀ SỬA
    // ================================================================
    $(document).on("click", ".toggle-password", function () {
        const input = $(this).closest(".input-group").find("input");
        const icon = $(this).find("i");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
            icon.removeClass("bi-eye-slash").addClass("bi-eye");
        } else {
            input.attr("type", "password");
            icon.removeClass("bi-eye").addClass("bi-eye-slash");
        }
    });

    // ================================================================
    // 7. VALIDATE FORM THÊM NHÂN VIÊN TRƯỚC KHI SUBMIT
    // ================================================================
    $("#formThemNV").on("submit", function (e) {
        const form = this;
        let isValid = true;

        $(form).find(".is-invalid").removeClass("is-invalid");
        $("#matkhau-error").hide().text("");

        if (!$("#nv-hoten").val().trim()) {
            $("#nv-hoten").addClass("is-invalid");
            isValid = false;
        }

        const sdt = $("#nv-sdt").val().trim();
        if (!sdt || !/^\d{10,11}$/.test(sdt)) {
            $("#nv-sdt").addClass("is-invalid");
            isValid = false;
        }

        if (!$("#nv-vaitro").val()) {
            $("#nv-vaitro").addClass("is-invalid");
            isValid = false;
        }

        if (!$("#nv-tendangnhap").val().trim()) {
            $("#nv-tendangnhap").addClass("is-invalid");
            isValid = false;
        }

        const matKhau = $("#nv-matkhau").val();
        if (!matKhau || matKhau.length < 6) {
            $("#nv-matkhau").addClass("is-invalid");
            $("#matkhau-error").text("Mật khẩu phải có ít nhất 6 ký tự.").show();
            isValid = false;
        }

        if (!isValid) {
            e.preventDefault();
            $("#modalThemNV .modal-body").scrollTop(0);
            return;
        }

        $("#btnLuuNV").prop("disabled", true)
            .html('<span class="spinner-border spinner-border-sm me-1"></span> Đang lưu...');
    });

    // ================================================================
    // 8. VALIDATE FORM SỬA NHÂN VIÊN TRƯỚC KHI SUBMIT
    // ================================================================
    $("#formSuaNV").on("submit", function (e) {
        const form = this;
        let isValid = true;

        $(form).find(".is-invalid").removeClass("is-invalid");
        $("#edit-matkhau-error").hide().text("");

        if (!$("#edit-hoten").val().trim()) {
            $("#edit-hoten").addClass("is-invalid");
            isValid = false;
        }

        const sdt = $("#edit-sdt").val().trim();
        if (!sdt || !/^\d{10,11}$/.test(sdt)) {
            $("#edit-sdt").addClass("is-invalid");
            isValid = false;
        }

        if (!$("#edit-vaitro").val()) {
            $("#edit-vaitro").addClass("is-invalid");
            isValid = false;
        }

        // Mật khẩu mới chỉ validate nếu có nhập
        const matKhauMoi = $("#edit-matkhaumoi").val();
        if (matKhauMoi && matKhauMoi.length > 0 && matKhauMoi.length < 6) {
            $("#edit-matkhaumoi").addClass("is-invalid");
            $("#edit-matkhau-error").text("Mật khẩu phải có ít nhất 6 ký tự.").show();
            isValid = false;
        }

        if (!isValid) {
            e.preventDefault();
            $("#modalSuaNV .modal-body").scrollTop(0);
            return;
        }

        $("#btnLuuSua").prop("disabled", true)
            .html('<span class="spinner-border spinner-border-sm me-1"></span> Đang lưu...');
    });

    // ================================================================
    // 9. RESET FORM KHI ĐÓNG MODAL THÊM
    // ================================================================
    $("#modalThemNV").on("hidden.bs.modal", function () {
        const form = document.getElementById("formThemNV");
        if (form) form.reset();
        $(form).find(".is-invalid").removeClass("is-invalid");
        $("#matkhau-error").hide().text("");
        $("#btnLuuNV").prop("disabled", false)
            .html('<i class="bi bi-check-lg me-1"></i>Lưu nhân viên');
        $("#nv-matkhau").attr("type", "password");
        $(".toggle-password i").removeClass("bi-eye").addClass("bi-eye-slash");
    });

    // ================================================================
    // 10. RESET FORM KHI ĐÓNG MODAL SỬA
    // ================================================================
    $("#modalSuaNV").on("hidden.bs.modal", function () {
        const form = document.getElementById("formSuaNV");
        if (form) form.reset();
        $(form).find(".is-invalid").removeClass("is-invalid");
        $("#edit-matkhau-error").hide().text("");
        $("#btnLuuSua").prop("disabled", false)
            .html('<i class="bi bi-check-lg me-1"></i>Lưu thay đổi');
        $("#edit-loading").show();
        $("#edit-content").hide();
        $("#btnLuuSua").hide();
    });

    console.log("Quản lý nhân viên MedVault đã sẵn sàng!");
});