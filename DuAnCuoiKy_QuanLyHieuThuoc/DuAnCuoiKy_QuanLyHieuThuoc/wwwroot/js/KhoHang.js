
document.addEventListener("DOMContentLoaded", function () {
    // ── SEARCH DEBOUNCE (Tự động tìm kiếm khi gõ) ──
    const searchInput = document.getElementById("searchInput");
    if (searchInput) {
        let timer;
        searchInput.addEventListener("input", function () {
            clearTimeout(timer);
            // Đợi 450ms sau khi người dùng ngừng gõ thì mới submit form
            timer = setTimeout(() => {
                document.getElementById("searchForm").submit();
            }, 450);
        });
    }

    // ── TOGGLE DROPDOWN LỌC LOẠI ──
    const btnFilter = document.getElementById("btnFilter");
    const filterDropdown = document.getElementById("filterDropdown");

    if (btnFilter && filterDropdown) {
        btnFilter.addEventListener("click", function (e) {
            e.stopPropagation(); // Ngăn sự kiện click lan ra ngoài
            filterDropdown.classList.toggle("open");
        });

        // Nhấn ra ngoài thì đóng menu
        document.addEventListener("click", function () {
            filterDropdown.classList.remove("open");
        });
    }
});