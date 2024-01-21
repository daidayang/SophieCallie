$(document).ready(function () {
    $('#ClientDropDown').change(function () {
        $("#requestUrl").val(window.location.href);
        $("#formChangeClient").submit();
    });

})