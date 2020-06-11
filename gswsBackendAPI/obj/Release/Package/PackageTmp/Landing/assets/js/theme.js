(function($) {
    "use strict";

    /*---------------------
    preloader
    --------------------- */
    $(window).on('load', function() {
        preloader();
    });
    function preloader() {
        $('#ht-preloader').fadeOut();
    };
    // $(window).on('load', function() {
    //     $('#preloader').fadeOut('slow', function() {
    //         $(this).remove();
    //     });
    // });

    $(window).on('resize', function() {
        var wWidth = $(this).width();

        var selectedMenu = $('.main-menu'); 
        if (wWidth > 991) {
            selectedMenu.addClass('menu-activated');
            $('.menu-activated >nav >ul >li ul').addClass('sub-menu');
            $('.menu-activated >nav >ul >li ul li:has(ul)').addClass('menu-item-has-children');
            $('.menu-activated >nav >ul >li:has(ul)').addClass('menu-item-has-children has-submenu');
            $('.prc-anim').addClass('prc-area');

        } else {
            $('.menu-activated >nav >ul >li ul').removeClass('sub-menu');
            $('.menu-activated >nav >ul li:has(ul)').removeClass('menu-item-has-children');
            $('.menu-activated >nav >ul >li:has(ul)').removeClass('menu-item-has-children has-submenu');
            selectedMenu.removeClass('menu-activated');
            $('.prc-anim').removeClass('prc-area');

            $('.feature-item-area .left-feature').removeClass('left-feature');
        }

    }).resize();

    $('ul#mobile_menu').slicknav({
        'appendTo': '.responsive-menu-wrap',
        'label': 'MENU',
    });

    (function() {
        $(".mobile-menu").append('<div class="mobile-menu-logo"></div>');
        $(".mobile-menu-logo").append($(".logo").clone());
    })();

    $(window).on('scroll', function() {
        if ($(window).scrollTop() > 85) {
            $('header').addClass('navbar-fixed-top');
        } else {
            $('header').removeClass('navbar-fixed-top');
        }
    });
     /*---------------------
    Owl carousel
    --------------------- */

    //Testimonial carousel
    function testimonial_carousel() {
        var owl = $(".testimonial-carousel");
        owl.owlCarousel({
            loop: true,
            margin: 20,
            responsiveClass: true,
            navigation: true,
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            nav: true,
            items: 5,
            smartSpeed: 2000,
            dots: true,
            autoplay: true,
            autoplayTimeout: 4000,
            center: true,
            responsive: {
                0: {
                    items: 1
                },
                480: {
                    items: 1
                },
                760: {
                    items: 1
                }
            }
        });
    }
    testimonial_carousel();

    //Testimonial2 carousel
    function testimonial2_carousel() {
        var owl = $(".testimonial2-carousel");
        owl.owlCarousel({
            loop: true,
            margin: 20,
            responsiveClass: true,
            navigation: true,
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            nav: true,
            items: 5,
            smartSpeed: 2000,
            dots: true,
            autoplay: true,
            autoplayTimeout: 4000,
            center: true,
            responsive: {
                0: {
                    items: 1
                },
                480: {
                    items: 1
                },
                768: {
                    items: 3
                }
            }
        });
    }
    testimonial2_carousel();


    //Testimonial3 carousel
    function testimonial3_carousel() {
        var owl = $(".testimonial3-carousel");
        owl.owlCarousel({
            loop: true,
            margin: 20,
            responsiveClass: true,
            navigation: true,
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            nav: true,
            items: 5,
            smartSpeed: 2000,
            dots: true,
            autoplay: false,
            autoplayTimeout: 4000,
            center: true,
            responsive: {
                0: {
                    items: 1
                },
                480: {
                    items: 1
                },
                768: {
                    items: 3
                }
            }
        });
    }
    testimonial3_carousel();


    //Testimonial8 carousel
    function testimonial8_carousel() {
        var owl = $(".testimonial8-carousel");
        owl.owlCarousel({
            loop: true,
            margin: 20,
            responsiveClass: true,
            navigation: true,
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            nav: true,
            items: 5,
            smartSpeed: 2000,
            dots: true,
            autoplay: false,
            autoplayTimeout: 4000,
            center: false,
            responsive: {
                0: {
                    items: 1
                },
                480: {
                    items: 1
                },
                760: {
                    items: 2
                }
            }
        });
    }
    testimonial8_carousel();


    //Testimonial8 carousel
    function portfolio3_carousel() {
        var owl = $(".portfolio3-carousel");
        owl.owlCarousel({
            loop: true,
            margin: 20,
            responsiveClass: true,
            navigation: true,
            navText: ["<i class='fa fa-long-arrow-left'></i>", "<i class='fa fa-long-arrow-right'></i>"],
            nav: true,
            items: 5,
            smartSpeed: 2000,
            dots: false,
            autoplay: false,
            autoplayTimeout: 4000,
            center: true,
            responsive: {
                0: {
                    items: 1
                },
                480: {
                    items: 1
                },
                760: {
                    items: 2
                }
            }
        });
    }
    portfolio3_carousel();

    $.scrollUp({
        scrollText: '<i class="fa fa-arrow-up" aria-hidden="true"></i>',
        easingType: 'linear',
        scrollSpeed: 1500,
        animation: 'fade'
    });

    $('.counter-up').counterUp();


    $('.smoothscroll').on('click', function(e) {
        e.preventDefault();
        var target = this.hash;

        $('html, body').stop().animate({
            'scrollTop': $(target).offset().top - 80
        }, 1200);
    });
	
}(jQuery));


//   Login

  $(document).ready(function(){
    var i=0;
    $("#login").hide();
    $(".hide").click(function(){
        i++;
        if(i%2==0){
            $("#login").hide();
            $("#text").show();
        }
        else{
      $("#login").show();
      $("#text").hide();
        }
    });
});



 // Login Employee - Citizen

    $('.pt-filter-btn').on('click', function(e) {
        if ($('.pt-filter-btn').hasClass('active')) {
            $('.pt-filter-btn').removeClass('active');
            $(this).addClass('active');
        }

        if ($('#employee-login').hasClass('active')) {
            $('.single-login').removeClass('active');
            $('.employee-section').addClass('active');
            setTimeout(function() {
                $('.spricing-info').css("transform", "rotateY(00deg)");
                $('.s-pricing-icon').css("transform", "rotateY(00deg)");
            }, 1);

        }
        if ($('#citizen-login').hasClass('active')) {
            $('.single-login').removeClass('active');
            $('.citizen-section').addClass('active');
            setTimeout(function() {
                $('.spricing-info').css("transform", "rotateY(360deg)");
                $('.s-pricing-icon').css("transform", "rotateY(360deg)");
            }, 1);
        }

    });

    $(window).on('load', function() {
        if ($('#employee-login').hasClass('active')) {
            $('.single-login').removeClass('active');
            $('.employee-section').addClass('active');

        }
        if ($('#citizen-login').hasClass('active')) {
            $('.single-login').removeClass('active');
            $('.citizen-section').addClass('active');

        }
    });


    $(document).ready(function () {
        $('a.bottom').click(function (){
          $('html, body').animate({
            scrollTop: $("body.top").offset().top
          }, 1000)
        })


});

$(document).ready(function () {

    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.scrollup').fadeIn();
        } else {
            $('.scrollup').fadeOut();
        }
    });

    $('.scrollup').click(function () {
        $("html, body").animate({
            scrollTop: 0
        }, 600);
        return false;
    });

});




