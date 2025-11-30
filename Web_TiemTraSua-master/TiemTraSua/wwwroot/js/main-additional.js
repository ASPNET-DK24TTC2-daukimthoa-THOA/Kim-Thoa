(function ($) {
    var btn = $('#btnTop');

    $(window).scroll(function () {
        if ($(window).scrollTop() > 300) {
            btn.addClass('show');
        } else {
            btn.removeClass('show');
        }
    });

    btn.on('click', function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: 0 });
    });

    //Navbar slide
    var prevScrollpos = window.scrollY;
    window.onscroll = function () {
        var currentScrollPos = window.scrollY;
        if (prevScrollpos > currentScrollPos) {
            document.getElementById("header-navbar").style.top = "0";
        } else {
            document.getElementById("header-navbar").style.top = "-84px";
        }
        prevScrollpos = currentScrollPos;
    }

    //Canvas Menu
    $(".canvas__open").on('click', function () {
        $(".offcanvas-menu-wrapper").addClass("active");
        $(".offcanvas-menu-overlay").addClass("active");
        $(".header").removeClass("fixed-top");
    });

    $(".offcanvas-menu-overlay, .offcanvas__close").on('click', function () {
        $(".offcanvas-menu-wrapper").removeClass("active");
        $(".offcanvas-menu-overlay").removeClass("active");
        $(".header").addClass("fixed-top");
    });

    $(document).ready(function () {
        $('#updateCart').on('click', function () {
            var updates = [];

            $('.quantity-input').each(function () {
                var productId = $(this).data('product-id');
                var quantity = $(this).val();
                
                updates.push({ productId: productId, quantity: quantity });
            });
            
            $.ajax({
                url: '/cart/update',
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(updates),
                success: function (result) {
                    updateProductList(result.cartItems);
                    $('#totalAmount').html(result.totalAmount.toLocaleString('vi-VN') + ' &#8363;');
                    console.log(result);
                },
                error: function (error) {
                    console.error(error);
                }
            });
        });
    });

    $(document).ready(function () {
        $('.removeItem').on('click', function () {
            var productId = $(this).data('product-id');

            $.ajax({
                url: '/cart/remove',
                method: 'POST',
                data: { maSp: productId },
                success: function (result) {
                    removeProduct(productId);
                    $('#totalAmount').html(result.totalAmount.toLocaleString('vi-VN') + ' &#8363;');
                    console.log(result);
                },
                error: function (error) {
                    console.error(error);
                }
            });
        })
    });
    
    function updateProductList(cartItems) {
        cartItems.forEach(function (item) {
            var productItem = $('.product-item[data-product-id="' + item.maSp + '"]');
            productItem.find('.quantity-input').val(item.soLuong);
            productItem.find('.cart__total').html(item.thanhTien.toLocaleString('vi-VN') + ' &#8363');
        });
    }

    function removeProduct(item) {
        var productItem = $('.product-item[data-product-id="' + item + '"]');
        productItem.remove();
    }

    // Ð?i cho trang du?c t?i hoàn thành
    document.addEventListener('DOMContentLoaded', function () {
        // L?y th? p ch?a thông báo gi? hàng
        var cartMessage = document.querySelector('#cartMessage');

        // L?y th? a thanh toán
        var checkoutLink = document.querySelector('#checkoutLink');

        // Ki?m tra xem có thông báo gi? hàng hay không
        if (cartMessage && cartMessage.textContent.trim() !== "") {
            // N?u có thông báo gi? hàng, thêm thu?c tính không có giá tr? d? ngan ch?n hành d?ng m?c d?nh c?a th? a
            checkoutLink.setAttribute('href', 'javascript:void(0)');
            
            checkoutLink.addEventListener('click', function (event) {
                event.preventDefault();
                alert("Vui lòng thêm s?n ph?m vào gi? hàng!");
            });
        }
    });
})(jQuery);
