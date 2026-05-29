// _KhoNhapHang.js - Logic bảng nhập hàng động

$(document).ready(function () {
    console.log("Trang Nhập hàng đã sẵn sàng!");

    // [HARDCODE] Sự kiện bấm nút "Thêm mặt hàng"
    $("#btnAddRow").click(function () {
        let newRow = `
            <tr>
                <td>
                    <input type="text" class="form-control" placeholder="Tên thuốc...">
                </td>
                <td style="width: 80px;"><input type="number" class="form-control" value="1"></td>
                <td style="width: 120px;"><input type="text" class="form-control" placeholder="0đ"></td>
                <td style="width: 100px;"><input type="text" class="form-control" placeholder="Số lô"></td>
                <td style="width: 150px;"><input type="date" class="form-control"></td>
                <td class="fw-bold text-end">0đ</td>
                <td class="text-center"><button class="btn btn-link text-danger p-0 btnDelete"><i class="bi bi-trash"></i></button></td>
            </tr>`;
        $("#receiptTable tbody").append(newRow);
    });

    // Xử lý xóa dòng
    $(document).on("click", ".btnDelete", function () {
        $(this).closest("tr").remove();
    });
});