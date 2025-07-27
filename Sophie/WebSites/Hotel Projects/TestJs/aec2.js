var clientid = 2;
var baseUrl = 'https://crmuat.maverickcrm.com/guest/WebConnect/';
var crm_mode = 0; //   0: start.  1. Login button active  2: Login Alredy
var mbr = null;
var crm_lastkeepsessioncalltime = new Date();

var slide = () => {
    console.log('crm_mode=' + crm_mode);

    if (window.location == null)
        return;

    var pageurl = window.location.href;
    if (pageurl == null)
        return;

    if (pageurl.indexOf("index.aspx") >= 0 || pageurl.indexOf("aec.html") >= 0) {
        if (crm_mode == 0) {
            let queryString = window.location.search;
            //		console.log(queryString);
            if (!queryString)
                return

            let urlParams = new URLSearchParams(queryString);
            //		console.log(urlParams);        
            if (!urlParams)
                return

            let content = urlParams.get('content')
            // console.log(content);
            if (!content)
                return

            var strMbr = parse_eilis_data(content)
            // var strMbr = '{"email":"test45@test.com","name":"Dai56 Dayang56","phone":"1231231234","birth":"2000-12-12"}'
            // console.log(strMbr);
            if (!strMbr)
                return

            if (strMbr.indexOf('"email"') < 0)
                return

            WcSaveData('123', strMbr);
            return;
        }

        if (crm_mode == 1) {
            let t = Date.now() - crm_lastkeepsessioncalltime.getTime();
            if (t > 5000) {
                checkstatus(123);
            }
            else {
                console.log('t=' + t);
            }
        }
    }

    if (pageurl.indexOf("combocheckout.aspx") >= 0 || pageurl.indexOf("checkout.aspx") >= 0) {
        if (crm_mode == 0) {
            checkstatus(123);
        }

        if (crm_mode > 0) {
            if (crm_mode == 1)
                fill_guest_info();

            let t = Date.now() - crm_lastkeepsessioncalltime.getTime();
            if (t > 5000) {
                checkstatus(123);
            }
            else {
                console.log('t=' + t);
            }
        }
    }
};

var loop_handle = setInterval(slide, 1000);

function checkstatus(apikey) {
    var url = baseUrl + 'WcGetData?clientid=' + clientid + '&ApiKey=' + encodeURIComponent(apikey);;
    console.log('sendData("' + url + '", "GET")');

    sendData(url, null, 'GET', on_status_load, on_status_error);
}

function on_status_error() {
    console.log('login_error');
}

function on_status_load(data) {
    console.log('status_load: ' + data);

    if (data && data.indexOf('"email"') >= 0) {
        if (crm_mode != 2)
            crm_mode = 1;
        mbr = JSON.parse(data);
        console.log('mbr.email=' + mbr.email);
    }
    crm_lastkeepsessioncalltime = new Date();
}

function fill_guest_info() {
    console.log('filling data')

    if (!mbr)
        return

    // var input_firstname = document.querySelector('input[name="firstname"]');
    let input_emails = document.querySelector('#txEmail');
    let input_firstname = document.querySelector('#txFirstName');
    // let input_lastname = document.querySelector('#txLastName');
    // let input_address1 = document.querySelector('#txAddress1');
    // let input_city = document.querySelector('#txCity');
    // let input_zip = document.querySelector('#txZip');
    let input_phone = document.querySelector('#txPhone');
    // let input_state = document.querySelector('#txState');
    // let select_country = document.querySelector('#cbCountry');
    // let select_state = document.querySelector('#cbState');


    if (input_emails) {
        input_emails.value = mbr.email
    }

    if (input_firstname)
        input_firstname.value = mbr.name

    if (input_phone)
        input_phone.value = mbr.phone

    crm_mode = 2
}


function on_save_error() {
    console.log('save_error');
}

function on_save_load(data) {
    // console.log('save_load: ' + data);
    if (data && data.indexOf('OK') >= 0) {
        crm_mode = 1;
    }
}

function htmlToElement(html) {
    var template = document.createElement('template');
    html = html.trim(); // Never return a text node of whitespace as the result
    template.innerHTML = html;
    return template.content.firstChild;
}


function build_eilis_data() {
    var key = "XHOJAT4QqQvLQmJfuLhElOInD9NiV8v2"
    var iv = "S6LrbzHhbyHgiJ88"
    var keyStr = CryptoJS.enc.Utf8.parse(key)
    var ivStr = CryptoJS.enc.Utf8.parse(iv)
    var nowTime = Date.now()

    var contentStr = "Testing, Testing"
    var options = {
        iv: ivStr,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    };
    var encryptStr = CryptoJS.AES.encrypt(contentStr, keyStr, options);
    var decryptStr = CryptoJS.AES.decrypt(encryptStr, keyStr, options);
    var decryptStr2 = decryptStr.toString(CryptoJS.enc.Utf8);

    console.log('encrytStr=' + encryptStr);
    console.log('decryptStr=' + decryptStr2);
    return encryptStr;
    // https://stackoverflow.com/questions/14958103/how-to-decrypt-message-with-cryptojs-aes-i-have-a-working-ruby-example
}

function parse_eilis_data(enstr) {
    var key = "XHOJAT4QqQvLQmJfuLhElOInD9NiV8v2"
    var iv = "S6LrbzHhbyHgiJ88"
    var keyStr = CryptoJS.enc.Utf8.parse(key)
    var ivStr = CryptoJS.enc.Utf8.parse(iv)
    var nowTime = Date.now()

    var options = {
        iv: ivStr,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    };
    var decryptStr = CryptoJS.AES.decrypt(enstr, keyStr, options);
    var decryptStr2 = decryptStr.toString(CryptoJS.enc.Utf8);

    // console.log('decryptStr=' + decryptStr2);
    return decryptStr2;
}


function sendData(url, data, openMethod, onload, onerror) {
    // console.log( 'Sending data' );

    const XHR = new XMLHttpRequest();

    // Define what happens on successful data submission
    XHR.addEventListener('load', function (event) {
        onload(XHR.response);
    });

    // Define what happens in case of error
    //  XHR.addEventListener( 'error', function(event) {
    //    alert( 'Oops! Something went wrong.' );
    //  } );

    // Set up our request
    XHR.open(openMethod, url);

    XHR.withCredentials = true;

    if (openMethod == 'POST') {
        // Add the required HTTP header for form data POST requests
        XHR.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');

        // Finally, send our data.
        XHR.send(data);
    }
    else {
        XHR.send();
    }
}

function WcSaveData(apikey, data) {
    var url = baseUrl + 'WcSaveData';

    var postdata = 'clientid=' + clientid;
    postdata += '&apikey=' + encodeURIComponent(apikey);
    postdata += '&data=' + encodeURIComponent(data);

    // console.log('sendData("' + url + '", "' + postdata + '", "POST")');

    sendData(url, postdata, 'POST', on_save_load, on_save_error);
}
