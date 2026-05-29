// =======================================================
// POS UI SCRIPT
// File: wwwroot/js/ban-hang.js
// =======================================================
document.addEventListener("DOMContentLoaded", function () {

    // ==========================================
    // 1. TOAST HELPER + AUTO HIDE
    // ==========================================

    // Hàm hiển thị toast động từ JS
    function showToast(message, type = "error") {
        const toast = document.getElementById("posToastDynamic");
        const icon = document.getElementById("posToastIcon");
        const msg = document.getElementById("posToastMsg");
        if (!toast) return;

        // Set nội dung
        msg.textContent = message;
        toast.className = "pos-toast"; // reset class
        if (type === "success") {
            toast.classList.add("pos-toast-success");
            icon.className = "fa-solid fa-circle-check";
        } else {
            toast.classList.add("pos-toast-error");
            icon.className = "fa-solid fa-circle-xmark";
        }

        // Hiện lên
        toast.classList.add("show");

        // Tự ẩn sau 3.5 giây
        clearTimeout(toast._hideTimer);
        toast._hideTimer = setTimeout(() => {
            toast.classList.remove("show");
        }, 3500);
    }

    // Tự ẩn toast server-side (TempData) sau 4 giây
    const serverToast = document.getElementById("posToastServer");
    if (serverToast) {
        setTimeout(() => serverToast.classList.remove("show"), 4000);
    }

    // ==========================================
    // 2. SEARCH DEBOUNCE (Giữ nguyên của bạn)
    // ==========================================
    const searchInput = document.getElementById("searchInput");
    if (searchInput) {
        let timeout = null;
        searchInput.addEventListener("input", function () {
            clearTimeout(timeout);
            timeout = setTimeout(() => {
                const form = document.getElementById("searchForm");
                if (form) form.submit();
            }, 500);
        });
    }

    // ==========================================
    // 3. XỬ LÝ LỌC DANH MỤC (COMBOBOX & THẺ TAG)
    // ==========================================
    const searchForm = document.getElementById('searchForm');
    const categorySelect = document.getElementById('categorySelect');

    if (searchForm && categorySelect) {
        // Lắng nghe khi chọn combobox
        categorySelect.addEventListener('change', function () {
            searchForm.submit();
        });

        // Lắng nghe khi bấm nút thẻ Tag (Sử dụng data-category thay vì onclick)
        const filterTags = document.querySelectorAll('.filter-tag');
        filterTags.forEach(tag => {
            tag.addEventListener('click', function (e) {
                e.preventDefault();
                const categoryName = this.getAttribute('data-category');
                categorySelect.value = categoryName;
                searchForm.submit();
            });
        });
    }

    // ==========================================
    // 4. CONFIRM CLEAR CART
    // ==========================================
    const clearCartBtn = document.querySelector(".btn-clear-cart");
    if (clearCartBtn) {
        clearCartBtn.addEventListener("click", function (e) {
            if (!confirm("Bạn có chắc muốn xóa toàn bộ giỏ hàng?")) {
                e.preventDefault();
            }
        });
    }

    // ==========================================================
    // 5. KIỂM TRA GIỎ HÀNG TRƯỚC KHI MỞ MODAL THANH TOÁN
    // ==========================================================
    const btnOpenPayment = document.getElementById("btnOpenPayment");
    if (btnOpenPayment) {
        btnOpenPayment.addEventListener("click", function () {
            const cartCount = parseInt(this.getAttribute("data-cart-count")) || 0;

            if (cartCount === 0) {
                showToast("Giỏ hàng đang trống! Vui lòng thêm sản phẩm trước khi thanh toán.", "error");
                return;
            }

            // Có hàng → mở modal thanh toán
            const payModal = new bootstrap.Modal(document.getElementById("paymentModal"));
            payModal.show();
        });
    }

    // ==========================================================
    // 6. PAYMENT MODAL - RECEIPT STYLE
    // ==========================================================
    const totalAmountEl = document.getElementById("modalTotalAmount");
    const customerCashInput = document.getElementById("customerCash");
    const changeAmountEl = document.getElementById("changeAmount");
    const btnConfirmPayment = document.getElementById("btnConfirmPayment");
    const checkoutForm = document.getElementById("checkoutForm");

    // --- Tính tiền thối khi khách nhập tiền đưa ---
    if (customerCashInput && changeAmountEl && totalAmountEl) {
        const recalc = () => {
            const total = parseInt(totalAmountEl.getAttribute("data-amount")) || 0;
            const cashGiven = parseInt(customerCashInput.value) || 0;
            const change = cashGiven - total;

            if (customerCashInput.value === "" || cashGiven === 0) {
                changeAmountEl.textContent = "0đ";
                changeAmountEl.style.color = "var(--primary)";
            } else if (change < 0) {
                changeAmountEl.textContent = "Thiếu " + Math.abs(change).toLocaleString('vi-VN') + "đ";
                changeAmountEl.style.color = "var(--danger)";
            } else {
                changeAmountEl.textContent = change.toLocaleString('vi-VN') + "đ";
                changeAmountEl.style.color = "var(--primary)";
            }
        };
        customerCashInput.addEventListener("input", recalc);
    }

    // --- Reset form khi mở modal ---
    const paymentModal = document.getElementById("paymentModal");
    if (paymentModal) {
        paymentModal.addEventListener("show.bs.modal", () => {
            if (customerCashInput) customerCashInput.value = "";
            if (changeAmountEl) changeAmountEl.textContent = "0đ";
            // Reset payment method về Tiền mặt
            document.querySelectorAll(".pay-method-btn").forEach(b => b.classList.remove("active"));
            const firstBtn = document.querySelector(".pay-method-btn[data-method='Tiền mặt']");
            if (firstBtn) firstBtn.classList.add("active");
        });
        paymentModal.addEventListener("shown.bs.modal", () => {
            if (customerCashInput) customerCashInput.focus();
        });
    }

    // --- Toggle phương thức thanh toán ---
    document.querySelectorAll(".pay-method-btn").forEach(btn => {
        btn.addEventListener("click", function () {
            document.querySelectorAll(".pay-method-btn").forEach(b => b.classList.remove("active"));
            this.classList.add("active");
        });
    });

    // --- Nút In & Xác nhận ---
    if (btnConfirmPayment && checkoutForm) {
        btnConfirmPayment.addEventListener("click", function () {
            const total = parseInt(totalAmountEl?.getAttribute("data-amount")) || 0;
            const cashGiven = parseInt(customerCashInput?.value) || 0;

            if (total > 0 && cashGiven > 0 && cashGiven < total) {
                showToast("Số tiền khách đưa chưa đủ!", "error");
                customerCashInput.focus();
                return;
            }

            // Khóa nút tránh bấm đúp
            btnConfirmPayment.disabled = true;
            btnConfirmPayment.innerHTML = `<i class="fa-solid fa-spinner fa-spin"></i> Đang xử lý...`;

            setTimeout(() => {
                checkoutForm.submit();
            }, 400);
        });
    }

    // ==========================================
    // 7. BUTTON LOADING EFFECT (Giữ nguyên cho các form khác)
    // ==========================================
    const forms = document.querySelectorAll("form");
    forms.forEach(form => {
        form.addEventListener("submit", function (e) {
            // Chặn hiệu ứng này nếu là checkoutForm vì checkoutForm đã được xử lý loading riêng ở trên
            if (form.getAttribute("id") === "checkoutForm") return;

            if (!e.defaultPrevented) {
                const btn = form.querySelector('button[type="submit"]');
                if (btn) {
                    btn.disabled = true;
                    btn.innerHTML = `<i class="fa-solid fa-spinner fa-spin"></i> Đang xử lý...`;
                }
            }
        });
    });

    // ==========================================
    // 8. HOVER EFFECT PRODUCT CARD
    // ==========================================
    const productCards = document.querySelectorAll(".product-card");
    productCards.forEach(card => {
        card.addEventListener("mouseenter", function () {
            this.style.transform = "translateY(-4px)";
        });
        card.addEventListener("mouseleave", function () {
            this.style.transform = "translateY(0px)";
        });
    });

    // ==========================================
    // 9. SCROLL SHADOW CART
    // ==========================================
    const cartItems = document.querySelector(".cart-items");
    if (cartItems) {
        cartItems.addEventListener("scroll", function () {
            if (this.scrollTop > 0) {
                this.classList.add("scrolling");
            } else {
                this.classList.remove("scrolling");
            }
        });
    }

});