// dashboard.js - Xử lý biểu đồ trang Dashboard MedVault

$(document).ready(function () {
    const ctx = document.getElementById('revenueChart').getContext('2d');

    // --- [VỊ TRÍ HARDCODE DỮ LIỆU BIỂU ĐỒ] ---
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['T1', 'T2', 'T3', 'T4', 'T5', 'T6'],
            datasets: [{
                label: 'Doanh thu (triệu)',
                data: [45, 60, 52, 85, 78, 88],
                backgroundColor: '#0d6efd',
                borderRadius: 10,
                barThickness: 40
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false, // Fix lỗi tuột biểu đồ
            plugins: {
                legend: { display: false }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: { display: false },
                    ticks: { display: false }
                },
                x: {
                    grid: { display: false }
                }
            }
        }
    });

    console.log("Biểu đồ Dashboard đã được vẽ!");
});