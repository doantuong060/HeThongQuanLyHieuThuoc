$(document).ready(function () {
    // 1. Tìm kiếm nhanh trong danh sách phiếu
    $("#searchPhieu").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $(".table-history tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    // 2. Logic khi bấm nút xem chi tiết (Mở Modal Ảnh 17)
    $(".btn-view-detail").click(function () {
        // [HARDCODE]: Trong thực tế sẽ gọi AJAX để lấy dữ liệu theo MaPhieu
        $("#modalChiTietPhieu").modal("show");
    });
});