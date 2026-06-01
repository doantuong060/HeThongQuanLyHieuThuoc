// =========================================
// _KhoTonKho.js — MedVault
// File: wwwroot/js/_KhoTonKho.js
// =========================================

$(document).ready(function () {

    // ── 1. Search client-side (giữ nguyên code cũ) ──
    // Chỉ dùng khi không submit form (search nhanh không reload)
    // Hiện nay form submit lên server, nên đoạn này dự phòng
    $("#inventorySearch").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $(".table-inventory tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // ── 2. Active tab: do Razor xử lý, không cần JS set ──

});

// ── 3. Lọc loại SP: set hidden input rồi submit form ──
function setLoai(loai) {
    document.getElementById('hidden-loai').value = loai;
    document.getElementById('form-filter').submit();
}

// ── 4. Xuất file CSV (Excel đọc được tiếng Việt nhờ UTF-8 BOM) ──
function xuatExcel() {
    const table = document.getElementById('table-tonkho');
    if (!table) return;

    const activeLoai = document.getElementById('hidden-loai')?.value ?? 'all';

    // Header cột — khớp đúng số cột trong <thead>
    const headers = [
        'Mã', 'Tên sản phẩm', 'Phân loại', 'Nhóm', 'ĐVT',
        'Tồn kho', 'Ngưỡng', 'Lô gần HSD', 'Trạng thái'
    ];

    const data = [];
    table.querySelectorAll('tbody tr').forEach(row => {
        // Bỏ qua dòng đang ẩn (bởi client-side search)
        if (row.style.display === 'none') return;

        const cells = row.querySelectorAll('td');
        if (cells.length < 7) return;

        // Cột "Tồn kho / Ngưỡng" có dạng "120\n/ 50" hoặc "120 / 50"
        // Tách ra thành 2 cột riêng trong Excel
        const tonNguong = (cells[5]?.innerText ?? '').replace(/\s+/g, ' ').trim();
        const parts = tonNguong.split('/');
        const ton = parts[0]?.trim() ?? '';
        const nguong = parts[1]?.trim() ?? '';

        data.push([
            cells[0]?.innerText.trim(),  // Mã
            cells[1]?.innerText.trim(),  // Tên
            cells[2]?.innerText.trim(),  // Phân loại (Thuốc / Vật tư)
            cells[3]?.innerText.trim(),  // Nhóm
            cells[4]?.innerText.trim(),  // ĐVT
            ton,                         // Tồn kho (tách riêng)
            nguong,                      // Ngưỡng (tách riêng)
            cells[6]?.innerText.trim(),  // Lô gần HSD
            cells[7]?.innerText.trim(),  // Trạng thái
        ]);
    });

    if (data.length === 0) {
        alert('Không có dữ liệu để xuất.');
        return;
    }

    // Build CSV
    const BOM = '\uFEFF'; // Byte Order Mark — Excel nhận diện UTF-8
    const sep = ',';
    const escCSV = v => `"${String(v ?? '').replace(/"/g, '""')}"`;

    let csv = BOM + headers.map(escCSV).join(sep) + '\r\n';
    data.forEach(row => {
        csv += row.map(escCSV).join(sep) + '\r\n';
    });

    // Tên file: TonKho_Thuoc_20231025.csv
    const now = new Date();
    const datePart = `${now.getFullYear()}${String(now.getMonth() + 1).padStart(2, '0')}${String(now.getDate()).padStart(2, '0')}`;
    const loaiLabel = activeLoai === 'THUOC' ? '_Thuoc'
        : activeLoai === 'VATTU' ? '_VatTu'
            : '';
    const filename = `TonKho${loaiLabel}_${datePart}.csv`;

    // Tải xuống
    const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
}