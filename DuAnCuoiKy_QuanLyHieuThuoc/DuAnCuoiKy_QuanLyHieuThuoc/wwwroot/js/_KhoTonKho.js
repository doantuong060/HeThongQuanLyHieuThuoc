// _KhoTonKho.js
$(document).ready(function () {
    // 1. Tìm kiếm thuốc trong kho (client-side, dùng khi không submit form)
    $("#inventorySearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $(".table-inventory tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    // 2. Chuyển đổi trạng thái Tab
    // ✅ Bỏ e.preventDefault() để link navigate lên server
    // Active tab đã được xử lý bên Razor (activeTab), không cần JS set nữa
});