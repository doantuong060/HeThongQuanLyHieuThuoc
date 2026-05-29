// Phieunhaps.js — xử lý thêm/xóa dòng và tính tổng

function removeRow(btn) {
    btn.closest('.import-row').remove();
    calcTotal();
}

function addRow() {
    const row = document.createElement('div');
    row.className = 'import-row';
    row.innerHTML = `
        <div class="drug-info-cell">
            <input class="qty-input" placeholder="Tên thuốc" style="font-weight:600;text-align:left;" />
        </div>
        <input class="qty-input" value="1" type="number" min="1" oninput="calcTotal()" />
        <input class="qty-input" value="0" style="text-align:right;" />
        <input class="qty-input" placeholder="Số lô" style="font-size:12px;" />
        <input type="date" class="date-input" />
        <div class="thanhtien">0</div>
        <div class="del-btn" onclick="removeRow(this)"><i class="bi bi-trash3"></i></div>
    `;
    document.getElementById('import-rows').appendChild(row);
}

function calcTotal() {
    // Placeholder — tích hợp logic thực từ model
    const rows = document.querySelectorAll('.import-row');
    let subtotal = 0;
    rows.forEach(row => {
        const qty = parseFloat(row.querySelector('input[type="number"]')?.value || 0);
        const price = parseFloat(row.querySelectorAll('.qty-input')[1]?.value?.replace(/,/g, '') || 0);
        subtotal += qty * price;
    });
    const vat = subtotal * 0.05;
    const total = subtotal + vat;

    const fmt = n => n.toLocaleString('vi-VN') + ' đ';
    const el = id => document.getElementById(id);
    if (el('subtotal')) el('subtotal').textContent = fmt(subtotal);
    if (el('vat')) el('vat').textContent = fmt(vat);
    if (el('total')) el('total').textContent = fmt(total);
}