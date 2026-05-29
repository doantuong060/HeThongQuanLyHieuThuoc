$(document).ready(function () {
    // Logic ẩn/hiện các trường thông tin theo Loại sản phẩm
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
});
// Logic xử lý trang Sản phẩm MedVault

$(document).ready(function () {
    console.log("Trang Sản phẩm đã sẵn sàng!");

    // Xử lý tìm kiếm nhanh (Hardcode search)
    $("#searchInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("table tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    // Sau này có thể thêm logic AJAX để load chi tiết Thuốc hoặc Vật tư khi bấm nút Sửa
    $(".btn-pencil").click(function () {
        alert("Chức năng chỉnh sửa đang được cập nhật!");
    });
});