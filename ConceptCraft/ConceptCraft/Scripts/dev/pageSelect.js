$(document).ready(function () {
    $(".pagedSelect").on("change", function () {
        if ($("#frmSearch") != null && $("#frmSearch").length > 0) {
            var pageSize = $(this).find("option:selected").val();
            $("#hidPageSize").val(pageSize);
            $("#hidPageNumber").val(1);
            $("#frmSearch").submit();
        }
    });
});