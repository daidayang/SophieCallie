$(document).ready(function () {
    $("#liSysLog").addClass("active");

    $('.dataTables_paginate').find('a').click(function () {
        var li = $(this).parents('li:first');
        if (li.hasClass('active') == false && li.hasClass('disabled') == false) {
            $('#hidPageNumber').val($(this).attr('data-dt-idx'));
            $('#frmSearch').submit();
        }
    });

    $('#btnSearch').click(function () {
        $('#hidPageNumber').val('1');
    });

    $('.footable-sort-indicator').click(function () {
        var table = $(this).parents('table:first');
        var tr = $(this).parents('th:first');
        var desc = true;
        if (tr.hasClass('footable-sorted-desc')) {
            desc = false;
        }
        table.find('.footable-sortable').each(function () {
            if ($(this).hasClass('footable-sorted-desc')) {
                $(this).removeClass('footable-sorted-desc');
            }
            if ($(this).hasClass('footable-sorted')) {
                $(this).removeClass('footable-sorted');
            }
        });

        if (desc) {
            tr.addClass('footable-sorted');
            $('#hidSortDirection').val('Asc');
        }
        else {
            tr.addClass('footable-sorted-desc');
            $('#hidSortDirection').val('Desc');
        }

        $('#hidSortColumn').val(tr.attr('data-sort-item'));
        $('#hidPageNumber').val('1');
        $('#frmSearch').submit();
    });

    var initSort = function () {
        var sortDirection = $('#hidSortDirection').val();
        var sortColumn = $('#hidSortColumn').val();

        $('#tbLog').find('th.footable-sortable').each(function () {
            if ($(this).attr('data-sort-item') == sortColumn) {

                if (sortDirection == 'Desc') {
                    $(this).addClass('footable-sorted');
                }
                else {
                    $(this).addClass('footable-sorted-desc');
                }
            }

        });
    };

    initSort();

    highlightsearch($('#searchName'), $('#tbLog').find('tbody'));

    initUserAccess();
});

var initUserAccess = function () {
    $('#divLogList').show();
    $('#divLogAdd').hide();
    var hidVisible = $('#hidVisible').val();

    if (!hidVisible) {
        //$("#btnSearch").attr("disabled", true);
    }

}