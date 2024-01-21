$(function () {
    var content = $("#ErrorMessage").text();
    if (content == null || content == "") {
        $("#ErrorMessageDiv").hide();
    }

    iclick();
});

function iclick() {
    $("#InitialPassword").attr("oninvalid", "setCustomValidity('" + $('#hidRequired').val().format($('#hidInitialPassword').val()) + "')");
    $("#InitialPassword").attr("oninput", "setCustomValidity('')");

    $("#NewPassword").attr("oninvalid", "setCustomValidity('" + $('#hidRequired').val().format($('#hidNewPassword').val()) + "')");
    $("#NewPassword").attr("oninput", "setCustomValidity('')");

    $("#ConfirmPassword").attr("oninvalid", "setCustomValidity('" + $('#hidRequired').val().format($('#hidConfirmPassword').val()) + "')");
    $("#ConfirmPassword").attr("oninput", "setCustomValidity('')");
}

// js format
String.prototype.format = function () {
    if (arguments.length == 0) return this;
    for (var s = this, i = 0; i < arguments.length; i++)
        s = s.replace(new RegExp("\\{" + i + "\\}", "g"), arguments[i]);
    return s;
};