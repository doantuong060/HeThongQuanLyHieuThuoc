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