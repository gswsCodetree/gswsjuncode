        /* slider */

        //$('.responsive').slick({
        //    dots: false,
        //    infinite: false,
        //    arrow: false,
        //    speed: 500,
        //   slidesToShow: 1,
        //    slidesToScroll: 1,
        //    autoplay: true,
           
           

        //    responsive: [
        //         {
        //            breakpoint: 1366,
        //            settings: {
        //                slidesToShow: 1,
        //                slidesToScroll: 1,                     


        //            }
        //        },
        //        {
        //            breakpoint: 1280,
        //            settings: {
        //                slidesToShow: 1,
        //                slidesToScroll: 1,
                        


        //            }
        //        }, {
        //            breakpoint: 1024,
        //            settings: {
        //                slidesToShow: 6,
        //                slidesToScroll: 1,
        //                infinite: false,


        //            }
        //        }, {
        //            breakpoint: 600,
        //            settings: {
        //                slidesToShow: 2,
        //                slidesToScroll: 2
        //            }
        //        }, {
        //            breakpoint: 480,
        //            settings: {
        //                slidesToShow: 1,
        //                slidesToScroll: 1
        //            }
        //        }
        //        // You can unslick at a given breakpoint now by adding:
        //        // settings: "unslick"
        //        // instead of a settings object
        //    ]
        //});

        //$('.navarathnalu').slick({
        //    dots: false,
        //    infinite: true,
        //    speed: 300,
        //    slidesToShow: 6,
        //    slidesToScroll: 1,
        //    autoplay: true,

        //    responsive: [
        //         {
        //            breakpoint: 1366,
        //            settings: {
        //                slidesToShow: 6,
        //                slidesToScroll: 1,
                        


        //            }
        //        },
        //        {
        //            breakpoint: 1280,
        //            settings: {
        //                slidesToShow: 5,
        //                slidesToScroll: 1,
                        


        //            }
        //        }, {
        //            breakpoint: 1024,
        //            settings: {
        //                slidesToShow: 2,
        //                slidesToScroll: 1,
        //                infinite: false,


        //            }
        //        }, {
        //            breakpoint: 600,
        //            settings: {
        //                slidesToShow: 2,
        //                slidesToScroll: 2
        //            }
        //        }, {
        //            breakpoint: 480,
        //            settings: {
        //                slidesToShow: 1,
        //                slidesToScroll: 1
        //            }
        //        }
        //        // You can unslick at a given breakpoint now by adding:
        //        // settings: "unslick"
        //        // instead of a settings object
        //    ]
        //});


        /*    Sidebar spandana */

        $(document).ready(function () {
            $('#sidebarCollapse').on('click', function () {
                $('#sidebar').toggleClass('active');
                $('#content').toggleClass('testing');
            });
        });
        $(document).ready(function () {
            $('#sidebarClose').on('click', function () {
                $('#sidebar').toggleClass('active');
                $('#content').toggleClass('testing');
            });
        });







        /* treeview */
        var toggler = document.getElementsByClassName("caret");
        var i;

        for (i = 0; i < toggler.length; i++) {
            toggler[i].addEventListener("click", function () {
                this.parentElement.querySelector(".nested").classList.toggle("active");
                this.classList.toggle("caret-down");
            });
        };




        /* Loader */
        $(window).on("load", function () {
            $(".loader").fadeOut("3000");
        });





        //Get the button
        var mybutton = document.getElementById("myBtn");

        // When the user scrolls down 20px from the top of the document, show the button
        window.onscroll = function () {
            scrollFunction()
        };

        function scrollFunction() {
            if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
                mybutton.style.display = "block";
            } else {
                mybutton.style.display = "none";
            }
        }

        // When the user clicks on the button, scroll to the top of the document
        function topFunction() {
            document.body.scrollTop = 0;
            document.documentElement.scrollTop = 0;
        }






        // for stuff that scrolls left on hover
        //        $(".scroll_on_hover").mouseover(function () {
        //            $(this).removeClass("ellipsis");
        //            var maxscroll = $(this).width();
        //            var speed = maxscroll * 15;
        //            $(this).animate({
        //                scrollLeft: maxscroll
        //            }, speed, "linear");
        //        });
        //
        //        $(".scroll_on_hover").mouseout(function () {
        //            $(this).stop();
        //            $(this).addClass("ellipsis");
        //            $(this).animate({
        //                scrollLeft: 0
        //            }, 'slow');
        //        });


        //Show More
        //
        //$(document).ready(function () {
        //    size_li = $(".nested li").size();
        //    x=3;
        //    $('.nested li:lt('+x+')').show();
        //    $('#loadMore').click(function () {
        //        x= (x+5 <= size_li) ? x+5 : size_li;
        //        $('.nested li:lt('+x+')').show();
        //    });
        //    $('#showLess').click(function () {
        //        x=(x-5<0) ? 3 : x-5;
        //        $('#myList li').not(':lt('+x+')').hide();
        //    });
        //});
