var countdownValue = 10; // Giá tr? d?m ngu?c ban d?u (s? giây)

document.addEventListener('DOMContentLoaded', function () {
    startCountdown();
});

function startCountdown() {
    // B?t d?u d?m ngu?c và chuy?n hu?ng sau khi k?t thúc
    var countdownInterval = setInterval(function () {
        countdownValue--;
        document.getElementById('countdownDisplay').innerHTML = countdownValue;
        if (countdownValue <= 0) {
            clearInterval(countdownInterval);
            window.location.href = '/'; // Thay 'TrangChinh.aspx' b?ng du?ng d?n thích h?p
        }
    }, 1000); // Ð?m ngu?c m?i giây (1000ms)
}
