// _KhoTonKho.js

$(document).ready(function () {
    // 1. Tìm kiếm thuốc trong kho
    $("#inventorySearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $(".table-inventory tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    // 2. Chuyển đổi trạng thái Tab (Giao diện)
    $(".inventory-tabs .nav-link").click(function (e) {
        e.preventDefault();
        $(".inventory-tabs .nav-link").removeClass("active");
        $(this).addClass("active");
    });
});