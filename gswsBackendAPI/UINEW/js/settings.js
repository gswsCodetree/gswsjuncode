(function($) {
  'use strict';
  $(function() {
    $(".nav-settings").on("click", function() {
      $("#right-sidebar").toggleClass("open");
    });
    $(".settings-close").on("click", function() {
      $("#right-sidebar,#theme-settings").removeClass("open");
    });

    $("#settings-trigger").on("click" , function(){
      $("#theme-settings").toggleClass("open");
    });


    //background constants
    var navbar_classes = "navbar-danger navbar-success navbar-warning navbar-dark navbar-light navbar-primary navbar-info navbar-pink";
    var sidebar_classes = "sidebar-light sidebar-dark";
    var $body = $("body");

    //sidebar backgrounds
    $("#sidebar-light-theme").on("click" , function(){
      $body.removeClass(sidebar_classes);
      $body.addClass("sidebar-light");
      $(".sidebar-bg-options").removeClass("selected");
      $(this).addClass("selected");
    });
    $("#sidebar-dark-theme").on("click" , function(){
      $body.removeClass(sidebar_classes);
      $body.addClass("sidebar-dark");
      $(".sidebar-bg-options").removeClass("selected");
      $(this).addClass("selected");
    });


    //Navbar Backgrounds
    $(".tiles.primary").on("click" , function(){
      $(".navbar").removeClass(navbar_classes);
      $(".navbar").addClass("navbar-primary");
      $(".tiles").removeClass("selected");
      $(this).addClass("selected");
    });
    $(".tiles.success").on("click" , function(){
      $(".navbar").removeClass(navbar_classes);
      $(".navbar").addClass("navbar-success");
      $(".tiles").removeClass("selected");
      $(this).addClass("selected");
    });
    $(".tiles.warning").on("click" , function(){
      $(".navbar").removeClass(navbar_classes);
      $(".navbar").addClass("navbar-warning");
      $(".tiles").removeClass("selected");
      $(this).addClass("selected");
    });
    $(".tiles.danger").on("click" , function(){
      $(".navbar").removeClass(navbar_classes);
      $(".navbar").addClass("navbar-danger");
      $(".tiles").removeClass("selected");
      $(this).addClass("selected");
    });
    $(".tiles.light").on("click" , function(){
      $(".navbar").removeClass(navbar_classes);
      $(".navbar").addClass("navbar-light");
      $(".tiles").removeClass("selected");
      $(this).addClass("selected");
    });
    $(".tiles.info").on("click" , function(){
      $(".navbar").removeClass(navbar_classes);
      $(".navbar").addClass("navbar-info");
      $(".tiles").removeClass("selected");
      $(this).addClass("selected");
    });
    $(".tiles.dark").on("click" , function(){
      $(".navbar").removeClass(navbar_classes);
      $(".navbar").addClass("navbar-dark");
      $(".tiles").removeClass("selected");
      $(this).addClass("selected");
    });
    $(".tiles.default").on("click" , function(){
      $(".navbar").removeClass(navbar_classes);
      $(".tiles").removeClass("selected");
      $(this).addClass("selected");
    });
  });
})(jQuery);

// Expand minimize refresh
$(function () {
  var codes = $('.highlight code');
  codes.each(function (ind, el) {
    hljs.highlightBlock(el);
  });

  $('.lobipanel').lobiPanel();
  $('#demoPanel11').lobiPanel();
  $('#lobipanel-basic').lobiPanel();
  $('#lobipanel-custom-control').lobiPanel({
    reload: false,
    close: false,
    editTitle: false
  });
  $('#lobipanel-font-awesome').lobiPanel({
    reload: false,
    close: false,
    editTitle: false,
    unpin: {
      icon: 'fa fa-arrows'
    }, minimize: {
      icon: 'fa fa-chevron-up',
      icon2: 'fa fa-chevron-down'
    },
    expand: {
      icon: 'fa fa-expand',
      icon2: 'fa fa-compress'
    }
  });
  $('#lobipanel-constrain-size').lobiPanel({
    minWidth: 300,
    minHeight: 300,
    maxWidth: 600,
    maxHeight: 480
  });
  $('#lobipanel-from-url').on('loaded.lobiPanel', function (ev, lobiPanel) {
    var $body = lobiPanel.$el.find('.panel-body');
    $body.html('<div class="highlight"><pre><code>' + $body.html() + '</code></pre></div>');
    hljs.highlightBlock($body.find('code')[0]);
  }).lobiPanel({
    loadUrl: 'bootstrap/dist/css/bootstrap.min.css',
    bodyHeight: 400
  });
  $('#lobipanel-multiple').find('.panel').lobiPanel({
    sortable: true
  });
});

