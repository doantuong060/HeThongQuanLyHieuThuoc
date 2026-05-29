// login.js - Xử lý logic giao diện trang Đăng nhập

$(document).ready(function () {
    // Logic ẩn/hiện mật khẩu
    $(".password-toggle").click(function () {
        const passwordInput = $("#passwordField");
        const icon = $(this).find("i");

        if (passwordInput.attr("type") === "password") {
            passwordInput.attr("type", "text");
            icon.removeClass("bi-eye-slash").addClass("bi-eye");
        } else {
            passwordInput.attr("type", "password");
            icon.removeClass("bi-eye").addClass("bi-eye-slash");
        }
    });

    console.log("Giao diện Login MedVault đã sẵn sàng!");
});