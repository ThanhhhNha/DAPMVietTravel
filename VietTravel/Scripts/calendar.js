// Hàm tạo lịch các ngày của tháng
function generateCalendar(month, year) {
    const daysContainer = document.querySelector('.calendar-dates');
    daysContainer.innerHTML = ''; // Xóa nội dung cũ

    const daysInMonth = new Date(year, month, 0).getDate(); // Số ngày trong tháng
    const firstDay = new Date(year, month - 1, 1).getDay(); // Ngày đầu tiên của tháng

    // Hiển thị các ô trống cho các ngày trước ngày đầu tiên của tháng
    for (let i = 0; i < firstDay; i++) {
        const emptyCell = document.createElement('div');
        daysContainer.appendChild(emptyCell);
    }

    // Hiển thị các ngày trong tháng
    for (let day = 1; day <= daysInMonth; day++) {
        const dayElement = document.createElement('div');
        dayElement.classList.add('calendar-dates-item'); // Thêm lớp CSS
        dayElement.textContent = day;

        dayElement.addEventListener('click', function () {
            // Xử lý chọn ngày
            document.querySelectorAll('.calendar-dates-item').forEach(el => el.classList.remove('selected'));
            dayElement.classList.add('selected');

            // Gọi AJAX để lấy thông tin chi tiết cho ngày đã chọn
            fetchTourDetails(day, month, year);
        });

        daysContainer.appendChild(dayElement);
    }
}

// Hàm gọi AJAX để lấy thông tin chi tiết tour
function fetchTourDetails(day, month, year) {
    const maTour = document.getElementById('maTour').value; // Lấy mã tour từ trường ẩn

    $.ajax({
        url: '/Travel/GetTourDetails',
        type: 'GET',
        data: {
            maTour: maTour,
            day: day,
            month: month,
            year: year
        },
        success: function (data) {
            updateTourDetails(data); // Cập nhật thông tin chi tiết
        },
        error: function () {
            alert('Không thể tải thông tin chi tiết của tour.');
        }
    });
}

// Hàm cập nhật chi tiết tour trên giao diện
function updateTourDetails(data) {
    const detailsContainer = document.getElementById('tour-details');
    if (data) {
        detailsContainer.innerHTML = `
            <h4>Phương tiện di chuyển: ${data.phuongTien}</h4>
            <p><strong>Ngày đi:</strong> ${data.ngayDi}</p>
            <p><strong>Ngày về:</strong> ${data.ngayKetThuc}</p>
            <p><strong>Số lượng người:</strong> ${data.soLuongNguoi}</p>
        `;
    } else {
        detailsContainer.innerHTML = '<p>Không tìm thấy thông tin cho ngày đã chọn.</p>';
    }
}

// Khởi tạo lịch khi trang được tải
document.addEventListener('DOMContentLoaded', function () {
    initCalendar();
});

// Hàm khởi tạo để chọn tháng
function initCalendar() {
    document.querySelectorAll('.calendar--tour__month__list--item').forEach(function (monthItem) {
        monthItem.addEventListener('click', function () {
            const month = parseInt(monthItem.getAttribute('data-month'));
            const year = parseInt(monthItem.getAttribute('data-year'));

            // Xóa đánh dấu tháng đã chọn
            document.querySelectorAll('.calendar--tour__month__list--item').forEach(el => el.classList.remove('selectedMonth'));
            monthItem.classList.add('selectedMonth');

            // Tạo lịch cho tháng đã chọn
            generateCalendar(month, year);
        });
    });

    // Tạo lịch ban đầu cho tháng đầu tiên
    const selectedMonth = document.querySelector('.calendar--tour__month__list--item.selectedMonth');
    if (selectedMonth) {
        const month = parseInt(selectedMonth.getAttribute('data-month'));
        const year = parseInt(selectedMonth.getAttribute('data-year'));
        generateCalendar(month, year);
    }
}
