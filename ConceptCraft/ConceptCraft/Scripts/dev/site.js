$(document).ready(function () {
    if ($('#hidCheckDocumentChange') != null && $('#hidCheckDocumentChange').length > 0) {
        window.formHasChanged = false;
        $('input').on('change', function () {
            window.formHasChanged = true;
        });
        window.onbeforeunload = function () {
            if (window.formHasChanged) {
                return "The data is not saved, do you want to leave?";
            }
        };
    }

    //$("#pagedSelect").on("change", function () {
    //    if ($("#frmSearch") != null && $("#frmSearch").length > 0) {
    //        var pageSize = $(this).find("option:selected").val();
    //        $("#hidPageSize").val(pageSize);
    //        $("#hidPageNumber").val(1);
    //        $("#frmSearch").submit();
    //    }
    //});


    if ($ && $.validator && $.validator.addMethod) {
          $.validator.addMethod("email", function (value, element) {
            return this.optional(element) || /(^[-!#$%&'*+/=?^_`{}|~0-9A-Z]+(\.[-!#$%&'*+/=?^_`{}|~0-9A-Z]+)*|^"([\001-\010\013\014\016-\037!#-\[\]-\177]|\\[\001-\011\013\014\016-\177])*")@((?:[A-Z0-9](?:[A-Z0-9-]{0,61}[A-Z0-9])?\.)+(?:[A-Z]{2,6}\.?|[A-Z0-9-]{2,}\.?)$)|\[(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}\]$/i.test(value);
        }, "Please enter a valid email address.");
    }

     setTimeout(function () {
        if ($('#page-wrapper')) {
            if ($('#page-wrapper').find('.fadeInRight')) {
                $('#page-wrapper').find('.fadeInRight').removeClass("fadeInRight");
            }
        }
    }, 1000);

});

function SmoothlyMenu() {
    if (!$('body').hasClass('mini-navbar') || $('body').hasClass('body-small')) {
        // Hide menu in order to smoothly turn on when maximize menu
        $('#side-menu').hide();
        // For smoothly turn on menu
        setTimeout(
            function () {
                $('#side-menu').fadeIn(500);
            }, 100);
    } else if ($('body').hasClass('fixed-sidebar')) {
        $('#side-menu').hide();
        setTimeout(
            function () {
                $('#side-menu').fadeIn(500);
            }, 300);
    } else {
        // Remove all inline style from jquery fadeIn function to reset menu state
        $('#side-menu').removeAttr('style');
    }
}



