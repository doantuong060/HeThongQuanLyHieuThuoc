// dashboard.js
// BUG FIX #6: Bỏ khởi tạo Chart.js ở đây.
// Biểu đồ được vẽ trực tiếp trong Index_Admin.cshtml với dữ liệu thật từ C# server.
// File này giữ lại để dùng cho các logic phụ nếu cần mở rộng sau.

$(document).ready(function () {
    // Highlight stat-card khi có cảnh báo
    const warningCount = parseInt($('.text-warning.stat-value').text()) || 0;
    const dangerCount = parseInt($('.text-danger.stat-value').text()) || 0;

    if (warningCount > 0) {
        $('.border-warning').addClass('bg-warning bg-opacity-10');
    }
    if (dangerCount > 0) {
        $('.border-danger').addClass('bg-danger bg-opacity-10');
    }
});