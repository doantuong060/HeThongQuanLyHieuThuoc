// nhacungcap.js - Logic nhà cung cấp

$(document).ready(function () {
    // Tìm kiếm nhanh trong bảng
    $("#nccSearchInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $(".table-ncc tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    console.log("Phân hệ Nhà cung cấp đã sẵn sàng!");
});