$(function () {
    $(function () {
        $('.lobipanel').lobiPanel();
        $('#demoPanel11').lobiPanel();
        $('#lobipanel-basic').lobiPanel();
        $('#lobipanel-custom-control').lobiPanel({
            reload: false,
            close: false,
            editTitle: false
        });
        $('#lobipanel-font-awesome').lobiPanel({
            reload: {
                icon: 'fa fa-refresh'
            },
            editTitle: {
                icon: 'fa fa-edit',
                icon2: 'fa fa-save'
            },
            unpin: {
                icon: 'fa fa-arrows'
            },
            minimize: {
                icon: 'fa fa-chevron-up',
                icon2: 'fa fa-chevron-down'
            },
            close: {
                icon: 'fa fa-times-circle'
            },
            expand: {
                icon: 'fa fa-expand',
                icon2: 'fa fa-compress'
            }
        });
        $('#lobipanel-constrain-size').lobiPanel({
            minWidth: 300,
            minHeight: 300,
            maxWidth: 600,
            maxHeight: 480
        });
        $('#lobipanel-from-url').on('loaded.lobiPanel', function (ev, lobiPanel) {
                var $body = lobiPanel.$el.find('.panel-body');
                $body.html('<div class="highlight"><pre><code>' + $body.html() + '</code></pre></div>');
                hljs.highlightBlock($body.find('code')[0]);
            })
            .lobiPanel({
                loadUrl: '/bootstrap/css/bootstrap.min.css',
                bodyHeight: 400
            });
        $('#lobipanel-multiple').find('.panel').lobiPanel({
            sortable: true
        });
    });
});
// Expand minimize refresh end

// Expand Indicator Space
$(document).ready(function () {
  $("#btnTest").on("click", function () {
      var $self = $(this);
      var $colRight = $("#colB");
      var $colLeft = $("#colA");
      
      //$colRight.removeClass("hidden");
      
      $self.toggleClass("disabled");
      var hasHidden = $colRight.hasClass("hidden");
    
      $colRight.toggleClass("hidden"); 
      if (!hasHidden) {
          $colRight.toggleClass("col-md-6 d-none");
          $colLeft.toggleClass("col-md-6 col-md-12");
      }
      else {
          setTimeout(function(){
              $colRight.toggleClass("col-md-6  d-none");
              $colLeft.toggleClass("col-md-6 col-md-12");
          }, 0);
      }
   
      setTimeout(function(){
        $self.toggleClass("disabled");
      }, 300);

  });
});
// Expand Indicator Space

// ADD and REMOVE Favorite

$(document).ready(function(){
  $("#addfav").click(function(){
      $(".stretch-card span").addClass("add-favorite");
      
  });
  $("#removefav").click(function(){
      $(".stretch-card span").removeClass("add-favorite");
  });
 
});

const $menu = $('.panel-body');

$(document).mouseup(function (e) {
   if (!$menu.is(e.target) // if the target of the click isn't the container...
   && $menu.has(e.target).length === 0) // ... nor a descendant of the container
   {
     $menu.removeClass('is-active');
  }
 });

$('.toggle').on('click', () => {
  $menu.toggleClass('is-active');
});


// Filltering

$(document).ready(function(){

  $(".filter-button").click(function(){
      var value = $(this).attr('data-filter');
      
      if(value == "all")
      {
          //$('.filter').removeClass('hidden');
          $('.filter').show('500');
      }
      else
      {
//            $('.filter[filter-item="'+value+'"]').removeClass('hidden');
//            $(".filter").not('.filter[filter-item="'+value+'"]').addClass('hidden');
          $(".filter").not('.'+value).hide('500');
          $('.filter').filter('.'+value).show('500');
          
      }
  });
  
  if ($(".filter-button").removeClass("active")) {
$(this).removeClass("active");
}
$(this).addClass("active");

});


// $('.accordion').on('shown.bs.collapse', function () {

//   var panel = $(this).find('.show');
//   $('html, body').animate({
//     scrollTop: panel.offset().top
//   }, 500);

// });