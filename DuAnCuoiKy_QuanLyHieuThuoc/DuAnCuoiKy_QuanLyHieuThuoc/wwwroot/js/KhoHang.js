document.addEventListener("DOMContentLoaded", function () {
    // 1. XỬ LÝ NÚT MỞ MENU LỌC LOẠI
    const btnFilter = document.getElementById("btnFilter");
    const filterDropdown = document.getElementById("filterDropdown");

    if (btnFilter && filterDropdown) {
        // Khi bấm vào nút Lọc -> Bật/Tắt class 'open' để xổ menu
        btnFilter.addEventListener("click", function (e) {
            e.stopPropagation(); // Ngăn sự kiện lan ra ngoài
            filterDropdown.classList.toggle("open");
        });

        // Khi bấm ra ngoài vùng menu -> Tự động đóng menu lại
        document.addEventListener("click", function (e) {
            if (!filterDropdown.contains(e.target) && !btnFilter.contains(e.target)) {
                filterDropdown.classList.remove("open");
            }
        });
    }

    // 2. XỬ LÝ Ô TÌM KIẾM (Gõ xong nhấn Enter là tự tìm)
    const searchInput = document.getElementById("searchInput");
    const searchForm = document.getElementById("searchForm");

    if (searchInput && searchForm) {
        searchInput.addEventListener("keypress", function (e) {
            if (e.key === "Enter") {
                e.preventDefault();
                searchForm.submit();
            }
        });
    }
});