// file: wwwroot/js/TongQuanCaLam.js

function initHoaDonChart(labels, data) {
    const nowHour = new Date().getHours();
    const startHour = 7;

    const colors = labels.map((_, i) => {
        const h = startHour + i;
        if (h < nowHour) return 'rgba(209,213,219,0.8)';
        if (h === nowHour) return 'rgba(37,99,235,1)';
        return 'rgba(37,99,235,0.25)';
    });

    const ctx = document.getElementById('chartHoaDonTheoGio');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: colors,
                borderRadius: 5,
                borderSkipped: false,
                barPercentage: 0.65,
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: {
                    callbacks: {
                        label: ctx => ctx.parsed.y + ' hóa đơn'
                    }
                }
            },
            scales: {
                x: {
                    grid: { display: false },
                    ticks: {
                        font: { size: 11, family: "'Be Vietnam Pro', sans-serif" },
                        color: '#9ca3af'
                    },
                    border: { display: false }
                },
                y: {
                    display: true,
                    beginAtZero: true,
                    suggestedMax: 20,
                    grid: {
                        color: 'rgba(229,231,235,0.7)',
                        drawBorder: false
                    },
                    border: { display: false, dash: [4, 4] },
                    ticks: {
                        stepSize: 5,
                        font: { size: 11, family: "'Be Vietnam Pro', sans-serif" },
                        color: '#9ca3af',
                        padding: 8,
                        callback: val => val % 5 === 0 ? val : null
                    }
                }
            }
        }
    });
}

document.addEventListener('DOMContentLoaded', function () {
    // 1. Vẽ biểu đồ (nếu có canvas)
    const ctx = document.getElementById('chartHoaDonTheoGio');
    if (ctx && typeof initHoaDonChart !== 'undefined') {
        // Gọi hàm initHoaDonChart với biến labelsJson/dataJson từ Razor truyền qua
    }

    // 2. AJAX cho nút Xem tất cả
    const btnXemTatCa = document.getElementById('btnXemTatCa');
    if (btnXemTatCa) {
        btnXemTatCa.addEventListener('click', function () {
            this.innerHTML = '<i class="fa-solid fa-spinner fa-spin"></i>';
            fetch('/TongQuanCaLam/GetTatCaHoaDon')
                .then(res => res.json())
                .then(res => {
                    const tbody = document.querySelector('.tq-table tbody');
                    tbody.innerHTML = res.data.map(hd => `
                        <tr>
                            <td><span class="tq-hd-code">${hd.maHD}</span></td>
                            <td><span class="tq-hd-time">${hd.thoiGian}</span></td>
                            <td class="right"><span class="tq-hd-amount">${hd.tongTien.toLocaleString('vi-VN')}đ</span></td>
                            <td><span class="tq-badge tq-badge-success">${hd.trangThai}</span></td>
                            <td><button class="tq-btn-eye btn-view-detail" data-mahd="${hd.maHD}"><i class="fa-regular fa-eye"></i></button></td>
                        </tr>`).join('');
                    document.getElementById('txtTieuDeBang').innerText = 'Toàn bộ hóa đơn trong ca';
                    this.style.display = 'none';
                });
        });
    }

    // 3. AJAX Modal chi tiết
    document.body.addEventListener('click', function (e) {
        const btn = e.target.closest('.btn-view-detail');
        if (btn) {
            const maHD = btn.getAttribute('data-mahd');
            fetch(`/TongQuanCaLam/GetChiTiet?maHD=${maHD}`)
                .then(res => res.json())
                .then(res => {
                    document.getElementById('modalMaHD').innerText = 'Chi tiết: ' + res.data.maHD;
                    document.getElementById('modalNgayBan').innerText = 'Thời gian: ' + res.data.ngayBan;
                    document.getElementById('modalTongTien').innerText = res.data.tongTien.toLocaleString('vi-VN') + 'đ';
                    document.getElementById('modalTbody').innerHTML = res.data.chiTiet.map(item => `
                        <tr><td>${item.tenSP}<br><small>${item.dvt}</small></td><td class="text-center">${item.soLuong}</td><td class="text-end">${item.donGia.toLocaleString('vi-VN')}đ</td><td class="text-end">${item.thanhTien.toLocaleString('vi-VN')}đ</td></tr>`
                    ).join('');
                    new bootstrap.Modal(document.getElementById('modalChiTietHoaDon')).show();
                });
        }
    });
});