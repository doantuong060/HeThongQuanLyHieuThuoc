// baocao.js - Biểu đồ phân tích MedVault

$(document).ready(function () {
    // 1. Biểu đồ doanh thu 12 tháng (Dạng cột)
    const ctxBar = document.getElementById('revenue12MonthsChart');
    if (ctxBar) {
        new Chart(ctxBar, {
            type: 'bar',
            data: {
                labels: ['T1', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'T8', 'T9', 'T10', 'T11', 'T12'],
                datasets: [{
                    label: 'Doanh thu',
                    data: [150, 200, 250, 230, 300, 280, 310, 350, 290, 380, 420, 452],
                    backgroundColor: '#0d6efd',
                    borderRadius: 5,
                    barThickness: 20
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: { legend: { display: false } },
                scales: {
                    y: { beginAtZero: true, grid: { borderDash: [5, 5], drawBorder: false } },
                    x: { grid: { display: false } }
                }
            }
        });
    }

    // 2. Biểu đồ cơ cấu loại thuốc (Dạng Donut)
    const ctxDonut = document.getElementById('categoryDonutChart');
    if (ctxDonut) {
        new Chart(ctxDonut, {
            type: 'doughnut',
            data: {
                labels: ['Thuốc kê đơn', 'Thực phẩm CN', 'Dược mỹ phẩm', 'Vật tư y tế'],
                datasets: [{
                    data: [45, 30, 15, 10],
                    backgroundColor: ['#0d6efd', '#4b5563', '#0dcaf0', '#adb5bd'],
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