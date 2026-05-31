// baocao.js - MedVault Report Charts
// Lưu ý: Biểu đồ CỘT doanh thu 12 tháng được khởi tạo trong
//        Index.cshtml (inline script) với data động từ SQL.
//        File này chỉ phụ trách biểu đồ DONUT cơ cấu loại SP.

$(document).ready(function () {

    // ================================================================
    // BIỂU ĐỒ DONUT — Cơ cấu loại sản phẩm
    // Data được đọc từ data-* attribute của các .cocau-item
    // trong View, không hard-code tại đây.
    // ================================================================
    const ctxDonut = document.getElementById('categoryDonutChart');
    if (ctxDonut) {
        const items = document.querySelectorAll('.cocau-item');
        const labels = Array.from(items).map(el => el.dataset.ten);
        const data = Array.from(items).map(el => parseFloat(el.dataset.phanTram) || 0);
        const colors = Array.from(items).map(el => el.dataset.mau || '#adb5bd');

        new Chart(ctxDonut, {
            type: 'doughnut',
            data: {
                labels: labels,
                datasets: [{
                    data: data,
                    backgroundColor: colors,
                    borderWidth: 0,
                    cutout: '80%'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: { legend: { display: false } }
            }
        });
    }

    console.log("Hệ thống phân tích báo cáo đã sẵn sàng!");
});