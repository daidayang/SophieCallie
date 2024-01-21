
$.validator.addMethod("Spacing", function (value, element) {
    var reg = new RegExp("[\\u4E00-\\u9FFF]+", "g");
    var regshu = /[~#^$@%&!?%;；￥……：:*]/gi;
    if (value.indexOf(' ') > 0 || reg.test(value) || regshu.test(value)) {
        return false;
    } else {
        return true;
    }
});

