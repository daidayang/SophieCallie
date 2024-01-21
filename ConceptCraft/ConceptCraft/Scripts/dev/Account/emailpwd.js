$(function () {
    var content = $("#ErrorMessage").text();
    if (content == null || content == "") {
        $("#ErrorMessageDiv").hide();
    }

    iclick();
});

function iclick() {
    $("#username").attr("oninvalid", "setCustomValidity('" + $('#hidRequired').val().format($('#hidUserName').val()) + "')");
    $("#username").attr("oninput", "setCustomValidity('')");
}

// js format
String.prototype.format = function () {
    if (arguments.length == 0) return this;
    for (var s = this, i = 0; i < arguments.length; i++)
        s = s.replace(new RegExp("\\{" + i + "\\}", "g"), arguments[i]);
    return s;
};