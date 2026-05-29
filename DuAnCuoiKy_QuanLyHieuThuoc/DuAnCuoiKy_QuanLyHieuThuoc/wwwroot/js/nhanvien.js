// nhanvien.js - Staff logic

$(document).ready(function () {
    // 1. Tìm kiếm nhân viên nhanh (Hardcode search)
    $("#staffSearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $(".table-staff tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    // 2. Logic ẩn/hiện mật khẩu trong Modal
    $(document).on("click", ".toggle-password", function () {
        const input = $(this).siblings("input");
        const icon = $(this).find("i");
        if (input.attr("type") === "password") {
            input.attr("type", "text");
            icon.removeClass("bi-eye-slash").addClass("bi-eye");
        } else {
            input.attr("type", "password");
            icon.removeClass("bi-eye").addClass("bi-eye-slash");
        }
    });

    console.log("Quản lý nhân viên MedVault đã sẵn sàng!");
});