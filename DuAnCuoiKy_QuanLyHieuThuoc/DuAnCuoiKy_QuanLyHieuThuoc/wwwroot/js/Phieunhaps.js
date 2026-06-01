// PhieuNhaps.js — xử lý thêm/xóa dòng và tính tổng
// Dùng cho trang Tạo phiếu nhập (PhieuNhaps/Create)

// -------------------------------------------------------------------
// Biến toàn cục: đếm số dòng hiện có để sinh index binding đúng
// -------------------------------------------------------------------
let chiTietIndex = 0;

// Gọi khi DOM đã sẵn sàng
document.addEventListener('DOMContentLoaded', function () {
    // Lấy số dòng hiện tại (do Razor render sẵn khi có lỗi validation)
    chiTietIndex = document.querySelectorAll('#chiTietBody tr').length;

    // Nút thêm dòng
    const btnThem = document.getElementById('btnThemDong');
    if (btnThem) {
        btnThem.addEventListener('click', addRow);
    }

    // Xóa dòng — dùng event delegation vì dòng được thêm động
    document.addEventListener('click', function (e) {
        if (e.target.closest('.btn-remove')) {
            removeRow(e.target.closest('.btn-remove'));
        }
    });

    // Tính tổng lần đầu
    calcTotal();

    // Tính lại mỗi khi người dùng nhập số lượng / giá
    document.getElementById('chiTietBody').addEventListener('input', function (e) {
        if (
            e.target.matches('[name$=".SoLuongNhap"]') ||
            e.target.matches('[name$=".GiaNhap"]')
        ) {
            calcTotal();
        }
    });
});

// -------------------------------------------------------------------
// Xóa một dòng
// -------------------------------------------------------------------
function removeRow(btn) {
    const row = btn.closest('tr');
    if (!row) return;

    // Nếu chỉ còn 1 dòng thì không cho xóa (tùy nghiệp vụ — bỏ nếu không cần)
    const tbody = document.getElementById('chiTietBody');
    if (tbody && tbody.querySelectorAll('tr').length <= 1) {
        alert('Phiếu nhập phải có ít nhất một mặt hàng.');
        return;
    }

    row.remove();
    calcTotal();
    reindexRows();   // Cập nhật lại index để ASP.NET model binding nhận đúng
}

// -------------------------------------------------------------------
// Thêm một dòng mới
// Danh sách sản phẩm (dsSanPham) được nhúng từ Razor vào biến JS bên dưới
// -------------------------------------------------------------------
function addRow() {
    const tbody = document.getElementById('chiTietBody');
    if (!tbody) return;

    const idx = chiTietIndex;
    const tr = document.createElement('tr');

    tr.innerHTML = `
        <td>
            <select name="DanhSachLoHang[${idx}].MaSp" class="form-select">
                <option value="">Chọn thuốc</option>
                ${buildOptions()}
            </select>
        </td>
        <td>
            <input name="DanhSachLoHang[${idx}].SoLuongNhap"
                   type="number" min="1" value="1"
                   class="form-control" />
        </td>
        <td>
            <input name="DanhSachLoHang[${idx}].GiaNhap"
                   type="number" min="0" value="0"
                   class="form-control" />
        </td>
        <td>
            <input name="DanhSachLoHang[${idx}].SoLo"
                   class="form-control" />
        </td>
        <td>
            <input name="DanhSachLoHang[${idx}].HanSuDung"
                   type="date"
                   class="form-control" />
        </td>
        <td class="text-center">
            <button type="button" class="btn btn-danger btn-sm btn-remove">X</button>
        </td>
    `;

    tbody.appendChild(tr);
    chiTietIndex++;
    calcTotal();
}

// -------------------------------------------------------------------
// Tính tổng tiền (tạm tính + VAT 5% + thành tiền)
// -------------------------------------------------------------------
function calcTotal() {
    const rows = document.querySelectorAll('#chiTietBody tr');
    let subtotal = 0;

    rows.forEach(row => {
        const qtyInput = row.querySelector('[name$=".SoLuongNhap"]');
        const priceInput = row.querySelector('[name$=".GiaNhap"]');

        const qty = parseFloat(qtyInput?.value || 0);
        const price = parseFloat(priceInput?.value?.replace(/,/g, '') || 0);
        const line = qty * price;

        subtotal += line;

        // Cập nhật cột thành tiền (nếu có)
        const thanhTienEl = row.querySelector('.thanh-tien');
        if (thanhTienEl) {
            thanhTienEl.textContent = formatVND(line);
        }
    });

    const vat = subtotal * 0.05;
    const total = subtotal + vat;

    setElText('subtotal', formatVND(subtotal));
    setElText('vat', formatVND(vat));
    setElText('total', formatVND(total));
}

// -------------------------------------------------------------------
// Helpers
// -------------------------------------------------------------------

/** Định dạng tiền VNĐ */
function formatVND(n) {
    return n.toLocaleString('vi-VN') + ' đ';
}

/** Gán text cho element theo id (bỏ qua nếu không tồn tại) */
function setElText(id, text) {
    const el = document.getElementById(id);
    if (el) el.textContent = text;
}

/**
 * Sinh <option> cho dropdown sản phẩm.
 * Dữ liệu được nhúng từ Razor vào biến window.__dsSanPham (xem bên dưới).
 */
function buildOptions() {
    const ds = window.__dsSanPham || [];
    return ds.map(item =>
        `<option value="${escHtml(item.value)}">${escHtml(item.text)}</option>`
    ).join('');
}

/** Escape HTML cơ bản để tránh XSS */
function escHtml(str) {
    return String(str)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;');
}

/**
 * Cập nhật lại thuộc tính name của tất cả dòng sau khi xóa,
 * đảm bảo index liên tục [0], [1], [2], … cho ASP.NET model binding.
 */
function reindexRows() {
    const rows = document.querySelectorAll('#chiTietBody tr');
    rows.forEach((row, i) => {
        row.querySelectorAll('[name]').forEach(el => {
            el.name = el.name.replace(/\[\d+\]/, `[${i}]`);
        });
    });
    chiTietIndex = rows.length;
}